using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HotelProjects
{
    public partial class MainWindow : Window
    {
        private List<Booking> bookings = new List<Booking>();
        private string dataFilePath = Path.Combine(
     AppDomain.CurrentDomain.BaseDirectory,
     "bookings.dat"
 );
        private string currentImagePath = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            LoadBookings();
            dpCheckIn.SelectedDate = DateTime.Today;
            dpCheckOut.SelectedDate = DateTime.Today.AddDays(1);

            // Для отладки - покажем где будет сохраняться файл
            Console.WriteLine($"Файл данных: {dataFilePath}");
        }

        private void BtnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                currentImagePath = openFileDialog.FileName;
                imgClient.Source = new BitmapImage(new Uri(currentImagePath));
            }
        }

        private void BtnBook_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка ввода
                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Введите ФИО клиента");
                    return;
                }

                var roomType = (cmbRoomType.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (string.IsNullOrEmpty(roomType))
                {
                    MessageBox.Show("Выберите тип номера");
                    return;
                }

                if (dpCheckIn.SelectedDate == null || dpCheckOut.SelectedDate == null)
                {
                    MessageBox.Show("Выберите даты заезда и выезда");
                    return;
                }

                if (dpCheckOut.SelectedDate <= dpCheckIn.SelectedDate)
                {
                    MessageBox.Show("Дата выезда должна быть позже даты заезда");
                    return;
                }


                decimal pricePerNight = GetPricePerNight(roomType);
                int nights = (int)(dpCheckOut.SelectedDate.Value - dpCheckIn.SelectedDate.Value).TotalDays;
                decimal totalPrice = pricePerNight * nights;

                var booking = new Booking
                {
                    FullName = txtFullName.Text,
                    RoomType = roomType,
                    CheckInDate = dpCheckIn.SelectedDate.Value,
                    CheckOutDate = dpCheckOut.SelectedDate.Value,
                    ImagePath = currentImagePath,
                    TotalPrice = totalPrice
                };

                // Добавление и сохранение
                bookings.Add(booking);
                SaveBookings();
                RefreshBookingsList();
                ClearForm();

                MessageBox.Show($"Бронирование успешно создано! Стоимость: {totalPrice:C}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при бронировании: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            try
            {
                txtFullName.Clear();
                cmbRoomType.SelectedIndex = -1;
                dpCheckIn.SelectedDate = DateTime.Today;
                dpCheckOut.SelectedDate = DateTime.Today.AddDays(1);
                imgClient.Source = null;
                currentImagePath = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при очистке формы: {ex.Message}");
            }
        }

        private decimal GetPricePerNight(string roomType)
        {
            switch (roomType)
            {
                case "Одноместный":
                    return 2000m;
                case "Двухместный":
                    return 3000m;
                case "Люкс":
                    return 5000m;
                case "Президентский":
                    return 10000m;
                default:
                    return 0m;
            }
        }

        private void DgBookings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgBookings.SelectedItem is Booking selectedBooking)
            {
                imgSelectedBooking.Source = selectedBooking.Image;
            }
        }

        private void LoadBookings()
        {
            try
            {
                if (File.Exists(dataFilePath))
                {
                    using (FileStream fs = new FileStream(dataFilePath, FileMode.Open))
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        bookings = new List<Booking>();
                        int count = reader.ReadInt32();

                        for (int i = 0; i < count; i++)
                        {
                            var booking = new Booking
                            {
                                FullName = reader.ReadString(),
                                RoomType = reader.ReadString(),
                                CheckInDate = DateTime.FromBinary(reader.ReadInt64()),
                                CheckOutDate = DateTime.FromBinary(reader.ReadInt64()),
                                ImagePath = reader.ReadString(),
                                TotalPrice = reader.ReadDecimal()
                            };

                            bookings.Add(booking);
                        }
                    }
                    RefreshBookingsList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void SaveBookings()
        {
            try
            {
                // Создаем директорию, если её нет
                string directory = Path.GetDirectoryName(dataFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (FileStream fs = new FileStream(dataFilePath, FileMode.Create))
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    writer.Write(bookings.Count);
                    foreach (var booking in bookings)
                    {
                        writer.Write(booking.FullName);
                        writer.Write(booking.RoomType);
                        writer.Write(booking.CheckInDate.ToBinary());
                        writer.Write(booking.CheckOutDate.ToBinary());
                        writer.Write(booking.ImagePath ?? "");
                        writer.Write(booking.TotalPrice);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void RefreshBookingsList()
        {
            Debug.WriteLine($"Обновление списка. Количество бронирований: {bookings.Count}");
            foreach (var b in bookings)
            {
                Debug.WriteLine($"{b.FullName} - {b.RoomType}");
            }

            dgBookings.ItemsSource = null;
            dgBookings.ItemsSource = bookings;
        }
    }

}