using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NeigbourSort
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int measureLimit = 10000;
            int reps = 5;
            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + 
                "/resultsRawSmall.csv", true, Encoding.UTF8))
            {
                sw.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")};Mateusz Ambroziak s16852;ilość próbek: " +
                    $"{reps};Limit tablicy: {measureLimit}; wartości: milisekundy");
                sw.WriteLine($"Ilość elementów tablic;średnia;minimum;maximum");
                for (int x = 10; x <= measureLimit; x += 10)
                {
                    int[] array = new int[x];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = (array.Length - i);
                    }

                    Measure(x.ToString(), sw, reps, () =>
                    {
                        SortAlgorithm(array);
                    });
                }
            }
            Console.ReadKey();
        }
        public static void SortAlgorithm(int[] arr)
        {
            int temp = 0;

            for (int write = 0; write < arr.Length; write++)
            {
                for (int sort = 0; sort < arr.Length - 1; sort++)
                {
                    if (arr[sort] > arr[sort + 1])
                    {
                        temp = arr[sort + 1];
                        arr[sort + 1] = arr[sort];
                        arr[sort] = temp;
                    }
                }
            }
        }
        public static void Measure(string what, StreamWriter streamWriter, int reps, Action action)
        {

            GC.Collect(); // force to clean the memory
            GC.WaitForPendingFinalizers();
            GC.Collect();
            //action(); // warm up

            double[] performance = new double[reps];

            for (int i = 0; i < reps; i++)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                action();
                performance[i] = stopwatch.Elapsed.TotalMilliseconds;
            }
            streamWriter.WriteLine($"{what};{performance.Average()};{performance.Min()};{performance.Max()}");
            Console.WriteLine($"{what.PadLeft(10)} - AVG: {performance.Average().ToString().PadLeft(10)}, MIN: " +
                $"{performance.Min().ToString().PadLeft(10)}, MAX: {performance.Max().ToString().PadLeft(10)}");
        }
    }
}
