using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using System.Reflection;
using System.Net.Mail;
using System.Net;

/// <summary>
/// Summary description for _Passenger_Service
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class _Passenger_Service : System.Web.Services.WebService
{

    public class userlist
    {
        public string PassengerID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Gender { get; set; }
        public string MobNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
    }

    public string  GenerateOTP()
    {
        Random r = new Random();
        int randNum = r.Next(1000000);
        string sixDigitNumber = randNum.ToString("D6");
        return sixDigitNumber;
    }
    public void sendMail(string Email,int Otp)
    {
        SmtpClient smtpClient = new SmtpClient();

        NetworkCredential basicCredential = new NetworkCredential("MD@SilverScreenTP.in","SilverScreen@2006");

        MailMessage message = new MailMessage();

        MailAddress fromAddress = new MailAddress("MD@SilverScreenTP.in");

        smtpClient.Host = "host.indiancloudhosting.com";

        smtpClient.Port = 587;

        smtpClient.EnableSsl = false;

        smtpClient.UseDefaultCredentials = false;

        smtpClient.Credentials = basicCredential;

        message.From = fromAddress;
        message.Subject ="Email Verification";
        //Set IsBodyHtml to true means you can send HTML email.
        message.IsBodyHtml = true;
        message.Body = "Verification Code for your Email  :  "+Otp;
        message.To.Add(Email);

        try
        {
            smtpClient.Send(message);
        }
        catch (Exception ex)
        {
            //Error, could not send the message
            
        }

    }
    [WebMethod]
    public string  SaveUser(userlist ph)
    {

        #region
        int otp =Convert.ToInt32( GenerateOTP());
        string msg = "";
        try
        {

           
            string sqlquery = "";
            sqlquery = "INSERT INTO  Tbl_Passenger  ([FName],[LName],[Email],[PhoneNo],[Gender],[Password],[OTP],[Flag])";
            sqlquery = sqlquery + " Values ('"+ph.FName+"','"+ph.LName+"','"+ph.Email+"','"+ph.MobNo+"','"+ph.Gender+"','"+ph.Password+"',"+otp+",0) ";
            int rsult = CommonClassFile.InsertOrUpdateorDelete(sqlquery);

            sendMail(ph.Email, otp);

            msg = ph.FName + " " + ph.LName + " details Registered Successfully. to activate your Account Please Check your Email Regarding your Verificaiton Code";
        }
        catch(Exception ex)
        {
            throw ex;
        }
        finally
        {

        }
        return msg;
        #endregion
    }

    [WebMethod]
    public string VerifyAccount(string Email,int OTP)
    {
        #region
        string msg = "";
        try
        {
            string check_existing_Otp = CommonClassFile.GetSingleValue("Select Otp from Tbl_Passenger where Flag=0 and Email='" + Email + "' and Otp="+ OTP + " ");

            if(Convert.ToInt32(check_existing_Otp) == OTP)
            {
                int result = CommonClassFile.InsertOrUpdateorDelete("Update Tbl_Passenger SET Flag=1,Otp=0 where Email='"+Email+"' ");
                msg = "Your Email "+ Email + " Account is Verified Successfully";
            }
        }
        catch(Exception ex)
        {

        }
        return msg;
        #endregion
    }

    [WebMethod(EnableSession = true)]
    public string Login(string Email,string Password) {
        #region
        string status = string.Empty;


        DataTable dt = CommonClassFile.SelectTable("Exec Spr_PassengerLogin  '" + Email + "','" + Password + "' ");
        if (dt.Rows.Count > 0)
        {

            status = dt.Rows[0]["Column1"].ToString();
            if (status == "1")
            {
                 
                DataTable dtlogin = CommonClassFile.SelectTable("Select PassengerID,FName,LName from Tbl_Passenger where   Email='" + Email + "' and Password='" + Password + "'");
                if (dtlogin.Rows.Count > 0)
                {
                    HttpContext.Current.Session["UserName"] = dtlogin.Rows[0]["FName"].ToString()+" "+ dtlogin.Rows[0]["LName"].ToString();
                    
                    HttpContext.Current.Session["UserID"] = dtlogin.Rows[0]["PassengerID"].ToString();

                }
            }
            else
            {
                status = "0";
            }


        }


        return status;
        #endregion
    }

    [WebMethod(EnableSession = true)]
    public string GetUserID()
    {
        string returnstring = "";

        try
        {
            if (HttpContext.Current.Session["UserID"] != null)
            {
                returnstring = Convert.ToString(HttpContext.Current.Session["UserID"]);
            }
            else
            {

            }

        }
        catch (Exception ex)
        {
            returnstring = ex.Message;
        }

        return returnstring;
    }

    [WebMethod(EnableSession = true)]
    public string GetUserName()
    {
        string returnstring = "";

        try
        {
            if (HttpContext.Current.Session["UserName"] != null)
            {
                returnstring = Convert.ToString(HttpContext.Current.Session["UserName"]);
            }
            else
            {

            }

        }
        catch (Exception ex)
        {
            returnstring = ex.Message;
        }

        return returnstring;
    }

    [WebMethod(EnableSession = true)]
    public void logout()
    {
        HttpContext.Current.Session.Clear();
        HttpContext.Current.Session.Abandon();
    }
}
