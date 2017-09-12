﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using QuestionLib;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;


namespace chAuto
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Exam : ContentPage
	{
		public Exam()
		{
			InitializeComponent ();
            IFormatter formatter = new BinaryFormatter();
            Question[][] chapters;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "chAuto.Droid.test.bin";
            Stream stream = assembly.GetManifestResourceStream(resourceName);
            //Stream stream = new FileStream("test.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
            chapters = formatter.Deserialize(stream) as Question[][];
            stream.Close();
            string finalText = "";

            foreach (var item in chapters)
            {
                foreach (var el in item)
                {
                    finalText += el.correctAnswer;
                    finalText += el.imageName;
                    finalText += el.questionText;
                    foreach (var answer in el.answers)
                    {
                        finalText += answer;
                    }
                }
            }
            textField.Text = finalText;
		}
        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}