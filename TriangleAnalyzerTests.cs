using Xunit;
using Lab1_FIO;
using System.Linq;

namespace Lab1_FIO.Tests
{
    public class TriangleAnalyzerTests
    {
        private readonly TriangleAnalyzer _analyzer = new();

        #region 1. Корректное определение типов треугольников
        [Fact]
        public void Analyze_EquilateralTriangle_ReturnsEquilateralType()
        {
            var result = _analyzer.Analyze("5", "5", "5");
            Assert.Equal("равносторонний", result.Type);
        }

        [Fact]
        public void Analyze_IsoscelesTriangle_ReturnsIsoscelesType()
        {
            var result = _analyzer.Analyze("5", "5", "8");
            Assert.Equal("равнобедренный", result.Type);
        }

        [Fact]
        public void Analyze_ScaleneTriangle_ReturnsScaleneType()
        {
            var result = _analyzer.Analyze("3", "4", "5");
            Assert.Equal("разносторонний", result.Type);
        }

        [Fact]
        public void Analyze_FloatSides_CorrectlyIdentifiesType()
        {
            var result = _analyzer.Analyze("3.5", "4.2", "5.1");
            Assert.Equal("разносторонний", result.Type);
        }
        #endregion

        #region 2. Нечисловые и пустые входные данные
        [Fact]
        public void Analyze_NonNumericString_ReturnsEmptyType()
        {
            var result = _analyzer.Analyze("abc", "4", "5");
            Assert.Equal("", result.Type);
        }

        [Fact]
        public void Analyze_EmptyString_ReturnsEmptyType()
        {
            var result = _analyzer.Analyze("", "4", "5");
            Assert.Equal("", result.Type);
        }

        [Fact]
        public void Analyze_WhitespaceOnly_ReturnsEmptyType()
        {
            var result = _analyzer.Analyze("   ", "4", "5");
            Assert.Equal("", result.Type);
        }

        [Fact]
        public void Analyze_MixedNonNumeric_ReturnsEmptyType()
        {
            var result = _analyzer.Analyze("3", "xyz", "5");
            Assert.Equal("", result.Type);
        }

        [Fact]
        public void Analyze_NullInput_ReturnsEmptyType()
        {
            var result = _analyzer.Analyze(null!, "4", "5");
            Assert.Equal("", result.Type);
        }
        #endregion

        #region 3. Неположительные длины сторон
        [Fact]
        public void Analyze_ZeroSide_ReturnsNotTriangle()
        {
            var result = _analyzer.Analyze("0", "5", "5");
            Assert.Equal("не треугольник", result.Type);
        }

        [Fact]
        public void Analyze_NegativeSide_ReturnsNotTriangle()
        {
            var result = _analyzer.Analyze("-3", "4", "5");
            Assert.Equal("не треугольник", result.Type);
        }

        [Fact]
        public void Analyze_AllNegativeSides_ReturnsNotTriangle()
        {
            var result = _analyzer.Analyze("-5", "-5", "-5");
            Assert.Equal("не треугольник", result.Type);
        }
        #endregion

        #region 4. Нарушение неравенства треугольника
        [Fact]
        public void Analyze_SumOfTwoLessThanThird_ReturnsNotTriangle()
        {
            var result = _analyzer.Analyze("1", "2", "10");
            Assert.Equal("не треугольник", result.Type);
        }

        [Fact]
        public void Analyze_DegenerateTriangle_ReturnsNotTriangle()
        {
            var result = _analyzer.Analyze("1", "2", "3"); // a+b=c
            Assert.Equal("не треугольник", result.Type);
        }

        [Fact]
        public void Analyze_InequalityViolationCase2_ReturnsNotTriangle()
        {
            var result = _analyzer.Analyze("10", "2", "1");
            Assert.Equal("не треугольник", result.Type);
        }
        #endregion

        #region 5. Проверка координат
        [Fact]
        public void Analyze_ValidTriangle_CoordinatesWithin0To100()
        {
            var result = _analyzer.Analyze("3", "4", "5");
            foreach (var coord in result.Coordinates)
            {
                Assert.InRange(coord.X, 0, 100);
                Assert.InRange(coord.Y, 0, 100);
            }
        }

        [Fact]
        public void Analyze_InvalidTriangle_CoordinatesAreMinusOne()
        {
            var result = _analyzer.Analyze("1", "2", "10");
            Assert.All(result.Coordinates, c => Assert.Equal((-1, -1), c));
        }

        [Fact]
        public void Analyze_NonNumeric_CoordinatesAreMinusTwo()
        {
            var result = _analyzer.Analyze("abc", "2", "3");
            Assert.All(result.Coordinates, c => Assert.Equal((-2, -2), c));
        }

        [Fact]
        public void Analyze_LargeTriangle_ScalesProperly()
        {
            var result = _analyzer.Analyze("1000", "1000", "1000");
            // Все координаты должны остаться в пределах поля 100x100
            Assert.All(result.Coordinates, c =>
            {
                Assert.InRange(c.X, 0, 100);
                Assert.InRange(c.Y, 0, 100);
            });
        }
        #endregion

        #region 6. Граничные случаи и точность float
        [Fact]
        public void Analyze_VerySmallTriangle_ReturnsValidType()
        {
            var result = _analyzer.Analyze("0.001", "0.001", "0.001");
            Assert.Equal("равносторонний", result.Type);
        }

        [Fact]
        public void Analyze_FloatPrecisionEdge_IsoscelesDetected()
        {
            // a и b почти равны, разница в пределах epsilon
            var result = _analyzer.Analyze("5.0000001", "5.0000002", "8");
            Assert.Equal("равнобедренный", result.Type);
        }

        [Fact]
        public void Analyze_CommaAsDecimalSeparator_ReturnsEmptyType()
        {
            // InvariantCulture требует точку, запятая должна считаться ошибкой
            var result = _analyzer.Analyze("3,5", "4,2", "5,1");
            Assert.Equal("", result.Type);
        }
        #endregion
    }
}