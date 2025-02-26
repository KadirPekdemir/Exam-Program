﻿using System;
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
    public partial class ViewScoresUser : Form
    {
        private string connectionString = "Server=localhost;Database=ExamProgram; Integrated Security = True;";
        private int userID;
        public ViewScoresUser(int userID)
        {
            this.userID = userID;
            InitializeComponent();
            dataGridViewSetting(dataGridViewScores);
            display();
        }
        public class Score
        {
            public int UserID { get; set; }
            public string UserName { get; set; }
            public int ExamID { get; set; }
            public int AdminID { get; set; }
            public string ExamName { get; set; }
            public int Point { get; set; }

        }
        private List<Score> GetAssignedExams(int userID)
        {
            List<Score> scores = new List<Score>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Scores.* FROM Scores " +
                               "WHERE Scores.UserID = @UserID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Score score = new Score
                            {
                                UserID = reader.GetInt32(0),
                                UserName = reader.GetString(1),
                                ExamID = reader.GetInt32(2),
                                AdminID = reader.GetInt32(3),
                                ExamName = reader.GetString(4),
                                Point = reader.GetInt32(5)
                            };
                            scores.Add(score);
                        }
                    }
                }
            }
            return scores;
        }
        private void display()
        {
            List<Score> assignedExams = GetAssignedExams(userID);

            // DataGridView kullanıyorsan
            dataGridViewScores.DataSource = assignedExams;
        }
        public void dataGridViewSetting(DataGridView dataGridView)
        {
            dataGridView.RowHeadersVisible = false;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.White;  // Varsayılan arka plan rengi
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.Gold;        // Seçilen hücrenin arka plan rengi
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.White;        //Seçilen hücrenin yazı rengi
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.White; //Başlık arka plan rengi
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Gold; //Başlık yazı rengi
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        private void ViewScoresUser_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide(); // Giriş formunu gizle
            UserScreen userScreen = new UserScreen(userID);
            userScreen.Show();
        }
    }
}
