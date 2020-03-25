$(document).ready(function () {
    $('[data-toggle="popover"]').popover();

    /* PROMO CODE STATUS TASKS */
    $('.glyphicon-ok-sign').on('click', function () { // MAKE STAUS INACTIVE

        var r = $(this).attr('id');

        UpdateStatus(r, function (r) {
            if (r == 'Fail') {
                alert('The update status failed.');
            } else {
                
                window.location.reload();
            }

        });

    });

    $('.glyphicon-remove-sign').on('click', function () { // MAKE STAUS ACTIVE

        var r = $(this).attr('id');

        UpdateStatus(r, function (r) {
            if (r == 'Fail') {
                alert('The update status failed.');
            } else {

                window.location.reload();
            }
        });

    });


    window.UpdateStatus = function (reg, callback) {
        var parameters = "{code: '" + reg + "'}";
        $.ajax({
            type: "POST",
            url: "UpdateStatus",
            data: parameters,
            contentType: "application/json; charset=utf-8",
            success: function (list) {
                callback(list);
            },
            // THE REQUEST DID NOT GET PROCESSED
            error: function (result) {
                alert('The update failed.');
            }
        });

    };
    /*********************************/

    /* UPDATE STATUS CODES */
    $('#AddNew').on('click', function () {
        $('#PromoCodeMgtForm').modal({ toggle: true, backdrop: false });
        //var r = $(this).attr('id');

        //UpdateStatus(r, function (r) {
        //    if (r == 'Fail') {
        //        alert('The update status failed.');
        //    } else {

        //        window.location.reload();
        //    }

        //});

    });

    $('.glyphicon-pencil').on('click', function () { // MAKE STAUS ACTIVE

        var r = $(this).attr('id');
        $('#promoCode').val(r);
        $('#codeVal').val(r);

        $('#PromoCodeEditForm').modal({ toggle: true, backdrop: false });
    });

    $('#PromoCodeEditForm').on('shown.bs.modal', function () {
        var pc = $('#promoCode').val();
        if (pc == '') {
            $("#loading").html('The code could not be found, please try again.');
        } else {
            $.ajax({
                url: "_promoedit/" + pc,
                cache: false,
                type: "get",
                dataType: "html",
                success: function (result) {
                    $("#editForm").html(result);
                }
            });

        }
    });

    /* SHOW THE CONFIRM DELETE MODAL DIALOG */
    //$('#PromoCodeMgtForm').on('shown.bs.modal', function () {
    //    if (img == '') {
    //        $("#loading").html('The code could not be found, please try again.');
    //    } else {
    //        /* SHOW THE IMAGE TO THE USER TO CONFIRM */
    //        $.ajax({
    //            url: "_deletesponsor/" + img,
    //            cache: false,
    //            type: "get",
    //            dataType: "html",
    //            success: function (result) {
    //                $("#deleteForm").html(result);
    //            }
    //        });

    //    }
    //});

    $('#form_NewPromoCode').on('submit', function (e) {
        e.preventDefault();  //prevent form from submitting

        AddNewCode(function (r) {
                if (r == "Success") {
                    window.location.reload();
                }
                else {
                    $('#alertText').html('Could not save the code at this time.');
                    $('#alertText').removeClass('hidden');

                    return false;
                }

        })

    });

    window.AddNewCode = function (callback) {
        var form = $('#form_NewPromoCode');
        var parameters = form.serialize();
        //console.log(parameters);
        //return false;
        $.ajax({
            type: "POST",
            url: "NewCode",
            data: parameters,
            dataType: "json",
            success: function (list) {
                callback(list);

            },
            // THE REQUEST DID NOT GET PROCESSED
            error: function (jqXHR, textStatus, errorThrown) {
                $("#alertText").html(jqXHR.status + ': ' + errorThrown);
                $("#alertText").removeClass('hidden');

                $("#loading").addClass('hidden');
            }
        });
    }


});