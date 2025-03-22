using System;

class Program
{
    static void Main()
    {
        double x = 1;
        double y = 1;

        double result1 = Math.Sqrt(Math.Pow(x, 8) + 8 * Math.Pow(x, 1)) - (1 / (5 * Math.Sqrt(Math.Pow(x, 8) + 8 * Math.Pow(x, 1)))) * Math.Sin(-1 / x);
        double result2 = (1 - Math.Tan(Math.Pow(x, Math.Cos((x - y)))));
        double result3 = Math.Pow(1 + 1 / Math.Pow(x, 2), x) - 12 * Math.Pow(x, 2) * y;
        double result4 = Math.Pow(x, y);

        Console.WriteLine("Результат");
        Console.WriteLine($"Результат 1: {result1}");
        Console.WriteLine($"Результат 2: {result2}");
        Console.WriteLine($"Результат 3: {result3}");
        Console.WriteLine($"Результат 4: {result4}");
    }
}
// (1−tgx )ctg x+cos ( x− y )