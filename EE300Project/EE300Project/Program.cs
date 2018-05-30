using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EE300Project
{
    class Program
    {
        static void Main(string[] args)
        {
            String[] death_list = Properties.Resources.EverestDeathList.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

            int n = 0;
            int total = 0;
            int no_heights = 21;

            int[] histogram = new int[] { 0, 0, 0, 0, 0 };

            int median = 0;
            double mean = 0;
            double variance = 0;
            double sd = 0;

            foreach (string s in death_list)
            {
                ++n;
                total += int.Parse(s);

                if (int.Parse(s) < 5000)
                    ++histogram[0];
                if ((int.Parse(s) >= 5000) && (int.Parse(s) < 6000))
                    ++histogram[1];
                if ((int.Parse(s) >= 6000) && (int.Parse(s) < 7000))
                    ++histogram[2];
                if ((int.Parse(s) >= 7000) && (int.Parse(s) < 8000))
                    ++histogram[3];
                if (int.Parse(s) >= 8000)
                    ++histogram[4];
            }

            Console.WriteLine("Deaths of all Climbers on Mount Everest vs. Elevation of Death");
            Console.WriteLine("The following calculations describe this population.");
            Console.WriteLine("n is equal to: {0}", n);
            Console.WriteLine("unbiased n value(total known deaths): {0}", n + no_heights);
            Console.WriteLine();

            mean = (double)total / n;
            Console.WriteLine("The mean height is: {0}", mean);

            List<int> heights = new List<int>();

            foreach(string s in death_list)
            {
                heights.Add(int.Parse(s));

                if(double.Parse(s) >= mean)
                    variance += Math.Pow((double.Parse(s) - mean), 2);
                else if(mean > double.Parse(s))
                    variance += Math.Pow((mean - double.Parse(s)), 2);
            }

            variance = variance / n;
            Console.WriteLine("The variance is: {0}", variance);

            sd = Math.Sqrt(variance);
            Console.WriteLine("The standard deviation is: {0}", sd);

            heights.Sort();
            median = heights.ElementAt(n / 2);
            Console.WriteLine("The median is: {0}", median);

            Console.WriteLine();
            Console.WriteLine("Histogram values: {0}, {1}, {2}, {3}, {4}", histogram[0], histogram[1], histogram[2], histogram[3], histogram[4]);

            Console.WriteLine();
            Console.WriteLine("The following examples are based off standardizing the random variable.  They represent z-score values.");

            //Probability of death at 5000m and 8000m using normal approximation.
            double test1 = 5000;
            double test2 = 8000;

            test1 = (test1 - mean) / sd;
            test2 = (test2 - mean) / sd;

            Console.WriteLine("The probability of death below 5000m is: {0}", test1);
            Console.WriteLine("The probability of death above 8000m is: {0}", test2);

            //Probability of death on the Lhotse face.(7800m ~ 8500m)
            double bound1 = 7800;
            double bound2 = 8500;

            bound1 = (bound1 - mean) / sd;
            bound2 = (bound2 - mean) / sd;

            Console.WriteLine("The probability of dying on the Lhotse face is: {0} - {1}", bound1, bound2);

            //Probability of dying in the Khumbu Icefall.(5300m ~ 5900m)
            double bound3 = 5300;
            double bound4 = 5900;

            bound3 = (bound3 - mean) / sd;
            bound4 = (bound4 - mean) / sd;

            Console.WriteLine("The probability of dying in the Khumbu Icefall is: {0} - {1}", bound3, bound4);

            Console.ReadLine();
        }
    }
}
