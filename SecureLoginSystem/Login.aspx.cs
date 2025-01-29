using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;

namespace WebApplication1
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            //string password = "password123";
          
            // Hash password using SHA256
            string hashedPassword = ComputeHash(password);
            string connectionString =  ConfigurationManager.ConnectionStrings["DefaultConnection"]
                                     .ConnectionString;
            //string connectionString = "your_connection_string_here";
            string query = "SELECT Role FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                conn.Open();
                object roleObj = cmd.ExecuteScalar();

                if (roleObj != null)
                {
                    string userRole = roleObj.ToString();

                    // Create an authentication ticket
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,                      // Ticket version
                        username,               // Username
                        DateTime.Now,           // Issue date
                        DateTime.Now.AddMinutes(2), // Expiration
                        false,                  // Persistent cookie
                        userRole,               // User data (e.g., roles)
                        FormsAuthentication.FormsCookiePath);

                    // Encrypt the ticket
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    // Create a cookie
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(authCookie);

                    // Redirect to the default page
                    Response.Redirect(FormsAuthentication.DefaultUrl);
                }
                else
                {
                    lblMessage.Text = "Invalid username or password.";
                }
            }
        }
        private string ComputeHash(string input)
        {
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}