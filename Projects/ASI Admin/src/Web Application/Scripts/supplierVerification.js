$(function () {  
    var groups = $("#MinorityGroups").val();    
    if (groups) {
        var addedMinarotyGroups = groups.split(";");
        $.each(addedMinarotyGroups, function () {            
            var group = this.toString();
            $("input[name='chkMinorityGroups'][value='" + group + "']").prop("checked", true);
        });
    }
    $("input[name='chkMinorityGroups']").click(function () {
        var minorityCategoryCount = $('input[name="chkMinorityGroups"]:checked').length;
        if (minorityCategoryCount > 0) {
            var minorityGroups = "";
            $('input[name="chkMinorityGroups"]:checked').each(function () {
                minorityGroups = minorityGroups + $(this).val() + ";";
            });
            minorityGroups = minorityGroups.substring(0, minorityGroups.length - 1);
            $("#MinorityGroups").val(minorityGroups);           
        }
    });
});