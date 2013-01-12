window.asi = window.asi || {};
asi.modal = asi.modal || {};

(function (modal, $, undefined) {
    modal.confirm = function (title, message, callback) {
        addModal(title, message, callback);
        $("#asi-modal").modal({});
    };
    function addModal(title, message, callback) {
        var parts = ['<div id="asi-modal" class="modal hide fade">'];
        parts.push('<div class="modal-header">');
        parts.push('<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>');
        parts.push('<h3>' + title + '</h3>');
        parts.push('</div>');
        parts.push('<div class="modal-body"><p>' + message + "</p></div>")
        parts.push('<div class="modal-footer"><a href="#" data-dismiss="modal" class="btn btn-primary">OK</a><a href="#" data-dismiss="modal" class="btn">Cancel</a></div>');
        parts.push('</div>');
        var div = $(parts.join("\n"));
        var ok = div.find("a.btn-primary");
        $("body").append(div);
        ok.on("click", null, callback);
    };
}(asi.modal, jQuery));
