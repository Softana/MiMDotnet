$(function () {
	$('.datepicker').datepicker(
		{
			dateFormat: 'dd-M-yy',
			changeMonth: true,
			changeYear: true,
			minDate: this.all,
			yearRange: '-120:+60',			
		}
	)
});