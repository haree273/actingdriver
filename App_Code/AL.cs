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

/// <summary>
/// Summary description for AL
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class AL : System.Web.Services.WebService
{

    [WebMethod(EnableSession = true)]

    public string AdmLogin(string UserName, string Password)
    {



        string status = string.Empty;


        DataTable dt = CommonClassFile.SelectTable("Exec Spr_AdminLogin  '" + UserName + "','" + Password + "' ");
        if (dt.Rows.Count > 0)
        {

            status = dt.Rows[0]["Column1"].ToString();
            if (status == "1")
            {

              

                DataTable dtlogin = CommonClassFile.SelectTable("Select ID,UserName from Tbl_Login where   UserName='" + UserName + "' and Password='" + Password + "'");
                if (dtlogin.Rows.Count > 0)
                {
                    HttpContext.Current.Session["UserName"] = dtlogin.Rows[0]["UserName"].ToString();
                    
                    HttpContext.Current.Session["UserID"] = dtlogin.Rows[0]["ID"].ToString();

                }
            }
            else
            {
                status = "0";
            }


        }


        return status;

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
