using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace Listing_Searcher
{
    public partial class LoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (ValidateUser(username, password))
            {
                Response.Redirect("MainPage.aspx");
            }
            else
            {
                string script = "alert('Incorrect username or password!');";
                ClientScript.RegisterStartupScript(this.GetType(), "Error", script, true);
                txtPassword.Focus();
            }
        }

        private bool ValidateUser(string username, string password)
        {
            bool isValid = false;
            string connectionString = ConfigurationManager.ConnectionStrings["ListingDatabase"].ConnectionString;

            using (SqlConnection myCon = new SqlConnection(connectionString))
            {
                string query = "SELECT Password FROM PropertyAccounts WHERE Username = @Username";
                SqlCommand cmd = new SqlCommand(query, myCon);
                cmd.Parameters.AddWithValue("@Username", username);

                myCon.Open();
                string storedHashedPassword = cmd.ExecuteScalar() as string;

                if (storedHashedPassword != null)
                {
                    isValid = VerifyPasswordHash(password, storedHashedPassword);
                }
            }
            return isValid;
        }

        private bool VerifyPasswordHash(string password, string storedHashedPassword)
        {
            string hashedPassword = HashPassword(password);
            return hashedPassword == storedHashedPassword;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
