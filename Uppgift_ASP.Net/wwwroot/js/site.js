// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('#FirstnName, #LastName, #Email, #Password1, #Password2').bind('keyup', function () {
	if (allFilled()) $('#button').removeAttr('disabled');

});

function allFilled() {
	var filled = true;
	$('body input').each(function () {
		if ($(this).val() == '') filled = false;
		else { $('#button').prop("disabled", true) }
	});
	return filled;
}