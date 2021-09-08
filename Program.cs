using System;
using System.Text.Json;
using System.IO;

namespace bnh.carrier
{
    public class AppConfig{
        public string Source { get; set; }
        public string Destination { get; set; }
        public int DayModified { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {            
            AppConfig config = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText("config.json"));
            Console.WriteLine($"Reading from {config.Source}");
            Console.WriteLine($"Copy to {config.Destination}");
            Console.WriteLine($"Which modified in {config.DayModified} day(s)");
            if(Directory.Exists(config.Destination)){
                Directory.Delete(config.Destination,true);
            }            
            Directory.CreateDirectory(config.Destination);
            Console.WriteLine($"{config.Destination} cleared and recreated");
            foreach(string sourcePath in Directory.GetFiles(config.Source)){
                DateTime modified = File.GetLastWriteTime(sourcePath);
                int modifiedDay = (DateTime.Now - modified).Days;
                //Console.WriteLine($"{sourcePath} @ {modified.ToShortDateString()} = {modifiedDay}");
                if(modifiedDay <= config.DayModified){
                    string filename = Path.GetFileName(sourcePath);
                    File.Copy(sourcePath,Path.Combine(config.Destination,filename));
                    Console.WriteLine(filename+" copied");
                }
            }
        }
    }
}
