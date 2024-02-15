class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите путь до папки:");
        string folderPath = Console.ReadLine();

        try
        {
            CleanUnusedFilesAndFolders(folderPath);
            Console.WriteLine("Операция завершена успешно.");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Ошибка доступа: {ex.Message}");
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine($"Указанная папка не существует: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }


    static void CleanUnusedFilesAndFolders(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            throw new DirectoryNotFoundException($"Папка не найдена: {folderPath}");
        }

        DirectoryInfo directory = new DirectoryInfo(folderPath);

        
        DateTime threshold = DateTime.Now.AddMinutes(-30);

        foreach (FileInfo file in directory.GetFiles())
        {
            if (file.LastAccessTime < threshold)
            {
                file.Delete();
                Console.WriteLine($"Удален файл: {file.FullName}");
            }
        }

        foreach (DirectoryInfo subDirectory in directory.GetDirectories())
        {
            CleanUnusedFilesAndFolders(subDirectory.FullName);
        }

        
        if (directory.GetFiles().Length == 0 && directory.GetDirectories().Length == 0)
        {
            directory.Delete();
            Console.WriteLine($"Удалена пустая папка: {directory.FullName}");
        }
    }
}