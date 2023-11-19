//using System.Reflection.Metadata;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PDFParser.Models;
using ConvertApiDotNet;
using PDFParser.Services;
using PDFParser.Enums;



using IRLib.IRIndexer;
using IRLib.IRLib.IRIndexer;
using IRLib.IRRetriever;
using IRLib.IRScorer.ProbabilisticModels;
using IRLib.IRCorpus;
using IRLib.IRIndexer.Enums;


namespace PDFParser.Controllers;

public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;
    private string INDEXFOLDER = "Bookshelves";
    private string CONVERT_API_KEY = "grDyhWweiOdyVv29";


    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }


    [HttpPost]
    [Route("/home/query")]
    public async Task<IActionResult> Query(string filename, string query, string generationaction)
    {


        Retriever r = new Retriever();
        Corpus _corpus = r.RetrieveFile(INDEXFOLDER + "/" +  filename + ".txt");
        BM25Ranker ranker = new BM25Ranker();
        Document[] docs = _corpus.GetDocuments().ToArray();
        int queryTermLength = query.Split(" ").Length;
        List<Document> records = ranker.GetTopN(_corpus, query, docs, queryTermLength);


        GeneratorService service = new GeneratorService(
                records,
                generationaction == "simplify" ? GeneratorAction.SIMPLIFY : GeneratorAction.EXPLAIN,
                query);
        GenerationResponse response = (GenerationResponse) await service.MakeRequest();


        ViewBag.SearchResults = records;
        ViewBag.FileName = filename.Split("/")[1];
        ViewBag.BookShelfName = filename.Split("/")[0];
        ViewBag.Response = response;

        return View("PdfViewer", new { filename = filename });
    }

    [HttpGet]
    [Route("/home/viewer")]
    public IActionResult PdfViewer(string filename, string bookShelfName)
    {
        ViewBag.BookShelfName = bookShelfName;
        ViewBag.FileName = filename;
        return View();
    }

    [HttpGet]
    [Route("/home/getfile")]
    public FileResult DownloadFile(string filename, string bookshelfname)
    {
        var folderPath = (INDEXFOLDER + "/" + bookshelfname);
        var path = Path.Combine(Directory.GetCurrentDirectory(), folderPath, filename);
        MemoryStream ms = new MemoryStream();
        using (var stream = new FileStream(path, FileMode.Open))
        {
            stream.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
        }
        return File(ms, "application/pdf", Path.GetFileName(path));
    }

    [HttpPost]
    [Route("/indexurl")]
    public async Task<IActionResult> IndexURL(string bookShelf, string url)
    {


        // file path
        string bookShelfPath = INDEXFOLDER + "/" + bookShelf + "/";


        using(HttpClient client = new HttpClient())
        {
            var result = await client.GetStringAsync(url);

            // make web request to the url.
            var convertApi = new ConvertApi(CONVERT_API_KEY);

            //Pass as stream
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(result));

            //Html to PDF API. Read more https://www.convertapi.com/html-to-pdf
            var convertToPdf = await convertApi.ConvertAsync("html", "pdf",
                new ConvertApiFileParam(stream, "test.html")
            );

            //PDF as stream
            var outputStream = await convertToPdf.Files[0].FileStreamAsync();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), bookShelfPath + "/file.pdf");
            using(var fs = new FileStream(filePath, FileMode.Create))
            {
                outputStream.CopyTo(fs);
            }

        }


        return RedirectToAction("Index", "Home");
    }


    // parse a file sent from a form
    [HttpPost]
    [Route("/uploadfile")]
    public IActionResult UploadFile(IFormFile file, string bookShelf)
    {


        // check if the file is null
        if (file == null)
        {
            // return the parsed data
            return View("Index");
        }

        // check if the file is a PDF
        if (file.ContentType != "application/pdf")
        {
            // return the parsed data
            return View("Index");
        }

        // check i
        if(System.IO.Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), INDEXFOLDER)) == false)
        {
            // make the folder
            System.IO.Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), INDEXFOLDER + "/" + bookShelf));
        }


        // file path
        string bookShelfPath = INDEXFOLDER + "/" + bookShelf;


        // write hte file to disk
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), bookShelfPath, file.FileName );
        using(var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }



        // file is written to disk.
        // open the file.
        var path = Path.Combine(Directory.GetCurrentDirectory(), bookShelfPath, file.FileName);
        MemoryStream ms = new MemoryStream();
        using(FileStream fs = new FileStream(path, FileMode.Open))
        {
            // convert to a memory stream.
            fs.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
        }



        // build the indexer.
        SFIndexer indexer = new SFIndexerBuilder(ms, file.FileName)
            .AddFileType(FileType.PDF)
            .AddFileDestination(FileDestination.Local, bookShelfPath)
            .Build();


        // invoke the index
        IndexerInvoker iInv = new IndexerInvoker(indexer);
        iInv.Fire();


        // build a new index of all the pdfs in teh bookshelf.
        BSBIndexer bIndexer = new BSBIndexerBuilder(bookShelfPath, bookShelfPath).Build();

        IndexerInvoker bInv = new IndexerInvoker(bIndexer);
        bInv.Fire();



        // return the parsed data
        return RedirectToAction("Index", "Home");

    }

    public IActionResult NewBookShelf(string newBookShelfName)
    {
        // make the new bookshelf (folder)
        if(System.IO.Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), INDEXFOLDER + "/" + newBookShelfName)) == false)
        {
            System.IO.Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), INDEXFOLDER + "/" + newBookShelfName));
        }

        ViewBag.NewBookShelf = false;
        return RedirectToAction("Index");
    }

    public IActionResult DeleteFile(string file, string bookshelf)
    {
        // check if the file exists.
        // delete the file and it's index.
        if(file == null || bookshelf == null)
            return RedirectToAction("Index");

        string folderPath = INDEXFOLDER + "/" + bookshelf;
        if(Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), folderPath)))
        {
            if(Path.Exists(Path.Combine(Directory.GetCurrentDirectory(), folderPath + "/" + file)))
            {
                string preExtension = file.Split(".")[0];
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), folderPath + "/" + preExtension + ".pdf"));
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), folderPath + "/" + preExtension + ".pdf.txt"));
            }
        }

        // check  if the folder contains a file for us to re-index.
        // reindex the folder structure.
        string[] files = System.IO.Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), folderPath));
        if(files.Length > 0)
        {
            BSBIndexer bIndexer = new BSBIndexerBuilder(folderPath, folderPath).Build();

            IndexerInvoker bInv = new IndexerInvoker(bIndexer);
            bInv.Fire();
        }


        return RedirectToAction("Index");
    }


    public IActionResult Index(string newBookShelf, string bookShelf)
    {

        if(newBookShelf == "true") {
            ViewBag.NewBookShelf = true;
            return View();
        }

        if(System.IO.Directory.Exists(INDEXFOLDER) == false)
        {
            ViewBag.Files = new string[]{};
            return View();
        }

        if(bookShelf != null && ! System.IO.Directory.Exists(INDEXFOLDER + "/" + bookShelf))
        {
            return View();
        }


        List<string> bookShelves = Directory.GetDirectories(INDEXFOLDER)
            .ToList();


        ViewBag.BookShelves = bookShelves;

        List<string> files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), INDEXFOLDER + "/" + bookShelf))
            .ToList();

        var filteredFiles = files
            .Select(x => Path.GetExtension(x) == ".pdf" ? Path.GetFileName(x) : null)
            .Where(x => x != null);

        ViewBag.Files = filteredFiles;
        ViewBag.BookShelf = bookShelf;
        ViewBag.NewBookShelf = false;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
