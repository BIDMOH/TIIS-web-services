function NextDay(e) {
    var datePicker = $telerik.findControl(document, "RadDatePicker1");
 
    var date = datePicker.get_selectedDate();
 
    if (datePicker.isEmpty())
        date = new Date();
 
    date.setDate(date.getDate() + 1);
    datePicker.set_selectedDate(date);
 
    logEvent("DatePicker.SelectedDate changed to " + date.format("MM/dd/yyyy"));
 
    return false;
}
 
function PrevDay(e) {
    var datePicker = $telerik.findControl(document, "RadDatePicker1");
 
    var date = datePicker.get_selectedDate();
 
    if (datePicker.isEmpty())
        date = new Date();
 
    date.setDate(date.getDate() - 1);
    datePicker.set_selectedDate(date);
    logEvent("DatePicker.SelectedDate changed to " + date.format("MM/dd/yyyy"));
    return false;
 
}
function CancelPopupClose(e) {
    if (e.stopPropagation)
        e.stopPropagation();
    e.cancelBubble = true;
}
 
function setMinDate(minDate) {
    var RadDatePicker1 = $telerik.findControl(document, "RadDatePicker1");
 
    if (minDate.checked) {
        var date1 = new Date(2005, 0, 1);
        RadDatePicker1.set_minDate(date1);
        logEvent("DatePicker.MinDate changed to 2005/01/01");
    }
    else {
        var date1 = new Date(1900, 0, 1);
        RadDatePicker1.set_minDate(date1);
        logEvent("DatePicker.MinDate changed to 1900/01/01");
    }
}
 
function setMaxDate(maxDate) {
    var RadDatePicker1 = $telerik.findControl(document, "RadDatePicker1");
 
    var date1 = new Date(2012, 0, 1);
     
    if (maxDate.checked) {
        RadDatePicker1.set_maxDate(date1);
        logEvent("DatePicker.MaxDate changed to 2012/01/01");
    }
    else {
        var date1 = new Date(2050, 0, 1);
        RadDatePicker1.set_maxDate(date1);
        logEvent("DatePicker.MaxDate changed to 2050/01/01");
    }
}
 
function clearText() {
    var RadDatePicker1 = $telerik.findControl(document, "RadDatePicker1");
 
    RadDatePicker1.clear();
    logEvent("DatePicker value cleared");
}