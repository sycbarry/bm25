

let showLoader = function() {
    var el = document.getElementById("bottom-search-box");
    var newEl = document.createElement("div");
    newEl.setAttribute("class", "lds-dual-ring");
    el.appendChild(newEl);
}

let showIndex = function() {
    const elements = document.querySelectorAll(".result-cell");
    for(var i = 0; i < elements.length; i++) {
       elements[i].classList.toggle("visible");
    }
}

let handleAddGenerationAction = function(action) {
    var els = document.querySelectorAll(".searchbox-explanation-button")
    els.forEach(x => {
        if(x.classList.contains("searchbox-explanation-button-active"))  {
            x.classList.toggle("searchbox-explanation-button-active");
        }
    })
    action = action.toLowerCase();
    if(action == 'explain') {
        addActionType(action)
        document.getElementById("explain-button").classList.toggle("searchbox-explanation-button-active");
    }
    if(action == 'simplify') {
        addActionType(action)
        document.getElementById("simplify-button").classList.toggle("searchbox-explanation-button-active");
    }


}

let addActionType = function(action) {
    document.getElementById("generationaction").value = action
}

/* Adding the explanation context bars event listeners. */


/* Function to make the pdf viewer */
function makePDF(fileName, bookShelfName) {

    var defaultState = {
        pdf: null,
        currentPage: 1,
        zoom: 1.3//1.5
    }

    // GET OUR PDF FILE
    var file = '/home/getfile?filename=' + fileName + '&bookshelfname=' + bookShelfName;
    //const params = window.location.search;
    //const urlParams = new URLSearchParams(params);
    //var file = "/home/getfile?filename=" + urlParams.get("filename")
    pdfjsLib.getDocument(file).then((pdf) => {
        defaultState.pdf = pdf;
        render();
    });





    function renderPage(pageNum) {

            defaultState.pdf.getPage(pageNum).then((page) => {

            var viewport = page.getViewport(defaultState.zoom);

            var canvasId = 'pdf-viewer-' + pageNum;
            var newCanvas = document.createElement("canvas");
            newCanvas.setAttribute("class", "pdfpage")
            newCanvas.setAttribute("id", canvasId);
            newCanvas.style.scrollBehavior = "smooth";
            var ctx = newCanvas.getContext('2d');


            newCanvas.width = viewport.width;
            newCanvas.height = viewport.height;


            // add the text element.
            var textContainer = document.createElement("div")
            var textContainerId = "pdf-viewer-" + pageNum + "-text";
            // textContainer.style.position = "fixed";
            textContainer.style.zIndex = "1000";
            textContainer.setAttribute("class", "textLayer");
            textContainer.setAttribute("id", textContainerId)



            var containerCanvas = document.createElement("div")
            containerCanvas.style.position = "relative";
            document.getElementById("canvas_container").appendChild(containerCanvas);
            containerCanvas.appendChild(newCanvas)
            containerCanvas.appendChild(textContainer)

            page.render({
                canvasContext: ctx,
                viewport: viewport
            });



            // add the text layer  over the canvas
            page.getTextContent().then(function (textContent) {

                // get the textLayer class element
                var textLayer = document.getElementById(textContainerId);

                // set the textLayer width and height
                textLayer.style.width = document.getElementById("canvas_container_wrapper").style.width;
                textLayer.style.height = document.getElementById("canvas_container_wrapper").style.height;

                textLayer.style.top = newCanvas.offsetTop + 'px';
                textLayer.style.left = newCanvas.offsetLeft + 'px';


                textLayer.innerHTML = '';
                pdfjsLib.renderTextLayer({
                    textContent: textContent,
                    container: textLayer,
                    viewport: viewport,
                    textDivs: []
                });

            });

        });

    }

    // RENDER PDF DOCUMENT
    function render() {
        for( var i = 1; i < defaultState.pdf._pdfInfo.numPages; i++ ) {
            renderPage(i);
        }
    }




    /*
    // FUNCTION GO TO PREVIOUS SITE
    document.getElementById('previous').addEventListener('click', (e) => {
        if (defaultState.pdf == null || defaultState.currentPage == 1)
            return;
        defaultState.currentPage -= 1;
        document.getElementById("current_page").value = defaultState.currentPage;
        render();
    });

    // FUNCTION GO TO PREVIOUS NEXT
    document.getElementById('next').addEventListener('click', (e) => {
        if (defaultState.pdf == null || defaultState.currentPage > defaultState.pdf._pdfInfo.numPages)
            return;
        defaultState.currentPage += 1;
        document.getElementById("current_page").value = defaultState.currentPage;
        render();
    });

    // FUNCTION GO TO CUSTUM SITE
    document.getElementById('current_page').addEventListener('keypress', (e) => {
        if (defaultState.pdf == null) return;

        var code = (e.keyCode ? e.keyCode : e.which);

        if (code == 13) { // ON CLICK ENTER GO TO SITE TYPED IN TEXT-BOX
            var desiredPage =
                document.getElementById('current_page').valueAsNumber;

            if (desiredPage >= 1 && desiredPage <= defaultState.pdf._pdfInfo.numPages) {
                defaultState.currentPage = desiredPage;
                document.getElementById("current_page").value = desiredPage;
                render();
            }
        }
    });

    // FUNCTION FOR ZOOM IN
    document.getElementById('zoom_in').addEventListener('click', (e) => {
        if (defaultState.pdf == null) return;
        defaultState.zoom += 0.5;
        render();
    });

    // FUNCTION FOR ZOOM OUT
    document.getElementById('zoom_out').addEventListener('click', (e) => {
        if (defaultState.pdf == null) return;
        defaultState.zoom -= 0.5;
        render();
    });
    */

}
