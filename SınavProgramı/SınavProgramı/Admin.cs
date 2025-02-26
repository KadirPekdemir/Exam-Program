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
using static SınavProgramı.ExamsForm;

namespace SınavProgramı
{
    public partial class Admin : Form
    {
        private int adminID;
        private string connectionString = "Server=localhost;Database=ExamProgram; Integrated Security = True;";
        public Admin(int adminID)
        {
            this.adminID = adminID;
            InitializeComponent();
            display();
        }
        private void display()
        {
            string query = "SELECT Name, Surname FROM Admins WHERE AdminID = @AdminID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametre ekle
                        command.Parameters.AddWithValue("@AdminID", adminID);

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

        private void Admin_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide(); // Giriş formunu gizle
            AdminScreen adminScreen = new AdminScreen(adminID);
            adminScreen.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Hide(); // Giriş formunu gizle
            ViewScores viewScores = new ViewScores(adminID);
            viewScores.Show();
        }

        private void loginMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginPage loginPage = new LoginPage();
            loginPage.Show();

        }
    }
}
