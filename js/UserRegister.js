$(document).ready(function () {


    $('#btnRegister').click(function () {

       
        Register();
    })



    function Register() {

        var Fname = $('#txtFName').val();
        var LName = $('#txtLName').val();
        var Gender = $('#ddlGender').val();
        var Email = $('#txtEmail').val();
        var MobNo = $('#txtMobNo').val();
        var Password = $('#txtPassword').val();
        $('#hfemail').val(Email);
        var user = {};
        user.FName = Fname;
        user.LName = LName;
        user.Gender = Gender;
        user.Email = Email;
        user.MobNo = MobNo;
        user.Password = Password;

        $.ajax({
            type: "POST",
            url: "wbsvc/_Passenger_Service.asmx/SaveUser",
            data: JSON.stringify({ ph: user, }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                alert(data.d);
                $('#myModal').modal({
                    backdrop: 'static',
                    keyboard: true
                });
                $('#myModal').modal('show');
                //VerifyAcoount(Email, Otp);
            },
            error: function (response) { alert(response.responseText); }
        });

    }
    
    $('#btnVerify').click(function () {

        var Email = $('#hfemail').val();
        var Otp = $('#txtOtp').val();

        VerifyAcoount(Email, Otp);
    });

    function VerifyAcoount(Email,Otp) {

       
        $.ajax({
            type: "POST",
            url: "wbsvc/_Passenger_Service.asmx/VerifyAccount",
            data: "{Email:'" + Email + "',OTP:'" + Otp + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                alert(data.d);

                window.location.reload(true);

            },
            error: function (response) { alert(response.responseText); }
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
            url: "wbsvc/_Passenger_Service.asmx/Login",
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
                window.location = "Booking.html";
            }
            else {
                alert('Invalid UserName or Password ...!!!');
            }

        }

    });
})