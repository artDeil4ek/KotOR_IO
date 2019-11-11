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
            Random rng = new Random(55);
            DirectoryInfo di = new DirectoryInfo("L:\\laned\\Documents\\kotor stuffs\\DumbIdea\\modules");
            foreach (FileInfo f in di.EnumerateFiles())
            {
                RIM r = KReader.ReadRIM(f.OpenRead());

                foreach (RIM.rFile rf in r.File_Table.Where(k => k.TypeID == 2027))
                {
                    GFF g = new GFF(rf.File_Data);

                    int temp = rng.Next(1, 508);
                    if (temp == 0 || temp == 29 || temp == 82) { temp = 200; }
                    g.Field_Array.Where(k => k.Label == "Appearance_Type").FirstOrDefault().Field_Data = temp;
                    g.Field_Array.Where(k => k.Label == "Appearance_Type").FirstOrDefault().DataOrDataOffset = temp;

                    MemoryStream ms = new MemoryStream();

                    kWriter.Write(g, ms);

                    rf.File_Data = ms.ToArray();
                }
                
                kWriter.Write(r, f.OpenWrite());

            }

            

            Console.WriteLine();

            


            //BIF b = KReader.ReadBIF(File.OpenRead("L:\\laned\\Documents\\kotor stuffs\\Biff Reader Test\\tes\\2da.bif"));
            //KEY k = KReader.ReadKEY(File.OpenRead("L:\\laned\\Documents\\kotor stuffs\\Biff Reader Test\\tes\\chitin.key"));
            //b.attachKey(k, "data\\2da.bif");
            //Console.WriteLine();
            //Stream s = new MemoryStream(b.Variable_Resource_Table[8].Entry_Data);

            //TwoDA t = KReader.Read2DA(s);

            //int n = t.Row_Count;

            //while (n > 1)
            //{
            //    n--;
            //    int l = ThreadSafeRandom.Rng.Next(n + 1);
            //    object value = t["modela", l];
            //    t["modela", l] = t["modela", n];
            //    t["modela", n] = value;
            //}


            //kWriter.Write(t, File.OpenWrite("L:\\laned\\Documents\\kotor stuffs\\Biff Reader Test\\tes\\appearance.2da"));
            //Console.WriteLine();

            //MemoryStream ms = new MemoryStream();

            //File.OpenRead("L:\\laned\\Documents\\kotor stuffs\\Biff Reader Test\\g_bandon.ute").CopyTo(ms);

            //e.Append_File("g_bandon", ms.ToArray());

            //byte[] b = e["g_bandon"];

            //Console.Write("");

            //GFF g = KReader.ReadGFF(File.OpenRead("L:\\laned\\Documents\\kotor stuffs\\Biff Reader Test\\g_bandon.ute"));

            //foreach (GFF.Field gf in g.Field_Array)
            //{
            //    string tmp;
            //    switch (gf.Type)
            //    {
            //        case 0:
            //            tmp = "byte";
            //            break;
            //        case 1:
            //            tmp = "char";
            //            break;
            //        case 2:
            //        case 3:
            //            tmp = "short";
            //            break;
            //        case 4:
            //        case 5:
            //            tmp = "int";
            //            break;
            //        case 6:
            //        case 7:
            //            tmp = "long";
            //            break;
            //        case 8:
            //            tmp = "float";
            //            break;
            //        case 9:
            //            tmp = "double";
            //            break;
            //        case 10:
            //            tmp = "string";
            //            break;
            //        case 11:
            //            tmp = "string";
            //            break;
            //        case 12:
            //            tmp = "string";
            //            break;
            //        case 13:
            //            tmp = "byte[]";
            //            break;
            //        case 14:
            //            tmp = "GFF.GFFStruct";
            //            break;
            //        case 15:
            //            tmp = "List<object>";
            //            break;
            //        default:
            //            tmp = "";
            //            break;
            //    }

            //    Console.WriteLine("public " + tmp + " " + gf.Label.TrimEnd('\0') + ";");


            //}


            //Console.ReadKey();

        }
    }
}
