namespace DirectoryManager
{
    public static class DirectoryExtension
    {
        private static long GetDirSize(DirectoryInfo d)
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
        public static void GetFolderInfo(DirectoryInfo[] folders, out long dTotalSize)
        {
            long dSize = 0;
            foreach (var folder in folders)
            {
                try
                {
                    dSize = dSize + GetDirSize(folder);
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
        public static void DelFolder(DirectoryInfo dirInfo)
        {
            int fCount = 0, dCount = 0, efCount = 0, edCount = 0; ;
            DateTime laccessTime;    //посл доступ
            int lifeSpan = 30;    //через сколько протухло
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