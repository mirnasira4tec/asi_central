using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Data.Common;

public partial class Conversion : System.Web.UI.Page
{
    protected string Password = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ConnectionStringSettings connSetting = ConfigurationManager.ConnectionStrings["ASIInternet"];
        string connString = connSetting.ConnectionString;
        string dbProvider = connSetting.ProviderName;
        using (SqlConnection conn = new SqlConnection())
        using (SqlConnection conn2 = new SqlConnection())
        {
            conn.ConnectionString = connString;
            conn.Open();
            conn2.ConnectionString = connString;
            conn2.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT [Email] FROM [asiinternet].[dbo].[LOAD_UserPassword]"
                              + " where  [LastLoginDate] >= convert(datetime,'2013-01-01 00:00:00.000') and [Password_MMS] is null"
                              + " order by [LastLoginDate] desc";
            using (DbDataReader dataReader = cmd.ExecuteReader())
            {
                int i = 0;
                while (dataReader.Read())
                {
                    string username = Membership.GetUserNameByEmail(dataReader["Email"].ToString());
                    if (!string.IsNullOrEmpty(username))
                    {
                        MembershipUser user = Membership.GetUser(username);
                        if (!user.IsLockedOut)
                        {
                            string password = user.GetPassword();
                            if (!string.IsNullOrEmpty(password))
                            {
                                SqlCommand cmd2 = new SqlCommand();
                                cmd2.Connection = conn2;
                                password = password.Replace("'", "''");
                                cmd2.CommandText = string.Format("UPDATE [dbo].[LOAD_UserPassword] SET [Password_MMS] = {0} WHERE [Email] = '{1}'",
                                                   string.Format("HASHBYTES('SHA1', ISNULL('{0}', ''))", password), dataReader["Email"].ToString());
                                cmd2.ExecuteNonQuery();
                                i++;
                            }
                        }
                    }
                }
                Password = i.ToString();
            }
        }
    }
}

