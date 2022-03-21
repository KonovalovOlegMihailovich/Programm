using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604

// Что Зачем И Почему
// Данная программа нужна для получения автомата по
// дисциплинам: "Разработка программных модулей" и 
// "Инструментальные средства разработки программного обеспечения"

namespace filecreater
{
    class Program
    {
        public delegate void func();

        private string path = $@"{Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString())}\test\";

        public Dictionary<String, func> menu = new Dictionary<string, func>();

        public readonly string[] array = { "FileCreater", "Search" };

        private void Filecreater()
        {
            if (Directory.Exists(path)) Directory.CreateDirectory(path);
            var rand = new Random();
            Console.Write("Enter count: ");
            int k = int.Parse(Console.ReadLine());
            for (int i = 0; i < k; i++)
            {
                string name = "";
                for (int j = 0; j < rand.Next(7, 19); j++)
                {
                    name += Convert.ToChar(rand.Next(97, 122));
                }
                name += ".";
                for (int j = 0; j < rand.Next(2, 5); j++)
                {
                    name += Convert.ToChar(rand.Next(97, 122));
                }
                Console.WriteLine(name);
                File.Create($"{path}{name}");
            }
        }

        private void Search()
        {
            Console.Write("Enter mask for search: ");
            string mask = Console.ReadLine();
            Directory.CreateDirectory($"{path}{mask}");
            string[] files = Directory.GetFiles(path);
            Regex reg = new Regex($@"[^\\]*{mask}[^\\]*$");
            foreach (string s in files)
            {
                string l = reg.Match(s).Value;
                if (l.Length != 0)
                {
                    Console.WriteLine(l);
                    File.Move($"{path}{l}", $"{path}{mask}\\{l}", true);
                }
            }

        }
        
        public Program()
        {
            menu[array[0]] = Filecreater;
            menu[array[1]] = Search;
        }

        static void Main(string[] args)
        {
            Program prog = new Program();
            for(int i = 0; i < prog.array.Length; i++) Console.WriteLine($"[{i}] {prog.array[i]}");
            prog.menu[prog.array[int.Parse(Console.ReadLine())]]();
        }
    }
}