using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace DC_dotNet
{
    public class FilesAndFolders
    {
        public List<string> ListDirectory;
        public List<Files> ListFiles;
        //static public string currentPath = "";
        int level = 0;
        public FilesAndFolders()
        {
            ListDirectory = new List<string>();
            ListFiles = new List<Files>();
        }

        public void GetListDir(string path)
        {
            level++;
            if (level > 2) return;
            try
            {
                string[] dirList = Directory.GetDirectories(Path.Combine(path));

                foreach (var item in dirList)
                {
                    DirectoryInfo curDir = new DirectoryInfo(item);
                    string name="";
                    if (level > 1)
                        name += " ∟ ";
                    name += curDir.Name;
                    ListDirectory.Add(name);
                    GetListDir(item);
                    level--;
                }
            }

            catch(UnauthorizedAccessException)
            {
                WorkSpace.Error(2, "Отказ в доступе" );
            }
            catch (DirectoryNotFoundException)
            {
                WorkSpace.Error(2, "Директория не найдена");
            }
            catch
            {
                WorkSpace.Error(2, "Ошибка директории");
            }

        }

        internal void GetListDirForPage(int page)
        {
            int countLinesDisplay = WorkSpace.windowHeigh - 8;
            int firstIndex = page * (WorkSpace.windowHeigh - 8);
            string[] pageDirList = new string[countLinesDisplay];
            for (int i = 0; i < countLinesDisplay; i++)
            {
                if (i < ListDirectory.Count())
                {
                    if (ListDirectory.Count>(page * countLinesDisplay + i))
                    {
                        string dirName = ListDirectory[page * countLinesDisplay + i];
                        if (dirName.Length > WorkSpace.windowWidth / 3 - 3)
                        {
                            dirName = dirName.Substring(0, WorkSpace.windowWidth / 3 - 4);
                            dirName += "~~";
                        }
                        pageDirList[i] = dirName;
                    }
                }
            }
            int countDir = ListDirectory.Count;
            ListDirectory.Clear();
            foreach (var item in pageDirList)
            {
                if (item!=null)
                    ListDirectory.Add(item);
            }
            ListDirectory.Add($"    Страница: {page + 1} из {((countDir - 1) / (WorkSpace.windowHeigh - 8)) + 1}");
        }

        public void GetListFile(string path, int page=0)
        {
            try
            {
                string[] fileList = Directory.GetFiles(Path.Combine(path));
                string fileSizeStr = "";


                for (int i = (WorkSpace.windowHeigh - 8) * page; i < (WorkSpace.windowHeigh - 8) * (page + 1); i++)
                {
                    if (i <= fileList.Length - 1)
                    {
                        string fileName = Path.GetFileName(fileList[i]);
                        string fileDate = File.GetLastWriteTime(fileList[i]).ToString();
                        double fileSize = new FileInfo(fileList[i]).Length;
                        if (fileSize > 1073741824)
                        {
                            fileSize = Math.Round(fileSize / 1073741824, 3);
                            fileSizeStr = fileSize.ToString();
                            fileSizeStr += " GB";
                        }
                        else if (fileSize > 1048576)
                        {
                            fileSize = Math.Round(fileSize / 1048576, 3);
                            fileSizeStr = fileSize.ToString();
                            fileSizeStr += " MB";
                        }
                        else if (fileSize > 1024)
                        {
                            fileSize = Math.Round(fileSize / 1024, 3);
                            fileSizeStr = fileSize.ToString();
                            fileSizeStr += " KB";
                        }
                        else
                        {
                            fileSizeStr = fileSize.ToString();
                            fileSizeStr += " B";
                        }

                        if (fileName.Length > (WorkSpace.windowWidth - (WorkSpace.windowWidth / 3) -34))
                        {
                            fileName = fileName.Substring(0, (WorkSpace.windowWidth - (WorkSpace.windowWidth / 3) - 36));
                            fileName += "~~";
                        }
                        ListFiles.Add(new Files(fileName, fileDate, fileSizeStr));
                    }
                        
                }
                ListFiles.Add(new Files($"    Страница: {page + 1} из {((fileList.Length - 1)/(WorkSpace.windowHeigh - 8))+1}"));
            }
            catch
            {
                WorkSpace.Error(30,"Ошибка открытия файла");
            }
        }
    }
}
