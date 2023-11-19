let AddBookContainer = class {

    //default values.
    constructor(parent) {
        // super(id);
        this.hidden = false;
        this.parent = parent;
        this.backdrop = "launchpoint-modal-backdrop";

        this.id = "launchpoint-modal-container";
    }


    render() {
        var container = document.createElement("div");
        container.setAttribute("id", this.id);
        container.setAttribute("class", "hide");

        var backdrop = document.createElement("div");
        backdrop.setAttribute("id", this.backdrop);
        backdrop.setAttribute("class", "launchpoint-modal-backdrop-hide");
        backdrop.onclick = () => this.hide();

        document.getElementById(this.parent).appendChild(backdrop);
        document.getElementById(this.parent).appendChild(container);



    }

    hide () {
        document.getElementById(this.backdrop).setAttribute("class", "launchpoint-modal-backdrop-hide");
        document.getElementById(this.id).setAttribute("class", "hide");

    }

    show () {
        document.getElementById(this.backdrop).setAttribute("class", "launchpoint-modal-backdrop-show");
        document.getElementById(this.id).setAttribute("class", "show");

        this.renderForm()



    }

    applyHandlers(){


    }
}


AddBookContainer.prototype.renderForm = function() {
    // here we show the form.
    document.getElementById(this.id).innerHTML = `
        <div id="book-list">
        </div>
        <div id="add-book-container">
            <form class="dropzone" id="addBookDropZone">
                <input style="display: none;" id="bookshelf" name="bookshelf" value='${jsBookshelf}'/>
            </form>
            <div>
                <p class="size" data-dz-size></p>
                <div class="progress progress-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0"></div>
                <div id="total-progress" class="progress progress-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0"></div>
                <div class="progress-bar progress-bar-success" style="width:0%;" data-dz-uploadprogress></div>
            </div>
            <button class="btn btn-primary start">
                <i class="glyphicon glyphicon-upload"></i>
                <span>Upload</span>
            </button>
            <button data-dz-remove class="btn btn-danger delete">
                <i class="glyphicon glyphicon-trash"></i>
                <span>Remove</span>
            </button>
        </div>

    `;

    bookList
        .map(book => {
        var childDiv = document.createElement("div");
        childDiv.setAttribute("class", "book-item");
        childDiv.innerHTML = `<a href=${book.url}>${book.book}</a>`;
        document.getElementById("book-list").appendChild(childDiv);
    })



    var myDropzone = new Dropzone("#addBookDropZone", { // Make the whole body a dropzone
        paramName: "file",
        url: "/uploadfile", // Set the url
        createImageThumbnails: false,
        thumbnailWidth: 80,
        thumbnailHeight: 80,
        uploadMultiple: false,
        maxFiles: 1,
        autoProcessQueue: false,
        clickable: true,
        renameFile: (file) => {
            var name = file.name ;
            name = name.split(" ")[0];
            file.name = name;
            console.log(file.name);
        }
    });


    // Update the total progress bar
    myDropzone.on("totaluploadprogress", function(progress) {
        document.querySelector("#total-progress").style.width = progress + "%";
    });

    // Hide the total progress bar when nothing's uploading anymore
    myDropzone.on("queuecomplete", function(progress) {
        document.querySelector("#total-progress").style.opacity = "0";
        setTimeout(() => {
            window.location.href = `/?bookshelf=${jsBookshelf}`;
        }, 100)
    });



    myDropzone.on("sending", function(file) {
        // Show the total progress bar when upload starts
        document.querySelector("#total-progress").style.opacity = "1";
        // And disable the start button
        //file.previewElement.querySelector(".start").setAttribute("disabled", "disabled");
    });

    // Setup the buttons for all transfers
    // The "add files" button doesn't need to be setup because the config
    // `clickable` has already been specified.
    document.querySelector(".start").onclick = function() {
        // myDropzone.enqueueFiles(myDropzone.getFilesWithStatus(Dropzone.ADDED));
        myDropzone.processQueue();

    }
    document.querySelector(".delete").onclick = function() {
        myDropzone.removeAllFiles(true);
    };

    this.applyHandlers();

}
AddBookContainer.prototype.hideForm = function() {
    // here we hide the form.
    this.hide()
}



// sendoff for the form.
AddBookContainer.prototype.submitForm = function() {
    window.alert(true);
}



var container = new AddBookContainer("index-container");
container.render();




let showAddBookContainer = function() {
    container.show()
}

let hideAddBookContainer = function() {
    container.hide();
}
