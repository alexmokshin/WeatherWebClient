function CheckInputParams() {
    var checkedCity = 0;
    for (let elem of document.getElementById('cityChekboxes').getElementsByTagName('input')) {
        if (elem.checked === true) {
            checkedCity++;
        }
    }
    if (checkedCity === 0)
        alert("You need checked city");
}