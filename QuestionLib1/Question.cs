﻿using System;
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
        public string questionText;
        public string imageName;
        public int correctAnswer;
        public string[] answers;
    }
}