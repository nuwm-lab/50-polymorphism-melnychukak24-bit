using System;

namespace VectorSystem
{
    // Базовий клас: система двох векторів
    class TwoVectors
    {
        protected double[] A = new double[2];
        protected double[] B = new double[2];

        public virtual void InputVectors()
        {
            Console.WriteLine("Введіть координати вектора A (a1 a2):");
            for (int i = 0; i < 2; i++)
                A[i] = double.Parse(Console.ReadLine());

            Console.WriteLine("Введіть координати вектора B (b1 b2):");
            for (int i = 0; i < 2; i++)
                B[i] = double.Parse(Console.ReadLine());
        }

        public virtual void DisplayVectors()
        {
            Console.WriteLine($"A = ({A[0]}, {A[1]})");
            Console.WriteLine($"B = ({B[0]}, {B[1]})");
        }

        public virtual bool IsLinearlyIndependent()
        {
            double det = A[0] * B[1] - A[1] * B[0];
            return Math.Abs(det) > 1e-9;
        }
    }

    // Похідний клас: система трьох векторів
    class ThreeVectors : TwoVectors
    {
        private double[] C = new double[3];

        public override void InputVectors()
        {
            A = new double[3];
            B = new double[3];

            Console.WriteLine("Введіть координати вектора A (a1 a2 a3):");
            for (int i = 0; i < 3; i++)
                A[i] = double.Parse(Console.ReadLine());

            Console.WriteLine("Введіть координати вектора B (b1 b2 b3):");
            for (int i = 0; i < 3; i++)
                B[i] = double.Parse(Console.ReadLine());

            Console.WriteLine("Введіть координати вектора C (c1 c2 c3):");
            for (int i = 0; i < 3; i++)
                C[i] = double.Parse(Console.ReadLine());
        }

        public override void DisplayVectors()
        {
            Console.WriteLine($"A = ({A[0]}, {A[1]}, {A[2]})");
            Console.WriteLine($"B = ({B[0]}, {B[1]}, {B[2]})");
            Console.WriteLine($"C = ({C[0]}, {C[1]}, {C[2]})");
        }

        public override bool IsLinearlyIndependent()
        {
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
            int choice = int.Parse(Console.ReadLine());

            TwoVectors system;

            if (choice == 2)
                system = new TwoVectors();
            else
                system = new ThreeVectors();

            system.InputVectors();
            Console.WriteLine("\nВведені вектори:");
            system.DisplayVectors();

            bool independent = system.IsLinearlyIndependent();

            Console.WriteLine($"\nСистема векторів {(independent ? "є" : "не є")} лінійно незалежною.");
        }
    }
}

