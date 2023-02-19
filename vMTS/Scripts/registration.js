$(document).ready(function(){
    $('#inputFirstName').focus();
    var today = new Date($('#START_DATE').val());//;
    var currentYear = today.getFullYear();
    //$('#inputCardYear').val(currentYear + 1); // Do not dispaly year in textbox

    window.GetMaxBirthDate = function () {
        var minimumYear = currentYear - 14;
        var currentMonth = today.getMonth() + 1;
        var currentDay = today.getDate();
        if (currentMonth <= 9) {
            currentMonth = '0' + currentMonth;
        }
        if (currentDay <= 9) {
            currentDay = '0' + currentDay;
        }
        //console.log(minimumYear + '-' + currentMonth + '-' + currentDay);

        return minimumYear + '-' + currentMonth + '-' + currentDay;
    }

    maxBirthDate = GetMaxBirthDate();

    //$('#inputDOB').datepicker({
    //    format: 'yyyy-mm-dd',
    //    endDate: maxBirthDate
    //});

    //based on the DOB input and class start date
    //window.DaysFrom15 = function (classDate, birthDate) {
    //    var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
    //    var firstDate = new Date(classDate.getFullYear,classDate.getMonth,classDate.getDate);
    //    var secondDate = new Date(birthDate.getFullYear, birthDate.getMonth, birthDate.getDate);

    //    var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));d
    //}

    window.GetAge = function (dateString) {
        var birthDate = new Date(dateString);
        var age = today.getFullYear() - birthDate.getFullYear();
        var m = today.getMonth() - birthDate.getMonth();
        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }
        //var daysTo15;
        //if (age == 15) {
            
        //    daysTo15 = DaysFrom15(today, birthDate);

        //    switch (true) {
        //        case (daysTo15 >= 365):
        //            //Waiver needs to be signed (on-site by parent)
        //            //Certificate cannot be used until person is age 15
        //            //Speak to parent/guardian before registration is accepted
        //            //Flag it on the report
        //            alert(daysTo15);
        //            break;

        //        case (daysTo15 < 365):
        //            //Waiver needs to be signed (on-site by parent)
        //            //Certificate cannot be used until person is age 15

        //            break;
        //    }
        //}
        //else if(age > 15 && age < 18) {
        //    //Waiver needs to be signed (on-site by parent)
        //}

        $('#AGE').val(age);

        return age;
    }

    //set initial age (so the value will not be null)
    //GetAge(maxBirthDate);

    /* REGISTRATION ACTIONS */
    $('#RegistrantForm').addClass('hidden');
    //if ($('#CLASS_TYPE_REG').val() === 'Basic RiderCourse I') {
    //    $('#RegistrantForm').addClass('hidden');
    //} else {
    //    $('#SurveyForm').addClass('hidden');
    //    // Remove required attribute because we are hiding SurveryFrom
    //    $("input[name='chkBicycle']").removeAttr('required');
    //    $("input[name='chkECourse']").removeAttr('required');
    //    $("input[name='chkKnowledgeTest']").removeAttr('required');
    //    $("input[name='chkGear']").removeAttr('required');
    //    $("input[name='chkPaymentAgreement']").removeAttr('required');
    //    $("input[name='chkCommunication']").removeAttr('required');
    //}

    $('#CheckoutForm').addClass('hidden');
    $('#loader').addClass('hidden');
    $("#SurveyErrors").addClass('hidden');

    var noSpecialChars = /[!@,#$%\^&\*\(\)\/\\\}\{\]\[\-]/; //no special characters allowed
    var zip5 = /^([0-9]{5,5})$/; //5 numbers only
    var phone10 = /^([0-9]{10,10})$/; //10 numbers only
    var ph_ext6 = /^([0-9]{1,6})$/; //6 numbers only
    var emailReg = /^([a-zA-Z0-9_\-\.]+)@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$/;
    var VisaccReg = /^4[0-9]{12}(?:[0-9]{3})?$/; //visa credit card
    var MCccReg = /^5[1-5][0-9]{14}$/; // master card credit card
    var DSccReg = /^6(?:011\d\d|5\d{4}|4[4-9]\d{3}|22(?:1(?:2[6-9]|[3-9]\d)|[2-8]\d\d|9(?:[01]\d|2[0-5])))\d{10}$/; // discover card credit card
    var cvv3 = /^([0-9]{3,3})$/; //3 numbers only
    var num2 = /^([0-9]{2,2})$/; //2 numbers only
    //var num4 = /^([0-9]{4,4})$/; //4 numbers only

    // FORM INPUT VALIDATION FUNCTIONS
    window.ValidateAddress = function (callback) {
        a = $('#inputAddressLine1').val()
        c = $('#inputCity').val()
        s = $('#inputState').val();
        z = $('#inputZip').val();

        var url = "../AddressValidation"
        var parms = "{a:'" + a + "',c:'" + c + "',s:'" + s + "',z:'" + z + "'}";


        $.ajax({
            type: "POST",
            url: url,
            data: parms,
            contentType: 'application/json; charset=utf-8',
            dataType: "xml",
            success: function (xml) {
                //console.log("Response: " + xml);

                var address = "";
                var city = "";
                var state = "";
                var zip5 = 0;
                var zip4 = 0;
                var errNum = 0;
                var errDesc;
                var returnValue = "Zip code could not be found.";

                // FIND ERRORS IF ANY
                $(xml).find('Error').each(function () {
                    errNum = $(this).find('Number').text();
                    errDesc = $(this).find('Description').text();
                    console.log(errDesc);
                });

                if (errNum == 0) {
                    $(xml).find('Address').each(function () {
                        address = $(this).find('Address2').text();
                        city = $(this).find('City').text();
                        state = $(this).find('State').text();
                        zip5 = $(this).find('Zip5').text();
                        zip4 = $(this).find('Zip4').text();
                        returnValue = zip5;
                    });

                } else {
                    returnValue = errNum + ' ' + errDesc;
                    $('#USPSerror').html(returnValue);
                    $('#BadAddressValues').html(a + '<br/>' + c + ' ' + s + ' ' + z);

                    $('#InvalidAddress').modal({
                        backdrop: false,
                        keyboard: false,
                        show: true
                    });

                    return false;
                }
                callback(returnValue);
            }
        });
    }

    $('#InvalidAddress').on('hidden.bs.modal', function (e) {
        $('#inputAddressLine1').focus();

    });


    window.FN = function () {
        var isValid;
        var input = $('#inputFirstName').val();
        if (input.length > 0) {
            if (noSpecialChars.test(input)) {
                isValid = false;
            } else {
                isValid = true;
            }
        } else {
            isValid = false;
        };

        // Add UX feedback for users
        if (isValid == false) {
            $('#form_FN').addClass('has-error');
            $('#inputFirstName').focus();
        } else {
            $('#form_FN').removeClass('has-error');

            $('#inputFirstName').val($('#inputFirstName').val().toUpperCase());

        }
        return isValid;
    }
    window.LN = function () {
        var isValid;
        var input = $('#inputLastName').val();
        if (input.length > 0) {
            if (noSpecialChars.test(input)) {
                isValid = false;
            } else {
                isValid = true;
            }
        } else {
            isValid = false;
        }

        if (isValid == false) {
            $('#form_LN').addClass('has-error');
            $('#inputLastName').focus();
        } else {
            $('#form_LN').removeClass('has-error');
            $('#inputLastName').val($('#inputLastName').val().toUpperCase());
            $('#inputMiddleName').val($('#inputMiddleName').val().toUpperCase());
        }
        return isValid;
    }
    window.ADDR = function () {
        var isValid;
        var input = $('#inputAddressLine1').val();
        if (input.length > 0) {
            if (noSpecialChars.test(input)) {
                isValid = false;
            } else {
                isValid = true;
            }
        } else {
            isValid = false;
        }

        if (isValid == false) {
            $('#form_ADDR').addClass('has-error');
            $('#inputAddressLine1').focus();
        } else {
            $('#form_ADDR').removeClass('has-error');
            $('#inputAddressLine1').val($('#inputAddressLine1').val().toUpperCase());
        }
        return isValid;
    }
    window.CITY = function () {
        var isValid;
        var input = $('#inputCity').val();
        if (input.length > 0) {
            if (noSpecialChars.test(input)) {
                isValid = false;
            } else {
                isValid = true;
            }
        } else {
            isValid = false;
        }

        if (isValid == false) {
            $('#form_CITY').addClass('has-error');
            $('#inputCity').focus();
        } else {
            $('#form_CITY').removeClass('has-error');
            $('#inputCity').val($('#inputCity').val().toUpperCase());
            $('#inputAddressLine2').val($('#inputAddressLine2').val().toUpperCase()); // put this here because city is the first validation after the address
        }
        return isValid;
    }
    window.ST = function () {
        var isValid;
        var input = $('#inputState').val();
        if (input.length == 0) {
            isValid = false;
        } else {
            isValid = true;
        }

        if (isValid == false) {
            $('#form_ADDR_ST').addClass('has-error');
            $('#inputState').focus();
        } else {
            $('#form_ADDR_ST').removeClass('has-error');
        }
        return isValid;
    }
    window.ZIP = function () {
        var isValid;
        var input = $('#inputZip').val();
        if (input.length == 5) {
            if (!zip5.test(input)) {
                isValid = false;
            } else {
                isValid = true;
            }
        } else {
            isValid = false;
        }

        if (isValid == false) {
            $('#form_ZIP').addClass('has-error');
            $('#inputZip').focus();
        } else {
            $('#form_ZIP').removeClass('has-error');
        }
        return isValid;
    }
    window.GN = function () {
        var isValid;
        var input = document.getElementsByName("Gender");
        if (input[0].checked == true) {
            isValid = true;
        } else if (input[1].checked == true) {
            isValid = true;
        } else {
            isValid = false;
        }

        if (isValid == false) {
            $('#form_GN').addClass('has-error');
            $('#inputGender').focus();
        } else {
            $('#form_GN').removeClass('has-error');
        }
        return isValid;
    }
    window.RACE = function () {

        // No need to capture race information.

        //var isValid;
        //var input = $('#inputRace').val();
        //if (input.length == 0) {
        //    $('#inputRace').val(8);
        //}

        //if (isValid == false) {
        //    $('#form_RC').addClass('has-error');
        //    $('#inputRace').focus();
        //} else {
        //    $('#form_RC').removeClass('has-error');
        //}
        //return isValid;
    }

    window.PHONE = function () {
        var isValid;
        var input = $('#inputPhone').val();
        if (!phone10.test(input)) {
            isValid = false;
        } else {
            isValid = true;
        }

        if (isValid == false) {
            $('#form_PN').addClass('has-error');
            $('#inputPhone').focus();
        } else {
            $('#form_PN').removeClass('has-error');
        }
        return isValid;
    }
    window.EMAIL = function () {
        var isValid;
        var input = $('#inputEmail').val();
        if (input.length > 0) {
            if (!emailReg.test(input)) {
                isValid = false;
            } else {
                isValid = true;
            }
        } else {
            isValid = false;
        }

        if (isValid == false) {
            $('#form_EM').addClass('has-error');
            $('#inputEmail').focus();
        } else {
            $('#form_EM').removeClass('has-error');
            $('#inputEmail').val($('#inputEmail').val().toLowerCase());
        }
        return isValid;
    }

    window.EMAILMATCH = function () {
        var isValid;
        var input = $('#inputEmail').val();
        var input2 = $('#inputEmail2').val();
        if (input2 != input) {
            isValid = false;
        } else {
            isValid = true;
        }

        if (isValid == false) {
            $('#form_EM2').addClass('has-error');
            $('#inputEmail2').focus();
            $('#mail_alert').removeClass('hidden');
            //$('#waiver_alert').html('The date you entered is not valid. Enter the value like this: 2000-01-01 (YEAR-MONTH-DAY)');

        } else {
            $('#form_EM2').removeClass('has-error');
            $('#inputEmail2').val($('#inputEmail2').val().toLowerCase());
            $('#mail_alert').addClass('hidden');
        }

        return isValid;
    }

    window.DOB = function () {
        var isValid;
        var age;
        var input = $('#inputDOB').val();
        var parse = Date.parse(input);
        if (isNaN(parse.valueOf())) {
            isValid = false;
        } else {
            isValid = true;
            age = GetAge(input); //get registrants age
        }

        if (isValid == false) {
            $('#form_DOB').addClass('has-error');
            $('#inputDOB').focus();

            $('#waiver_alert').removeClass('hidden');
            $('#waiver_alert').html('The date you entered is not valid. Enter the value like this: 2000-01-01 (YEAR-MONTH-DAY)');

        } else {
            $('#form_DOB').removeClass('has-error');

            if (age <= 17) {
                $('#waiver_alert').removeClass('hidden');
                $('#waiver_alert').html('A parent will need to sign a waiver since you are under 18.');
            } else {
                $('#waiver_alert').addClass('hidden');
                $('#waiver_alert').html('&nbsp;');

            }



        }
        return isValid;
    }

    /* 01.21.2020
     * IF THE CLASS IS Basic RiderCourse II
     * WILL SET A PROMO CODE TO REDUCE THE PRICE OF THE CLASS BY $100
     * */
    window.EVAL_TF = function () {
        var isValid;
        if ($('#CLASS_TYPE_REG').val() === 'Basic RiderCourse II') {
          var input = document.getElementsByName("Eval");
            if (input[0].checked == true) {
                isValid = true;

                // REMOVE PROMO CODE IF A LICENSE IS NEEDED
                $('#inputPromo').val('');
            } else if (input[1].checked == true) {
                isValid = true;

                // SET PROMO CODE IF A LICENSE IS NOT NEEDED
                $('#inputPromo').val('Already Licensed');
            } else {
                isValid = false;
            }

            if (isValid == false) {
                $('#form_EV').addClass('has-error');
                $('#inputEval').focus();

                // REMOVE PROMO CODE IF A LICENSE IS NEEDED
                $('#inputPromo').val('');

            } else {
                $('#form_EV').removeClass('has-error');
            }
        } else {
            isValid = true;
        }
        return isValid;
    }

    window.MOTOR = function () {
        var isValid;
        var inputCLS = $('#CLASS_TYPE_REG').val();
        if (inputCLS == 'Basic RiderCourse II' || '3 Wheel (Trike) Course') {
            var radioValue = $("input[name='Eval']:checked").val();
            if (radioValue) { // radio is checked
                $('#form_EV').removeClass('has-error');
            } else { //nothing checked
                isValid = false;
                $('#form_EV').addClass('has-error');

                return isValid;
            }
        }

        var inputYR = $('#inputMotorYR').val();
        var inputYR = $('#inputMotorMK').val();
        var inputYR = $('#inputMotorMD').val();
        //if (inputCLS == 'Basic RiderCourse I') {

        //    if (inputYR.length <= 0) {
        //        isValid = false;
        //        $('#inputMotorYR').focus();
        //    } else if (inputMK.length <= 0) {
        //        isValid = false;
        //        $('#inputMotorMK').focus();
        //    } else if (inputMD.length <= 0) {
        //        isValid = false;
        //        $('#inputMotorMD').focus();
        //    }


        //    if (isValid == false) {
        //        $('#form_MC_TYPE').addClass('has-error');

        //    } else {
        //        $('#form_MC_TYPE').removeClass('has-error');

        //    }
        //} else {
        //    isValid = true;
        //};
        return isValid;
    }

    window.DL_NU = function () {
        var isValid;
        var input = $('#inputDLNumber').val();
        if (input.length == 0) {
            isValid = false;
        } else {
            isValid = true;
        }

        if (isValid == false) {
            $('#form_DL_NUM').addClass('has-error');
            $('#inputDLNumber').focus();
        } else {
            $('#form_DL_NUM').removeClass('has-error');
            $('#inputDLNumber').val($('#inputDLNumber').val().toUpperCase());
        }
        return isValid;
    }
    window.DL_ST = function () {
        var isValid;
        var input = $('#inputDLState').val();
        if (input.length == 0) {
            isValid = false;
        } else {
            isValid = true;
        }

        if (isValid == false) {
            $('#form_DL_ST').addClass('has-error');
            $('#inputDLState').focus();
        } else {
            $('#form_DL_ST').removeClass('has-error');
        }
        return isValid;
    }

    window.ValidateRegistrant = function () {
        var isValid = true;

        if (FN() == false) { return false; }; //First Name validation
        if (LN() == false) { return false; }; //Last Name validation
        if (ADDR() == false) { return false; }; //Address validation
        if (CITY() == false) { return false; }; //City validation
        if (ST() == false) { return false; }; //State validation
        if (ZIP() == false) { return false; }; //Zip validation
        if (GN() == false) { return false; }; //Gender validation
        if (RACE() == false) { return false; }; //Race validation - make sure a value is entered if left blank
        if (PHONE() == false) { return false; }; //Phone validation
        if (EMAIL() == false) { return false; }; //Email validation
        if (EMAILMATCH() == false) { return false; }; //Email match validation
        if (DOB() == false) { return false; }; //Birth date validation
        if (MOTOR() == false) { return false; }; //Motorcycle validation
        //if (DL_ST() == false) { return false; }; //Drivers License state validation

        return isValid;
    };

    // The checkout button was clicked
    $('#btn_Checkout').click(function (e) {
        e.preventDefault();

        // Validate the registration form
        var v = ValidateRegistrant();
        if (v == true) {
            ValidateAddress(function (r) {
                //console.log(r);

                $('#inputPaymentType').focus();

                $('#RegistrantForm').addClass('hidden'); // Hide the registration information
                $('#CheckoutForm').removeClass('hidden'); // Show the payment information
            });
            } else {
            //alert('Correct Errors');
            return false;
        }
    });

    // Validate inputs when user exits an input
    $('#inputFirstName').blur(function () { FN(); });
    $('#inputLastName').blur(function () { LN(); });
    $('#inputAddressLine1').blur(function () { ADDR(); });
    $('#inputCity').blur(function () { CITY(); });
    $('#inputState').change(function () { ST(); });
    $('#inputZip').blur(function () { ZIP(); });

    /* CHANGED ON 3/4/2017 - NOT NEEDED TO BE REQUIRED */
    //$('#inputRace').change(function () { RACE(); });
    $('#inputEmail').blur(function () { EMAIL(); });
    $('#inputEmail2').blur(function () { EMAILMATCH(); });
    $('#inputDOB').blur(function () { DOB(); });

    /*
     ADDED ON 01.21.2020
     WILL ACTIVATE WHEN THE MOTORCYCLE LICENSE QUESTION IS ANSWERED
     FOR ONLY BASIC RIDERCOURSE II CLASSES
     */
    $('#inputEvalNo').click(function () { EVAL_TF(); });
    $('#inputEvalYes').click(function () { EVAL_TF(); });

 //$('#inputDLNumber').blur(function () { DL_NU(); });
    //$('#inputDLState').change(function () { DL_ST(); });

    // Validate the payment fields for the registration
    window.PTPE = function () {
        var isValid;
        var input = $('#inputPaymentType').val();
        if (input.length == 0) {
            isValid = false;
        } else {
            isValid = true;
        }

        if (isValid == false) {
            $('#form_PMT_TPE').addClass('has-error');
            $('#inputPaymentType').focus();
        } else {
            $('#form_PMT_TPE').removeClass('has-error');
        }
        return isValid;

    }
    window.CARD_NAME = function () {
        var isValid;
        var input = $('#inputCardName').val();
        if (input.length > 0) {
            if (noSpecialChars.test(input)) {
                isValid = false;
            } else {
                isValid = true;
            }
        } else {
            isValid = false;
        };

        // Add UX feedback for users
        if (isValid == false) {
            $('#form_CD_NM').addClass('has-error');
            $('#inputCardName').focus();
        } else {
            $('#form_CD_NM').removeClass('has-error');
            $('#inputCardName').val($('#inputCardName').val().toUpperCase());
        }
        return isValid;
    }
    window.CARD_NUM = function () {
        var isValid;
        var input = $('#inputCardNum').val();
        var payType = $('#inputPaymentType').val();

        if (input.length > 0) {
            // Visa Verification
            if (payType == 'Visa') {
                if (!VisaccReg.test(input)) {
                    isValid = false;
                } else {
                    isValid = true;
                }
            };
            // Master Card Verification
            if (payType == 'Master Card') {
                if (!MCccReg.test(input)) {
                    isValid = false;
                } else {
                    isValid = true;
                }
            };
            // Discover Verification
            if (payType == 'Discover') {
                if (!DSccReg.test(input)) {
                    isValid = false;
                } else {
                    isValid = true;
                }
            }
            // Gift Card Verification
            if (payType == 'Gift') {
                if (input.length <= 1) {
                    isValid = false;
                } else {
                    isValid = true;
                }
            };

        } else {
            isValid = false;
        };

        // Add UX feedback for users
        if (isValid == false) {
            $('#form_CD_NU').addClass('has-error');
            $('#inputCardNum').focus();
        } else {
            $('#form_CD_NU').removeClass('has-error');
        }
        return isValid;
    }
    window.EXP_MN = function () {
        var isValid;
        var input = $('#inputCardMonth').val();
        if (!num2.test(input)) {
            isValid = false;
        } else {
            isValid = true;
        };

        // Add UX feedback for users
        if (isValid == false) {
            $('#form_CD_EXP').addClass('has-error');
            $('#inputCardMonth').focus();
        } else {
            $('#form_CD_EXP').removeClass('has-error');
        }
        return isValid;
    }
    window.EXP_YR = function () {
        var isValid;
        var input = $('#inputCardYear').val();
        var isNum = parseInt(input, 10);
        if (isNaN(isNum)) {
            isValid = false;
        } else {
            if (isNum < currentYear) {
                isValid = false;
            } else {
                isValid = true;
            }
        };

        // Add UX feedback for users
        if (isValid == false) {
            $('#form_CD_EXP').addClass('has-error');
            $('#inputCardYear').focus();
        } else {
            $('#form_CD_EXP').removeClass('has-error');
        }
        return isValid;
    }
    window.CVV = function () {
        var isValid;
        var input = $('#inputCardCVV').val();
        var isNum = parseInt(input, 10);
        if (input.length == 3) {
            if (!cvv3.test(input)) {
                isValid = false;
            } else {
                isValid = true;
            }
        } else {
            isValid = false;
        };

        // Add UX feedback for users
        if (isValid == false) {
            $('#form_CD_CVV').addClass('has-error');
            $('#inputCardCVV').focus();
        } else {
            $('#form_CD_CVV').removeClass('has-error');
        }
        return isValid;
    }

    window.ValidatePayment = function () {
        var isValid = true;
        var payType = $('#inputPaymentType').val();

        if (payType == 'Pre-Paid') {

        }
        else if (payType == 'Gift') { // Payment is a gift certificate

            if (CARD_NAME() == false) { return false; }; //Card name validation
            if (CARD_NUM() == false) { return false; }; //Card number validation

        } else if (payType != 'Check') { // Payment is a credit card
            if (CARD_NAME() == false) { return false; }; //Card name validation
            if (CARD_NUM() == false) { return false; }; //Card number validation
            if (EXP_MN() == false) { return false; }; //Expiration month validation
            if (EXP_YR() == false) { return false; }; //Expiration year validation
            if (CVV() == false) { return false; }; //CVV validation
        }


        return isValid;
    };

    $('#inputPaymentType').change(function (e) {
        if (PTPE() == false) { return false; }; //Payment Type validation

        //$('#inputCardName').val($('#inputFirstName').val() + ' ' + $('#inputLastName').val());
        $('#inputCardName').focus();

        if ($(this).val() == 'Check') {
            $('#inputCardName').val('');

            $('#CheckNote').removeClass('hidden');
            $('#CardFields').addClass('hidden');
            $('#CC_Fields').addClass('hidden');
            $('#PROMO_FIELDS').removeClass('hidden'); //Hide credit card only fields
        }
        else if ($(this).val() == 'Pre-Paid') {
            $('#CheckNote').addClass('hidden'); // Hide check note
            $('#CC_Fields').addClass('hidden'); //Hide credit card only fields
            $('#PROMO_FIELDS').addClass('hidden'); //Hide credit card only fields
            $('#CardFields').removeClass('hidden'); // Show card name/number fields
        }
        else if ($(this).val() == 'Gift') {
            $('#CheckNote').addClass('hidden'); // Hide check note
            $('#CC_Fields').addClass('hidden'); //Hide credit card only fields
            $('#PROMO_FIELDS').addClass('hidden'); //Hide credit card only fields
            $('#CardFields').removeClass('hidden'); // Show card name/number fields
        }
        else {
            $('#CheckNote').addClass('hidden'); // Hide check note
            $('#CardFields').removeClass('hidden'); // Show card name/number fields
            $('#CC_Fields').removeClass('hidden'); // Show credit card only fields
            $('#PROMO_FIELDS').removeClass('hidden'); //Hide credit card only fields
        }

        //ValidatePayment();
    });
    $('#inputCardYear').attr("min", currentYear);
    $('#inputCardYear').attr("max", currentYear + 15);

    $('#inputCardName').blur(function () { CARD_NAME(); });
    $('#inputCardNum').blur(function () { CARD_NUM(); });
    $('#inputCardMonth').blur(function () { EXP_MN(); });
    $('#inputCardYear').blur(function () { EXP_YR(); });
    $('#inputCardCVV').blur(function () { CVV(); });

    // The Submit Payment button was clicked
    $('#CourseRegForm').on("submit", function (e) {
        e.preventDefault();
        var v = ValidatePayment();

        if (v == true) {
            SubmitRegistration(function (r) {
                if (r.length >= 20) {
                    console.log(r);
                } else {
                    ShowRegistrationConfirmation(r);
                    /*
                    $('#RegSuccess').modal({
                        backdrop: false,
                        keyboard: false,
                        show: true
                    });
                    
                    $('#regNum').val(r);
                    */
                }
            });
        };
    });

    window.SubmitRegistration = function (callback) {
        var form = $('#CourseRegForm');
        var parameters = form.serialize();
        $('#loader').removeClass('hidden');
        $.ajax({
            type: "POST",
            url: "../NewClassRegistration",
            data: parameters,
            //contentType: "application/json; charset=utf-8",
            success: function (list) {
                callback(list);
                $('#loader').addClass('hidden');
            }
        });

    }

    window.ShowRegistrationConfirmation = function (r) {
        //console.log("Registration Confirmed: " + $('#regNum').val());
        $('#CourseRegForm').get(0).reset();

        window.location.href = '../RegistrationConfirmation/' + r;
    }

    $('#VerifyPromo').click(function () {
        var p = $('#inputPromo').val();
        if (p == '') {
            //known invalid code entered
        } else {
            // check the code entered for a valid code in the database
            VerifyPromoCode(p, function (r) {
                if (r == 'false') {
                    // code was invalid
                    $('#PromoAlert').removeClass('hidden');
                } else if (r != 'True') {
                    $('#PromoAlert').removeClass('hidden');
                    $('#PromoAlert').html(r);
                }
                else if(r=='True') {
                    // code was valid
                    $('#PromoAlert').removeClass('hidden');
                    $('#PromoAlert').html("Promo code has been accepted");
                }
            })
        }
    });

    window.VerifyPromoCode = function (p, callback) {
        var parameters = "{code:'" + p + "'}";
        //console.log(parameters);
        $.ajax({
            type: "POST",
            url: "../VerifyPromoCode",
            data: parameters,
            contentType: "application/json; charset=utf-8",
            success: function (list) {
                callback(list);
            }
        });

    }

    $('#RegSuccess').on('hidden.bs.modal', function (e) {
        ShowRegistrationConfirmation();

    })

    $('#RegConfirm').click(function () { $('#RegSuccess').modal('hide'); });

    // The Cancel payment button was clicked
    $('#CancelPayment').click(function () {
        $('#CheckoutForm').addClass('hidden'); // Hide the payment information
        $('#RegistrantForm').removeClass('hidden'); // Show the payment information
    });
    /*******************/

    /** CONFIRM REGISTRATION **/
    $('#ConfirmReg').on("submit", function (e) {
        e.preventDefault();
        $('#confirm_progress').removeClass('hidden');
        $('#ConfirmReg').addClass('hidden');
        ConfirmRegistration(function (r) {
            if (r == 'Fail') {
                console.log(r);
                $('#confirm_progress').addClass('hidden');
                $('#confirmAlert').removeClass('hidden');
                $('#confirmAlert').html('Your information could not be confirmed at this time, please contact the site administrator.')
                $('#ConfirmReg').removeClass('hidden');
            } else {
                $('#confirm_progress').addClass('hidden');
                $('#confirmAlert').removeClass('hidden');
                $('#confirmAlert').html('Congratulations, your information has been confirmed.')

            }
        });
    });

    $('#btn_SurveySubmit').click(function () {
        $("#SurveyErrors").addClass('hidden');

        if ($("input[name='chkBicycle']:checked").val() &&
            $("input[name='chkECourse']:checked").val() &&
            $("input[name='chkKnowledgeTest']:checked").val() &&
            $("input[name='chkGear']:checked").val() &&
            $("input[name='chkPaymentAgreement']:checked").val() &&
            $("input[name='chkCommunication']:checked").val()) {
            $('#SurveyForm').addClass('hidden'); // Hide survey information
            $('#RegistrantForm').removeClass('hidden'); // Show the registration information
        }
        else {
            $("#SurveyErrors").removeClass('hidden');
        }
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

    /*******************/
});