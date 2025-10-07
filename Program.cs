using System;
using System.Linq;

namespace VectorSystem
{
    // Базовий клас для системи векторів
    abstract class VectorSystemBase : IDisposable
    {
        protected double[][] Vectors; // масив векторів
        public int Dimension { get; protected set; } // розмірність

        // Конструктор із вказанням кількості векторів і розмірності
        protected VectorSystemBase(int vectorCount, int dimension)
        {
            Dimension = dimension;
            Vectors = new double[vectorCount][];
            for (int i = 0; i < vectorCount; i++)
                Vectors[i] = new double[dimension];
        }

        // Метод для введення координат
        public virtual void InputVectors()
        {
            for (int v = 0; v < Vectors.Length; v++)
            {
                Console.WriteLine($"Введіть координати вектора {v + 1} ({string.Join(" ", Enumerable.Range(1, Dimension).Select(i => $"x{i}"))}):");
                for (int i = 0; i < Dimension; i++)
                {
                    while (true)
                    {
                        Console.Write($"x{i + 1} = ");
                        if (double.TryParse(Console.ReadLine(), out double value))
                        {
                            Vectors[v][i] = value;
                            break;
                        }
                        Console.WriteLine("❌ Помилка введення! Спробуйте ще раз.");
                    }
                }
            }
        }

        // Вивід векторів
        public virtual void DisplayVectors()
        {
            for (int v = 0; v < Vectors.Length; v++)
                Console.WriteLine($"V{v + 1} = ({string.Join(", ", Vectors[v])})");
        }

        // Перевірка лінійної незалежності — абстрактна, реалізується у похідних класах
        public abstract bool IsLinearlyIndependent();

        // IDisposable (для керування ресурсами)
        public void Dispose()
        {
            Vectors = null;
            GC.SuppressFinalize(this);
        }

        ~VectorSystemBase() => Dispose();
    }

    // Система двох векторів
    class TwoVectors : VectorSystemBase
    {
        public TwoVectors() : base(2, 2) { }

        public override bool IsLinearlyIndependent()
        {
            double det = Vectors[0][0] * Vectors[1][1] - Vectors[0][1] * Vectors[1][0];
            return Math.Abs(det) > 1e-9;
        }
    }

    // Система трьох векторів
    class ThreeVectors : VectorSystemBase
    {
        public ThreeVectors() : base(3, 3) { }

        public override bool IsLinearlyIndependent()
        {
            double[] A = Vectors[0];
            double[] B = Vectors[1];
            double[] C = Vectors[2];

            double det =
                A[0] * (B[1] * C[2] - B[2] * C[1]) -
                A[1] * (B[0] * C[2] - B[2] * C[0]) +
                A[2] * (B[0] * C[1] - B[1] * C[0]);

            return Math.Abs(det) > 1e-9;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Оберіть тип системи векторів (2 або 3):");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 2 && choice != 3))
            {
                Console.WriteLine("❌ Невірний вибір! Введіть 2 або 3:");
            }

            using (VectorSystemBase system = choice == 2 ? new TwoVectors() : new ThreeVectors())
            {
                system.InputVectors();
                Console.WriteLine("\nВведені вектори:");
                system.DisplayVectors();

                bool independent = system.IsLinearlyIndependent();
                Console.WriteLine($"\n✅ Система векторів {(independent ? "є" : "не є")} лінійно незалежною.");
            }
        }
    }
}

