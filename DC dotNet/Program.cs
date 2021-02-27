using System;
using System.IO;
using System.Text.Json;

namespace DC_dotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            //путь до программы для сохранения настроек
            string progPath = Environment.CurrentDirectory;
            //задаем размеры окна
            WorkSpace.windowWidth = 100;
            WorkSpace.windowHeigh = 40;
            Console.SetWindowSize(WorkSpace.windowWidth, WorkSpace.windowHeigh);

            string loadPath = "";
            //если файл настроек существует, загружаем путь к текущей папке
            if (File.Exists("settings.json"))
            {
                loadPath = JsonSerializer.Deserialize<string>(File.ReadAllText("settings.json"));
            }
            else
                loadPath = Path.Combine(@"c:\");

            Environment.CurrentDirectory = loadPath;

            //команда для перехода в текущую папку
            string command = $"cd {Environment.CurrentDirectory}";

            while (command != "exit")
            {
                Commands.setCommand(command);
                command = Console.ReadLine();
            }
            //при выходе сохраняем последний активный путь
            File.WriteAllText(Path.Combine(progPath,"settings.json"), JsonSerializer.Serialize(Environment.CurrentDirectory));
        }
    }
}
