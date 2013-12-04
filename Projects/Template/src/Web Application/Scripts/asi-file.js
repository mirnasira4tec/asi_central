window.asi = window.asi || {};

//defining file/upload features - requires jquery
asi.file = asi.file || {};
(function (file, $, undefined) {
    function createUploadIframe(id, uri) {
        //create frame
        var frameId = 'jUploadFrame' + id;
        var iframeHtml = '<iframe id="' + frameId + '" name="' + frameId + '" style="position:absolute; top:-9999px; left:-9999px"';
        if (window.ActiveXObject) {
            if (typeof uri == 'boolean') iframeHtml += ' src="' + 'javascript:false' + '"';
            else if (typeof uri == 'string') iframeHtml += ' src="' + uri + '"';
        }
        iframeHtml += ' />';
        $(iframeHtml).appendTo(document.body);

        return $('#' + frameId).get(0);
    };
    function createUploadForm(id, fileElementId, data) {
        //create form	
        var formId = 'jUploadForm' + id;
        var fileId = 'jUploadFile' + id;
        var form = $('<form  action="" method="POST" name="' + formId + '" id="' + formId + '" enctype="multipart/form-data"></form>');
        if (data) {
            for (var i in data) {
                $('<input type="hidden" name="' + i + '" value="' + data[i] + '" />').appendTo(form);
            }
        }
        var oldElement = $('#' + fileElementId);
        var newElement = $(oldElement).clone(true);
        $(oldElement).attr('id', fileId);
        $(oldElement).before(newElement);
        $(oldElement).appendTo(form);
        //set attributes
        $(form).css('position', 'absolute');
        $(form).css('top', '-1200px');
        $(form).css('left', '-1200px');
        $(form).appendTo('body');
        return form;
    };
    function ajaxFileUpload(s) {
        s = $.extend({}, $.ajaxSettings, s);
        var id = new Date().getTime();
        var form = createUploadForm(id, s.fileElementId, (typeof (s.data) == undefined ? false : s.data));
        var io = createUploadIframe(id, s.secureuri);
        var frameId = 'jUploadFrame' + id;
        var formId = 'jUploadForm' + id;
        // Watch for a new set of requests
        if (s.global && !$.active++) {
            $.event.trigger("ajaxStart");
            asi.modal.showProcessing();
        }
        var requestDone = false;
        // Create the request object
        var xml = {};
        if (s.global)
            $.event.trigger("ajaxSend", [xml, s]);
        // Wait for a response to come back
        var uploadCallback = function (isTimeout) {
            var io = document.getElementById(frameId);
            try {
                if (io.contentWindow) {
                    xml.responseText = io.contentWindow.document.body ? io.contentWindow.document.body.innerHTML : null;
                    xml.responseXML = io.contentWindow.document.XMLDocument ? io.contentWindow.document.XMLDocument : io.contentWindow.document;

                } else if (io.contentDocument) {
                    xml.responseText = io.contentDocument.document.body ? io.contentDocument.document.body.innerHTML : null;
                    xml.responseXML = io.contentDocument.document.XMLDocument ? io.contentDocument.document.XMLDocument : io.contentDocument.document;
                }
            } catch (e) {
                handleError(s, xml, null, e);
            }
            if (xml || isTimeout == "timeout") {
                requestDone = true;
                var status;
                try {
                    status = isTimeout != "timeout" ? "success" : "error";
                    // Make sure that the request was successful or notmodified
                    if (status != "error") {
                        // process the data (runs the xml through httpData regardless of callback)
                        var data = uploadHttpData(xml, s.dataType);
                        // If a local callback was specified, fire it and pass it the data
                        if (s.success)
                            s.success(data, status);

                        // Fire the global callback
                        if (s.global)
                            $.event.trigger("ajaxSuccess", [xml, s]);
                    } else
                        handleError(s, xml, status);
                } catch (e) {
                    status = "error";
                    handleError(s, xml, status, e);
                }

                // The request was completed
                if (s.global) $.event.trigger("ajaxComplete", [xml, s]);
                asi.modal.hideProcessing();

                // Handle the global AJAX counter
                if (s.global && ! --$.active) $.event.trigger("ajaxStop");

                // Process result
                if (s.complete) s.complete(xml, status);

                $(io).unbind();
                setTimeout(function () {
                    try {
                        $(io).remove();
                        $(form).remove();

                    } catch (e) {
                        handleError(s, xml, null, e);
                    }
                }, 100);

                xml = null;
            }
        }
        // Timeout checker
        if (s.timeout > 0) {
            setTimeout(function () {
                // Check to see if the request is still happening
                if (!requestDone) uploadCallback("timeout");
            }, s.timeout);
        }
        try {
            var form = $('#' + formId);
            $(form).attr('action', s.url);
            $(form).attr('method', 'POST');
            $(form).attr('target', frameId);
            if (form.encoding) $(form).attr('encoding', 'multipart/form-data');
            else $(form).attr('enctype', 'multipart/form-data');
            $(form).submit();
        } catch (e) {
            handleError(s, xml, null, e);
        }
        $('#' + frameId).load(uploadCallback);
        return { abort: function () { } };
    };
    function uploadHttpData(r, type) {
        var data = !type;
        data = type == "xml" || data ? r.responseXML : r.responseText;
        // If the type is "script", eval it in global context
        if (type == "script")
            $.globalEval(data);
        // Get the JavaScript object, if JSON is used.
        if (type == "json")
            eval("data = " + data);
        // evaluate scripts within html
        if (type == "html")
            $("<div>").html(data).evalScripts();

        return data;
    };
    function handleError(s, xhr, status, e) {
        // If a local callback was specified, fire it
        if (s.error) s.error.call(s.context || window, xhr, status, e);

        // Fire the global callback
        if (s.global) (s.context ? $(s.context) : $.event).trigger("ajaxError", [xhr, s, e]);
    };
    function GetContentType(filename) {
        var extension = /^.+\.([^.]+)$/.exec(filename);
        extension = extension == null ? "" : extension[1].toLowerCase();
        if (extension == "pdf")
            return "application/pdf";
        else if (extension == "ps" || extension == "eps" || extension == "ai")
            return "application/postscript";
        else if (extension == "jpeg" || extension == "jpg")
            return "image/jpeg";
        else if (extension == "gif")
            return "image/gif";
        else if (extension == "mpeg" || extension == "mpg")
            return "video/mpeg";
        else if (extension == "swf")
            return "application/x-shockwave-flash";
        else if (extension == "wmv")
            return "video/x-ms-wmv";
        else if (extension == "mov")
            return "video/quicktime";
        else if (extension == "flv")
            return "video/x-flv";
        else if (extension == "avi")
            return "video/x-msvideo";
        else return "";
    }
    file.ValidateExtension = function (file) {
        var accept = file.accept;
        if (accept == undefined || accept.length == 0)
            accept = $(file).data("accept");
        if (accept != undefined && accept.length > 0) {
            var arrayFile = accept.split(",");
            var content = GetContentType(file.value);
            var isValid = false;
            if (arrayFile.length > 0) {
                $.each(arrayFile, function (index, value) {
                    if (content == value) {
                        isValid = true;
                    }
                });
                return isValid;
            }
        }
        else {
            return true;
        }
    }
    file.ajaxUpload = function(url, fieldIdentifier, EventHandler,displayFieldIdentifier, errorFieldIdentifier) {
        var secureUri = ('https:' == document.location.protocol);
        if (displayFieldIdentifier == undefined || displayFieldIdentifier == '') displayFieldIdentifier = fieldIdentifier + "-display";
        ajaxFileUpload
        (
            {
                url: url,
                secureuri: secureUri,
                fileElementId: fieldIdentifier,
                dataType: 'json',
                data: { "fileType": $("#" + fieldIdentifier).attr("accept") },
                success: function (data, status) {
                    if (typeof (data.error) != 'undefined') {
                        if (data.error != '') {
                            if (errorFieldIdentifier != 'undefined') $("#" + errorFieldIdentifier).html(data.error);
                            else alert(data.error);
                        } else {
                            if (errorFieldIdentifier != 'undefined') $("#" + errorFieldIdentifier).html(data.msg);
                            else alert(data.msg);
                        }
                    }
                    else {
                        $("#" + displayFieldIdentifier).val(data.fileName);
                        EventHandler(data.fileName);
                    }
                },
                error: function (data, status, e) {
                    if (errorFieldIdentifier != 'undefined') $("#" + errorFieldIdentifier).html(e);
                    else alert(e);
                    $("#" + displayFieldIdentifier).val("");
                    EventHandler("");
                }
            }
        )
        return false;
    }
    file.insertControls = function () {
        var ieVersion = asi.util.getIEVersion();
        $(".file-upload").each(function (index) {
            if (!ieVersion || ieVersion > 8) {
                var html = '<label for="' + $(this).data('id') + '"><input id="' + $(this).data('id') + '-display" type="text" readonly="true"';
                if (!asi.util.isIE()) html += 'onclick="$(\'#' + $(this).data('id') + '\').trigger(\'click\');return false;"';
                html += '/><input class="hidden" type="file" id="' + $(this).data('id') + '" name="' + $(this).data('id') + '" data-accept="' + $(this).data('accept') + '" />';
                html += '<button class="btn btn-primary" type="button"';
                if (!asi.util.isIE()) html +='onclick="$(\'#' + $(this).data('id') + '\').trigger(\'click\');return false;"';
                html += '>Browse</button></label>';
                $(this).html(html);
            }
            else {
                $(this).html('<input id="' + $(this).data('id') + '-display" type="text" readonly="true" /> <input style="width:83px;" type="file" id="' + $(this).data('id') + '" name="' + $(this).data('id') + '" data-accept="' + $(this).data('accept') + '" >');
            }
        });
    }
    file.enable = function(id) {
        $("#" + id).removeAttr("disabled");
        $("#" + id).parent().find("button").removeAttr("disabled");
    }
    file.disable = function (id) {
        $("#" + id).attr("disabled", "disabled");
        $("#" + id).parent().find("button").attr("disabled", "disabled");
        $("#" + id).attr("value", "");
        $("#" + id + "-display").val("");
    }
    file.displayName = function (fileName, dataID) {
        if (fileName != "" && dataID != "") {
            var fil = fileName.substr(0, fileName.lastIndexOf("_"));
            var ex = fileName.substr(fil.length, fileName.lastIndexOf("."));
            var ext = ex.split(".");
            $("#" + dataID + "-display").val(fil + "." + ext[1]);

        }
    }
    file.loadDisplayName = function (logoPath, cartID, dataID) {
       
        if (logoPath != "") {
            var newName = logoPath.substr((cartID + "_").length, logoPath.length);
                var fil = newName.substr(0, newName.lastIndexOf("_"));
                var ex = newName.substr(fil.length, newName.lastIndexOf("."));
                var ext = ex.split(".");
                $("#" + dataID + "-display").val(fil + "." + ext[1]);
               
            }
        }
}(asi.file, jQuery));
