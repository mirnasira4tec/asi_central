window.asi = window.asi || {};
asi.chat = asi.chat || {};
(function (chat, $, undefined) {
    var eGainServer = "https://chat.asicentral.com";
    var popupInterval = 30000; //(e.g. 15000 = 15 seconds)
    var noThanksClicked = "";
    var chatWindow = null;

    chat.setup = function (queue, name, interval, showPopup) {
        var eGainURL = eGainServer + "/system/web/view/live/templates/asi_offers/chat.jsp?entryPointId=" + queue + "&fieldname_3=asicentral-team@asinetwork.local&fieldname_1=Store_" + name.replace(/ /g, '-');
        var params = "height=650,width=580,resizable=yes,scrollbars=yes,toolbar=no";
        if (showPopup == undefined) showPopup = true;
        if (interval != undefined) popupInterval = interval;

        var div = '<div id="chat-with-person-div" class="hide "><div class="chat-div-body modal-body"><h3>Chat Live with ASI</h3><p>Welcome to ASI Store for ' + name + '.</p><p>Can I help you with your order?</p>';
        div += '<div class="chat-div-footer"><a id="chat-with-person-div-close" href="#" data-dismiss="modal"><small>× No Thanks</small></a><button id="chat-with-person-div-button" class="btn btn-small btn-primary" data-dismiss="modal">Chat Now</button></div></div></div>';
        $("body").append(div);

        $("#chat-with-person-div-close").click(function () { closeChat(); });
        $("#chat-with-person-div-button").click(function () {
            if (chatWindow && !chatWindow.closed) {
                chatWindow.focus();
            }
            else {
                chatWindow = window.open(eGainURL, 'asichat', params);
                usedChat();
                closeChat();
            }
        });
        $("#chat-with-person-on").click(function () {
            if (chatWindow && !chatWindow.closed) {
                chatWindow.focus();
            }
            else {
                chatWindow = window.open(eGainURL, 'asichat', params);
                usedChat();
            }
        });
        // availability check right after page load for image
        if ($("#chat-with-person-on").length > 0 || showPopup) {
            setTimeout(function () { showOffer(0, queue, showPopup) }, 0);
            setTimeout(function () { showOffer(popupInterval, queue, showPopup) }, popupInterval);
        }
        else {
        }
    };

    function showOffer(time, queue, showPopup) {
        if ('XDomainRequest' in window && window.XDomainRequest !== null) {
            var xdr = new XDomainRequest(); // Use Microsoft XDR
            xdr.open('get', eGainServer + "/system/egain/chat/entrypoint/agentAvailability/" + queue);
            xdr.onload = function () {
                var dom = new ActiveXObject('Microsoft.XMLDOM');
                if (xdr.responseText != null && xdr.responseText.contains("<agentAvailability")) {
                    parseChatXml($.parseXML("<xml><agentAvailability available='true' /></xml>"), time, queue, showPopup);
                }
            };
            xdr.onerror = function () {
                _result = false;
            };
            xdr.send();
        }
        else {
            $.ajax({
                type: "GET",
                url: eGainServer + "/system/egain/chat/entrypoint/agentAvailability/" + queue,
                dataType: "xml",
                success: function parseXml(xml) {
                    parseChatXml(xml, time, queue, showPopup);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert("new error" + errorThrown);
                },
                global: false,
            });
        }
    }
    function parseChatXml(xml, time, queue, showPopup) {
        $(xml).find("agentAvailability").each(function () {
            var available = $(this).attr('available');
            if (available == 'true') {
                $("#chat-with-person-on").show();
                $("#chat-with-person-off").hide();
                if (!chatDeclined() && time > 0 && showPopup) showChat();
            }
            else {
                $("#chat-with-person-on").hide();
                $("#chat-with-person-off").show();
                if (!chatDeclined()) {
                    //check again at the interval
                    setTimeout(function () { showOffer(popupInterval, queue, showPopup) }, popupInterval);
                }
            }
        });
    }
    function chatDeclined() {
        return noThanksClicked == "true";
    }
    function closeChat() {
        noThanksClicked = "true";
        $("#chat-with-person-div").hide();
    }
    function usedChat() {
    }
    function showChat() {
        if (!chatWindow || chatWindow.closed) $("#chat-with-person-div").show();
    }
}(asi.chat, jQuery));

