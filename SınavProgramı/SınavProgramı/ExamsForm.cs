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
    public partial class ExamsForm : Form
    {
        private string connectionString = "Server=localhost;Database=ExamProgram; Integrated Security = True;";

        private int _userID;
        public ExamsForm(int userID)
        {
            InitializeComponent();
            _userID = userID;
            dataGridViewSetting(dataGridViewExams);
            LoadAssignedExams();
        }

        private void ExamsForm_Load(object sender, EventArgs e)
        {

        }
        private void LoadAssignedExams()
        {
            List<Exam> assignedExams = GetAssignedExams(_userID);

            // DataGridView kullanıyorsan
            dataGridViewExams.DataSource = assignedExams;

           /* // ListBox kullanıyorsan
            listBoxExams.Items.Clear();
            foreach (var exam in assignedExams)
            {
                listBoxExams.Items.Add(exam.Name);
            }*/
        }
        public class Exam
        {
            public int ExamID { get; set; }
            public int AdminID { get; set; }
            public string Name { get; set; }
            public string Deadline { get; set; }
            public string Duration { get; set; }
            public string Explanation { get; set; }

        }
        private List<Exam> GetAssignedExams(int studentId)
        {
            List<Exam> exams = new List<Exam>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Exams.* FROM Exams " +
                               "INNER JOIN Users ON Exams.AdminID = Users.AdminID " +
                               "LEFT JOIN Scores ON Exams.ExamID = Scores.ExamID AND Scores.UserID = @StudentId " +
                               "WHERE Users.UserID = @StudentId AND Scores.ExamID IS NULL;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime deadline = reader.GetDateTime(3);
                            string stringDeadline = deadline.ToString("yyyy-MM-dd");
                            Exam exam = new Exam
                            {
                                ExamID = reader.GetInt32(0),
                                AdminID = reader.GetInt32(1),
                                Name = reader.GetString(2),
                                Deadline = stringDeadline,
                                Duration = reader.GetString(4),
                                Explanation = reader.GetString(5)
                                
                            };
                            exams.Add(exam);
                        }
                    }
                }
            }
            return exams;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridViewExams.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewExams.SelectedRows[0];
                Exam selectedExam = (Exam)selectedRow.DataBoundItem;

                // Yeni formu aç
                ExamDetails examDetails = new ExamDetails(selectedExam, _userID);
                examDetails.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Lütfen bir sınav seçin.");
            }

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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide(); // Giriş formunu gizle
            UserScreen userScreen = new UserScreen(_userID);
            userScreen.Show();
        }
    }
}
