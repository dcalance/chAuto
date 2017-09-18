using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace QuestionLib
{
    [DataContract]
    public class Question
    {
        [DataMember]
        public string questionText;
        [DataMember]
        public string imageName;
        [DataMember]
        public int correctAnswer;
        [DataMember]
        public string[] answers;
        [DataMember]
        public string correctAnswerText;
    }
}
