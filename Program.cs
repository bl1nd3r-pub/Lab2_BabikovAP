using System;     
using System.Linq;

namespace Lab1_FIO
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Определение типа треугольника и координат вершин ===");
            Console.WriteLine("Для выхода введите 'exit', 'quit' или просто нажмите Enter без ввода.\n");

            while (true)
            {
                Console.WriteLine(new string('-', 50));

                Console.Write("Сторона A (или exit для выхода): ");
                string s1 = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(s1) || s1.Trim().ToLower() == "exit" || s1.Trim().ToLower() == "quit")
                    break;

                Console.Write("Сторона B: ");
                string s2 = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(s2)) break;

                Console.Write("Сторона C: ");
                string s3 = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(s3)) break;

                // Защита от null
                s1 ??= "";
                s2 ??= "";
                s3 ??= "";

                var analyzer = new TriangleAnalyzer();
                var res = analyzer.Analyze(s1, s2, s3);

            }

            Console.WriteLine("\n Завершение работы. Логи сохранены в файлах triangle_lab_*.log");
        }
    }
}
