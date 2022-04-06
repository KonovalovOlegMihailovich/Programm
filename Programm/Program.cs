using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace filecreater
{
    class Program
    {
        public delegate void func();
        private readonly string[] Array = { "Create array of files", "Sort files" };
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvmxyz0123456789";
        public string Path;
        public List<string> Files;
        internal Dictionary<string, func> Menu;
        public Random Rand;

        private string RandomString(int length)
        {
            return new string(Enumerable.Repeat(Chars, length)
                .Select(s => s[Rand.Next(s.Length)]).ToArray());
        }

        private void CreateArrayFiles()
        {
            Console.Write("Enter count: ");
            int.TryParse(Console.ReadLine(), out int k);
            for (;k > 0; k--)
            {
                string name = RandomString(Rand.Next(1, 19)) + "." + RandomString(Rand.Next(2, 5));
                Console.WriteLine(name);
                File.Create(@$"{Path}\{name}");
            }
        }
        private void SortFiles()
        {
            for (int i = 0; i < 3; i++) _search();

            Regex reg = new Regex($@"[^\\]+$");
            Directory.CreateDirectory($@"{Path}\NS");
            for (int i = 0; i < Files.Count; i++)
            {
                File.Move($@"{Files[i]}", $@"{Path}\NS\{reg.Match(Files[i]).Value}");
            }
        }

        private void _search()
        {
            Console.Write("Enter mask for search: ");
            string mask = Console.ReadLine();
            Directory.CreateDirectory(@$"{Path}\{mask}");
            Regex reg = new Regex($@"[^\\]+{mask}[^\\]+$");
            for (int i = 0; i < Files.Count; i++)
            { 
                string f = reg.Match(Files[i]).Value;
                if (f.Length > 0)
                {
                    try
                    {
                        File.Move($@"{Path}\{f}", @$"{Path}\{mask}\{f}");
                        Console.WriteLine(f);
                    } 
                    catch 
                    {
                        Console.WriteLine($"Error move file {f}");
                    }
                    Files.RemoveAt(i);
                }
            }

        }

        public void Select(int menu)
        {
            try
            {
                Menu[Array[menu]]();
            }
            catch 
            {
                throw new Exception();
            }
        }

        public void PrintMenu()
        {
            for (int i = 0; i < Array.Length; i++) Console.WriteLine($"[{i}] {Array[i]}");
            Console.WriteLine("[*] Exit");
        }

        public Program()
        {
            Console.Write("Enter work directory: ");
            Path = Console.ReadLine();
            Rand = new Random();
            Files = Directory.GetFiles(Path).ToList();
            Menu = new Dictionary<string, func>
            {
                [Array[0]] = CreateArrayFiles,
                [Array[1]] = SortFiles
            };
        }

        static void Main()
        {
            Program prog = new Program();
            int r = 0;
            while (true)
            {
                prog.PrintMenu();
                Console.Write("Enter number of functions: ");
                int.TryParse(Console.ReadLine(), out r);
                try { prog.Select(r); }
                catch { break; }
                Console.WriteLine("\t-------");
            }
        }
    }
}