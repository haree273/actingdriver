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
/// Summary description for _bookingService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class _bookingService : System.Web.Services.WebService
{

    public class bookingList
    {
        public string BookingID { get; set; }
        public string BookingDate { get; set; }
        public string BookingTime { get; set; }
        public string DriverID { get; set; }
        public string DriverName { get; set; }
        public string DContactNo { get; set; }
        public string PassengerID { get; set; }
        public string PassengerName { get; set; }
        public string PContactNo { get; set; }
        public string PGender { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Km_to_Travel { get; set; }
        public string Travel_Charge { get; set; }
        public string Status { get; set; }

    }

    [WebMethod(EnableSession = true)]
    public string SaveBooking(bookingList ph)
    {
        #region
        string msg = "";
        try
        {
            string sqlquery = "  DECLARE @BookID Bigint; SET @BookID = isnull(((SELECT max(BookID) FROM Tbl_Booking) + 1),'1') ";
            sqlquery = sqlquery + " INSERT INTO [dbo].[Tbl_Booking] ([BookID],[BookDate],[BookTime],[DriverID],[PassengerID],[Source],[Destination],[Km_To_Travel],[Travel_Charge],[Status],[Flag]) ";
            sqlquery = sqlquery + " Values (@BookID,'" + ph.BookingDate + "','" + ph.BookingTime + "'," + ph.DriverID + "," + ph.PassengerID + ",'" + ph.Source + "','" + ph.Destination + "','" + ph.Km_to_Travel + "','" + ph.Travel_Charge + "','Pending',0 ) ";

            int result = CommonClassFile.InsertOrUpdateorDelete(sqlquery);
            string bokid = CommonClassFile.GetSingleValue("Select MAX(BookID) from Tbl_Booking where PassengerID=" + ph.PassengerID + " ");
            string PassengerName = CommonClassFile.GetSingleValue("select FName+''+LName as PassengerName from tbl_Passenger where PassengerID=" + ph.PassengerID + " ");
            string driveremail = CommonClassFile.GetSingleValue("Select Email from tbl_Driver where  DriverID in (select DriverID from Tbl_Booking where DriverID=" + ph.DriverID + ") ");
            string mymsg = "A booking {0} from a passenger ({1}) with ({2}) have been queued to your request box for the trip from ({3}) to the ({4}) on the date of ({5}) on ({6}).Make your response";
            string EmailContent =string.Format(mymsg,"B00"+ bokid, "P00" +ph.PassengerID,PassengerName,ph.Source,ph.Destination,ph.BookingDate,ph.BookingTime);
            SendMail_to_Driver(driveremail, EmailContent);
           
            msg = "B00" + bokid + " details is Booked Successfully,please wait to get Driver Approval";
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return msg;
        #endregion
    }

    [WebMethod]
    public List<bookingList> GetAllBookingList()
    {
        #region
        var details = new List<bookingList>();
        details.Clear();
        details.TrimExcess();
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClassFile.SelectTable("Select * from Tbl_Booking order by BookDate DESC ");
        }
        catch(Exception ex)
        {
            throw ex;
        }
        finally
        {
            foreach(DataRow dr in dt.Rows)
            {
                bookingList ojb = new bookingList
                {
                    BookingID = dr["BookID"].ToString(),
                    BookingDate = dr["BookDate"].ToString(),
                    BookingTime = dr["BookTime"].ToString(),
                    DriverID = dr["DriverID"].ToString(),
                    DriverName = CommonClassFile.GetSingleValue("Select FName+''+LName as DriverName from Tbl_Driver where DriverID=" + dr["DriverID"].ToString() + ""),
                    DContactNo = CommonClassFile.GetSingleValue("Select MobNo from Tbl_Driver where DriverID=" + dr["DriverID"].ToString() + ""),

                    PassengerID = dr["PassengerID"].ToString(),
                    PassengerName = CommonClassFile.GetSingleValue("Select FName+''+LName as UserName from Tbl_Passenger where PassengerID=" + dr["PassengerID"].ToString() + ""),
                    PContactNo = CommonClassFile.GetSingleValue("Select PhoneNo from Tbl_Passenger where PassengerID=" + dr["PassengerID"].ToString() + ""),
                    PGender = CommonClassFile.GetSingleValue("Select Gender from Tbl_Passenger where PassengerID=" + dr["PassengerID"].ToString() + ""),
                    Source = dr["Source"].ToString(),
                    Destination = dr["Destination"].ToString(),
                    Km_to_Travel = dr["Km_To_Travel"].ToString(),
                    Travel_Charge = dr["Travel_Charge"].ToString(),
                    Status = dr["Status"].ToString()
                };

                details.Add(ojb);
            }
        }
        return details;
        #endregion
    }

    [WebMethod(EnableSession =true)]
    public List<bookingList> Mybookinglist()
    {
        #region
        var details = new List<bookingList>();
        details.Clear();
        details.TrimExcess();
        DataTable dt = new DataTable();
        try
        {
            string UserId = HttpContext.Current.Session["UserID"].ToString();
            dt = CommonClassFile.SelectTable("Select * from Tbl_Booking where PassengerID="+UserId+" order by BookDate DESC ");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            foreach (DataRow dr in dt.Rows)
            {
                bookingList ojb = new bookingList
                {
                    BookingID = dr["BookID"].ToString(),
                    BookingDate = dr["BookDate"].ToString(),
                    BookingTime = dr["BookTime"].ToString(),
                    DriverID = dr["DriverID"].ToString(),
                    DriverName = CommonClassFile.GetSingleValue("Select FName+''+LName as DriverName from Tbl_Driver where DriverID=" + dr["DriverID"].ToString() + ""),
                    DContactNo = CommonClassFile.GetSingleValue("Select MobNo from Tbl_Driver where DriverID=" + dr["DriverID"].ToString() + ""),

                    PassengerID = dr["PassengerID"].ToString(),
                    PassengerName = CommonClassFile.GetSingleValue("Select FName+''+LName as UserName from Tbl_Passenger where PassengerID=" + dr["PassengerID"].ToString() + ""),
                    PContactNo = CommonClassFile.GetSingleValue("Select PhoneNo from Tbl_Passenger where PassengerID=" + dr["PassengerID"].ToString() + ""),
                    PGender = CommonClassFile.GetSingleValue("Select Gender from Tbl_Passenger where PassengerID=" + dr["PassengerID"].ToString() + ""),
                    Source = dr["Source"].ToString(),
                    Destination = dr["Destination"].ToString(),
                    Km_to_Travel = dr["Km_To_Travel"].ToString(),
                    Travel_Charge = dr["Travel_Charge"].ToString(),
                    Status = dr["Status"].ToString()
                };

                details.Add(ojb);
            }
        }
        return details;
        #endregion
    }


    public void SendMail_to_Driver(string Email, string emailcontent)
    {
        SmtpClient smtpClient = new SmtpClient();

        NetworkCredential basicCredential = new NetworkCredential("MD@SilverScreenTP.in", "SilverScreen@2006");

        MailMessage message = new MailMessage();

        MailAddress fromAddress = new MailAddress("MD@SilverScreenTP.in");

        smtpClient.Host = "host.indiancloudhosting.com";

        smtpClient.Port = 587;

        smtpClient.EnableSsl = false;

        smtpClient.UseDefaultCredentials = false;

        smtpClient.Credentials = basicCredential;

        message.From = fromAddress;
       
        message.Subject = "New booking request  from Passenger";
        //Set IsBodyHtml to true means you can send HTML email.
        message.IsBodyHtml = true;
        message.Body = emailcontent;
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


    public void ReceiveMail_from_Driver(string Email,string  stype)
    {
        SmtpClient smtpClient = new SmtpClient();

        NetworkCredential basicCredential = new NetworkCredential("MD@SilverScreenTP.in", "SilverScreen@2006");

        MailMessage message = new MailMessage();

        MailAddress fromAddress = new MailAddress("MD@SilverScreenTP.in");

        smtpClient.Host = "host.indiancloudhosting.com";

        smtpClient.Port = 587;

        smtpClient.EnableSsl = false;

        smtpClient.UseDefaultCredentials = false;

        smtpClient.Credentials = basicCredential;

        message.From = fromAddress;
        string mymsg = "";

        if (stype == "Approval")
        {
            mymsg = "Your Booking is Approved";
        }
        else if (stype == "Reject")
        {
            mymsg = "Your Booking was Rejected";
        }
        message.Subject ="Response from driver regarding Booking Status";
        //Set IsBodyHtml to true means you can send HTML email.
        message.IsBodyHtml = true;
        message.Body = mymsg;
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

    [WebMethod(EnableSession =true)]
    public string Update_Booking_Status(string Stype, int BookingID, string Passenger)
    {
        #region
        string msg = "";
        try
        {
            string driveremail = CommonClassFile.GetSingleValue("Select Email from tbl_Driver where  DriverID in (select DriverID from Tbl_Booking where BookID=" + BookingID + ") ");
            string chkpassengerEamil = CommonClassFile.GetSingleValue("Select Email from tbl_Passenger where  PassengerID in (select PassengerID from Tbl_Booking where BookID=" + BookingID + ") ");
            if (Stype == "Approval")
            {

                ReceiveMail_from_Driver(chkpassengerEamil, Stype);

                 
                int result = CommonClassFile.InsertOrUpdateorDelete("Update Tbl_Booking SET Flag=1,Status='Approved' where BookID=" + BookingID + " ");

                msg = BookingID.ToString() + "-" + Passenger + " Details Approved Successfully... ";
            }
            else if (Stype == "Reject")
            {
                ReceiveMail_from_Driver(chkpassengerEamil, Stype);
                int result = CommonClassFile.InsertOrUpdateorDelete("Update Tbl_Booking SET Flag=2,Status='Rejected' where BookID=" + BookingID + " ");
                msg = BookingID.ToString() + "-" + Passenger + " Details Rejected unfortunately... ";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return msg;
        #endregion
    }
}
