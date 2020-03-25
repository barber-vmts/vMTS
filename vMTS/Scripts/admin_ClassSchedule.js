$(document).ready(function () {
    $('[data-toggle="popover"]').popover();

    var subsite = "adminschedule";

    $('#classnum a').click(function (e) {
        e.preventDefault()
        var tabID = $(this).attr("href").substr(1);
        var tpe = $(this).attr("title");
        //alert(tpe);
        //return false;
        $.ajax({
            url: "/" + tabID,
            cache: false,
            type: "get",
            dataType: "html",
            success: function (result) {
                $("#Schedule").html(result);

                $('#bs-modal-lg').modal({
                    show: true,
                    keyboard: false,
                    backdrop: false
                });

                $('#bs-modal-lg').on('shown.bs.modal', function (e) {
                    //var button = $(e.relatedTarget);
                    //var recipient = button.data('title');
                    var modal = $(this)
                    modal.find('.modal-title').text(tpe)
                    //$('#modal_title').html('Test'); + recipient
                });
            }
        });
    });

    window.ReloadClassSchedule = function (tabID) {
        //$.ajax({
        //    url: subsite + "/" + tabID,
        //    cache: false,
        //    type: "get",
        //    dataType: "html",
        //    success: function (result) {
        //        $("#current_schedule").html(result);
        //    }
        //});
        window.location.reload();
    }
    //$("#CLASS_TYPE").on('change', function (e) {
    //    var v = $("#CLASS_TYPE").val();
    //    if (v == "Aggressive Skills") {
    //        $("#OPEN_SEATS").val("10");
    //        $("#CLASS_DESCRIPTION").val("Would you and your friends like to learn some advance motorcycle skills used by the motor cops? This is your chance to test your skills with other riders. We are offering a one-time class to be conducted with an experienced Motor Trooper. Several skills courses will be setup and instruction provided. After some practice, each rider will get the opportunity to compete against each other like the motor cops do in their bike skills training and rodeo.");
    //    } else {
    //        $("#OPEN_SEATS").val("11");
    //        $("#CLASS_DESCRIPTION").val("The Basic RiderCourse is designed to teach the novice motorcyclist the skills necessary to ride safely on the street. The course takes the beginning rider through the basics of motorcycle operation: straight line riding, turning, shifting, stopping and safe riding strategies.");
    //    }
    //});

    $("#CLASS_START_DATE").datepicker({ startDate: '0d' });
    $('#CLASS_END_DATE').datepicker({ startDate: '0d' });

    $('#form_NewClass').on('submit', function (e) {
        e.preventDefault();  //prevent form from submitting

        var form = $('#form_NewClass');
        var parameters = form.serialize();
        //console.log(parameters);
        //return false;
        $.ajax({
            type: "POST",
            url: subsite + "/ClassScheduleChanges",
            data: parameters,
            dataType: "json",
            success: function (list) {
                console.log(list);
                //$('#alertText').html(list);
                $('#ScheduleAlert').modal({ toggle: true, backdrop: false });

                if (list == "Schedule Changed Successfully") {
                        ReloadClassSchedule('_schedule');
                }
                else {
                    
                    $('#alertText').html(list);
                    $('#alertText').removeClass('hidden');

                    return false;
                }
            }
        });
    });

    $('#form_EditClass').on('submit', function (e) {
        e.preventDefault();  //prevent form from submitting

        var form = $('#form_EditClass');
        var parameters = form.serialize();
        //console.log(parameters);
        //return false;
        $.ajax({
            type: "POST",
            url: subsite + "/ClassScheduleChanges",
            data: parameters,
            dataType: "json",
            success: function (list) {
                //$('#alertText').html(list);

                if (list == "Schedule Changed Successfully") {
                    ReloadClassSchedule('_schedule');
                }
                else {
                    $('#alertText').html(list);
                    $('#alertText').removeClass('hidden');

                    return false;
                }
            }
        });
    });

    $('#form_DeleteClass').on('submit', function (e) {
        e.preventDefault();  //prevent form from submitting

        var form = $('#form_DeleteClass');
        var parameters = form.serialize();

        $.ajax({
            type: "POST",
            url: subsite + "/CancelClass",
            data: parameters,
            dataType: "json",
            success: function (list) {
                if (list == "Class Canceled Successfully") {
                        ReloadClassSchedule('_schedule');
                }
                else {
                    $('#alertText').html(list);
                    $('#alertText').removeClass('hidden');

                    return false;
                }
            }
        });
    });

    $('#form_Confirmation').on('submit', function (e) {
        e.preventDefault();  //prevent form from submitting

        var form = $('#form_Confirmation');
        var parameters = form.serialize();

        $.ajax({
            type: "POST",
            url: subsite + "/CheckConfirmationCode",
            data: parameters,
            dataType: "json",
            success: function (list) {
                if (list == "Code Matched") {
                    //console.log(list);
                    $('#ClearScheduleAction').modal({ toggle: true, backdrop: false });

                    //$('#ScheduleAlert').on('hidden.bs.modal', function () {
                    //    $('#bs-modal-lg').modal('toggle');

                    //    ReloadClassSchedule('_Schedule');

                    //})

                    //window.location.reload();
                }
                else {
                    console.log(list);
                    return false;
                }
            }
        });
    });

    $('#form_ClearSchedule').on('click', function (e) {
        e.preventDefault();  //prevent form from submitting

        var parameters = "";

        $.ajax({
            type: "POST",
            url: subsite + "/ClearSchedule",
            data: parameters,
            dataType: "json",
            success: function (list) {
                if (list == "Schedule Has Been Cleared") {
                    //console.log(list);
                    $('#before').hide();
                    $('#before_footer').hide();

                    $('#after').show();
                    $('#after_footer').show();

                    $('#text').html(list);

                    $('#ClearScheduleAction').on('hidden.bs.modal', function () {
                        //$('#bs-modal-lg').modal('toggle');

                        //ReloadClassSchedule('_Schedule');

                        window.location.reload();
                    })

                }
                else {
                    $('#before').hide();
                    $('#before_footer').hide();

                    $('#after').show();
                    $('#after_footer').show();

                    $('#text').html(list);

                    //console.log(list);
                    return false;
                }
            }
        });
    });

});