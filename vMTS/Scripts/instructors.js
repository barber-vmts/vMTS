//$(document).ready(function () {
var site = "";///qa
    window.ParseBio = function (e,abt) {

        $.ajax({
            async: true,
            url: site + '/Content/InstructorBios/'+abt,
            dataType: 'text',
            success: function (data) {
                $('#'+ e).html(data);
                //console.log(data);
            }
        });
    }
//});