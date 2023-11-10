using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLabs
{
    internal class Question
    {
        private int _id;
        private string _text;
        private int _correctAnswer;

        public string Text { get { return _text; }}
        public int ID { get { return _id; }}
        public List<Answer> Answers { get; set; }

        public Question(int id, string text, int correctAnswer)
        {
            _id = id; 
            _text = text;
            _correctAnswer = correctAnswer;
        }

        public override string ToString()
        {
            return _text;
        }

        public Answer GetAnswer(int id)
        {
            if (id > Answers.Count || id < 0) return null;
            return Answers[id];
        }
        public string GetCorrectAnswerText()
        {
            return Answers[_correctAnswer].ToString();
        }
        public bool CheckAnswer(int num) => num == _correctAnswer;
    }
}
