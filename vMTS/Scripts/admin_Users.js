$(document).ready(function () {
    $('[data-toggle="popover"]').popover();

    var subsite = "";//qa/
    /*

    THIS IS THE USER SECTION

*/
    //$('button').click(function (e) {
    //    e.preventDefault();

    //    var n = $(this).attr('name');

    //    alert(n);
    //    return false;
    //});
    /*************************/
    window.ReloadUsers = function (tabID) {
        $.ajax({
            url: subsite + tabID,
            cache: false,
            type: "get",
            dataType: "html",
            success: function (result) {
                $("#allusers").html(result);
            }
        });

    };

    $('#form_NewUser').on('submit', function (e) {
        e.preventDefault();  //prevent form from submitting

        var form = $('#form_NewUser');
        var parameters = form.serialize();
        //console.log(parameters);
        //return false;
        $.ajax({
            type: "POST",
            url: subsite + "NewUser",
            data: parameters,
            dataType: "json",
            success: function (list) {
                //console.log(list);
                $('#UsersAlert').modal({ toggle: true, backdrop: false });

                //$('#modal-title').html();
                $('#UsersAlert').on('shown.bs.modal', function (e) { var r = $(e.relatedTarget); var m = $(this); m.find('.modal-title').text('New User'); });

                if (list == "Pass") {

                    $('#alertText').html('User added successfully');

                    $('#UsersAlert').on('hidden.bs.modal', function () {
                        $('#bs-modal-lg').modal('toggle');

                        ReloadUsers('_Users');

                        $('#form_NewUser')[0].reset();


                    });
                }
                else {
                    //console.log(list);
                    $('#alertText').html(list);

                    $('#UsersAlert').modal({ toggle: true, backdrop: false });
                    return false;
                }
            }
        });
    });

});