using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Project.Classes
{
    public static class Generator
    {
        private static Random _rand = new Random();

        public static string GetFirstName(bool dday=false)
        {
            if (dday)
            {
                string name = Dictionaries.Names.DDayNames[0];
                Dictionaries.Names.DDayNames.RemoveAt(0);
                return name;
            }
            else
            {
                return Dictionaries.Names.FirstNames[_rand.Next(0, Dictionaries.Names.FirstNames.Count)];
            }
        }

        public static float GetPrice(int max)
        {
            return (float)Math.Round((_rand.NextDouble() * max) + 1, 2);
        }

        public static float GetProbability (int min = 0, int max = 100)
        {
            return (float)Math.Round((min + (_rand.NextDouble() * (max - min))) / 100f, 2);
        }
    }
}
