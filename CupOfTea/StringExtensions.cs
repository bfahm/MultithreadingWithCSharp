using System;

namespace CupOfTea
{
    static class StringExtensions
    {
        public static void Dump(this string _string)
        {
            Console.WriteLine(_string);
        }

        public static void Dump(this int _int)
        {
            Console.WriteLine(_int);
        }
    }
}
