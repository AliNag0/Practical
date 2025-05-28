using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestProjects;

/// <summary>
/// Логика взаимодействия для MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<Question> questions = new List<Question>();
    private int currentQuestion = 0;
    private int correctAnswers = 0;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtQuestion.Text))
        {
            MessageBox.Show("Введите вопрос");
            return;
        }

        var answers = new string[]
        {
            txtAnswer1.Text,
            txtAnswer2.Text,
            txtAnswer3.Text,
        };

        int correctIndex = rb1.IsChecked == true ? 0 :
                          rb2.IsChecked == true ? 1 :
                          rb3.IsChecked == true ? 2 : 3;

        questions.Add(new Question
        {
            Text = txtQuestion.Text,
            Answers = answers,
            CorrectAnswer = correctIndex
        });

        txtQuestion.Text = "";
        txtAnswer1.Text = "";
        txtAnswer2.Text = "";
        txtAnswer3.Text = "";
        rb1.IsChecked = true;

        MessageBox.Show("Вопрос добавлен! Всего вопросов: " + questions.Count);
    }

    private void BtnStartTest_Click(object sender, RoutedEventArgs e)
    {
        if (questions.Count == 0)
        {
            MessageBox.Show("Добавьте хотя бы один вопрос");
            return;
        }

        editPanel.Visibility = Visibility.Collapsed;
        testPanel.Visibility = Visibility.Visible;
        currentQuestion = 0;
        correctAnswers = 0;
        ShowQuestion(0);
    }

    private void ShowQuestion(int index)
    {
        if (index < 0 || index >= questions.Count) return;

        currentQuestion = index;
        var q = questions[index];

        lblQuestion.Content = (index + 1) + ". " + q.Text;
        testRb1.Content = q.Answers[0];
        testRb2.Content = q.Answers[1];
        testRb3.Content = q.Answers[2];
        testRb4.Content = q.Answers[3];

        testRb1.IsChecked = false;
        testRb2.IsChecked = false;
        testRb3.IsChecked = false;
        testRb4.IsChecked = false;

        btnPrev.IsEnabled = index > 0;
        btnNext.Content = index < questions.Count - 1 ? "Далее" : "Завершить";
    }

    private void BtnPrev_Click(object sender, RoutedEventArgs e)
    {
        SaveAnswer();
        ShowQuestion(currentQuestion - 1);
    }

    private void BtnNext_Click(object sender, RoutedEventArgs e)
    {
        SaveAnswer();

        if (currentQuestion < questions.Count - 1)
        {
            ShowQuestion(currentQuestion + 1);
        }
        else
        {
            ShowResults();
        }
    }

    private void SaveAnswer()
    {
        if (!(testRb1.IsChecked == true || testRb2.IsChecked == true ||
              testRb3.IsChecked == true || testRb4.IsChecked == true))
            return;

        int selectedAnswer = testRb1.IsChecked == true ? 0 :
                           testRb2.IsChecked == true ? 1 :
                           testRb3.IsChecked == true ? 2 : 3;

        if (selectedAnswer == questions[currentQuestion].CorrectAnswer)
            correctAnswers++;
    }
    private void ShowResults()
    {
        testPanel.Visibility = Visibility.Collapsed;
        resultPanel.Visibility = Visibility.Visible;

        double percentage = (double)correctAnswers / questions.Count * 100;
        lblResult.Content = $"Результат: {correctAnswers} из {questions.Count}\n" +
                          $"({percentage:0}% правильных ответов)";
    }

    private void BtnRestart_Click(object sender, RoutedEventArgs e)
    {
        questions.Clear();
        resultPanel.Visibility = Visibility.Collapsed;
        editPanel.Visibility = Visibility.Visible;
    }
}
