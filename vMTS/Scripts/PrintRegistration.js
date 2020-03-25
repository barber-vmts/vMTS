$(document).ready(function () {

    window.RegistrationPrint = function () {
        $(document).load(this.href);
    }

    RegistrationPrint();

    setTimeout("window.print()", 1500);

    setTimeout("window.close()", 45000);

});