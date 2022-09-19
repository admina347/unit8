using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask
{
    [Serializable]
    class Student
    {
        public Student(string name, string group, DateTime dateOfBirth)
        {
            Name = name;
            Group = group;
            DateOfBirth = dateOfBirth;
        }

        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            string fName = "Students.dat";
            string desctopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            DirectoryInfo dirInfo = new DirectoryInfo(desctopPath + "/Students/");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            else    //Удалим файлы чтобы не дописывать в них.
            {
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
            }
            string dirFullName = dirInfo.FullName;
            BinaryFormatter formatter = new BinaryFormatter();
            Student[] students = new Student[0];
            using (var fs = new FileStream(fName, FileMode.OpenOrCreate))
            {
                try
                {
                    students = (Student[])formatter.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Array.Sort(students, (x, y) => String.Compare(x.Name, y.Name));     //Сортировка
            foreach (Student student in students)
            {
                string filePath = dirFullName + student.Group + ".txt";
                string strToWrite = student.Name + ", " + student.DateOfBirth.ToShortDateString();
                if (!File.Exists(filePath))
                {
                    using (StreamWriter sw = File.CreateText(filePath))
                    {
                        sw.WriteLine(strToWrite);
                    }
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    using (StreamWriter sw = fileInfo.AppendText())
                    {
                        sw.WriteLine(strToWrite);
                    }
                }
            }
            Console.WriteLine("Студенты раскиданы)");
        }
    }
}