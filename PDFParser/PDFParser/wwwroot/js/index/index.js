
let makeNewBookShelf = function() {
    window.location.href = "/?newbookshelf=true"
}
let showDeleteOptions = function() {
    const elements = document.querySelectorAll(".file-trash");
    for(var i = 0; i < elements.length; i++) {
       elements[i].classList.toggle("file-trash-hidden");
    }
}
// delete the file and its index.
let deleteFile = function(file, bookShelf) {
    var path = "/home/deletefile";
    var params = {"file": file, "bookshelf": bookShelf};
    post(path, params)

}
function post(path, params, method='post') {

  // The rest of this code assumes you are not using a library.
  // It can be made less verbose if you use one.
  const form = document.createElement('form');
  form.method = method;
  form.action = path;

  for (const key in params) {
    if (params.hasOwnProperty(key)) {
      const hiddenField = document.createElement('input');
      hiddenField.type = 'hidden';
      hiddenField.name = key;
      hiddenField.value = params[key];
      form.appendChild(hiddenField);
    }
  }

  document.body.appendChild(form);
  form.submit();
}
