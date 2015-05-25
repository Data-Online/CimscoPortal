////// var InitiateScreenMsgs = function () {
//////     return {
//////         init: function (msgType) {
//////             alert(msgType);
//////             var template = document.getElementById(msgType).getElementsByTagName("LI")[0].innerHTML;
//////             var footer = document.getElementById(msgType).getElementsByTagName("LI")[1].innerHTML;
//////             document.getElementById(msgType).innerHTML = '';
//////             var newTemplate = '';
//////             var myArray = [];
//////             var jq = $.getJSON("/portal/GetNavbarData/" + msgType)
//////              .done(function (data) {
//////                  for (var i = 0; i < data.length; i++)
//////                  {
//////                      newTemplate += "<li>" + insertMessageText(template, data[i]) + "</li>";
//////                  }
//////                  if (data.length > 0)
//////                  {
//////                      newTemplate += "<li class='dropdown-footer'>" + footer + "</li>";
//////                      document.getElementById(msgType).innerHTML = newTemplate;
//////                      removeClass(document.getElementById(msgType+"-button"), "btn-disable");
//////                  }
//////                  else 
//////                  { addClass(document.getElementById(msgType+"-button"), "btn-disable" ); }
//////              });
//////         }
//////     }
////// }();


////// $(window).bind("load", function () {
//////    //            InitiateScreenMsgs.init("pg-alert");

//////    var AJAX = [];
//////    AJAX.push(getData("pg-alert"));
//////    AJAX.push(getData("pg-note"));
//////    AJAX.push(getData("pg-todo"));

//////    $.when.apply($, AJAX).done(function (data) {
//////        var obj = [];
//////        for (var i = 0, len = arguments.length; i < len; i++) {
//////            obj.push(arguments[i][0]);
//////        }
//////        //alert(obj[0][1].Message);
//////        updateMessageElementOnPage("pg-alert", obj[0]);
//////        updateMessageElementOnPage("pg-note", obj[1]);
//////    });
//////});


function updateMessageElementOnPage(elementType, data) {
    if (data.length > 0) {
        var template = document.getElementById(elementType).getElementsByTagName("LI")[0].innerHTML;
        var footer = document.getElementById(elementType).getElementsByTagName("LI")[1].innerHTML;
        document.getElementById(elementType).innerHTML = '';
        var newTemplate = '';

        for (var i = 0; i < data.length; i++) {
            newTemplate += "<li>" + insertModelDataIntoElement(template, data[i]) + "</li>";
        }
        //if (data.length > 0) {
            if (footer.length > 0) {
                newTemplate += "<li class='dropdown-footer'>" + footer + "</li>";
            }
            document.getElementById(elementType).innerHTML = newTemplate;
            removeClass(document.getElementById(elementType + "-button"), "btn-disable");
        //}        
    }
    else { addClass(document.getElementById(elementType + "-button"), "btn-disable"); }
}

var populateNavbarData = function () {
    return {
        init: function () {
            //alert('alerts');
            var AJAXdata = [];
            AJAXdata.push({});
            AJAXdata.push(getJsonData('GetNavbarData', 'pg-alert'));
            AJAXdata.push(getJsonData('GetNavbarData', 'pg-note'));
            AJAXdata.push(getJsonData('GetNavbarData', 'pg-todo'));
            $.when.apply($, AJAXdata).done(function (data) {
                var obj = [];
                obj.push({});
                for (var i = 1, len = arguments.length; i < len; i++) {
                    obj.push(arguments[i][0]);
                }
                for (var i = 1, len = obj.length; i < len; i++) {
                    updateMessageElementOnPage(obj[i].headerData.dataFor, obj[i].alertData);
                }
            });
        }
    }
}();

//function getData(msgType) {
//    var url = "/portal/GetNavbarData/" + msgType
//    return $.getJSON(url);
//}

// function insertMessageText(element, data) {
//     var $currentElem = new RegExp();
//     for (var key in data) {
//         $currentElem = new RegExp('{{' + key + '}}');
//         element = element.replace($currentElem, data[key]);
//     }
//    return  element;
//}


