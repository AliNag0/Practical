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
using System.Windows.Threading;

namespace Game
{
    public partial class MainWindow : Window
    {
        private List<Card> cards = new List<Card>();
        private Card firstSelected = null;
        private Card secondSelected = null;
        private DispatcherTimer timer = new DispatcherTimer();
        private int moveCount = 0;

        public MainWindow()
        {
            InitializeComponent(); 

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            NewGame();

        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void NewGame()
        {
            
            moveCount = 0;
            tbMoves.Text = "Ходов: 0";

           
            gameGrid.Children.Clear();
            cards.Clear();
            firstSelected = null;
            secondSelected = null;

          
            var values = new List<string>()
        {
            "Red", "Green", "Blue", "Yellow",
            "Orange", "Purple", "Cyan", "Pink"
        };

           
            var cardValues = values.Concat(values).ToList();

           
            Shuffle(cardValues);

           
            foreach (var value in cardValues)
            {
                var button = new Button
                {
                    Content = "", 
                    FontSize = 20,
                    Tag = value,
                    Background = Brushes.Gray
                };
                button.Click += Card_Click;

                var card = new Card
                {
                    Button = button,
                    Value = value
                };

                cards.Add(card);
                gameGrid.Children.Add(button);
            }
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled) return; 

            var clickedButton = sender as Button;
            var selectedCard = cards.FirstOrDefault(c => c.Button == clickedButton);

            if (selectedCard == null || selectedCard.IsMatched || selectedCard.Button.Content.ToString() != "")
                return;

           
            RevealCard(selectedCard);

            if (firstSelected == null)
            {
                firstSelected = selectedCard;
            }
            else if (secondSelected == null)
            {
                secondSelected = selectedCard;
                moveCount++;
                tbMoves.Text = $"Ходов: {moveCount}";

                if (firstSelected.Value == secondSelected.Value)
                {
                    firstSelected.IsMatched = true;
                    secondSelected.IsMatched = true;

                    firstSelected = null;
                    secondSelected = null;

                    CheckWinCondition();
                }
                else
                {
                    timer.Start();
                }
            }
        }

        private void RevealCard(Card card)
        {
            switch (card.Value)
            {
                case "Red": card.Button.Background = Brushes.Red; break;
                case "Green": card.Button.Background = Brushes.Green; break;
                case "Blue": card.Button.Background = Brushes.Blue; break;
                case "Yellow": card.Button.Background = Brushes.Yellow; break;
                case "Orange": card.Button.Background = Brushes.Orange; break;
                case "Purple": card.Button.Background = Brushes.Purple; break;
                case "Cyan": card.Button.Background = Brushes.Cyan; break;
                case "Pink": card.Button.Background = Brushes.Pink; break;
            }
        }

        private void HideCards()
        {
            firstSelected.Button.Background = Brushes.Gray;
            secondSelected.Button.Background = Brushes.Gray;

            firstSelected = null;
            secondSelected = null;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            HideCards();
        }

        private void CheckWinCondition()
        {
            if (cards.All(c => c.IsMatched))
            {
                MessageBox.Show($"Поздравляем! Вы нашли все пары за {moveCount} ходов.", "Игра окончена");
            }
        }

        
        private void Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}