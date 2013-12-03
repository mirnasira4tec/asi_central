$("#IsNonUSAddress").change(function (evt) {
    if ($("#country-div").attr("class").trim() == "control-group")
        $("#country-div").attr("class", "control-group hidden");
    else
        $("#country-div").attr("class", "control-group");
    if ($("#country-div").attr("class") == "control-group hidden") {
        $("#Country").val("USA");
        $("#State").val("");
        $("#state-div").attr("class", "control-group");
        $("#international-state-div").attr("class", "control-group hidden");
        $("#InternationalState").val("N/A");
    }
    else {
        $("#Country").val("");
        $("#state-div").attr("class", "control-group hidden");
        $("#State").val("AL");
        $("#InternationalState").val("");
        $("#international-state-div").attr("class", "control-group");
    }
});
$("#add-secondary-contact-button").click(function (evt) {
    if ($("#secondary-contact-person").attr('class').trim() != "collapse") {
        //reset the optional contact field when user makes option disappear
        $("#OptionalContactFirst").val("");
        $("#OptionalContactLast").val("");
        $("#OptionalContactTitle").val("");
        $("#OptionalContactTelephone").val("");
        $("#OptionalContactEmail").val("");
    }
});
$("#add-secondary-contact-button").click(function (evt) {
    if ($("#secondary-contact-person").attr("class").indexOf("in") == -1) {
        //showing
        $("#add-secondary-contact-button").html("- I want to remove additional contact person");
    }
    else {
        $("#add-secondary-contact-button").html("+ I want to add another contact person");
        $("#OptionalContactFirst").val("");
        $("#OptionalContactLast").val("");
        $("#OptionalContactTitle").val("");
        $("#OptionalContactTelephone").val("");
        $("#OptionalContactEmail").val("");
    }
});