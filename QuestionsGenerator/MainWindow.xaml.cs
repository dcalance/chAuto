using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.Win32;
using System.Windows;

namespace QuestionsGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int currentQuestionBoxes = 1;
        List<List<QuestionLib.Question>> chapters = new List<List<QuestionLib.Question>>();

        public MainWindow()
        {
            InitializeComponent();

            Init();
        }
        private void newChapterBtn_Click(object sender, RoutedEventArgs e)
        {
            chapters.Add(new List<QuestionLib.Question>() { new QuestionLib.Question() });
            updateChapterList();
        }
        private void updateChapterList()
        {
            while (chaptersList.Items.Count > 0)
            {
                chaptersList.Items.RemoveAt(0);
            }
            for (int i = 0; i < chapters.Count; i++)
            {
                chaptersList.Items.Add("Chapter" + (i + 1));
            }
            chaptersList.SelectedIndex = 0;
        }
        private void updateQuestionList()
        {
            while (questionsList.Items.Count > 0)
            {
                questionsList.Items.RemoveAt(0);
            }

            if (chaptersList.SelectedIndex == -1) //kastily
            {
                chaptersList.SelectedIndex = 0;
            }

            for (int i = 0; i < chapters[chaptersList.SelectedIndex].Count; i++)
            {
                questionsList.Items.Add("Question" + (i + 1));
            }
            questionsList.SelectedIndex = 0;
        }
        private void updateFields()
        {
            questionBox.Text = chapters[chaptersList.SelectedIndex][questionsList.SelectedIndex].questionText;
            imageBox.Text = chapters[chaptersList.SelectedIndex][questionsList.SelectedIndex].imageName;
            correctAnswerBox.Text = chapters[chaptersList.SelectedIndex][questionsList.SelectedIndex].correctAnswer.ToString();
            correctAnswerText.Text = chapters[chaptersList.SelectedIndex][questionsList.SelectedIndex].correctAnswerText;
            if (chapters[chaptersList.SelectedIndex][questionsList.SelectedIndex].answers != null)
            {
                while (chapters[chaptersList.SelectedIndex][questionsList.SelectedIndex].answers.Length > currentQuestionBoxes)
                {
                    addAnswerBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                while (chapters[chaptersList.SelectedIndex][questionsList.SelectedIndex].answers.Length < currentQuestionBoxes)
                {
                    removeAnswerBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                for (int i = 0; i < chapters[chaptersList.SelectedIndex][questionsList.SelectedIndex].answers.Length; i++)
                {
                    TextBox element = FindChild<TextBox>(Application.Current.MainWindow, "answerBox" + (i + 1));
                    element.Text = chapters[chaptersList.SelectedIndex][questionsList.SelectedIndex].answers[i];
                }
            }
            else
            {
                answerBox1.Text = "";
                while (currentQuestionBoxes > 1)
                {
                    removeAnswerBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
            } 
        }
        private void saveChanges(int selectedChapter, int selectedQuestion)
        {
            List<string> answers = new List<string>();
            chapters[selectedChapter][selectedQuestion].questionText = questionBox.Text;
            chapters[selectedChapter][selectedQuestion].imageName = imageBox.Text;
            chapters[selectedChapter][selectedQuestion].correctAnswerText = correctAnswerText.Text;
            int result;
            int.TryParse(correctAnswerBox.Text, out result);
            chapters[selectedChapter][selectedQuestion].correctAnswer = result;
            int fieldNr = 1;
            TextBox element = FindChild<TextBox>(Application.Current.MainWindow, "answerBox" + fieldNr);
            while (element != null)
            {
                fieldNr += 1;
                answers.Add(element.Text);
                element = FindChild<TextBox>(Application.Current.MainWindow, "answerBox" + fieldNr);
            }
            chapters[selectedChapter][selectedQuestion].answers = answers.ToArray();
        }

        private void chaptersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (chaptersList.IsKeyboardFocusWithin)
            {
                ListBox prev = (ListBox)sender;
                if (chaptersList.Items.Count > 0)
                {
                    updateQuestionList();
                    updateFields();
                    //saveChanges(prev.SelectedIndex, questionsList.SelectedIndex);
                }
            }
        }
        private void newQuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            chapters[chaptersList.SelectedIndex].Add(new QuestionLib.Question());
            updateQuestionList();
        }
        private void deleteChapterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (chapters.Count > 1)
            {
                chapters.RemoveAt(chaptersList.SelectedIndex);
                chaptersList.SelectedIndex = 0;
                updateChapterList();
                updateQuestionList();
                updateFields();
            }
        }

        private void addAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionBoxes += 1;
            stackPanel.Children.Add(new Label() {Content = "Answer " + currentQuestionBoxes });
            stackPanel.Children.Add(new TextBox() { Name = "answerBox" + currentQuestionBoxes });
        }

        private void removeAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestionBoxes > 1)
            {
                currentQuestionBoxes -= 1;
                for (int i = 0; i < 2; i++)
                {
                    stackPanel.Children.RemoveAt(stackPanel.Children.Count - 1);
                }
            }
        }

        private void questionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (questionsList.IsKeyboardFocusWithin)
            {
                ListBox prev = (ListBox)sender;
                updateFields();
                //saveChanges(chaptersList.SelectedIndex, prev.SelectedIndex);
            }
        }

        private void saveChangesBtn_Click(object sender, RoutedEventArgs e)
        {
            saveChanges(chaptersList.SelectedIndex, questionsList.SelectedIndex);
        }

        private void deleteQuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (chapters[chaptersList.SelectedIndex].Count > 1)
            {
                chapters[chaptersList.SelectedIndex].RemoveAt(questionsList.SelectedIndex);
                questionsList.SelectedIndex = 0;
                updateQuestionList();
                updateFields();
            }
        }
        private void Init()
        {
            chapters.Add(new List<QuestionLib.Question>());
            updateChapterList();

            chapters[0].Add(new QuestionLib.Question());
            updateQuestionList();
        }

        private void clearAllBtn_Click(object sender, RoutedEventArgs e)
        {
            chapters = new List<List<QuestionLib.Question>>();
            Init();
            updateFields(); 
        }

        private void upChapterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (chaptersList.SelectedIndex > 0)
            {
                SwapChapters(chaptersList.SelectedIndex, chaptersList.SelectedIndex - 1);
                chaptersList.SelectedIndex -= 1;
                updateQuestionList();
            }
        }
        private void downChapterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (chaptersList.SelectedIndex < chaptersList.Items.Count - 1 && chaptersList.SelectedIndex >= 0)
            {
                SwapChapters(chaptersList.SelectedIndex, chaptersList.SelectedIndex + 1);
                chaptersList.SelectedIndex += 1;
                updateQuestionList();
            }
        }
        private void SwapChapters(int index1, int index2)
        {
            List<QuestionLib.Question> temp = chapters[index1];
            chapters[index1] = chapters[index2];
            chapters[index2] = temp;
        }
        private void SwapQuestions(int index1, int index2)
        {
            QuestionLib.Question temp = chapters[chaptersList.SelectedIndex][index1];
            chapters[chaptersList.SelectedIndex][index1] = chapters[chaptersList.SelectedIndex][index2];
            chapters[chaptersList.SelectedIndex][index2] = temp;
        }

        private void upQuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (questionsList.SelectedIndex > 0)
            {
                SwapQuestions(questionsList.SelectedIndex, questionsList.SelectedIndex - 1);
                questionsList.SelectedIndex -= 1;
                updateFields();
            }
        }

        private void downQuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (questionsList.SelectedIndex < questionsList.Items.Count - 1 && questionsList.SelectedIndex >= 0)
            {
                SwapQuestions(questionsList.SelectedIndex, questionsList.SelectedIndex + 1);
                questionsList.SelectedIndex += 1;
                updateFields();
            }
        }
        public static T FindChild<T>(DependencyObject parent, string childName)
        where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        private void saveAsBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "bin"; // Default file name
            dlg.DefaultExt = ".bin"; // Default file extension
            dlg.Filter = "Binary File (.bin)|*.bin"; // Filter files by extension
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                List<QuestionLib.Question[]> a = new List<QuestionLib.Question[]>();
                foreach (var item in chapters)
                {
                    a.Add(item.ToArray());
                }
                QuestionLib.Question[][] final = new QuestionLib.Question[chapters.Count][];
                for (int i = 0; i < a.Count; i++)
                {
                    final[i] = a[i];
                }

                var formatter = new DataContractSerializer(typeof(QuestionLib.Question[][]));
                using (Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.WriteObject(stream, final);
                }
            }
        }
        private void openBinaryBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var formatter = new DataContractSerializer(typeof(QuestionLib.Question[][]));
                QuestionLib.Question[][] deserealisedArray;
                using (Stream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    deserealisedArray = (QuestionLib.Question[][])formatter.ReadObject(stream);
                }
                chapters = new List<List<QuestionLib.Question>>();
                foreach (var item in deserealisedArray)
                {
                    chapters.Add(new List<QuestionLib.Question>());
                    foreach (var element in item)
                    {
                        chapters[chapters.Count - 1].Add(element);
                    }
                }
                updateChapterList();
                updateQuestionList();
                updateFields();
            }
                
        }
    }
}
