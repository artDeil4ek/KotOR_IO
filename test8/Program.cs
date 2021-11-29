﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KotOR_IO;

namespace test8
{
    class Program
    {
        public static void FisherYatesShuffle<T>(IList<T> list)
        {
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.Rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static class ThreadSafeRandom
        {
            [ThreadStatic]
            private static Random Local;

            public static Random Rng
            {
                get
                {
                    return Local ?? (Local = new Random(Seed));
                }
            }

            public static int Seed { get; private set; } = unchecked(Environment.TickCount * 31 + System.Threading.Thread.CurrentThread.ManagedThreadId);

            public static int GenerateSeed()
            {
                Seed = Rng.Next();
                return Seed;
            }

            public static void SetSeed(int seed)
            {
                Seed = seed;
                Local = null;
            }

            public static void RestartRng()
            {
                Local = null;
            }
        }


        static void Main(string[] args)
        {
            var filename = @"C:\Dev\KIO Test\test1.git";
            var fileinfo = new FileInfo(filename);
            Console.WriteLine($" file size: {fileinfo.Length:N0} bytes");

            GFF test = new GFF(filename);
            Console.WriteLine($" read size: {test.ToRawData().Length:N0} bytes");

            var filename2 = @"C:\Dev\KIO Test\test2.git";
            test.WriteToFile(filename2);

            var fileinfo2 = new FileInfo(filename2);
            Console.WriteLine($"write size: {fileinfo2.Length:N0} bytes");

            var test2 = new GFF(filename2); // Exception: Stack overflow
            var raw2 = test2.ToRawData();

            var filename3 = @"C:\Dev\KIO Test\test3.git";
            test2.WriteToFile(filename3);


            //DirectoryInfo di = new DirectoryInfo("C:\\Program Files (x86)\\Steam\\steamapps\\common\\swkotor\\modules - Copy");
            //foreach (FileInfo fi in di.EnumerateFiles())
            //{
            //    RIM r = new RIM(Path.Combine(fi.DirectoryName, fi.Name));
            //    r.WriteToFile(Path.Combine("C:\\Program Files (x86)\\Steam\\steamapps\\common\\swkotor\\modules\\", fi.Name));
            //}


            //RIM r = new RIM("D:\\ExampleFiles\\danm13.rim");
            //r.WriteToFile("D:\\ExampleFiles\\danm13T.rim");
            //RIM r2 = new RIM("D:\\ExampleFiles\\danm13T.rim");

            Console.ReadLine();
        }
    }
}
