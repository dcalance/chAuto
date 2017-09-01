using System;
using System.Collections.Generic;
using System.Text;

namespace chAuto
{
    [Serializable]
    public class Question
    {
        string questionText { get; set; }
        string imageName { get; set; }
        int correctAnswer { get; set; }
        string[] answers { get; set; }
    }
}
