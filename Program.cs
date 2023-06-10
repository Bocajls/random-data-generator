using System.Diagnostics;

while (true)
{
    Console.Write("Input amount of files to generate: ");

    if (!int.TryParse(Console.ReadLine(), out int amountOfFiles) || amountOfFiles <= 0)
    {
        Console.WriteLine($"Amount of files must be {(amountOfFiles <= 0 ? "greater than 0!" : "a whole number!")}...");
        Console.ReadLine();
        Console.Clear();
        continue;
    }

    Console.Write("Input size of each file in megabytes: ");

    if (!int.TryParse(Console.ReadLine(), out int sizeInMb) || sizeInMb <= 0)
    {
        Console.WriteLine($"Size of each file must be {(sizeInMb <= 0 ? "greater than 0 megabytes!" : "a whole number!")}...");
        Console.ReadLine();
        Console.Clear();
        continue;
    }

    Stopwatch watch = new();
    watch.Start();

    var filePath = "files/";
    Directory.CreateDirectory(filePath);
    Parallel.For(0, amountOfFiles, x =>
    {
        var fileName = $"{new Random().Next(10 ^ 8, int.MaxValue)}.txt";
        const int blockSize = 1024 * 8;
        const int blocksPerMb = (1024 * 1024) / blockSize;
        byte[] data = new byte[blockSize];
        Random rng = new();
        using FileStream stream = File.OpenWrite(filePath + fileName);
        for (int i = 0; i < sizeInMb * blocksPerMb; i++)
        {
            rng.NextBytes(data);
            stream.Write(data, 0, data.Length);
        }
        Console.WriteLine($"[Thread {Environment.CurrentManagedThreadId,2}][{sizeInMb}Mb] Generated {fileName}");
    });

    Console.WriteLine($"Generated {amountOfFiles} files of {sizeInMb}Mb, totalling {amountOfFiles * sizeInMb}Mb");
    Console.WriteLine($"Generation took {watch.ElapsedMilliseconds / 1000.0f} seconds");
    Console.ReadLine();
    Console.Clear();
}