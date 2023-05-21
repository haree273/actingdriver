$(document).ready(function () {

    display();

    function display() {

        $('#tblBooking tbody').empty();


        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "wbsvc/_bookingService.asmx/GetAllBookingList",
            data: "{}",
            dataType: "json",
            success: function (data) {
                if (data.d.length > 0) {


                    for (var i = 0; i < data.d.length; i++) {

                        html = "<tr>";
                        html = html + "<td><input type='hidden' id='hfBookD'  value='" + data.d[i].BookingID + "'> " + data.d[i].BookingID + "</td>";

                        html = html + "<td>" + data.d[i].BookingDate.replace(" 00:00:00", "") + " " + data.d[i].BookingTime + "</td>";
                        html = html + "<td>P00" + data.d[i].PassengerID + " - " + data.d[i].PassengerName + "</td>";
                        html = html + "<td>" + data.d[i].PGender + "</td>";
                        html = html + "<td>" + data.d[i].PContactNo + "</td>";
                        html = html + "<td>D00" + data.d[i].DriverID + " -" + data.d[i].DriverName + "</td>";
                        html = html + "<td>" + data.d[i].DContactNo + "</td>";
                        html = html + "<td>" + data.d[i].Source + "</td>";
                        html = html + "<td>" + data.d[i].Destination + "</td>";
                        html = html + "<td>" + data.d[i].Km_to_Travel + "</td>";
                        html = html + "<td>" + data.d[i].Travel_Charge + "</td>";
                        if (data.d[i].Status == "Pending") {
                            html = html + "<td><span style='color:blue;font-weight:900;'><b> " + data.d[i].Status + "</b> </span></td>";
                            html = html + "<td><button type='button' class='btn btn-success btn-sm Approve'>Approve</button>  |  <button type='button' class='btn btn-danger btn-sm Reject'>Reject</button></td>";
                        }
                        else if (data.d[i].Status == "Approved") {
                            html = html + "<td><span style='color:green;font-weight:900;'><b> " + data.d[i].Status + " </b> </span></td>";
                            html = html + "<td> <button type='button' class='btn btn-danger btn-sm Reject'>Reject</button></td>";
                        }
                        else if (data.d[i].Status == "Rejected") {
                            html = html + "<td><span style='color:red;font-weight:900;'><b> " + data.d[i].Status + " </b> </span></td>";
                            html = html + "<td></td>";
                        }



                        html = html + "</tr>";

                        $("#tblBooking tbody").append(html);
                    }
                }
                else {
                    $("#tblBooking tbody").append("<tr ><td style='text-align:center;' colspan='9'><span style='color:red;font-weight:600;'>No Record Found...!!!</span></td></tr>");
                }
            },
            error: function (result) {
                alert("Error");
            }
        });

    }

    $("#tblBooking").on('click', '.Approve', function () {
        // get the current row
        var currentRow = $(this).closest("tr");

        var BookingID = currentRow.find("td:eq(0)").text(); // get current row 1st TD value
        var bookdate = currentRow.find("td:eq(1)").text();
        var Passenger = currentRow.find("td:eq(2)").text();
        $('#hfPName').val(Passenger);
        $('#lblHed').text("Approval");
        $('#lblHed').css('color', 'green');
        $('#hfBookingId').val(BookingID);
        $('.mymsg').text('Are you sure want to Approve this ( ' + Passenger + ' - ' + bookdate + ' ) Booking Details ');
        $('.mymsg').css('color', 'green');
        $('#btnReject').hide();
        $('#btnApprove').show();
        $('#myModal').modal({
            backdrop: 'static',
            keyboard: true
        });
        $('#myModal').modal('show');

    });

    $("#tblBooking").on('click', '.Reject', function () {
        // get the current row
        var currentRow = $(this).closest("tr");

        var BookingID = currentRow.find("td:eq(0)").text(); // get current row 1st TD value
        var bookdate = currentRow.find("td:eq(1)").text();
        var Passenger = currentRow.find("td:eq(2)").text();
        $('#hfPName').val(Passenger);
        $('#lblHed').text("Reject");
        $('#lblHed').css('color', 'red');
        $('#hfBookingId').val(BookingID);
        $('.mymsg').text('Are you sure want to Reject this ( ' + Passenger + ' - ' + bookdate + ' ) Booking Details ');
       
        $('#btnReject').show();
        $('#btnApprove').hide();
        $('#myModal').modal({
            backdrop: 'static',
            keyboard: true
        });
        $('#myModal').modal('show');


        //$.ajax({
        //    type: "POST",
        //    contentType: "application/json; charset=utf-8",
        //    url: "wbsvc/_Driver_Service.asmx/ApproveStatus",
        //    data: "{DriverID:'" + DriverID + "'}",
        //    dataType: "json",
        //    success: function (data) {


        //    },
        //    error: function (result) {
        //        alert("Error");
        //    }
        //});

    });

    $('#btnCancel').click(function () {
        window.location.reload(true);
    })

    $('#btnApprove').click(function () {

        var BookingId = $('#hfBookingId').val();
        var Passenger = $('#hfPName').val();
        var Stype = "Approval"
        Update_Booking_Status(Stype, BookingId, Passenger);
    });

    $('#btnReject').click(function () {

        var BookingId = $('#hfBookingId').val();
        var Passenger = $('#hfPName').val();
        var Stype = "Reject";
        Update_Booking_Status(Stype, BookingId, Passenger);
    });

    function Update_Booking_Status(Stype, BookingId, Passenger) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "wbsvc/_bookingService.asmx/Update_Booking_Status",
            data: "{Stype:'" + Stype + "',BookingID:'" + BookingId + "',Passenger:'" + Passenger + "'}",
            dataType: "json",
            success: function (data) {

                alert(data.d);
                window.location.reload(true);

            },
            error: function (result) {
                alert("Error");
            }
        });
    }
})