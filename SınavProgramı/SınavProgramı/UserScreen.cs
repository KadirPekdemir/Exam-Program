using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SınavProgramı
{
    public partial class UserScreen : Form
    {
        private string connectionString = "Server=localhost;Database=ExamProgram; Integrated Security = True;";
        private int userID;
        public UserScreen(int userID)
        {
            this.userID = userID;
            InitializeComponent();
            display();
        }
        private void display()
        {
            string query = "SELECT Name, Surname FROM Users WHERE UserID = @UserID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametre ekle
                        command.Parameters.AddWithValue("@UserID", userID);

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            string fullName = reader["Name"].ToString() + " " + reader["Surname"].ToString();
                            nameSurname.Text = fullName;
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }


        private void UserScreen_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            ExamsForm examsForm = new ExamsForm(userID);
            examsForm.Show();
            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Hide(); // Giriş formunu gizle
            ViewScoresUser viewScoresUser = new ViewScoresUser(userID);
            viewScoresUser.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void loginMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
        }
    }
}
