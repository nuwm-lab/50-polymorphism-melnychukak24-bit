using System;
using System.Globalization;

namespace VectorSystemApp
{
    /// <summary>
    /// Абстрактний базовий клас для системи векторів.
    /// Реалізує базові методи введення, виведення та звільнення ресурсів.
    /// </summary>
    public abstract class VectorSystemBase : IDisposable
    {
        /// <summary>
        /// Константа точності для порівняння з нулем.
        /// </summary>
        protected const double DeterminantEpsilon = 1e-9;

        private readonly double[][] _vectors;
        private bool _disposed;

        /// <summary>
        /// Наданий захищений доступ до векторів для похідних класів.
        /// </summary>
        protected double[][] Vectors => _vectors;

        /// <summary>
        /// Конструктор базового класу.
        /// </summary>
        protected VectorSystemBase(int vectorCount, int dimension)
        {
            if (vectorCount <= 0)
                throw new ArgumentException("Кількість векторів має бути більшою за 0.");
            if (dimension <= 0)
                throw new ArgumentException("Розмірність має бути більшою за 0.");

            _vectors = new double[vectorCount][];
            for (int i = 0; i < vectorCount; i++)
                _vectors[i] = new double[dimension];
        }

        /// <summary>
        /// Абстрактний метод для перевірки лінійної незалежності векторів.
        /// </summary>
        public abstract bool IsLinearlyIndependent();

        /// <summary>
        /// Введення координат векторів користувачем.
        /// Може бути перевизначений у похідних класах для специфічного форматування.
        /// </summary>
        public virtual void InputVectors()
        {
            for (int i = 0; i < _vectors.Length; i++)
            {
                Console.WriteLine($"\nВведіть координати вектора {i + 1}:");
                for (int j = 0; j < _vectors[i].Length; j++)
                {
                    double value;
                    while (true)
                    {
                        Console.Write($"  Координата {j + 1}: ");
                        string? input = Console.ReadLine();
                        if (input == null)
                        {
                            Console.WriteLine("❌ Введення перервано. Повторіть спробу.");
                            continue;
                        }

                        if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                            break;

                        Console.WriteLine("  ❌ Невірний формат. Використовуйте крапку як десятковий роздільник.");
                    }
                    _vectors[i][j] = value;
                }
            }
        }

        /// <summary>
        /// Виведення всіх векторів у консоль.
        /// </summary>
        public virtual void DisplayVectors()
        {
            Console.WriteLine("\n--- Вектори системи ---");
            for (int i = 0; i < _vectors.Length; i++)
            {
                string formatted = string.Join("  ", Array.ConvertAll(_vectors[i],
                    x => x.ToString("0.###", CultureInfo.InvariantCulture)));
                Console.WriteLine($"Вектор {i + 1}: ({formatted})");
            }
        }

        #region IDisposable Implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Керовані ресурси (у нас лише масив)
                // Не потрібно явно звільняти _vectors — GC зробить це автоматично
            }

            _disposed = true;
        }
        #endregion
    }

    /// <summary>
    /// Клас для системи з двох векторів (2D).
    /// </summary>
    public class TwoVectors : VectorSystemBase
    {
        public TwoVectors() : base(2, 2) { }

        public override void DisplayVectors()
        {
            Console.WriteLine("\n--- Вектори у 2D ---");
            for (int i = 0; i < Vectors.Length; i++)
            {
                Console.WriteLine($"Вектор {i + 1}: ({Vectors[i][0]}, {Vectors[i][1]})");
            }
        }

        public override bool IsLinearlyIndependent()
        {
            double determinant = Vectors[0][0] * Vectors[1][1] - Vectors[0][1] * Vectors[1][0];
            return Math.Abs(determinant) > DeterminantEpsilon;
        }
    }

    /// <summary>
    /// Клас для системи з трьох векторів (3D).
    /// </summary>
    public class ThreeVectors : VectorSystemBase
    {
        public ThreeVectors() : base(3, 3) { }

        public override void DisplayVectors()
        {
            Console.WriteLine("\n--- Вектори у 3D ---");
            for (int i = 0; i < Vectors.Length; i++)
            {
                Console.WriteLine($"Вектор {i + 1}: ({Vectors[i][0]}, {Vectors[i][1]}, {Vectors[i][2]})");
            }
        }

        public override bool IsLinearlyIndependent()
        {
            double det =
                Vectors[0][0] * (Vectors[1][1] * Vectors[2][2] - Vectors[1][2] * Vectors[2][1]) -
                Vectors[0][1] * (Vectors[1][0] * Vectors[2][2] - Vectors[1][2] * Vectors[2][0]) +
                Vectors[0][2] * (Vectors[1][0] * Vectors[2][1] - Vectors[1][1] * Vectors[2][0]);

            return Math.Abs(det) > DeterminantEpsilon;
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
            Console.WriteLine("1 — система з 2 векторів (2D)");
            Console.WriteLine("2 — система з 3 векторів (3D)");
            Console.Write("Ваш вибір: ");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
            {
                Console.Write("❌ Вибір невірний. Введіть 1 або 2: ");
            }

            using VectorSystemBase system = (choice == 1)
                ? new TwoVectors()
                : new ThreeVectors();

            system.InputVectors();
            system.DisplayVectors();

            bool independent = system.IsLinearlyIndependent();
            Console.WriteLine($"\nРезультат: {(independent ? "✅ Вектори лінійно незалежні" : "❌ Вектори лінійно залежні")}");
        }
    }
}
