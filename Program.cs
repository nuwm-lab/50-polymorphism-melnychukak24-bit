using System;
using System.Globalization;

abstract class VectorSystemBase : IDisposable
{
    private bool _disposed;
    private readonly double[][] _vectors;
    protected const double DeterminantEpsilon = 1e-9;

    protected VectorSystemBase(int vectorCount, int dimension)
    {
        _vectors = new double[vectorCount][];
        for (int i = 0; i < vectorCount; i++)
            _vectors[i] = new double[dimension];
    }

    protected double[][] Vectors => _vectors;

    // ✅ Введення векторів з урахуванням локалі (кома або крапка)
    public virtual void InputVectors()
    {
        for (int i = 0; i < _vectors.Length; i++)
        {
            Console.WriteLine($"Введіть координати вектора {i + 1} через пробіл:");
            string? input = Console.ReadLine()?.Replace(',', '.');

            if (input == null)
            {
                Console.WriteLine("Помилка вводу! Рядок порожній.");
                i--;
                continue;
            }

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != _vectors[i].Length)
            {
                Console.WriteLine("Неправильна кількість координат, спробуйте ще раз.");
                i--;
                continue;
            }

            for (int j = 0; j < _vectors[i].Length; j++)
            {
                if (!double.TryParse(parts[j], NumberStyles.Float, CultureInfo.InvariantCulture, out _vectors[i][j]))
                {
                    Console.WriteLine("Помилка при парсингу числа, спробуйте ще раз.");
                    i--;
                    break;
                }
            }
        }
    }

    // ✅ Єдиний метод форматованого виводу
    public virtual void DisplayVectors()
    {
        Console.WriteLine("Вектори системи:");
        foreach (var v in _vectors)
            Console.WriteLine("(" + string.Join(", ", Array.ConvertAll(v, x => x.ToString("F2", CultureInfo.InvariantCulture))) + ")");
    }

    public abstract bool IsLinearlyIndependent();

    // ✅ Спрощена реалізація Dispose (без фіналізатора)
    public void Dispose()
    {
        if (!_disposed)
        {
            // очищаємо посилання
            for (int i = 0; i < _vectors.Length; i++)
                Array.Clear(_vectors[i], 0, _vectors[i].Length);

            _disposed = true;
        }
    }
}

// ===================== Два вектори =====================

class TwoVectors : VectorSystemBase
{
    public TwoVectors() : base(2, 2) { }

    public override bool IsLinearlyIndependent()
    {
        double det = Vectors[0][0] * Vectors[1][1] - Vectors[0][1] * Vectors[1][0];
        return Math.Abs(det) > DeterminantEpsilon;
    }
}

// ===================== Три вектори =====================

class ThreeVectors : VectorSystemBase
{
    public ThreeVectors() : base(3, 3) { }

    public override bool IsLinearlyIndependent()
    {
        double[][] v = Vectors;
        double det =
            v[0][0] * (v[1][1] * v[2][2] - v[1][2] * v[2][1]) -
            v[0][1] * (v[1][0] * v[2][2] - v[1][2] * v[2][0]) +
            v[0][2] * (v[1][0] * v[2][1] - v[1][1] * v[2][0]);

        return Math.Abs(det) > DeterminantEpsilon;
    }
}

// ===================== Main =====================

class Program
{
    static void Main()
    {
        Console.WriteLine("Оберіть систему: 1 — два вектори, 2 — три вектори");
        string? choice = Console.ReadLine();

        VectorSystemBase system = choice == "1" ? new TwoVectors() : new ThreeVectors();

        system.InputVectors();
        system.DisplayVectors();

        Console.WriteLine(system.IsLinearlyIndependent()
            ? "Вектори лінійно незалежні."
            : "Вектори лінійно залежні.");

        system.Dispose();
    }
}
