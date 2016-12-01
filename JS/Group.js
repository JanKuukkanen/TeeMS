
function H1Click() {
    var grouptags = ["h1", "h2", "h3", "h4", "h5", "h6"];
    var grouphSet = [];

    for (var i in tags) {
        var these = document.getElementsByTagName(tags[i]);
        if (these.length) {
            for (var n = 0, m = these.length; n < m; n++) {
                these[n].addEventListener("click", function () { alert("hello"); }, false);
            }
        }
    }
}

function SayHello() {
    alert("Hello!");
}