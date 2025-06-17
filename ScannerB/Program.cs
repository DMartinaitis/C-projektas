using System.IO.Pipes;
using System.Diagnostics;
using System.Threading;
using System;

class Program
{
    static void Main(string[] eil)
    {
        string mypipe = "agent2"; 
        string failokelias = eil.Length > 0 ? eil[0] : @"C:\Users\Dovmar\Desktop\csharp\ScannerB\TextFiles"; 

        SetProcessorAffinity(2);

        var siuntimas = new Thread(() => SendData(mypipe, failokelias)); 
        siuntimas.Start(); 
    }

    static void SendData(string mypipe, string kelias) 
    {
        var failai = Directory.GetFiles(kelias, "*.txt"); 
        var skaiciavimai = new List<string>(); 

        foreach (var file in failai) 
        {
            var tekstas = File.ReadAllText(file); 
            var zodziai = tekstas.Split(new[] { ' ', ',', '.', ';', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var zodziuGrup = zodziai.GroupBy(vienodas => vienodas).Select(g => $"{Path.GetFileName(file)}:{g.Key}:{g.Count()}");

            skaiciavimai.AddRange(zodziuGrup); 
        }

        using var pipe = new NamedPipeClientStream(".", mypipe, PipeDirection.Out); 
        pipe.Connect(); 

        using var writer = new StreamWriter(pipe) { AutoFlush = true }; 
        foreach (var line in skaiciavimai)  
            writer.WriteLine(line); 
    }

    static void SetProcessorAffinity(int core) 
    {
        if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux()) 
        {
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)0x4; 
        }
        else
        {
            Console.WriteLine("Procesoriaus branduolio prioritetas negali buti nustatytas ant sios sistemos."); 
        }
    }
}
