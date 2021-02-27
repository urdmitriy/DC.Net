using System;
using System.IO;
using System.Text.Json;

namespace DC_dotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            string progPath = Environment.CurrentDirectory;

            WorkSpace.windowWidth = 100;
            WorkSpace.windowHeigh = 40;
            Console.SetWindowSize(WorkSpace.windowWidth, WorkSpace.windowHeigh);

            string loadPath = "";

            if (File.Exists("settings.json"))
            {
                loadPath = JsonSerializer.Deserialize<string>(File.ReadAllText("settings.json"));
            }
            else
                loadPath = Path.Combine(@"c:\");

            Environment.CurrentDirectory = loadPath;

            string command = $"cd {Environment.CurrentDirectory}";
            while (command != "exit")
            {
                Commands.setCommand(command);
                command = Console.ReadLine();
            }

            File.WriteAllText(Path.Combine(progPath,"settings.json"), JsonSerializer.Serialize(Environment.CurrentDirectory));
        }
    }
}
