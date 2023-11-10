using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DatabaseLabs
{
    internal class Program
    {
        static Exception loopException = new Exception("\n\n** Answer Wrong Format. needs to be a whole number! **\n\n");
        const string CONNECTION_STRING = "Server=localhost;Database=Quiz;Uid=student;Pwd=secret;";

        static Random random = new Random();
        static void Main(string[] args)
        {
            List<Question> questions = GetQuestions("select * from questions");


            string input = null;
            
            while (true)
            {
                try
                {

                    Console.WriteLine("\nWrite '#' at any time if you want to exit");
                    if(input == null)input = UserInput();
                    Question question = questions[random.Next(Math.Max(questions.Count, 0))];
                    Console.WriteLine(question);
                    for (int i = 0; i < question.Answers.Count; i++)
                        Console.WriteLine("{0}. {1}", i + 1, question.Answers[i]);

                    Console.Write("\nChoose number: ");
                    string answer = UserInput();
                    int num;
                    if (!int.TryParse(answer, out num))
                        throw new Exception("\n\n** Answer Wrong Format. needs to be a whole number! **\n\n", new Exception("loop"));
                    Console.WriteLine(question.CheckAnswer(num - 1) ? "\nCorrect!" : "\nIncorrect. ");
                    Console.WriteLine("The Answer is: {0}", question.GetCorrectAnswerText());
                }catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    if(ex == loopException) continue;
                    break;
                }
            }
        }

        static string UserInput()
        {
            string input = Console.ReadLine();
            if (input == "#") throw new Exception("User entered exit string");
            return input;
        }

        static List<Question> GetQuestions(string executionCommand)
        {
            try
            {
                List<Question> questions = new List<Question>();
                StringBuilder builder = new StringBuilder();
                using (MySqlConnection conn = new MySqlConnection(CONNECTION_STRING))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(executionCommand, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            questions.Add(new Question(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
                        }
                        reader.Close();
                    }
                    conn.Close();
                }
                UpdateQuestions(ref questions);
                return questions;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        static void UpdateQuestions(ref List<Question> questions)
        {
            using (MySqlConnection connection = new MySqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("select * from Answers", connection))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    foreach (Question question in questions)
                    {
                        cmd.CommandText = $"Select * from Answers Where qid = {question.ID}";
                        connection.Close();
                        connection.Open() ;
                        reader = cmd.ExecuteReader();
                        List<Answer> answers = new List<Answer>();
                        while (reader.Read()) answers.Add(new Answer(reader.GetInt32(0), reader.GetInt32(2), reader.GetString(3)));
                        question.Answers = answers;
                        reader.Close();
                        connection.Close();
                    }
                    reader.Dispose();
                }
            }
        }
    }
}
