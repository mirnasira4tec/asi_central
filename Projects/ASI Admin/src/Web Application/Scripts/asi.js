window.asi = window.asi || {};
asi.util = asi.util || {};

//defining modal funcionality requires jquery and bootstrap
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
            okBtn.on("click", null, function () { modalClosing(true, div, callback) });
            div.on("keypress", null, function (e) {
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code === 13) { okBtn.click(); }
            });
            div.on("hide", null, function () { modalClosing(false, div, callback) });
            div.modal({});
        }));
    };

    function modalClosing(okClicked, div, callback) {
        div.remove();
        if (callback) callback(okClicked);
        callback = null;
    };
}(asi.modal, jQuery));

//defining utility features - requires jquery
asi.util = asi.util || {};

(function (util, $, undefined) {
    util.debug = false;

    util.log = function (msg) {
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
}(asi.util, jQuery));

//Trim does not work in older version of IE
if (typeof String.prototype.trim !== 'function') {
    String.prototype.trim = function () {
        return this.replace(/^\s+|\s+$/g, '');
    }
}

//Enabling tooltip by default
$("[rel='tooltip']").tooltip();
