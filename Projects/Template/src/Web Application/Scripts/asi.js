window.asi = window.asi || {};
//defining modal funcionality requires jquery and bootstrap
//templates need TemplateController and the Views/Template folder
asi.modal = asi.modal || {};
(function (modal, $, undefined) {
    modal.confirm = function (title, message, callback, okText, cancelText) {
        var modalData = {
            title: title,
            message: message,
            okText: (okText ? okText : "OK"),
            cancelText: (cancelText ? cancelText : "Cancel"),
        };
        asi.util.log("Show Modal Dialog");
        showModal('/Template/Dialog', modalData, callback);
    };
    function showModal(template, modalData, callback) {
        if ($('#asi-modal')) $('#asi-modal').parent().remove();
        $('body').append($('<div>').load(template, function () {
            ko.applyBindings(modalData);
            var div = $('#asi-modal');
            var okBtn = div.find("a.btn-primary");
            if (okBtn.text() == "#hide") okBtn.attr('class', okBtn.attr('class') + " hide");
            else {
                okBtn.on("click", null, function () { modalClosing(true, div, callback) });
                div.on("keypress", null, function (e) {
                    var code = (e.keyCode ? e.keyCode : e.which);
                    if (code === 13) { okBtn.click(); }
                });
            }
            div.on("hide", null, function () { modalClosing(false, div, callback) });
            div.modal({});
        }));
    };
    function modalClosing(okClicked, div, callback) {
        div.remove();
        asi.util.log("ok clicked: " + okClicked);
        if (callback) callback(okClicked);
        callback = null;
    };
    //processing modal
    var processingDiv = $('<div class="modal hide" data-backdrop="static" data-keyboard="false"><div class="modal-header"><h3>Loading... Please wait <img src="/Images/store/loader-blue.gif" alt="loading"></h1></div><div class="modal-body"></div></div>');
    modal.showProcessing = function () {
        processingDiv.modal();
    };
    modal.hideProcessing = function () {
        processingDiv.modal('hide');
    };
}(asi.modal, jQuery));
//defining utility features - requires jquery
asi.util = asi.util || {};
(function (util, $, undefined) {
    util.debug = false;
    util.log = function(msg) {
        if (util.debug) {
            if (window.console) {
                //firefox and chrome
                console.log(msg)
            }
            else {
                //everything else
                $("body").append('<div class="text-warning">' + msg + '</div>');
            }
        }
    };
    util.center = function (element) {
        element.css("position", "absolute");
        element.css("top", Math.max(0, (($(window).height() - $(element).outerHeight()) / 2) + $(window).scrollTop()) + "px");
        element.css("left", Math.max(0, (($(window).width() - $(element).outerWidth()) / 2) + $(window).scrollLeft()) + "px");
    }
    util.submit = function (url, parameters) {
        var formData = {
            Url: url,
            Parameters: parameters
        };
        asi.util.log("Form url: " + url);
        $('body').append($('<div>').load('/Template/Form', function () {
            ko.applyBindings(formData);
            $('#asi-form').submit(); //No need to remove the form as page getting refreshed
        }));
    };
    util.isIE = function () {
        return /*@cc_on!@*/0;
    };
    util.getIEVersion = function () {
        var undef, v = 3, div = document.createElement('div'), all = div.getElementsByTagName('i');

        while (
			div.innerHTML = '<!--[if gt IE ' + (++v) + ']><i></i><![endif]-->',
			all[0]
		);

        return v > 4 ? v : undef;
    };
    //funtion to format the Total Cost in decimals. 1) Add seperator(,) after every three digits. 2) Convert the number into decimal, with to digits after decimal.
    util.formatAmount = function (n, sep, decimals) {
        var strReturn = n;
        sep = sep || "."; // Default to period as decimal separator
        decimals = decimals || 2; // Default to 2 decimals
        var arr = n.toLocaleString().split(sep);
        if (arr.length == 1)
            strReturn = n.toLocaleString().split(sep)[0] + sep + "00";
        else if (arr.length == 2)
            strReturn = n.toLocaleString().split(sep)[0] + sep + n.toFixed(decimals).split(sep)[1]
        return strReturn;
    };
}(asi.util, jQuery));
//Trim does not work in older version of IE
if (typeof String.prototype.trim !== 'function') {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    }
}
if (typeof String.prototype.contains !== 'function') {
    String.prototype.contains = function (value) {
        return this.indexOf(value) >= 0;
    }
}
//Enabling tooltip by default
if ($("[rel='tooltip']") != null) $("[rel='tooltip']").tooltip();
$(document).ajaxError(function (error) {
    alert("We are sorry, there has been some unexpected error. Please try again");
    $.event.trigger("ajaxComplete", error);
});
$(".single-line").change(function () {
    this.value = $.trim(this.value);
});
