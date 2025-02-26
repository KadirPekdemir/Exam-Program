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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Xml.Linq;
using static SınavProgramı.ExamsForm;
using System.Drawing.Text;
using static SınavProgramı.MakeExam;

namespace SınavProgramı
{
    public partial class MakeExam : Form
    {
        private string connectionString = "Server=localhost;Database=ExamProgram; Integrated Security = True;";
        private int userID;
        private int examID;
        private int AdminID;
        private string examName;
        private string duration;
        private TimeSpan remainingTime;
        List<Question> questions;
        private int i = 0;
        private string[] selected;
        private string[] answerKey;


        public MakeExam(int userID, int examID,int AdminID, string examName, string duration)
        {
            InitializeComponent();
            this.userID = userID;
            this.examID = examID;
            this.AdminID = AdminID;
            this.examName = examName;
            this.duration = duration;
            questions = getQuestions(examID);
            displayQuestion(questions[0]);
            questionButton();
            answerKey = new string[questions.Count];
            for (int i = 0; i < questions.Count; i++)
            {
                answerKey[i] = questions[i].Answer; 
            }
            remainingTime = TimeSpan.FromMinutes(int.Parse(duration)); 
            lblRemainingTime.Text = remainingTime.ToString("mm\\:ss");
            timer1.Interval = 1000;
            timer1.Start();
        }
        
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
          
        }
        private void progressBar()
        {
            int j = 0;
            for (int k = 0; k < questions.Count; k++)
            {
                if (questions[k].SelectedOption != null)
                {
                    j++;
                }
            }
            marked.Text = j.ToString();
            unmarked.Text = (questions.Count - j).ToString();
            progressBar1.Value = (100*j)/questions.Count;
            lblProgress.Text = "%" + progressBar1.Value.ToString();
        }
        private void questionButton()
        {
            for (int i = 1; i <= 20; i++) 
            {
                
                Button button = this.Controls["Soru" + i.ToString()] as Button;

                if (button != null)
                {
                    
                    if (i <= questions.Count)
                    {
                        button.Visible = true; 
                    }
                    else
                    {
                        button.Visible = false;
                    }
                }
            }
        }
        
        private void displayQuestion(Question question)
        {
            questionNo.Text = $"Soru {question.QuestionNo}";
            question_.Text = $"{question._question}";
            OptionA.Text = $"{question.A}";
            OptionB.Text = $"{question.B}";
            OptionC.Text = $"{question.C}";
            OptionD.Text = $"{question.D}";
            point.Text = $"Puan {question.Point}";
            progressBar();
            if (question.SelectedOption != null)
            {
                if(question.SelectedOption == "A")
                {
                    A.Checked = true;
                }
                else if (question.SelectedOption == "B")
                {
                    B.Checked = true;
                }
                else if (question.SelectedOption == "C")
                {
                    C.Checked = true;
                }
                else if (question.SelectedOption == "D")
                {
                    D.Checked = true;
                }
            }
            else
            {
                A.Checked = false;
                B.Checked = false;
                C.Checked = false;
                D.Checked = false;
            }
        }
        public class Question
        {
            public int QuestionID { get; set; }
            public int ExamID { get; set; }
            public int QuestionNo { get; set; }
            public int Point { get; set; }
            public string _question { get; set; }
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
            public string D { get; set; }
            public string Answer { get; set; }
            public string SelectedOption { get; set; }

        }
        private List<Question> getQuestions(int examID)
        {
            List<Question> questions = new List<Question>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Questions.* FROM Questions " +
                               "WHERE Questions.ExamID = @ExamID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("ExamID", examID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Question question = new Question
                            {
                                QuestionID = reader.GetInt32(0),
                                ExamID = reader.GetInt32(1),
                                QuestionNo = reader.GetInt32(2),
                                Point = reader.GetInt32(3),
                                _question = reader.GetString(5),
                                A = reader.GetString(6),
                                B = reader.GetString(7),
                                C = reader.GetString(8),
                                D = reader.GetString(9),
                                Answer = reader.GetString(11),
                                SelectedOption = null
                            };
                            questions.Add(question);
                        }
                    }
                }
            }
            return questions;
        }
        
        private string GetSelectedAnswer()
        {
            foreach (Control control in groupBox1.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Checked)
                {
                    return radioButton.Name;
                }
            }
            return null;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i+1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            
          

            if (i < questions.Count-1)
            {
                i++;
                displayQuestion(questions[i]); 
            }
            if (i == questions.Count-1)
            {
                nextQuestion.Visible = false;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
        }

        private void beforeQuestion_Click(object sender, EventArgs e)
        {
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            if (i > 0)
            {
                i--;
                displayQuestion(questions[i]);
            }
            if (i == 0)
            {
                beforeQuestion.Visible = false;
            }
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
        }
        public string GetUserNameByUserID(int userID)
        {
           
            string userName = string.Empty;

            string query = "SELECT Name FROM Users WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametreyi ekle
                        command.Parameters.AddWithValue("@UserID", userID);

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            // Name değerini al ve string değişkenine ata
                            userName = reader["Name"].ToString();
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata: " + ex.Message);
                }
            }

            return userName;
        }

        private void finish_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            int totalPoint = 0;
            for (int i = 0; i < questions.Count; i++)
            {
                if (answerKey[i] == selected[i])
                {
                    totalPoint += questions[i].Point;
                }
            }
            string userName = GetUserNameByUserID(userID);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Scores (UserID,UserName, ExamID, AdminID, ExamName, Point) " +
                               "VALUES (@Column1, @Column2, @Column3, @Column4, @Column5, @Column6)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Column1", userID);
                    command.Parameters.AddWithValue("@Column2", userName);
                    command.Parameters.AddWithValue("@Column3", examID);
                    command.Parameters.AddWithValue("@Column4", AdminID);
                    command.Parameters.AddWithValue("@Column5", examName);
                    command.Parameters.AddWithValue("@Column6", totalPoint);

                    int rowsAffected = command.ExecuteNonQuery();

                }
            }
            this.Hide();
            ExamsForm examForm = new ExamsForm(userID);
            examForm.Show();
        }

        private void Soru1_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 0;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
            beforeQuestion.Visible = false;

        }

        private void Soru2_Click(object sender, EventArgs e)
        {

            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 1;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru3_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 2;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru4_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 3;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru5_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 4;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru6_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 5;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru7_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 6;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru8_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 7;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru9_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 8;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru10_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 9;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru11_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 10;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru12_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 11;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru13_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 12;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru14_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 13;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru15_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 14;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru16_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 15;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru17_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 16;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru18_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 17;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru19_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 18;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void Soru20_Click(object sender, EventArgs e)
        {
            if (selected == null)
            {
                selected = new string[questions.Count];
            }
            selected[i] = GetSelectedAnswer();
            if (selected[i] != null)
            {
                questions[i].SelectedOption = selected[i];
                Button button = this.Controls["Soru" + (i + 1).ToString()] as Button;
                button.BackColor = Color.Gold;
            }
            i = 19;
            displayQuestion(questions[i]);
            if (nextQuestion.Visible == false)
            {
                nextQuestion.Visible = true;
            }
            if (beforeQuestion.Visible == false)
            {
                beforeQuestion.Visible = true;
            }
            if (i == questions.Count - 1)
            {
                nextQuestion.Visible = false;
            }
        }

        private void point_Click(object sender, EventArgs e)
        {

        }

        private void MakeExam_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));

            
            lblRemainingTime.Text = remainingTime.ToString("mm\\:ss");

    
            if (remainingTime.TotalSeconds <= 0)
            {
                timer1.Stop();
                MessageBox.Show("Süre doldu! Sınav sonlandırılıyor.");
                finish_Click(sender, e);
            }


            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
