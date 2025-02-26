using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SınavProgramı
{
    public partial class LoginPage : Form
    {
        private string connectionString = "Server=localhost;Database=ExamProgram; Integrated Security = True;";


        public LoginPage()
        {
            InitializeComponent();
        }

        /*public void LogIn_Page_Loaad(object sender, EventArgs e)
        {
            connection = new SqlConnection("server=.; Initial Catalog=Store2; Integrated Security=SSPI");
            command = new SqlCommand();
            command.Connection = connection;
        }*/

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string adminID = textBox1.Text;
            string password = textBox2.Text;

            if (AuthenticateAdmin(adminID, password))
            {
                //MessageBox.Show("Giriş başarılı!");
                int intAdminID = int.Parse(adminID);
                this.Hide(); // Giriş formunu gizle
                Admin admin = new Admin(intAdminID);
                admin.Show();
            }
            else
            {
                MessageBox.Show("Geçersiz kullanıcı adı veya şifre.");
            }
        }
        private bool AuthenticateAdmin(string adminID, string password)
        {
            bool isAuthenticated = false;

            // SQL sorgusunu hazırlayın
            string query = "SELECT COUNT(1) FROM Admins WHERE AdminID = @AdminID AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Parametreleri ekleyin
                    command.Parameters.AddWithValue("@AdminID", adminID);
                    command.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        connection.Open();
                        // Sorguyu çalıştırın
                        int result = (int)command.ExecuteScalar();

                        // Eğer sonuç 1 ise, kullanıcı başarıyla doğrulandı
                        if (result == 1)
                        {
                            isAuthenticated = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hata: {ex.Message}");
                    }
                }
            }

            return isAuthenticated;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string userID = textBox4.Text;
            int intUserID = int.Parse(userID);
            string password = textBox3.Text;

            if (AuthenticateUser(userID, password))
            {
                UserScreen userScreen = new UserScreen(intUserID);
                userScreen.Show();
                this.Hide();
                //MessageBox.Show("Giriş başarılı!");

                
            }
            else
            {
                MessageBox.Show("Geçersiz kullanıcı adı veya şifre.");
            }
        }
        private bool AuthenticateUser(string userID, string password)
        {
            bool isAuthenticated = false;

            // SQL sorgusunu hazırlayın
            string query = "SELECT COUNT(1) FROM Users WHERE UserID = @UserID AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Parametreleri ekleyin
                    command.Parameters.AddWithValue("@UserID", userID);
                    command.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        connection.Open();
                        // Sorguyu çalıştırın
                        int result = (int)command.ExecuteScalar();

                        // Eğer sonuç 1 ise, kullanıcı başarıyla doğrulandı
                        if (result == 1)
                        {
                            isAuthenticated = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Hata: {ex.Message}");
                    }
                }
            }

            return isAuthenticated;
        }

        private void LoginPage_Load(object sender, EventArgs e)
        {

        }
    }
}
