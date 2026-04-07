using System;     
using System.Linq;

namespace Lab1_FIO
{
    internal class Program
    {
        static void Main()
        {

            Console.WriteLine("=== Определение типа треугольника и координат вершин ===");
            Console.WriteLine("Введите три строки (длины сторон A, B, C). Пустой ввод будет обработан как ошибка.\n");

            Console.Write("Сторона A: ");
            string s1 = Console.ReadLine();

            Console.Write("Сторона B: ");
            string s2 = Console.ReadLine();

            Console.Write("Сторона C: ");
            string s3 = Console.ReadLine();

            // Защита от null (на случай закрытия потока ввода)
            s1 ??= "";
            s2 ??= "";
            s3 ??= "";

            var analyzer = new TriangleAnalyzer();
            var res = analyzer.Analyze(s1, s2, s3);

            Console.WriteLine("\n--- Результат ---");
            Console.WriteLine($"Тип: '{res.Type}'");
            Console.WriteLine($"Координаты вершин: [{string.Join(", ", res.Coordinates.Select(c => $"({c.X},{c.Y})"))}]");
        }
    }
}
