using System;
using System.Collections.Generic;
using System.IO;

namespace SplitTanksData
{
    class Program
    {
        static Dictionary<string,StreamWriter> tanks = new();
        static string path;
        static string[] ParamIds = { "622", "625", "626", "628", "661", "692", "717", "719", "726", "728", "729", "730", "756" };
        static string[] outputValues = new string[ParamIds.Length];
        static string prevTankTitle = "";
        static long prevTimeStamp = 0;


        static void Main(string[] args)
        {
            Console.WriteLine("Please enter full file name");
            var filename = Console.ReadLine();
            StreamReader reader = new StreamReader(File.OpenRead(filename));

            var line = reader.ReadLine();//header
            path = filename.Replace(Path.GetFileName(filename), "");
            int counter = 0;
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
               
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
        static void WriteLine(string line)
        {
            var values = line.Split(',');
            var streamWriter = GetStramWriter(values[0]);

            long newTimeStamp = Convert.ToInt64(values[4]);
            if (prevTankTitle != values[0] || newTimeStamp != prevTimeStamp)
            {
                //new
                if(prevTimeStamp != 0)
                {            
                    string newLine = "";
                    foreach(var value in outputValues)
                        newLine += value + ",";
                    outputValues = new string[13];
                    double newdateTime = newTimeStamp / 86400.0 + 25569.0;
                    newLine += $"{(long)newdateTime} , {newdateTime % 1}";
                    streamWriter.WriteLine(newLine);
                }
                prevTimeStamp = newTimeStamp;
                prevTankTitle = values[0];
            }
            var index = Array.IndexOf(ParamIds, values[1]);
            outputValues[index] = values[2];
        }
        static StreamWriter GetStramWriter(string tankTitle)
        {
            if (tanks.ContainsKey(tankTitle))
                return tanks[tankTitle];
            StreamWriter streamWriter = new(path+tankTitle+".csv");
            string header = "";
            foreach (var param in ParamIds)
                header += $"{param},";
            header += "Date,Time"; 
            streamWriter.WriteLine(header);
            tanks.Add(tankTitle, streamWriter);
            return streamWriter;
        }
    }
}
