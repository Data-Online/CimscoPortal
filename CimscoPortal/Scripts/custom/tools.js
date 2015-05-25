    //Add Classes Function
    function addClass(elem, cls) {
        var oldCls = elem.className;
        if (oldCls) {
            oldCls += " ";
        }
        elem.className = oldCls + cls;
    }

//Remove Classes Function
function removeClass(elem, cls) {
    var str = " " + elem.className + " ";
    elem.className = str.replace(" " + cls, "").replace(/^\s+/g, "").replace(/\s+$/g, "");
}

//Has Classes Function
function hasClass(elem, cls) {
    var str = " " + elem.className + " ";
    var testCls = " " + cls + " ";
    return (str.indexOf(testCls) != -1);
}
