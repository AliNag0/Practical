using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Cards
{
    public partial class MainWindow : Window
    {
        private List<Card> originalCards;
        private List<Card> currentCards;
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            InitializeCards();
        }

        private void InitializeCards()
        {
            originalCards = GenerateRandomCards(10);
            currentCards = new List<Card>(originalCards);
            UpdateCardsDisplay();
        }

        private List<Card> GenerateRandomCards(int count)
        {
            var cards = new List<Card>();
            var cardValues = Enum.GetValues(typeof(CardEnum)).Cast<CardEnum>().ToList();

            for (int i = 0; i < count; i++)
            {
                var randomValue = cardValues[random.Next(cardValues.Count)];
                cards.Add(new Card
                {
                    Key = i + 1,
                    Name = randomValue.ToString(),
                    Value = randomValue
                });
                cardValues.Remove(randomValue);

                if (cardValues.Count == 0)
                    cardValues = Enum.GetValues(typeof(CardEnum)).Cast<CardEnum>().ToList();
            }

            return cards;
        }

        private void UpdateCardsDisplay()
        {
            CardsListBox.ItemsSource = null;
            CardsListBox.ItemsSource = currentCards;
        }

        private void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            currentCards = currentCards.OrderBy(c => random.Next()).ToList();
            UpdateCardsDisplay();
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            currentCards = currentCards.OrderBy(c => c.Name).ToList();
            UpdateCardsDisplay();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            currentCards = new List<Card>(originalCards);
            UpdateCardsDisplay();
            SearchTextBox.Clear();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                MessageBox.Show("Введите название карты для поиска");
                return;
            }

            var searchText = SearchTextBox.Text.ToLower();
            var foundCards = originalCards
                .Where(c => c.Name.ToLower().Contains(searchText))
                .ToList();

            CardsListBox.ItemsSource = foundCards;
        }
    }
}