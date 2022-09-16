﻿namespace FileScaner
{
    public static class DirectoryExtension
    {
        public static long GetDirSize(DirectoryInfo d)
        {
            long size = 0;
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += GetDirSize(di);
            }
            return size;
        }
    }
    class Program
    {
        public static void Main(string[] args)
        {
            //
            long dTotalSize = 0, fTotalSize = 0;
            long totalSizeKb = 0, totalSizeMb = 0;

            string dirPath = "/home/admina/SFtest";
            Console.WriteLine("Введите путь к папке:");
            //dirPath = Console.ReadLine();
            Console.WriteLine("Указан путь: {0}", dirPath);
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            //Console.WriteLine("root: {0}", dirInfo.Root);
            //вложенные папки и файлы
            var folders = dirInfo.GetDirectories();
            if (!dirInfo.Exists)
            {
                Console.WriteLine("Папка: {0} не существует!", dirPath);
            }
            else    //папка есть
            {
                //
                GetFileInfo(dirInfo, out fTotalSize);
                //
                GetFolderInfo(folders, out dTotalSize);
                Console.WriteLine("Размер пакпки {0}: {1} байт", dirInfo.Name, dTotalSize + fTotalSize);
                //totalSizeKb = (dTotalSize + fTotalSize) / 1024;
                //totalSizeMb = totalSizeKb / 1024;
                //Console.WriteLine("Размер пакпки {0}: {1} Mб", dirInfo.Name, totalSizeMb);
            }
        }
        public static void GetFolderInfo(DirectoryInfo[] folders, out long dTotalSize)
        {
            long dSize = 0;
            //Console.WriteLine("папки: ");
            foreach (var folder in folders)
            {
                try
                {
                    //Console.WriteLine("{0} - {1}байт", folder.Name, DirectoryExtension.GetDirSize(folder));
                    dSize = dSize + DirectoryExtension.GetDirSize(folder);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            dTotalSize = dSize;
        }
        public static void GetFileInfo(DirectoryInfo d, out long fTotalSize)
        {
            //
            long fSize = 0;
            //Console.WriteLine("Файлы: ");
            foreach(var file in d.GetFiles())
            {
                //Console.WriteLine("{0} - {1}байт", file.Name, file.Length);
                fSize = fSize + file.Length;
            }
            fTotalSize = fSize;
        }
    }
}