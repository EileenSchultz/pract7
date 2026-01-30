namespace App;

/// <summary>
/// Считывает и валидирует ввод пользователя.
/// </summary>
public static class Input
{
    /// <summary>
    /// Считывает от пользователя URL файлов из интернета.
    /// </summary>
    public static string[] GetUrls()
    {
        string[] result = [];
        var valid = false;
        while (valid is false)
        {
            Console.Write("Введите нужные URL через пробел: ");
            result = Console.ReadLine()!.Split(' ');
            
            valid = result.Any(x => IsValidUrl(x) is false);
            if (valid is false)
            {
                Console.WriteLine("Ошибка! Вы ввели некорректные URL!");
            }
        }

        return result;
    }

    private static bool IsValidUrl(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
        {
            return false;
            
        }
        if (!Uri.TryCreate(str, UriKind.Absolute, out var uri))
        {
            return false;
        }
        return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
    }

    
    
    /// <summary>
    /// Считывает от пользователя путь до файла с результатом.
    /// </summary>
    public static FileInfo GetOutputFile()
    {
        while (true)
        {
            Console.Write("Введите путь до файла с результатом: ");
            try
            {
                var path = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(path))
                {
                    Console.WriteLine("Вы ввели пустой путь :(");
                    continue;
                }
                var file = new FileInfo(path);
                
                int counter = 1;

                while (file.Exists)
                {
                    //автоматически переименовываем файл если он уже существует
                    file = new FileInfo(Path.Combine(file.DirectoryName ?? Directory.GetCurrentDirectory(),
                        $"{Path.GetFileNameWithoutExtension(file.Name)}{counter}{file.Extension}"));
                    
                    counter++;
                }
                
                Console.WriteLine($"Файл сохранён: {file.FullName}");
                return file;
            }
            catch
            {
                Console.WriteLine("Произошла ошибка, вероятно вы ввели некорректный путь. Попробуйте ещё раз.");
            }
        }
    }
}