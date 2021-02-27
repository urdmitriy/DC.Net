using System;
using System.IO;

namespace DC_dotNet
{
    class WorkSpace
    {
        public static int windowWidth { get; set; }
        public static int windowHeigh { get; set; }

        static void HorizontalLine(int x1, int x2, int y, char symbol)
        {
            for (int i = x1; i <= x2; i++)
            {
                Console.SetCursorPosition(i, y);
                Console.Write(symbol);
            }
        }

        public static void VerticalLine(int x, int y1, int y2, char symbol)
        {
            for (int i = y1; i < y2; i++)
            {
                Console.SetCursorPosition(x, i);
                Console.Write(symbol);
            }
        }

        static void LabelName(int x, int y, string name)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(name);
        }

        public static void Error (int x, string message)
        {
            Console.SetCursorPosition(x, windowHeigh-3);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(2, windowHeigh - 1);
        }

        static public void PrintDirAndFiles(int pageDir=0, int pageFile=0)
        {
            if (pageDir > 0) pageDir--; //для юзер-френдли)
            if (pageFile > 0) pageFile--;

            LabelName(8, 1, Environment.CurrentDirectory);
            FilesAndFolders dir = new FilesAndFolders();
            string _path = Path.Combine(Environment.CurrentDirectory);
            dir.GetListDir(_path);
            dir.GetListDirForPage(pageDir);
            int line=3;
            foreach (var item in dir.ListDirectory)
            {
                Console.SetCursorPosition(2, line);
                if (item.Length>1)
                    if (item[1] == ' ')
                        Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(item);
                Console.ForegroundColor = ConsoleColor.White;
                line++;
            }

            dir.GetListFile(_path, pageFile);
            line = 3;

            foreach (var item in dir.ListFiles)
            {
                Console.SetCursorPosition(windowWidth / 3 + 2, line);
                if (item.name[0] == ' ')
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.Write(item.name);
                Console.SetCursorPosition(windowWidth / 3 + 35, line);
                Console.Write(item.date);
                Console.SetCursorPosition(windowWidth / 3 + 57, line);
                Console.Write(item.size);

                Console.ForegroundColor = ConsoleColor.White;
                line++;
            }
            Console.SetCursorPosition(2, windowHeigh - 1);
        }
        static public void DrawFrame()
        {
            Console.Clear();

            //Верхняя рамка
            HorizontalLine(0, windowWidth, 0, '=');

            //путь
            HorizontalLine(0, windowWidth, 2, '-');

            LabelName(1, 1, " Путь: ");
            LabelName(3, 2, " Каталоги ");

            //Левая рамка
            VerticalLine(0, 1, windowHeigh, '|');

            //Правая рамка
            VerticalLine(windowWidth, 1, windowHeigh, '|');

            //Нижняя рамка
            HorizontalLine(0, windowWidth, windowHeigh, '=');

            //Разделитель файлов и папок
            VerticalLine(windowWidth / 3, 3, windowHeigh - 4, '|');

            LabelName(windowWidth / 3 + 3, 2, " Файлы ");
            LabelName(windowWidth / 3 + 38, 2, " Отредактирован ");
            LabelName(windowWidth / 3 + 59, 2, " Размер ");

            //Разделитель файлов и информации
            HorizontalLine(0, windowWidth, windowHeigh - 4, '-');

            LabelName(3, windowHeigh - 4, " Информация ");

            //Разделитель информации и командной строки
            HorizontalLine(0, windowWidth, windowHeigh - 2, '-');

            LabelName(3, windowHeigh - 2, " Командная строка ");

            Console.SetCursorPosition(2, windowHeigh - 1);
        }
    }
}
