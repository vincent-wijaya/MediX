var bookings = [];
$(".bookings").each(function () {
    var dateTime = $(".dateTime", this).text().trim();
    var notes = $(".notes", this).text().trim();
    var booking = {
        "dateTime": dateTime,
        "notes": notes
    };
    bookings.push(booking);
});
$("#calendar").fullCalendar({
    locale: 'au',
    bookings: bookings,
    dayClick: function (date, allDay, jsEvent, view) {
        var d = new Date(date);
        var m = moment(d).format("YYYY-MM-DD");
        m = encodeURIComponent(m);
        var uri = "/Bookings/Create?date=" + m;
        $(location).attr('href', uri);
    }
});