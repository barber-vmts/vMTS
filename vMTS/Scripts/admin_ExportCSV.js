$(document).ready(function () {
	$('[data-toggle="popover"]').popover();

	/* EXPORT INDIVIDUAL STUDENT RECORDS */
	$("input[type=button").click(function (e) {
		e.preventDefault();
		var exp = $(this).attr('id').substr(0, 4);
		if (exp == "Exp_") {
			$('#ExportProgress').modal({ toggle: true, backdrop: false });
			$('#resultText').html('');
			$('#loading').removeClass('hidden');

			var exportBTN = $(this).attr('name');
			ExportToCSV(exportBTN, function (r) {
				if (r == 'Success') {
					$('#resultText').html('The file has been created, successfully.');

				} else {
					$('#resultText').html(r);
				}

				$('#loading').addClass('hidden');
				$('#resultText').removeClass('hidden');
			});
		} else if (exp == "Emai") {

			$('#loadingEmail').addClass('hidden');
			$('#emailResultText').removeClass('hidden');
			$('#emailResultText').html('');

			//get data-id attribute of the clicked element
			var receiptId = $(this).attr("id").split("_")[1];
			console.log("ReceiptID: ", receiptId);
			$("#receiptId").val(receiptId);
			$('#SendEmailModal').modal({ toggle: true, backdrop: false });
		}
	});

	window.sendEmailConfirm = function() {
		$('#loadingEmail').removeClass('hidden');

		var re = $(':hidden#receiptId').val();
		console.log("Hidden ", re);
		var sendEmailUrl = "/Registration/SendRegistrationEmil/" + re;
		
		$.ajax({
			type: "GET",
			url: sendEmailUrl,
			contentType: "application/json; charset=utf-8",
			success: function (list) {
				$('#emailResultText').html('Email has been sent.');
				$('#loadingEmail').addClass('hidden');
				setTimeout(function () {
					$('#SendEmailModal').modal('hide');
				}, 1500);
				
			}, error: function (request, status, error) {
				$('#emailResultText').html(error);
				$('#loadingEmail').addClass('hidden');
			}
		});
		
	}

	window.ExportToCSV = function (exp, callback) {
		var parameters = "{r: " + exp + "}";

		$.ajax({
			type: "POST",
			url: "ExportToCSV",
			data: parameters,
			contentType: "application/json; charset=utf-8",
			success: function (list) {
				callback(list);
			}
		});

	};

	///* EXPORT CLASS ROSTER */
	//    $('.glyphicon-export').on('click', function (e) {
	//        e.preventDefault();
	//        var exp = $(this).attr('id').substr(0, 9);
	//        if (exp == "ClassExp_") {
	//            $('#ExportProgress').modal({ toggle: true, backdrop: false });
	//            $('#resultText').html('');
	//            $('#loading').removeClass('hidden');

	//            var exportBTN = $(this).attr('name');
	//            ExportClassToCSV(exportBTN, function (r) {
	//                if (r == 'Success') {
	//                    $('#resultText').html('The file has been created, successfully.');

	//                } else {
	//                    $('#resultText').html(r);
	//                }

	//                $('#loading').addClass('hidden');
	//                $('#resultText').removeClass('hidden');
	//            });
	//        }
	//    });

	//    window.ExportClassToCSV = function (exp, callback) {
	//        var parameters = "{COURSE_ID: " + exp + "}";
	//        window.location = '@Url.Action("DownloadAttachment", "PostDetail", new { studentId = 123 })';
	//        $.ajax({
	//            type: "POST",
	//            url: "ExportClassToCSV",
	//            data: parameters,
	//            contentType: "application/json; charset=utf-8",
	//            success: function (list) {
	//                window.location = '@Url.Action("DownloadAttachment", "PostDetail", new { studentId = 123 })';

	//            }
	//        });

	//    };

});