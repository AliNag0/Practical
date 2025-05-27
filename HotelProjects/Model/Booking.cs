using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public class Booking
{
    public string FullName { get; set; }
    public string RoomType { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public string ImagePath { get; set; }
    public decimal TotalPrice { get; set; }

    public ImageSource Image
    {
        get
        {
            if (!string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(ImagePath);
                image.EndInit();
                return image;
            }
            return null;
        }
    }
}