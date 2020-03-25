$(document).ready(function () {
    $('[data-toggle="popover"]').popover();

    window.ReloadRoster = function (classID) {
        $.ajax({
            url: "students/" + classID,
            cache: false,
            type: "get",
            dataType: "html",
            success: function (result) {
                $("#students").html(result);
            }
        });

    }

    /* CONFIRM REGISTRATION TASKS */
    $('.glyphicon-thumbs-down').on('click',function () {
        //console.log('boom');
        var r = $(this).attr('id');

        // SHOW THE LOADING PROGRESS ICON
        $('#CL_' + r + '').removeClass('hidden');

        $('#' + r + '').addClass('hidden');

        ConfirmRegistration(r, function (r) {
            if (r == 'Fail') {
                alert('The confirmation failed.');
            } else {
                var c = $('#class_id').val();

                ReloadRoster(c);
            }
            $('#CL_' + r + '').addClass('hidden');
            $('#' + r + '').removeClass('hidden');
        });

    });

    window.ConfirmRegistration = function (reg, callback) {
        var parameters = "{r: " + reg + "}";
        $.ajax({
            type: "POST",
            url: "RegConfirmation",
            data: parameters,
            contentType: "application/json; charset=utf-8",
            success: function (list) {
                callback(list);
            },
            // THE REQUEST DID NOT GET PROCESSED
            error: function(result) {
                //console.log(result);
                $('#CL_' + reg + '').addClass('hidden');
                $('#' + reg + '').removeClass('hidden');
                alert('The confirmation failed.');
            }
        });

    }
    /*********************************/


    /* PAYMENT PROCESSED TASKS glyphicon-usd*/
    $('.glyphicon-alert').on('click', function () {

        $('#ConfirmDelete').modal({ toggle: true, backdrop: false });

        var r = $(this).attr('id');
        //console.log(r);

        // SHOW THE LOADING PROGRESS ICON
        $('#PL_' + r + '').removeClass('hidden');

        $('#' + r + '').addClass('hidden');

        ProcessPayment(r, function (r) {
            if (r == 'Fail') {
                alert('The process payment failed.');
            } else {
                var c = $('#class_id').val();

                ReloadRoster(c);
            }
            $('#PL_' + r + '').addClass('hidden');
            $('#' + r + '').removeClass('hidden');
        });

    });

    window.ProcessPayment = function (reg, callback) {
        var parameters = "{r: " + reg + "}";
        $.ajax({
            type: "POST",
            url: "ProcessPayment",
            data: parameters,
            contentType: "application/json; charset=utf-8",
            success: function (list) {
                callback(list);
            },
            // THE REQUEST DID NOT GET PROCESSED
            error: function (result) {
                //console.log(result);
                $('#PL_' + reg + '').addClass('hidden');
                $('#' + reg + '').removeClass('hidden');
                alert('The process payment failed.');
            }
        });

    }
    /*********************************/

    /* CANCEL REGISTRATION TASKS */
    $('.glyphicon-remove-circle').on('click', function () {
        //console.log('boom');
        var r = $(this).attr('id');

        // SHOW THE LOADING PROGRESS ICON
        $('#CR_' + r + '').removeClass('hidden');

        $('#' + r + '').addClass('hidden');

        CancelRegistration(r, function (r) {
            if (r == 'Fail') {
                alert('Registration cancellation failed.');
            } else {
                var c = $('#class_id').val();

                //ReloadRoster(c);
                window.location.reload();
            }
            $('#CR_' + r + '').addClass('hidden');
            $('#' + r + '').removeClass('hidden');
        });

    });

    window.CancelRegistration = function (reg, callback) {
        var parameters = "{r: " + reg + "}";
        $.ajax({
            type: "POST",
            url: "CancelRegistration",
            data: parameters,
            contentType: "application/json; charset=utf-8",
            success: function (list) {
                callback(list);
            },
            // THE REQUEST DID NOT GET PROCESSED
            error: function (result) {
                //console.log(result);
                $('#CR_' + reg + '').addClass('hidden');
                $('#' + reg + '').removeClass('hidden');
                alert('Registration cancellation failed.');
            }
        });

    }
    /*********************************/

    /* SWITCH REGISTRATION TASKS */
    $('.glyphicon-random').on('click', function () {

        $('#SwitchClass').modal({ toggle: true, backdrop: false });

        //console.log('boom');
        var r = $(this).attr('id');
        r= r.substring(3,r.length);
        
        $.ajax({
            url: "_classchange/" + r,
            cache: false,
            type: "get",
            dataType: "html",
            success: function (result) {
                $("#changeForm").html(result);
            }
        });

    });

    $('#form_ClassChange').on('submit', function (e) {
        e.preventDefault();  //prevent form from submitting
        var form = $('#form_ClassChange');
        var parameters = form.serialize();
        //console.log(parameters);
        if ($('#n').val() == '') {
            alert('The new class cannot be empty');
            return false;
        } else {
            //return false;
            SwitchClasses(parameters, function (r) {
                if (r == 'Fail') {
                    alert('Registration switch failed.');
                } else {
                    var c = $('#o').val();

                    //ReloadRoster(c);
                    window.location.reload();
                    $('#SwitchClass').modal({ toggle: true, backdrop: false });
                }
            });
        }

    });

    window.SwitchClasses = function (parameters, callback) {
        //console.log(parameters);
        //return false;
        $.ajax({
            type: "POST",
            url: "SwitchClasses",
            data: parameters,
            //contentType: "application/json; charset=utf-8",
            success: function (list) {
                callback(list);
            },
            // THE REQUEST DID NOT GET PROCESSED
            error: function (result) {
                //console.log(result);
                alert('Registration switch failed.');
            }
        });

    }
    /*********************************/

});