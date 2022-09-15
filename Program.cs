namespace DirectoryManager
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            string dirPath = "/home/admina/SFtest";
            int fCount = 0, dCount = 0, efCount = 0, edCount = 0;;
            DateTime laccessTime;    //посл доступ
            int lifeSpan = 3;    //через сколько протухло
            DateTime expTime;   //время просрочки
            DateTime curTime = DateTime.Now;    //время сейчас
            Console.WriteLine("Введите путь к папке:");
            dirPath = Console.ReadLine();
            Console.WriteLine("Указан путь: {0}", dirPath);
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            if (!dirInfo.Exists)
            {
                Console.WriteLine("Папка: {0} не существует!", dirPath);
            }
            else    //папка есть
            {
                //проверим файлы
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
                if(fCount > 0)
                {
                    Console.WriteLine("Файлы {0}шт в папке: {1} которые не использовались более {2} минут - удалены!", fCount, dirInfo.Name, lifeSpan);
                }
                else
                {
                    Console.WriteLine("Файлы в папке: {0} свежие", dirInfo.Name);
                }
                if(efCount > 0 )     
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
                if(dCount > 0)
                {
                    Console.WriteLine("Папки {0}шт в каталоге: {1} которые не использовались более {2} минут - удалены!", dCount, dirInfo.Name, lifeSpan);
                }
                else
                {
                    Console.WriteLine("Папки в каталоге: {0} свежие", dirInfo.Name);
                }
                if(edCount > 0 )     
                    Console.WriteLine("Не удалось удалить папки: {0}шт.", edCount);
            }
        }
    }
}