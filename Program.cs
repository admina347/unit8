namespace DirectoryManager
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
            long dTotalSize = 0, fTotalSize = 0;
            long sSize = 0;    //Исходный размер
            long eSize = 0; //размер после очистки
            long fSpace = 0;    //очищено байт
            string dirPath = "/home/admina/SFtest";
            Console.WriteLine("Введите путь к папке:");
            dirPath = Console.ReadLine();
            Console.WriteLine("Указан путь: {0}", dirPath);
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            //вложенные папки и файлы
            try
            {
                var folders = dirInfo.GetDirectories();
                if (!dirInfo.Exists)
                {
                    Console.WriteLine("Папка: {0} не существует!", dirPath);
                }
                else    //папка есть
                {
                    //получить исходный размер
                    GetFileInfo(dirInfo, out fTotalSize);
                    GetFolderInfo(folders, out dTotalSize);
                    sSize = dTotalSize + fTotalSize;
                    Console.WriteLine("Исходный размер пакпки {0}: {1} байт", dirInfo.Name, sSize);                    
                    //Очистить папку
                    DelFolder(dirInfo);
                    Console.WriteLine("Удаление завершено.");
                    //получить список директорий после очистки.
                    folders = dirInfo.GetDirectories();
                    //получить размер после очистки
                    GetFileInfo(dirInfo, out fTotalSize);
                    GetFolderInfo(folders, out dTotalSize);
                    eSize = dTotalSize + fTotalSize;
                    fSpace = sSize - eSize;
                    Console.WriteLine("Освобождено места: {0} байт", fSpace);
                    Console.WriteLine("Текущий размер {0}: {1} байт", dirInfo.Name, eSize);
                }
            }
            catch (DirectoryNotFoundException dirEx)
            {
                Console.WriteLine("Директория не найдена " + dirEx.Message);
            }
        }
        public static void GetFolderInfo(DirectoryInfo[] folders, out long dTotalSize)
        {
            long dSize = 0;
            foreach (var folder in folders)
            {
                try
                {
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
            long fSize = 0;
            foreach (var file in d.GetFiles())
            {
                fSize = fSize + file.Length;
            }
            fTotalSize = fSize;
        }

        //проверим файлы
        public static void DelFolder(DirectoryInfo dirInfo)
        {
            int fCount = 0, dCount = 0, efCount = 0, edCount = 0; ;
            DateTime laccessTime;    //посл доступ
            int lifeSpan = 1;    //через сколько протухло
            DateTime expTime;   //время просрочки
            DateTime curTime = DateTime.Now;    //время сейчас
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                laccessTime = file.LastAccessTime;
                expTime = laccessTime.AddMinutes(lifeSpan);
                //если срок вышел
                if (curTime > expTime)
                {
                    try //Пробуем удалить
                    {
                        file.Delete();
                        fCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        efCount = efCount + 1;
                    }
                }
            }
            if (fCount > 0)
            {
                Console.WriteLine("Файлы {0}шт в папке: {1} которые не использовались более {2} минут - удалены!", fCount, dirInfo.Name, lifeSpan);
            }
            else
            {
                Console.WriteLine("Файлы в папке: {0} свежие или отсутствуют", dirInfo.Name);
            }
            if (efCount > 0)
                Console.WriteLine("Не удалось удалить файлы: {0}шт.", efCount);
            //проверим подпапки
            foreach (DirectoryInfo dir in dirInfo.GetDirectories())
            {
                laccessTime = dir.LastAccessTime;
                expTime = laccessTime.AddMinutes(lifeSpan);
                //если срок вышел
                if (curTime > expTime)
                {
                    try //Пробуем удалить
                    {
                        dir.Delete(true);
                        dCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        edCount = edCount + 1;
                    }
                }
            }
            if (dCount > 0)
            {
                Console.WriteLine("Папки {0}шт с файлами и подпапками в каталоге: {1} которые не использовались более {2} минут - удалены!", dCount, dirInfo.Name, lifeSpan);
            }
            else
            {
                Console.WriteLine("Папки в каталоге: {0} свежие или отсутствуют", dirInfo.Name);
            }
            if (edCount > 0)
                Console.WriteLine("Не удалось удалить папки: {0}шт.", edCount);
        }
    }
}