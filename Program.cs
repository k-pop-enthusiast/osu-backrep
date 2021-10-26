using System;
using System.IO;
using CommandLine;
using System.Linq;
using System.Text.RegularExpressions;

namespace osu_backrep
{
    class Program
    {
        static Random random = new Random();
        public class Options
        {
            [Option('o',"osdir",Required = true, HelpText = "Osu song directory path (where osu.exe is + \"/Songs\").")]
            public string osupath {get; set;}
            [Option('n',"newpic",Required = true, HelpText = "path To replacement picture")]
            public string picpath {get; set;}
            [Option('f',"filetype",Required = true, HelpText = "type of the new background (jpg,png)")]
            public string filetype {get; set;}
        }
        static void Main(string[] args)
        {
            string osupath = "";
            string picturepath = "";
            string filetype = "";
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o => {osupath = o.osupath; picturepath = o.picpath; filetype = o.filetype;});
            var directories = Directory.GetDirectories(osupath);
            foreach(string curdir in directories)
            {
                string filename = $"replacement{random.Next()}.{filetype}";

                File.Copy(picturepath,$"{curdir}\\{filename}");
                foreach(string file in Directory.GetFiles(curdir))
                {
                    if(file.EndsWith(".osu"))
                    {
                        Console.WriteLine($"Working on {file}");
                        string content = File.ReadAllText(file);
                        string result = Regex.Replace(content,"0,0,*\"*?.*[jpg,png]\"*,0,0", $"0,0,\"{filename}\",0,0");
                        File.WriteAllText(file,result);
                    }
                }
            }

            Environment.Exit(0);
        }
    }
}
