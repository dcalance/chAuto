using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionsGenerator
{
    [Serializable]
    public class Question
    {
        public string questionText { get; set; }
        public string imageName { get; set; }
        public int correctAnswer { get; set; }
        public string[] answers { get; set; }
    }
}
