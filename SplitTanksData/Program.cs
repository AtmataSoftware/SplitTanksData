using System;
using System.Collections.Generic;
using System.IO;

namespace SplitTanksData
{
    class Program
    {
        static Dictionary<string,StreamWriter> tanks = new();
        static string path, header;
        static int[] ParamIds = {622,625,626,628,661,692,717,719,726,728,729,730,756 };

        static void Main(string[] args)
        {
            Console.WriteLine("Please enter full file name");
            var filename = Console.ReadLine();
            StreamReader reader = new StreamReader(File.OpenRead(filename));

            header = reader.ReadLine();//header
            path = filename.Replace(Path.GetFileName(filename), "");
            int counter = 0;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
               
                try
                {
                    WriteLine(line);
                }
                catch
                {
                    Console.WriteLine("Error in line "+line,ConsoleColor.Red);
                }
                if (counter++ % 1000 == 0)
                    Console.WriteLine(counter);
            }
            foreach (var tank in tanks)
                tank.Value.Close();
            Console.WriteLine(counter);
            Console.WriteLine("Done");
            Console.ReadKey();

           
        }
        static void  WriteLine(string line)
        {
            var values = line.Split(',');
            var streamWriter = GetStramWriter(values[0]);
            values[4] = $"{Convert.ToDouble(values[4]) / 86400 + 25569}";
            line = "";
            foreach (var value in values)
                line += value + ',';
            streamWriter.WriteLine(line);


        }
        private static StreamWriter GetStramWriter(string tankTitle)
        {
            if (tanks.ContainsKey(tankTitle))
                return tanks[tankTitle];
            StreamWriter streamWriter = new(path+tankTitle+".csv");
            streamWriter.WriteLine(header);
            tanks.Add(tankTitle, streamWriter);
            return streamWriter;
        }
    }
}
