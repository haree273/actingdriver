$(document).ready(function () {

    display();

    $('#btnRegister_dvr').click(function () {
        Register();

    });

    function Register() {

        var FName     = $('#txtFName').val();
        var LName     = $('#txtLName').val();
        var Gender    = $('input[name="gender"]').val();
        var MobNo     = $('input[name="txtEmpPhone"]').val();
        var DLNo      = $('#txtDLNo').val();
        var Email     = $('#txtEmail_dvr').val();
        var Location = $('#txtLocation_dvr').val();
        var Password  = $('#txtPassword').val();
        var DLExpDate = $('#txtDlExpDate').val();

        var Driver = {};

        Driver.FName = FName;
        Driver.LName = LName;
        Driver.Gender = Gender;
        Driver.MobNo = MobNo;
        Driver.DLNo = DLNo;
        Driver.Email = Email;
        Driver.Location = Location;
        Driver.Password = Password;
        Driver.DLExpDate = DLExpDate;


        $.ajax({
            type: "POST",
            url: "wbsvc/_Driver_Service.asmx/SaveRecord",
            data: JSON.stringify({ ph: Driver, }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                alert(data.d);

                window.location.reload(true);
                 
            },
            error: function (response) { alert(response.responseText); }
        });

    }

    function display() {

        $('#tblDriver tbody').empty();


        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "wbsvc/_Driver_Service.asmx/GetAllDriverlist",
            data: "{}",
            dataType: "json",
            success: function (data) {
                if (data.d.length > 0) {


                    for (var i = 0; i < data.d.length; i++) {

                        html = "<tr>";
                        html = html + "<td><input type='hidden' id='hfDrvID'  value='" + data.d[i].DriverID + "'> " + data.d[i].DriverID + "</td>";
                       
                        html = html + "<td>" + data.d[i].FName + " " + data.d[i].LName + "</td>";
                        html = html + "<td>" + data.d[i].Gender + "</td>";
                        html = html + "<td>" + data.d[i].MobNo + "</td>";

                        html = html + "<td>" + data.d[i].Email + "</td>";
                        html = html + "<td>" + data.d[i].Location + "</td>";
                        html = html + "<td>" + data.d[i].DLNo + "</td>";
                        html = html + "<td>" + data.d[i].DLExpDate.replace(" 00:00:00", "") + "</td>";
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

                        $("#tblDriver tbody").append(html);
                    }
                }
                else {
                    $("#tblDriver tbody").append("<tr ><td style='text-align:center;' colspan='9'><span style='color:red;font-weight:600;'>No Record Found...!!!</span></td></tr>");
                }
            },
            error: function (result) {
                alert("Error");
            }
        });

    }

    $("#tblDriver").on('click', '.Approve', function () {
        // get the current row
        var currentRow = $(this).closest("tr");

        var DriverID = currentRow.find("td:eq(0)").text(); // get current row 1st TD value
        var DriverName = currentRow.find("td:eq(1)").text();
        $('#hfDriverName').val(DriverName);
        $('#lblHed').text("Approval");
        $('#lblHed').css('color', 'green');
        $('#hfDriverId').val(DriverID);
        $('.mymsg').text('Are you sure want to Approve this ( ' + DriverName + ' ) Driver Details ');
        $('.mymsg').css('color', 'green');
        $('#btnReject').hide();
        $('#btnApprove').show();
        $('#myModal').modal({
            backdrop: 'static',
            keyboard: true
        });
        $('#myModal').modal('show');
         
    });

    $("#tblDriver").on('click', '.Reject', function () {
        // get the current row
        var currentRow = $(this).closest("tr");

        var DriverID = currentRow.find("td:eq(0)").text(); // get current row 1st TD value
        var DriverName = currentRow.find("td:eq(1)").text();
        $('#hfDriverName').val(DriverName);
        $('#lblHed').text("Reject");
        $('#lblHed').css('color', 'red');
        $('#hfDriverId').val(DriverID);
        $('.mymsg').text('Are you sure want to Reject this ( ' + DriverName + ' ) Driver Details ');
        $('.mymsg').css('color', 'red');
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

        var DriverId = $('#hfDriverId').val();
        var DriverName = $('#hfDriverName').val();
        var Stype = "Approval"
        Update_DriverStatus(Stype,DriverId, DriverName);
    });

    $('#btnReject').click(function () {

        var DriverId = $('#hfDriverId').val();
        var DriverName = $('#hfDriverName').val();
        var Stype = "Reject";
        Update_DriverStatus(Stype, DriverId, DriverName);
    });

    function Update_DriverStatus(Stype, DriverId, DriverName) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "wbsvc/_Driver_Service.asmx/Update_Driver_Status",
            data: "{Stype:'" + Stype + "',DriverId:'" + DriverId + "',DriverName:'" + DriverName + "'}",
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

    $('#btnlogin').click(function () {


        var UserName = $('[id*=txtUserName]').val();
        var Paswd = $('[id*=txtPswd]').val();


        if (UserName == "") {
            document.getElementById('txtUserName').focus();
            document.getElementById('txtUserName').style.borderColor = "red";
            return UserName;
        }
        else {
            document.getElementById('txtUserName').style.borderColor = "#f1f1f1";
        }

        if (Paswd == "") {
            document.getElementById('txtPswd').focus();
            document.getElementById('txtPswd').style.borderColor = "red";
            return Paswd;
        }
        else {
            document.getElementById('txtPswd').style.borderColor = "#f1f1f1";
        }






        $.ajax({
            type: "POST",
            url: "wbsvc/_Driver_Service.asmx/Login",
            contentType: "application/json; charset=utf-8",
            data: "{Email:'" + UserName + "',Password:'" + Paswd + "'}",
            dataType: "json",
            success: onSuccessGetUserId,
            // error: onError,
            cache: false
        });

        function onSuccessGetUserId(data, status) {

            var ChkUsrName = data.d;

            if (ChkUsrName == "1") {
                window.location = "driver_dashboard.html";
            }
            else {
                alert('Invalid UserName or Password ...!!!');
            }

        }

    });
})