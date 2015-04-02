     var InitiateScreenMsgs = function () {
         return {
             init: function (msgType) {
                 alert(msgType);
                 var template = document.getElementById(msgType).getElementsByTagName("LI")[0].innerHTML;
                 var footer = document.getElementById(msgType).getElementsByTagName("LI")[1].innerHTML;
                 document.getElementById(msgType).innerHTML = '';
                 var newTemplate = '';
                 var myArray = [];
                 var jq = $.getJSON("/portal/GetNavbarData/" + msgType)
                  .done(function (data) {
                      for (var i = 0; i < data.length; i++)
                      {
                          newTemplate += "<li>" + insertMessageText(template, data[i]) + "</li>";
                      }
                      if (data.length > 0)
                      {
                          newTemplate += "<li class='dropdown-footer'>" + footer + "</li>";
                          document.getElementById(msgType).innerHTML = newTemplate;
                          removeClass(document.getElementById(msgType+"-button"), "btn-disable");
                      }
                      else 
                      { addClass(document.getElementById(msgType+"-button"), "btn-disable" ); }
                  });
             }
         }
     }();

     
     $(window).bind("load", function () {
        //            InitiateScreenMsgs.init("pg-alert");
         
        var AJAX = [];
        AJAX.push(getData("pg-alert"));
        AJAX.push(getData("pg-note"));
        AJAX.push(getData("pg-todo"));

        $.when.apply($, AJAX).done(function (data) {
            var obj = [];
            for (var i = 0, len = arguments.length; i < len; i++) {
                obj.push(arguments[i][0]);
            }
            //alert(obj[0][1].Message);
            updateElementOnPage("pg-alert", obj[0]);
            updateElementOnPage("pg-note", obj[1]);
        });
    });

     
     function updateElementOnPage(elementType, data) {
        var template = document.getElementById(elementType).getElementsByTagName("LI")[0].innerHTML;
        var footer = document.getElementById(elementType).getElementsByTagName("LI")[1].innerHTML;
        document.getElementById(elementType).innerHTML = '';
        var newTemplate = '';

        for (var i = 0; i < data.length; i++) {
            newTemplate += "<li>" + insertMessageText(template, data[i]) + "</li>";
        }
        if (data.length > 0) {
            if (footer.length > 0) {
                newTemplate += "<li class='dropdown-footer'>" + footer + "</li>";
            }
            document.getElementById(elementType).innerHTML = newTemplate;
            removeClass(document.getElementById(elementType + "-button"), "btn-disable");
        }
        else { addClass(document.getElementById(elementType + "-button"), "btn-disable"); }

    }

function getData(msgType) {
    var url = "/portal/GetNavbarData/" + msgType
    return $.getJSON(url);
}

    function insertMessageText(element, data) {
        return  element
                        .replace('{{MessageText}}', data.Message)
                        .replace('{{MessageFooter}}', data.Footer)  
                        .replace('{{MessageTime}}', data.TimeStamp)
                        .replace('{{Element1}}', data.Element1)
                        .replace('{{Element2}}', data.Element2)
        ;
    }
