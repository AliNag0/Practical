using System;

class Program
{
    static void Main()
    {
        Console.Write("Трехзначное число: ");
        int number;

        while (!int.TryParse(Console.ReadLine(), out number) || number <100 || number >999)
        {
            Console.WriteLine();
        }

        int sum = CalculateSumOfDigits(number);

        Console.WriteLine($"Сумма цифр числа {number} равна {sum}.");
    }

    static int CalculateSumOfDigits(int number)
    {
        int sum = 0;

        sum += number / 100;
        sum += (number / 10) % 10;
        sum += number % 10;

        return sum;
    }
}


