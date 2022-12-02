$(document).ready(function () {


    let isMain = $(this).is(":checked");


    if (isMain) {

        $("#ParentList").addClass("d-none");
        $("#MainImage").removeClass("d-none");
    }
    else {
        $("#ParentList").removeClass("d-none");
        $("#MainImage").addClass("d-none");

    }

    $("#IsMain").click(function () {
        let isMain = $(this).is(":checked");


        if (isMain) {

            $("#ParentList").addClass("d-none");
            $("#MainImage").removeClass("d-none");
        }
        else {
            $("#ParentList").removeClass("d-none");
            $("#MainImage").addClass("d-none");

            }

    })


})