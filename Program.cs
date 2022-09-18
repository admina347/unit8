namespace DirectoryManager
{
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
                    DirectoryExtension.GetFileInfo(dirInfo, out fTotalSize);
                    DirectoryExtension.GetFolderInfo(folders, out dTotalSize);
                    sSize = dTotalSize + fTotalSize;
                    Console.WriteLine("Исходный размер пакпки {0}: {1} байт", dirInfo.Name, sSize);                    
                    //Очистить папку
                    DirectoryExtension.DelFolder(dirInfo);
                    Console.WriteLine("Удаление завершено.");
                    //получить список директорий после очистки.
                    folders = dirInfo.GetDirectories();
                    //получить размер после очистки
                    DirectoryExtension.GetFileInfo(dirInfo, out fTotalSize);
                    DirectoryExtension.GetFolderInfo(folders, out dTotalSize);
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
    }
}