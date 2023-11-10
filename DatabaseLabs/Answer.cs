using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLabs
{
    internal class Answer
    {
        public string Text { get; set; }
        private int _id;
        private int _optId;
        public int ID { get { return _id; } }
        public int OptionID { get {  return _optId; } }

        public Answer(int id, int optionID, string text) {
            _id = id;
            _optId = optionID;
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
