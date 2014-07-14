<style>
.Chat-banner-number-text{
	text-align:left;
	top:140px;
	left:30px;
	position:absolute;
}
.phone-number{
	font-size:18px;
	font-weight:bold;
	line-height:14px;
}
/*------------------------Chat Div-----------------*/
#chat-with-person-div{
    width:500px;
    height:200px;
    position: fixed;
    top: 3em;
    left: 50%;
    background-color:#F0F0F0;
    border:1px solid #58585a;
    background-image:url('images/chat-div-bg-img.jpg');
    background-repeat:no-repeat;
    background-position:right top;
    z-index:1000;
    margin-left:-254px;
}
.chat-div-body h3{
    padding-bottom:15px;
}
.chat-div-body p{
    padding-bottom:0;
}
.chat-div-footer{
    margin-top:30px;
}
    .chat-div-footer button{
        margin-left:20px;
    }
</style>

    <div id="content_home_right" class="shadow">
	<span class="Chat-banner-number-text"><p class="phone-number">(215) 964-1234</p></span>
	<img id="chat-with-person-off" src="images/banner-asistoreheader-phoneempty.jpg" width="293" height="80" border="0" alt="Call ASI Member Services at (215) 964-1234 with questions.">
	<a id="chat-with-person-on" class="hide" href="#"><img src="images/banner-asistoreheader-phoneempty-chat.jpg" width="293" height="80" border="0" alt="Call ASI Member Services at (215) 964-1234 with questions."></a>

<script type="text/javascript" src="js/bootstrap.min.js"></script>
<script type="text/javascript" src="js/asi-chat.js"></script>
<script type="text/javascript">
$(document).ready(function () {
	asi.chat.setup('1003', 'Join Now', 30000);
});
</script>
