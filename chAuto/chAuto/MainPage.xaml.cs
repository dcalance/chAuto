using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace chAuto
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}
        void OnPracticeClicked(object sender, EventArgs args)
        {
            Navigation.PushModalAsync(new Practice());
        }

        private void OnExamClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Exam());
        }
    }
}
