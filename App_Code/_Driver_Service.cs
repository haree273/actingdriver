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
/// Summary description for _Driver_Service
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class _Driver_Service : System.Web.Services.WebService
{

     public class Driverlist
    {
      public string DriverID { get; set; }
      public string FName { get; set; }
      public string LName { get; set; }
      public string Gender { get; set; }
      public string Location { get; set; }
      public string MobNo { get; set; }
      public string Email { get; set; }
      public string Password { get; set; }
      public string DLNo { get; set; }
      public string DLExpDate { get; set; }
      public string Status { get; set; }
    }
    [WebMethod]
    public List<Driverlist> GetAllDriverlist()
    {
        #region
        var details = new List<Driverlist>();
        details.Clear();
        details.TrimExcess();
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClassFile.SelectTable("Select * from Tbl_Driver");
        }
        catch(Exception ex)
        {
            throw ex;
        }
        finally
        {
            foreach(DataRow dr in dt.Rows)
            {
                Driverlist objdrv = new Driverlist
                {
                    DriverID  = dr["DriverID"].ToString(),
                    FName     = dr["FName"].ToString(),
                    LName     = dr["LName"].ToString(),
                    Gender    = dr["Gender"].ToString(),
                    MobNo     = dr["MobNo"].ToString(),
                    Email     = dr["Email"].ToString(),
                    Location  = dr["Location"].ToString(),
                    Password  = dr["Password"].ToString(),
                    DLNo      = dr["DLNo"].ToString(),
                    DLExpDate = dr["DLExpDate"].ToString(),
                    Status    = dr["Status"].ToString()
                };

                details.Add(objdrv);
            }
        }

        return details;
        #endregion
    }
    [WebMethod]
    public string SaveRecord(Driverlist ph)
    {
        #region
        string msg = "";
        try
        {
             

            string SqlQuery = " DECLARE @DriverID Bigint; SET @DriverID = isnull(((SELECT max(DriverID) FROM Tbl_Driver) + 1),'1') ";
            SqlQuery = SqlQuery + " INSERT INTO Tbl_Driver (DriverID,FName,LName,Gender,Email,MobNo,DLNo,DLExpDate,Flag,CDate,Status,Location,Password) ";
            SqlQuery = SqlQuery + " Values (@DriverID,'"+ph.FName+"','"+ph.LName+"','"+ph.Gender+"','"+ph.Email+"','"+ph.MobNo+"','"+ph.DLNo+"','"+ph.DLExpDate+"',0,GetDate(),'Pending','"+ph.Location+"','"+ph.Password+"') ";
            int result = CommonClassFile.InsertOrUpdateorDelete(SqlQuery);

            msg =ph.FName+"- "+ph.LName+ " Detail Registered Successfully.  Admin Should Approve your Account ...  ";

        }
        catch(Exception ex)
        {

        }
        finally
        {

        }
        #endregion
        return msg;
    }

    [WebMethod]
    public string Update_Driver_Status(string Stype, int DriverId,string DriverName)
    {
        #region
        string msg = "";
        try
        {
            if (Stype == "Approval")
            {
                int result = CommonClassFile.InsertOrUpdateorDelete("Update Tbl_Driver SET Flag=1,Status='Approved' where DriverID="+DriverId+" ");

                msg = DriverId.ToString() + "-" + DriverName + " Details Approved Successfully... ";
            }
            else if (Stype == "Reject")
            {
                int result = CommonClassFile.InsertOrUpdateorDelete("Update Tbl_Driver SET Flag=2,Status='Rejected' where DriverID=" + DriverId + " ");
                msg = DriverId.ToString() + "-" + DriverName + " Details Rejected unfortunately... ";
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
        return msg;
        #endregion
    }


    [WebMethod]
    public List<Driverlist> Get_Driverlist_FilterBy_Location(string Location)
    {
        #region
        var details = new List<Driverlist>();
        details.Clear();
        details.TrimExcess();
        DataTable dt = new DataTable();
        try
        {
            if (Location == "0")
            {
                dt = CommonClassFile.SelectTable("Select * from Tbl_Driver where  Status='Approved' and Flag=1  ");
            }
            else
            {
                dt = CommonClassFile.SelectTable("Select * from Tbl_Driver where  Status='Approved' and Flag=1 and Location='" + Location + "' ");
            }
           
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            foreach (DataRow dr in dt.Rows)
            {
                Driverlist objdrv = new Driverlist
                {
                    DriverID = dr["DriverID"].ToString(),
                    FName = dr["FName"].ToString(),
                    LName = dr["LName"].ToString(),
                    Gender = dr["Gender"].ToString(),
                    MobNo = dr["MobNo"].ToString(),
                    Email = dr["Email"].ToString(),
                    Location = dr["Location"].ToString(),
                    Password = dr["Password"].ToString(),
                    DLNo = dr["DLNo"].ToString(),
                    DLExpDate = dr["DLExpDate"].ToString(),
                    Status = dr["Status"].ToString()
                };

                details.Add(objdrv);
            }
        }

        return details;
        #endregion
    }

    [WebMethod]
    public List<ListItem> getddl_Location()
    {
        #region
        var details = new List<ListItem>();
        details.Clear();
        details.TrimExcess();
        DataTable dt = new DataTable();
        try
        {
            dt = CommonClassFile.SelectTable("Select distinct Location from Tbl_Driver order by Location Asc");
        }
        catch(Exception ex)
        {

        }
        finally
        {
            foreach (DataRow dr in dt.Rows)
            {
                ListItem lst = new ListItem
                {
                    Value = dr["Location"].ToString(),
                    Text = dr["Location"].ToString()
                };

                details.Add(lst);
            }

        }
        return details;
        #endregion
    }


    [WebMethod(EnableSession = true)]
    public string Login(string Email, string Password)
    {
        #region
        string status = string.Empty;


        DataTable dt = CommonClassFile.SelectTable("Exec Spr_DriverLogin  '" + Email + "','" + Password + "' ");
        if (dt.Rows.Count > 0)
        {

            status = dt.Rows[0]["Column1"].ToString();
            if (status == "1")
            {

                DataTable dtlogin = CommonClassFile.SelectTable("Select DriverID,FName,LName from Tbl_Driver where   Email='" + Email + "' and Password='" + Password + "'");
                if (dtlogin.Rows.Count > 0)
                {
                    HttpContext.Current.Session["UserName"] = dtlogin.Rows[0]["FName"].ToString() + " " + dtlogin.Rows[0]["LName"].ToString();

                    HttpContext.Current.Session["UserID"] = dtlogin.Rows[0]["DriverID"].ToString();

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
