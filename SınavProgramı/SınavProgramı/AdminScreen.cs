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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlTypes;
using System.Runtime.Remoting.Messaging;
using System.Net;

namespace SınavProgramı
{
    public partial class AdminScreen : Form
    {
        private string connectionString = "Server=localhost;Database=ExamProgram; Integrated Security = True;";
        int examID = 0;
        private int adminID;
        private bool isRecorded = false;
        private List<Question> questions = new List<Question>();
        public AdminScreen(int adminID)
        {
            InitializeComponent();
            this.adminID = adminID;
        }

        
        private int InsertExamData(int adminID, string name, string deadline, string duration, string explanation)
        {
            string query = @"
                INSERT INTO Exams (AdminID,Name, Deadline, Duration, Explanation)
                VALUES (@AdminID, @Name, @Deadline, @Duration, @Explanation);
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AdminID", adminID);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Deadline", deadline);
                command.Parameters.AddWithValue("@Duration", duration);
                command.Parameters.AddWithValue("@Explanation", explanation);

                try
                {
                    connection.Open();
                    // Son eklenen kaydın ID'sini döndür
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int newExamId))
                    {
                        return newExamId;
                    }
                }
                catch (Exception ex)
                {
                    // Hata mesajını yazdırın veya kaydedin
                    MessageBox.Show("Veri kaydedilirken bir hata oluştu: " + ex.Message);
                }
            }

            return -1; // Hata durumu
        }
        private bool InsertQuestionData(int examID, int questionNo, int point, string chapter, string question, string optionA, string optionB, string optionC, string optionD, string answer)
        {
            string query = "INSERT INTO Questions (ExamID, QuestionNo, Point, Chapter, Question, OptionA, OptionB, OptionC, OptionD, Answer) VALUES (@InputData1, @InputData2, @InputData3, @InputData4, @InputData5, @InputData6, @InputData7, @InputData8, @InputData9, @InputData10)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@InputData1", examID);
                command.Parameters.AddWithValue("@InputData2", questionNo);
                command.Parameters.AddWithValue("@InputData3", point);
                command.Parameters.AddWithValue("@InputData4", chapter);
                command.Parameters.AddWithValue("@InputData5", question);
                command.Parameters.AddWithValue("@InputData6", optionA);
                command.Parameters.AddWithValue("@InputData7", optionB);
                command.Parameters.AddWithValue("@InputData8", optionC);
                command.Parameters.AddWithValue("@InputData9", optionD);
                command.Parameters.AddWithValue("@InputData10", answer);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    // Hata mesajını yazdırın veya kaydedin
                    MessageBox.Show("Veri kaydedilirken bir hata oluştu: " + ex.Message);
                    return false;
                }
            }
        }
        public class Question
        {
            public string question_ { get; set; }
            public int QuestionNo { get; set; }
            public int Point { get; set; }
            public string Chapter { get; set; }
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string Answer { get; set; }

        }
        private string GetSelectedAnswer()
        {
            // groupBox1 içindeki radioButton'lardan hangisinin seçili olduğunu kontrol et
            foreach (Control control in groupBox1.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Checked)
                {
                    return radioButton.Name;
                }
            }
            return null;
        }
        
        private void resetQuestion()
        {
            question.Text = string.Empty;
            questionNo.Text = string.Empty;
            point.Text = string.Empty;
            chapter.Text = string.Empty;
            optionA.Text = "A) ";
            optionB.Text = "B) ";
            optionC.Text = "C) ";
            optionD.Text = "D) ";
            A.Checked = false;
            B.Checked = false;
            C.Checked = false;
            D.Checked = false;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (GetSelectedAnswer() == null)
            {
                MessageBox.Show("Bir cevap seçilmedi.Bu yüzden soru kaydedilmedi.");
            }
            else
            {
                Question _question = new Question
                {
                    question_ = question.Text,
                    QuestionNo = int.Parse(questionNo.Text),
                    Point = int.Parse(point.Text),
                    Chapter = chapter.Text,
                    A = optionA.Text,
                    B = optionB.Text,
                    C = optionC.Text,
                    D = optionD.Text,
                    Answer = GetSelectedAnswer(),
                };
                questions.Add(_question);
                resetQuestion();
            }

        }
        private void resetExam()
        {
            name.Text = string.Empty;
            deadline.Text = string.Empty;
            duration.Text = string.Empty;
            explanation.Text = string.Empty;
            resetQuestion();
            questions.Clear();
        }
        private void save_Click(object sender, EventArgs e)
        {
            string inputName = name.Text;
            string inputDeadline = deadline.Text;
            string inputDuration = duration.Text;
            string inputExplanation = explanation.Text;

            examID = InsertExamData(adminID, inputName, inputDeadline, inputDuration, inputExplanation);
            if (question.Text != null)
            {
                button1_Click(sender, e);
            }

            for (int i = 0; i < questions.Count; i++)
            {
                bool success2 = InsertQuestionData(examID, questions[i].QuestionNo, questions[i].Point, questions[i].Chapter, questions[i].question_, questions[i].A, questions[i].B, questions[i].C, questions[i].D, questions[i].Answer);
            }
            MessageBox.Show("Sınav başarıyla kaydedildi.");
            resetExam();
        }

        private void back_Click(object sender, EventArgs e)
        {
            questions.Clear();
            this.Hide();
            Admin admin = new Admin(adminID);
            admin.Show();
        }

        private void optionA_TextChanged(object sender, EventArgs e)
        {

        }

        private void AdminScreen_Load(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void B_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
