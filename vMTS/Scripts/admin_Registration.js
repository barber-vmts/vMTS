$(document).ready(function () {
    $('[data-toggle="popover"]').popover();

    $('#curreg a').click(function (e) {
        e.preventDefault();
        var tabID = $(this).attr("href").substr(1);
        if (tabID != undefined) {
            $('#mycollapse').removeClass('hidden');
            $("#students").html('');
            $('#StudentLoader').removeClass('hidden');

            GetStudents(tabID);
        }
    });

    var test = "";
    window.GetStudents = function (tabID) {
        test = "";///qa/admin/
        $.ajax({
            url: test + tabID,
            cache: false,
            type: "get",
            dataType: "html",
            success: function (result) {
                $("#students").html(result);
                $('#mycollapse').removeClass('hidden');

                $('#StudentLoader').addClass('hidden');
            }
        });

    }

    $('#ConfirmReg').on("submit", function (e) {
        e.preventDefault();

        ConfirmRegistration(function (r) {
            if (r == 'Fail') {
                //console.log(r);
                alert('The confirmation failed.');

            } else {
                window.location.reload();

            }
        });
    });

    window.ConfirmRegistration = function (callback) {
        var form = $('#ConfirmReg');
        var parameters = form.serialize();
        $.ajax({
            type: "POST",
            url: "../ConfirmClassRegistration",
            data: parameters,
            //contentType: "application/json; charset=utf-8",
            success: function (list) {
                callback(list);
            }
        });

    }


});