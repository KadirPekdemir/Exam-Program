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
    public partial class ExamDetails : Form
    {
        private string connectionString = "Server=localhost;Database=ExamProgram; Integrated Security = True;";

        private Exam _exam;
        private int userID;
        public ExamDetails(Exam exam, int userID)
        {
            InitializeComponent();
            _exam = exam;
            this.userID = userID;
            displayExamDetails();
        }
        private void displayExamDetails()
        {
            int totalPoint = getTotalPoints(_exam.ExamID);
            // Sınav adını Label kontrolüne yazdır
            name.Text = $"Sınav Adı:  {_exam.Name}";
            time.Text = $"{_exam.Duration}";
            point.Text = $"{totalPoint}";
            explanation.Text = $"{_exam.Explanation}";
            deadline.Text = $"{_exam.Deadline}";

        }
        public int getTotalPoints(int examId)
        {
            int totalPoints = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT SUM(Point) FROM Questions WHERE ExamID = @ExamID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ExamID", examId);

                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        totalPoints = Convert.ToInt32(result);
                    }
                }
            }

            return totalPoints;
        }
        private void ExamDetails_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide(); // Giriş formunu gizle
            MakeExam makeExam = new MakeExam(userID, _exam.ExamID,_exam.AdminID, _exam.Name, _exam.Duration);
            makeExam.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide(); // Giriş formunu gizle
            ExamsForm examsForm = new ExamsForm(userID);
            examsForm.Show();
        }
    }
}
