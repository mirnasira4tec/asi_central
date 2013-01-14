window.asi = window.asi || {};
asi.util = asi.util || {};

//defining modal funcionality requires jquery and bootstrap
asi.modal = asi.modal || {};

(function (modal, $, undefined) {
    var modalCallback = null;

    modal.confirm = function (title, message, callback, okText, cancelText) {
        modalCallback = callback;
        addModal(title, message, okText, cancelText);
        $("#asi-modal").modal({});
    };

    function addModal(title, message, okText, cancelText) {
        var parts = ['<div id="asi-modal" class="modal hide fade" tabindex="-1">'];
        parts.push('<div class="modal-header">');
        parts.push('<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>');
        parts.push('<h3>' + title + '</h3>');
        parts.push('</div>');
        parts.push('<div class="modal-body"><p>' + message + "</p></div>")
        parts.push('<div class="modal-footer">');
        if (okText == undefined)
            parts.push('<a href="#" data-dismiss="modal" class="btn btn-primary">OK</a>');
        else
            parts.push('<a href="#" data-dismiss="modal" class="btn btn-primary">' + okText + '</a>');
        if (cancelText == undefined)
            parts.push('<a href="#" data-dismiss="modal" class="btn btn-cancel">Cancel</a></div>');
        else
            parts.push('<a href="#" data-dismiss="modal" class="btn btn-cancel">' + cancelText + '</a>');
        parts.push('</div>');
        parts.push('</div>');
        var div = $(parts.join("\n"));
        $("body").append(div);
        var okBtn = div.find("a.btn-primary");
        okBtn.on("click", null, function() {if (modalCallback) {modalCallback(true); modalCallback = null;}});
        div.on("keypress", null, function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) { okBtn.click();}
        });
        div.on("hide", null, function (){if (modalCallback) {modalCallback(false); modalCallback = null;}});
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