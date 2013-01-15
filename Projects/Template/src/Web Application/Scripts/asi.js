window.asi = window.asi || {};
asi.util = asi.util || {};

//defining modal funcionality requires jquery and bootstrap
asi.modal = asi.modal || {};

(function (modal, $, undefined) {
    var modalCallback = null;

    modal.confirm = function (title, message, callback, okText, cancelText) {
        modalCallback = callback;
        var modalModel = {
            title: title,
            message: message,
            okText: (okText ? okText : "OK"),
            cancelText: (cancelText ? cancelText : "Cancel"),
        };
        addModal(modalModel);
    };

    function addModal(modalModel) {
        if ($('#asi-modal')) $('#asi-modal').parent().remove();
        $('body').append($('<div>').load('/Scripts/templates/dialog.html', function () {
            ko.applyBindings(modalModel);
            var div = $('#asi-modal');
            var okBtn = div.find("a.btn-primary");
            okBtn.on("click", null, function () { if (modalCallback) { modalCallback(true); modalCallback = null; } });
            div.on("keypress", null, function (e) {
                var code = (e.keyCode ? e.keyCode : e.which);
                if (code === 13) { okBtn.click(); }
            });
            div.on("hide", null, function () { if (modalCallback) { modalCallback(false); modalCallback = null; } });
            $("#asi-modal").modal({});
        }));
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
}(asi.util, jQuery));

//Enabling tooltip by default
$("[rel='tooltip']").tooltip();
