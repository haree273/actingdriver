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
                            
                        }
                        else if (data.d[i].Status == "Approved") {
                            html = html + "<td><span style='color:green;font-weight:900;'><b> " + data.d[i].Status + " </b> </span></td>";
                             
                        }
                        else if (data.d[i].Status == "Rejected") {
                            html = html + "<td><span style='color:red;font-weight:900;'><b> " + data.d[i].Status + " </b> </span></td>";
                            
                        }



                        html = html + "</tr>";

                        $("#tblBooking tbody").append(html);
                    }
                }
                else {
                    $("#tblBooking tbody").append("<tr ><td style='text-align:center;' colspan='12'><span style='color:red;font-weight:600;'>No Record Found...!!!</span></td></tr>");
                }
            },
            error: function (result) {
                alert("Error");
            }
        });

    }
})