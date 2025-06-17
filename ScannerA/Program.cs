using System.IO.Pipes;
using System.Diagnostics;
using System.Collections.Concurrent;

class Program
{
    static BlockingCollection<string> wordQueue = new(); 

    static string mypipe = "agent1"; 

    static void Main(string[] eil) 
    {
        string failovieta = eil.Length > 0 ? eil[0] : @"C:\Users\Dovmar\Desktop\csharp\ScannerA\TextFiles";
        SetProcessorAffinity(1); 

        var skaitymas = new Thread(() => ReadFiles(failovieta)); 
        var siuntimas = new Thread(() => SendData(mypipe)); 

        skaitymas.Start(); 
        siuntimas.Start();  

        skaitymas.Join(); 
        wordQueue.CompleteAdding(); 
        siuntimas.Join(); 
    }

    static void ReadFiles(string kelias) 
    {
        var masyvas = Directory.GetFiles(kelias, "*.txt"); 

        foreach (var file in masyvas) 
        {
            var tekstas = File.ReadAllText(file); 
            var zodziai = tekstas.Split(new[] { ' ', ',', '.', ';','\n', '\r' }, StringSplitOptions.RemoveEmptyEntries); 
            var zodziuGrup = zodziai.GroupBy(vienodas => vienodas); 

            foreach (var grupe in zodziuGrup) 
            {
                string line = $"{Path.GetFileName(file)}:{grupe.Key}:{grupe.Count()}"; 
                wordQueue.Add(line); 
            }
        }
    }

    static void SendData(string mypipe) 
    {
        using var pipe = new NamedPipeClientStream(".", mypipe, PipeDirection.Out); 
        pipe.Connect(); 

        using var writer = new StreamWriter(pipe) { AutoFlush = true };  

        foreach (var line in wordQueue.GetConsumingEnumerable()) 
        {
            writer.WriteLine(line); 
        }
    }

    static void SetProcessorAffinity(int core) 
    {
        if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux()) 
        {
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)0x2;
        }
        else
        {
            Console.WriteLine("Procesoriaus branduolio prioritetas negali buti nustatytas ant sios sistemos."); 
        }
    }
}

