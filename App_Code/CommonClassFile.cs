using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;

/// <summary>
/// Summary description for CommonClassFile
/// </summary>
 
    public class CommonClassFile
    {
        public static string DBName;

        public static bool isAnyArrear = false;



        public static string connectionstring = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;



        public static string Get24HourTime(int hour, int minute, string ToD, double AddTime)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            if (ToD.ToUpper() == "PM") hour = (hour % 12) + 12;
            DateTime dati = new DateTime(year, month, day, hour, minute, 0);
            dati = dati.AddMinutes(AddTime);
            return dati.ToString("HH:mm");
        }
        public static string Get12HourTime(int hour, int minute, string ToD)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            //if (ToD.ToUpper() == "PM") hour = (hour % 12) + 12;
            DateTime dati = new DateTime(year, month, day, hour, minute, 0);
            // dati = dati.AddMinutes(AddTime);
            return dati.ToString("hh:mm tt");
        }
        public static SqlConnection openConnection()
        {

            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);


            Conn.Open();

            return Conn;
        }

        public static string TostringEvenNull(string str)
        {

            if (string.IsNullOrEmpty(str))
            {
                str = "";
            }
            return str.ToString();
        }

        public static string TostringEvenNull(object str)
        {

            if (str == null)
            {
                str = "";
            }
            return str.ToString();
        }

        //public static string TostringEvenNull(double  str)
        //{

        //    if (str == null)
        //    {
        //        str = 0.0;
        //    }
        //    return str.ToString();
        //}
        public static string indiandateToMysqlDate(string ddmmyy)
        {
            if (string.IsNullOrEmpty(ddmmyy))
            {
                ddmmyy = "";
            }
            string strAuctionDate = "";
            if (ddmmyy.Trim() == "")
            {
                strAuctionDate = "";
            }
            else
            {
                strAuctionDate = ddmmyy.Split('/')[2] + "/" + ddmmyy.Split('/')[1] + "/" + ddmmyy.Split('/')[0];
            }

            return strAuctionDate;
        }
        public static string ReplaceJunk(string ip)
        {
            string Result = "";

            Result = ip.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("'", "''").Replace("\\", "\\\\");
            return Result;

        }
        public static SqlCommand GetCommand(string commandText, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand();

            command.CommandText = commandText;
            command.CommandTimeout = 900;
            return command;
        }

        //Insert or update
        public static int InsertOrUpdateorDelete(string cmdText)
        {
            using (SqlConnection conn = openConnection())
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    cmd.CommandTimeout = 900;
                    return cmd.ExecuteNonQuery();

                }


            }

        }

        //Select and Return DataTable
        public static DataTable SelectTable(string strQuery)
        {
            DataTable dtSelectedTable = new DataTable();
            using (SqlConnection myCon = openConnection())
            {

                using (SqlCommand myCmd = new SqlCommand(strQuery, myCon))
                {
                    SqlDataAdapter myAdapter = new SqlDataAdapter(myCmd);
                    myAdapter.Fill(dtSelectedTable);
                    return dtSelectedTable;
                }

            }


        }

        public static SqlDataReader MyReader(string strQuery)
        {

            using (SqlConnection myCon = openConnection())
            {

                using (SqlCommand myCmd = new SqlCommand(strQuery, myCon))
                {
                    SqlDataReader myAdapter = null;
                    myAdapter = myCmd.ExecuteReader();

                    return myAdapter;
                }

            }


        }








        //avoid Sql injection and query error
        public static string MySQLEscapeString(string usString)
        {
            if (usString == null)
            {
                return null;
            }
            // it escapes \r, \n, \x00, \x1a, \, ', and "
            return System.Text.RegularExpressions.Regex.Replace(usString, @"[\r\n\x00\x1a\\'""]", @"\$0");
        }


        //Convert .NET ShortDateString (mm/dd/yyyy) to MySQLDate (yyyy-mm-dd)
        public static string DateConversion_DotNetDateToMySQLDate(string shortDate)
        {
            string[] dateParts = shortDate.Split('/');
            return dateParts[2] + "/" + dateParts[0] + "/" + dateParts[1];
        }


        public static string DateConversion_DotNetDateToMySQLDate(DateTime shortDate)
        {


            return shortDate.Year + "/" + shortDate.Month + "/" + shortDate.Day;
        }

        //Convert  MySQLDate to .NET ShortDateString (mm/dd/yyyy)  (yyyy-mm-dd)
        public static string DateConversion_MySQLDateToDotNetDate(string shortDate)
        {
            string[] dateParts = shortDate.Split('/');
            return dateParts[1] + "/" + dateParts[2] + "/" + dateParts[0];
        }
        public static string DateConversion_MySQLDateToDotNetDate(DateTime shortDate)
        {


            return shortDate.Month + "/" + shortDate.Day + "/" + shortDate.Year;
        }
        //Convert   .NET ShortDateString  (mm/dd/yyyy)   to indian (dd/mm/yyyy)

        public static string DateConversion_DotNetDateToIndian(DateTime shortDate)
        {


            return shortDate.Day + "/" + shortDate.Month + "/" + shortDate.Year;
        }
        public static string DateConversion_DotNetDateToIndian(string shortDate)
        {


            string[] dateParts = shortDate.Split('/');
            return dateParts[1] + "/" + dateParts[0] + "/" + dateParts[2];
        }
        public static string DateConversion_IndianToDotNet(string shortDate)
        {


            string[] dateParts = shortDate.Split('/');
            return dateParts[1] + "/" + dateParts[0] + "/" + dateParts[2];
        }

        public static string DateConversion_MysqlDateToIndian(string shortDate)
        {


            string[] dateParts = shortDate.Split('/');
            return dateParts[2] + "/" + dateParts[1] + "/" + dateParts[0];
        }
        public static string DateConversion_MysqlDateToIndian(DateTime shortDate)
        {

            return shortDate.Day + "/" + shortDate.Month + "/" + shortDate.Year;
        }



        public static string ConvertToIndianCurrency(string fare)
        {
            decimal parsed = decimal.Parse(fare, CultureInfo.InvariantCulture);
            CultureInfo hindi = new CultureInfo("hi-IN");
            string op = string.Format(hindi, "{0:c}", parsed).Replace("रु", "");
            return op;
        }

        public static string GetSingleValue(string cmdText)
        {
            string op = null;
            using (SqlConnection conn = openConnection())
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {

                    object obj = cmd.ExecuteScalar();
                    if (obj != null)
                    {
                        op = Convert.ToString(obj);
                    }
                    else
                    {
                        op = "";
                    }

                }
            }


            return op;
        }

        public static SqlDataReader ExecuteReader(string cmdText)
        {
            SqlConnection conn = new SqlConnection(connectionstring);
            using (SqlCommand cmd = new SqlCommand(cmdText, conn))
            {

                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
        //public static string indiandateToMysqlDate(string ddmmyy)
        //{
        //    string strDate = ddmmyy.Split('/')[2] + "/" + ddmmyy.Split('/')[1] + "/" + ddmmyy.Split('/')[0];
        //    return strDate;
        //}


        public static string NumberToText(int number, bool isUK)
        {
            if (number == 0) return "Zero";
            string and = isUK ? "and " : ""; // deals with UK or US numbering
            if (number == -2147483648) return "Minus Two Billion One Hundred " + and +
            "Forty Seven Million Four Hundred " + and + "Eighty Three Thousand " +
            "Six Hundred " + and + "Forty Eight";
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Million ", "Billion " };
            num[0] = number % 1000;           // units
            num[1] = number / 1000;
            num[2] = number / 1000000;
            num[1] = num[1] - 1000 * num[2];  // thousands
            num[3] = number / 1000000000;     // billions
            num[2] = num[2] - 1000 * num[3];  // millions
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10;              // ones
                t = num[i] / 10;
                h = num[i] / 100;             // hundreds
                t = t - 10 * h;               // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i < first) sb.Append(and);
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }

    }