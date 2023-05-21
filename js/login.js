

$(document).ready(function () {

     

    $('#btnLogin').click(function () {


        var UserName = $('[id*=txtUserId]').val();
        var Paswd = $('[id*=txtPswd]').val();
        
        if (UserName == "") {
            document.getElementById('txtUserId').focus();
            document.getElementById('txtUserId').style.borderColor = "red";
            return UserName;
        }
        else {
            document.getElementById('txtUserId').style.borderColor = "#f1f1f1";
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
            url: "wbsvc/AL.asmx/AdmLogin",
            contentType: "application/json; charset=utf-8",
            data: "{UserName:'" + UserName + "',Password:'" + Paswd + "'}",
            dataType: "json",
            success: onSuccessGetUserId,
            // error: onError,
            cache: false
        });

        function onSuccessGetUserId(data, status) {

            var ChkUsrName = data.d;

            if (ChkUsrName == "1") {
                window.location = "Driver_List.html";
            }
            else {
                alert('Invalid UserName or Password ...!!!');
            }

        }

    });

    
});