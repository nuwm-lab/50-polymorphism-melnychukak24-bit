using System;
using System.Globalization;

namespace VectorSystemApp
{
    /// <summary>
    /// Абстрактний базовий клас для системи векторів.
    /// Реалізує IDisposable для правильного звільнення ресурсів.
    /// </summary>
    public abstract class VectorSystemBase : IDisposable
    {
        // Константа для перевірки нульового детермінанта (замість "магічного числа")
        protected const double Epsilon = 1e-9;

        // Захищене поле для зберігання векторів (масив масивів)
        protected double[][] Vectors;

        private bool _disposed; // прапорець для контролю повторного виклику Dispose

        /// <summary>
        /// Конструктор базового класу для ініціалізації системи векторів.
        /// </summary>
        /// <param name="vectorCount">Кількість векторів</param>
        /// <param name="dimension">Розмірність кожного вектора</param>
        protected VectorSystemBase(int vectorCount, int dimension)
        {
            if (vectorCount <= 0)
                throw new ArgumentException("Кількість векторів має бути більшою за 0.");

            if (dimension <= 0)
                throw new ArgumentException("Розмірність має бути більшою за 0.");

            Vectors = new double[vectorCount][];
            for (int i = 0; i < vectorCount; i++)
                Vectors[i] = new double[dimension];
        }

        /// <summary>
        /// Абстрактний метод для перевірки лінійної незалежності.
        /// </summary>
        public abstract bool IsLinearlyIndependent();

        /// <summary>
        /// Введення координат векторів користувачем.
        /// </summary>
        public virtual void InputVectors()
        {
            for (int i = 0; i < Vectors.Length; i++)
            {
                Console.WriteLine($"\nВведіть координати вектора {i + 1}:");
                for (int j = 0; j < Vectors[i].Length; j++)
                {
                    double value;
                    while (true)
                    {
                        Console.Write($"  Координата {j + 1}: ");
                        string? input = Console.ReadLine();
                        if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                            break;

                        Console.WriteLine("  ❌ Невірний формат. Спробуйте ще раз (використовуйте крапку як роздільник).");
                    }
                    Vectors[i][j] = value;
                }
            }
        }

        /// <summary>
        /// Виведення координат усіх векторів.
        /// </summary>
        public virtual void DisplayVectors()
        {
            Console.WriteLine("\n--- Вектори системи ---");
            for (int i = 0; i < Vectors.Length; i++)
            {
                Console.Write($"Вектор {i + 1}: ");
                Console.WriteLine(string.Join("  ", Vectors[i]));
            }
        }

        /// <summary>
        /// Реалізація шаблону IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Захищений метод для звільнення ресурсів.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Тут можна звільняти керовані ресурси, якщо вони є
                Vectors = null!;
            }

            // Некеровані ресурси (якби були) звільняються тут

            _disposed = true;
        }

        /// <summary>
        /// Фіналізатор (викликається лише якщо Dispose() не був викликаний).
        /// </summary>
        ~VectorSystemBase()
        {
            Dispose(false);
        }
    }

    /// <summary>
    /// Клас для системи з двох векторів (перевірка лінійної незалежності).
    /// </summary>
    public class TwoVectors : VectorSystemBase
    {
        public TwoVectors() : base(2, 2) { }

        public override bool IsLinearlyIndependent()
        {
            double determinant = Vectors[0][0] * Vectors[1][1] - Vectors[0][1] * Vectors[1][0];
            return Math.Abs(determinant) > Epsilon;
        }
    }

    /// <summary>
    /// Клас для системи з трьох векторів (у тривимірному просторі).
    /// </summary>
    public class ThreeVectors : VectorSystemBase
    {
        public ThreeVectors() : base(3, 3) { }

        public override bool IsLinearlyIndependent()
        {
            double det =
                Vectors[0][0] * (Vectors[1][1] * Vectors[2][2] - Vectors[1][2] * Vectors[2][1]) -
                Vectors[0][1] * (Vectors[1][0] * Vectors[2][2] - Vectors[1][2] * Vectors[2][0]) +
                Vectors[0][2] * (Vectors[1][0] * Vectors[2][1] - Vectors[1][1] * Vectors[2][0]);

            return Math.Abs(det) > Epsilon;
        }
    }

    /// <summary>
    /// Головний клас програми.
    /// </summary>
    internal static class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Перевірка лінійної незалежності векторів ===");
            Console.WriteLine("1 — система з 2 векторів (у площині)");
            Console.WriteLine("2 — система з 3 векторів (у просторі)");
            Console.Write("Ваш вибір: ");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
            {
                Console.Write("❌ Вибір невірний. Введіть 1 або 2: ");
            }

            using VectorSystemBase system = (choice == 1) ? new TwoVectors() : new ThreeVectors();

            system.InputVectors();
            system.DisplayVectors();

            bool independent = system.IsLinearlyIndependent();
            Console.WriteLine($"\nРезультат: {(independent ? "✅ Вектори лінійно незалежні" : "❌ Вектори лінійно залежні")}");
        }
    }
}
