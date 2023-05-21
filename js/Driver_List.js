$(document).ready(function () {
    show_loction_list();

    $('#ddlLocation').on('change', function () {
        var SearchText = $('#ddlLocation').val();
        driver_Filter_by_Location(SearchText);

    });

    $(document).on('click', '.bk', function () {

         
        $('.col-md-2 input[type=checkbox]').each(function () {
            if ($(this).is(":checked")) {
                var DriverID =$(this).attr('value');
                $('#hfDriverId').val(DriverID);
                $('#myModalBooking').modal({
                    backdrop: 'static',
                    keyboard: true
                });
                $('#myModalBooking').modal('show');
            }
        });
        

    });

    function driver_Filter_by_Location(SearchText) {

        $('#dvr').empty();


        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "wbsvc/_Driver_Service.asmx/Get_Driverlist_FilterBy_Location",
            data: "{Location:'" + SearchText + "'}",
            dataType: "json",
            success: function (data) {
                if (data.d.length > 0) {

                     
                    for (var i = 0; i < data.d.length; i++) {

                      
                        
                        html = "<div id=" + data.d[i].DriverID + " class='col-md-2'>";

                        html = html + "<div class='panel panel-info'>";

                        html = html + "<div class='panel-heading' style='text-align:center;' ><span> Driver Info  100" + data.d[i].DriverID + " </span> </div>";

                        html = html + "<div class='panel-body' style='border:10px solid #ccdfed;'>";

                       

                        html = html + "<table class='table table-bordered'>";
                        html = html + "<tr><td><i class='glyphicon glyphicon-user' style='color:skyblue;'></i>  " + data.d[i].FName + " " + data.d[i].LName + "</td></tr>";
                        
                        html = html + "<tr><td><span><i class='glyphicon glyphicon-map-marker' style='color:skyblue;'></i>  <b>" + data.d[i].Location + "</b></span></td></tr>";
                        
                        html = html + "<tr><td><i class='glyphicon glyphicon-phone' style='color:brown;'></i> " + data.d[i].MobNo + "</td></tr>";
                        html = html + "</table>";

                       


                        html = html + "</div>";

                        html = html + "<div class='panel-heading' style='text-align:center;'><input type='checkbox' style='height:15px;width:15px;' name='chk' value=" + data.d[i].DriverID + " >   <button type='button'   name='book' class='btn btn-default btn-sm bk'><i class='glyphicon glyphicon-book' style='color:green;'></i>  Book Now</button></div> ";




                        html = html + "</div>";

                        html = html + "</div>";

                        html = html + "</div>";

                        $("#dvr").append(html);
                    }

                     

                    
                }
                else {
                     
                }
            },
            error: function (result) {
                alert("Error");
            }
        });

    }

    function show_loction_list() {
        $.ajax({
            type: "POST",
            url: "wbsvc/_Driver_Service.asmx/getddl_Location",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                var ddlCustomers = $("[id*=ddlLocation]");
                ddlCustomers.empty().append('<option   value="0">--select--</option>');

                $.each(r.d, function () {
                    ddlCustomers.append($("<option></option>").val(this['Value']).html(this['Text']));
                });


            }
        });
    }


    $('#btnCreate').click(function () {
        Savebooking();
    })
    
    function Savebooking() {

        var BookingDate = $('input[name="bookingDate"]').val();
        var BookingTime = $('input[name="bookingTime"]').val();
        var Source        = $('input[name="Source"]').val();
        var Destination   = $('input[name="Destination"]').val();
        var Km_to_Drive   = $('input[name="Km"]').val();
        var Travel_Charge = $('input[name="Amount"]').val();
        var DriverId      = $('#hfDriverId').val();
        var PassengerId   = $('#lblUserId').text();


        if (BookingDate == "") {

            $('input[name="BookingDate"]').focus();
            return BookingDate;
        }
        else if (BookingTime == "") {
            $('input[name="BookingTime"]').focus();
            return BookingTime;
        }
        else if (Source == "") {
            $('input[name="Source"]').focus();
            return Source;
        }
        else if (Destination == "") {
            $('input[name="Destination"]').focus();
            return Destination;
        }
        else if (Km_to_Drive == "") {
            $('input[name="Km"]').focus();
            return Km_to_Drive;
        }
        else if (Travel_Charge == "") {
            $('input[name="Amount"]').focus();
            return Travel_Charge;
        }
        else {

            var Book = {};

            Book.BookingDate = BookingDate;
            Book.BookingTime = BookingTime;
            Book.DriverID = DriverId;
            Book.PassengerID = PassengerId;
            Book.Source = Source;
            Book.Destination = Destination;
            Book.Km_to_Travel = Km_to_Drive;
            Book.Travel_Charge = Travel_Charge;

            $.ajax({
                type: "POST",
                url: "wbsvc/_bookingService.asmx/SaveBooking",
                data: JSON.stringify({ ph: Book, }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    alert(data.d);

                    window.location.reload(true);

                },
                error: function (response) { alert(response.responseText); }
            });

        }
    }
    
     
})