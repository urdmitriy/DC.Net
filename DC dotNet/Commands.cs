using System;
using System.IO;

namespace DC_dotNet
{
    class Commands
    {
        internal static void setCommand(string command)
        {
            int indexChar = 0;
            string curCommand = "";
            //массив для принимаемых параметров
            string[] param = new string[20];

            //процедура пропуска пробелов при парсинге
            void SkipSpace()
            {
                bool stop = false;
                //если символ пробел, каретка перемещается на следующий символ
                while (indexChar < command.Length && stop == false)
                {
                    if (command[indexChar] == ' ')
                    {
                        indexChar++;
                    }
                    else
                    {
                        stop = true;
                        continue;
                    }
                }
            }

            //процедура парсинга параметров командной строки, помещает результат в указанную перенную
            void ParseParam(ref string parameter)
            {
                bool stop = false;
                {    //прохогоняет коммандную строку до разделителя (пробел) и помещает результат в массив параметров
                    while (indexChar < command.Length && stop == false)
                    {    //если путь/параметр указан в ""
                        if (command[indexChar] == '"')
                        {
                            indexChar++;
                            while (command[indexChar] != '"')
                            {
                                parameter += command[indexChar];
                                indexChar++;
                            }
                            indexChar++;
                            stop = true;
                        }
                        else
                        { //если параметры разделены пробелом
                            if (command[indexChar] != ' ')
                            {
                                parameter += command[indexChar];
                                indexChar++;
                            }
                            else
                            {
                                indexChar++;
                                stop = true;
                                continue;
                            }
                        }
                    }
                }
                //пропускаем подвторяющиеся пробелы
                SkipSpace();
            }

            //парсим команду
            ParseParam(ref curCommand);

            //парсим передаваемые параметры
            for (int i = 0; i < param.Length; i++)
            {
                ParseParam(ref param[i]);
            }


            //процедура поиска параметров среди принятых
            string searchParametr(string paramSearch)
            {
                bool searched = false;

                //перебор параметров
                for (int i = 0; i < param.Length; i++)
                {
                    //поиск запрашиваемого параметра
                    for (int j = 0; j < paramSearch.Length; j++)
                    {
                        string paramString = param[i];

                        if (paramString != null)
                        {
                            if (paramString[j] == paramSearch[j])
                                searched = true;
                            else
                            {
                                searched = false;
                                break;
                            }
                        }
                        else
                            break;
                    }
                    //если требуемый параметр был найден, возвращаем его значение
                    if (searched == true)
                    {
                        return param[i + 1];
                    }
                }
                return "0";
            }

            //процедура смены каталога
            void changeDir()
            {
                WorkSpace.DrawFrame();
                //если имеется параметр команды:
                if (param[0] != null)
                {
                    string path = param[0];
                    try
                    {
                        //обработка .. для переход в родительский каталог
                        if (path == "..")
                        {
                            string parentDir="";

                            //если родительский каталог существует
                            if (Directory.GetParent(Environment.CurrentDirectory)!=null)
                            {
                                parentDir = Directory.GetParent(Environment.CurrentDirectory).ToString();
                                //смена текущего каталога на родительский     
                                Environment.CurrentDirectory = parentDir;
                            }
                        }
                        else
                        {
                            //смена текущего каталога
                            string newPath = Path.Combine(Environment.CurrentDirectory, path);
                            Environment.CurrentDirectory = newPath;
                        }
                            
                        //если указаны номера страниц листинга, передаем их
                        WorkSpace.PrintDirAndFiles(Convert.ToInt32(searchParametr("pd")), Convert.ToInt32(searchParametr("pf")));
                    }
                    catch (Exception)
                    {
                        //если что-то пошло не так
                        WorkSpace.DrawFrame();
                        WorkSpace.PrintDirAndFiles();
                        WorkSpace.Error(2, "Не удалось сменить директорию");
                    }
                   
                }
            }

            //процедура создания каталога
            void mkDir()
            {
                string pathNewDir = Path.Combine(Environment.CurrentDirectory, param[0]);
                try
                {
                    WorkSpace.DrawFrame();
                    Directory.CreateDirectory(pathNewDir);
                    WorkSpace.PrintDirAndFiles();
                }
                catch (Exception)
                {
                    WorkSpace.DrawFrame();
                    WorkSpace.Error(3, $"Не удалось создать каталог {pathNewDir}");
                    WorkSpace.PrintDirAndFiles();
                }
            }

            //процедура удаления каталога
            void rmDir()
            {
                string pathDelDir = Path.Combine(Environment.CurrentDirectory, param[0]);
                try
                {
                    WorkSpace.DrawFrame();
                    Directory.Delete(pathDelDir);
                    WorkSpace.PrintDirAndFiles();
                }
                catch (Exception)
                {
                    WorkSpace.DrawFrame();
                    WorkSpace.PrintDirAndFiles();
                    WorkSpace.Error(3, $"Не удалось удалить каталог {pathDelDir}");
                }
            }

            //создать файл
            void cf()
            {
                string nameFile = param[0];
                string fileText = param[1];
                try
                {
                    File.WriteAllText(Path.Combine(Environment.CurrentDirectory, nameFile), fileText);
                }
                catch (Exception)
                {
                    WorkSpace.DrawFrame();
                    WorkSpace.PrintDirAndFiles();
                    WorkSpace.Error(2, "Не удалось создать файл");
                }
                WorkSpace.DrawFrame();
                WorkSpace.PrintDirAndFiles();
            }

            //показать содержание файла
            void cat()
            {
                string file = Path.Combine(Environment.CurrentDirectory, param[0]);
                if (File.Exists(file))
                {
                    string text = File.ReadAllText(file);
                    WorkSpace.DrawFrame();
                    WorkSpace.VerticalLine(WorkSpace.windowWidth / 3, 3, WorkSpace.windowHeigh - 4, ' ');
                    Console.SetCursorPosition(2, 5);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(text);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(5, WorkSpace.windowHeigh - 5);
                    Console.WriteLine("Нажмите Enter для выхода");
                    Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    WorkSpace.DrawFrame();
                    WorkSpace.PrintDirAndFiles();
                }
                else
                {
                    WorkSpace.DrawFrame();
                    WorkSpace.PrintDirAndFiles();
                    WorkSpace.Error(2, "Не удалось открыть файл");
                }
            }

            //процедура копирования файла
            void cpFile()
            {
                string sourceFile = Path.GetFullPath(param[0]);
                string destPath = Path.GetFullPath(Path.Combine(param[1]));
                if (File.Exists(sourceFile))
                {
                    try
                    {
                        File.Copy(sourceFile, Path.Combine(destPath,Path.GetFileName(sourceFile)), true);
                        WorkSpace.DrawFrame();
                        WorkSpace.PrintDirAndFiles();
                    }
                    catch (Exception)
                    {
                        WorkSpace.DrawFrame();
                        WorkSpace.PrintDirAndFiles();
                        WorkSpace.Error(3, $"Не удалось скопировать файл {sourceFile}");
                    }
                }
                else
                {
                    WorkSpace.DrawFrame();
                    WorkSpace.PrintDirAndFiles();
                    WorkSpace.Error(3, $"Файл {sourceFile} не найден");
                }
            }

            //переименование файла
            void rn()
            {
                if (param[0]!=null && param[1] != null)
                {
                    string sourceFile = Path.GetFullPath(param[0]);
                    string newNameFile = param[1];

                    if (File.Exists(sourceFile))
                    {
                        try
                        {
                            File.Copy(sourceFile, Path.Combine(Environment.CurrentDirectory, Path.GetFileName(newNameFile)), true);
                            //если файл создался, то удаляем старый
                            if (File.Exists(Path.Combine(Environment.CurrentDirectory, newNameFile)))
                                File.Delete(sourceFile);
                            WorkSpace.DrawFrame();
                            WorkSpace.PrintDirAndFiles();
                        }
                        catch (Exception)
                        {
                            WorkSpace.DrawFrame();
                            WorkSpace.PrintDirAndFiles();
                            WorkSpace.Error(3, $"Не удалось переименовать файл {sourceFile}");
                        }
                    }
                    else
                    {
                        WorkSpace.DrawFrame();
                        WorkSpace.PrintDirAndFiles();
                        WorkSpace.Error(3, $"Файл {sourceFile} не найден");
                    }
                }
                
            }

            //удаление файла
            void del()
            {
                string fileToDel = Path.Combine(Environment.CurrentDirectory, param[0]);
                if (File.Exists(fileToDel))
                {
                    try
                    {
                        File.Delete(fileToDel);
                    }
                    catch (Exception)
                    {
                        WorkSpace.DrawFrame();
                        WorkSpace.PrintDirAndFiles();
                        WorkSpace.Error(2, $"Не удалось удалить файл {fileToDel}");
                    }
                    WorkSpace.DrawFrame();
                    WorkSpace.PrintDirAndFiles();
                }
                else
                {
                    WorkSpace.DrawFrame();
                    WorkSpace.PrintDirAndFiles();
                    WorkSpace.Error(2, $"Не удалось найти файл {fileToDel}");
                }
            }

            //help
            void helpPrint()
            {
                int str = 3; //начальная строка

                //процедура добавления строки на экран
                void addString(string strToAdd)
                {
                    Console.SetCursorPosition(1, str);
                    Console.Write(strToAdd);
                    str++;
                }

                WorkSpace.DrawFrame();
                WorkSpace.VerticalLine(WorkSpace.windowWidth / 3, 3, WorkSpace.windowHeigh - 4, ' ');
                Console.ForegroundColor = ConsoleColor.Yellow;
                addString("Доступные команды:");
                addString("cd [..] путь [pd 2] [pf 3] - переход к папке по абсолютному или относительному пути");
                addString("             [pd 2] [pf 3] - номера страниц листинга директорий и файлов");
                addString("    ..                     - переход в родительский каталог");
                addString("setpage [pd 2] [pf 3]      - смена номера листинга страниц");
                addString("mkdir (название каталога)  - создание каталога");
                addString("rmdir (название каталога)  - удаление каталога");
                addString("cp (имя файла) (путь копирования) - копирование файла");
                addString("rn (имя файла) (новое имя файла)  - переименование файла");
                addString("cf (имя файла) (\"содержание файла\") - создание файла");
                addString("cat (имя файла)                     - просмотр файла");
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(2, WorkSpace.windowHeigh - 1);
            }

            //обработчик команд
            switch (curCommand)
            {
                case "cd":
                    changeDir();
                    break;

                case "setpage":
                    WorkSpace.DrawFrame();
                    WorkSpace.PrintDirAndFiles(Convert.ToInt32(searchParametr("pd")), Convert.ToInt32(searchParametr("pf")));
                    break;

                case "mkdir":
                    mkDir();
                    break;

                case "rmdir":
                    rmDir();
                    break;

                case "cp":
                    cpFile();
                    break;

                case "rn":
                    rn();
                    break;

                case "del":
                    del();
                    break;

                case "cf":
                    cf();
                    break;

                case "cat":
                    cat();
                    break;

                case "help":
                    helpPrint();
                    break;



                default:
                    WorkSpace.DrawFrame();
                    WorkSpace.Error(2, "Неверная команда");
                    break;
            }
        }
    }
}
