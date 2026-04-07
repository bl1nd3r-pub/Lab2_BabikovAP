using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_FIO
{
    public class TriangleAnalyzer
    {
        // Допуск для сравнения вещественных чисел
        private const float Epsilon = 1e-5f;

        /// <summary>
        /// Анализирует входные строки, определяет тип треугольника и вычисляет координаты вершин.
        /// </summary>
        public (string Type, (int X, int Y)[] Coordinates) Analyze(string s1, string s2, string s3)
        {
            string requestStr = $"[{s1}, {s2}, {s3}]";
            var notTriangleCoords = new[] { (-1, -1), (-1, -1), (-1, -1) };
            var invalidCoords = new[] { (-2, -2), (-2, -2), (-2, -2) };

            try
            {
                // 1. Парсинг входных данных
                bool p1 = float.TryParse(s1, NumberStyles.Float, CultureInfo.InvariantCulture, out float a);
                bool p2 = float.TryParse(s2, NumberStyles.Float, CultureInfo.InvariantCulture, out float b);
                bool p3 = float.TryParse(s3, NumberStyles.Float, CultureInfo.InvariantCulture, out float c);

                if (!p1 || !p2 || !p3)
                {
                    string type = "";
                    Logger.LogFailure(requestStr, $"Тип='{type}', Координаты=(-2,-2).\nПричина: нечисловые данные.");
                    return (type, invalidCoords);
                }

                // 2. Проверка на положительные числа
                if (a <= 0 || b <= 0 || c <= 0)
                {
                    string type = "не треугольник";
                    Logger.LogFailure(requestStr, $"Тип='{type}', Координаты=(-1,-1).\nПричина: неположительные значения.");
                    return (type, notTriangleCoords);
                }

                // 3. Проверка неравенства треугольника
                bool isTriangle = (a + b > c + Epsilon) && (a + c > b + Epsilon) && (b + c > a + Epsilon);

                if (!isTriangle)
                {
                    string type = "не треугольник";
                    Logger.LogFailure(requestStr, $"Тип='{type}', Координаты=(-1,-1).\nПричина: нарушено неравенство треугольника.");
                    return (type, notTriangleCoords);
                }

                // 4. Определение типа
                bool ab = Math.Abs(a - b) < Epsilon;
                bool bc = Math.Abs(b - c) < Epsilon;
                bool ac = Math.Abs(a - c) < Epsilon;

                string triangleType = (ab && bc) ? "равносторонний"
                                    : (ab || bc || ac) ? "равнобедренный"
                                    : "разносторонний";

                // 5. Вычисление координат
                var coords = CalculateScaledCoordinates(a, b, c);
                string coordsStr = string.Join(", ", coords.Select(c => $"({c.X},{c.Y})"));

                // 6. Логирование успешного запроса
                Logger.LogSuccess(requestStr, triangleType, coordsStr);

                return (triangleType, coords);
            }
            catch (Exception ex)
            {
                // Фоллбэк на случай непредвиденных ошибок
                Logger.LogFailure(requestStr, $"Критическая ошибка: {ex.Message}", ex.StackTrace);
                return ("", invalidCoords);
            }
        }

        /// <summary>
        /// Масштабирует треугольник так, чтобы он помещался в поле 100x100 px.
        /// </summary>
        private (int X, int Y)[] CalculateScaledCoordinates(float a, float b, float c)
        {
            // Размещаем вершину 1 в (0,0), вершину 2 на оси X на расстоянии a
            float baseLen = a;
            float x = (baseLen * baseLen + b * b - c * c) / (2 * baseLen);
            float y = (float)Math.Sqrt(Math.Max(0, b * b - x * x));

            var rawPoints = new[] { (0f, 0f), (baseLen, 0f), (x, y) };

            // Находим границы для масштабирования
            float minX = rawPoints.Min(p => p.Item1);
            float maxX = rawPoints.Max(p => p.Item1);
            float minY = rawPoints.Min(p => p.Item2);
            float maxY = rawPoints.Max(p => p.Item2);

            float width = maxX - minX;
            float height = maxY - minY;

            // Вычисляем коэффициент масштабирования (сохраняем пропорции)
            float scale = Math.Min(100f / (width > Epsilon ? width : 1), 100f / (height > Epsilon ? height : 1));

            var result = new (int, int)[3];
            for (int i = 0; i < 3; i++)
            {
                int sx = (int)Math.Round((rawPoints[i].Item1 - minX) * scale);
                int sy = (int)Math.Round((rawPoints[i].Item2 - minY) * scale);
                result[i] = (Math.Clamp(sx, 0, 100), Math.Clamp(sy, 0, 100));
            }

            return result;
        }
    }
}
