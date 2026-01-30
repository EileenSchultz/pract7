using App;

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, _) => cts.Cancel();

var urls = Input.GetUrls();
var dest = Input.GetOutputFile();
var destStream = dest.OpenWrite();

await Parallel.ForEachAsync(urls, cts.Token, async (url, ct) =>
{
    try
    {
        ct.ThrowIfCancellationRequested();
         
        using var http = new HttpClient();
        await using var content = await http.GetStreamAsync(url, ct);
        await content.CopyToAsync(destStream, ct);
        
        Console.WriteLine($"Загрузка файла {url} в файл {destStream.Name}");
    }
    catch(Exception e)
    {
        if (e is OperationCanceledException)
        {
            destStream.Close();
            File.Delete(destStream.Name);
        }
        Console.WriteLine("Отмена операции");
    }
});

await destStream.DisposeAsync();
