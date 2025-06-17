using System.IO.Pipes; 
using System.Diagnostics; 
using System.Collections.Concurrent; 
using System.Threading; 

class Master
{
    static ConcurrentBag<string> rezultatas = new ConcurrentBag<string>(); 
static void Main(string[] eil) 
{
        SetProcessorAffinity(0); 

        string pipe1 = eil.Length > 0 ? eil[0] : "agent1"; 
        string pipe2 = eil.Length > 1 ? eil[1] : "agent2"; 

        var thread1 = new Thread(() => ReceiveData(pipe1));
        var thread2 = new Thread(() => ReceiveData(pipe2));

        thread1.Start(); 
        thread2.Start(); 

        thread1.Join(); 
        thread2.Join(); 
Console.WriteLine("Duomenys surinkti iš abieju agentu: ");


        var visiduomenys = rezultatas 
    .Select(line => 
     {
        var duomenys = line.Split(':'); 
        return (file: duomenys[0], word: duomenys[1], count: int.Parse(duomenys[2])); 
     })                     
     .GroupBy(grupavimas => (grupavimas.file, grupavimas.word)) 
     .Select(grupe => $"{grupe.Key.file}:{grupe.Key.word}:{grupe.Sum(grupavimas => grupavimas.count)}"); 

        Console.WriteLine("Zodžiu indeksas:");
        foreach (var line in visiduomenys) 
        Console.WriteLine(line);
    }

    static void ReceiveData(string mypipe) 
    {
        using var pipe = new NamedPipeServerStream(mypipe, PipeDirection.In);
        pipe.WaitForConnection();

        using var reader = new StreamReader(pipe); 
        string? line;
        while ((line = reader.ReadLine()) != null)
        rezultatas.Add(line);
    }

    static void SetProcessorAffinity(int core) 
    {
        if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux()) 
        {
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)0x1; 
        }
        else
        {
            Console.WriteLine("Procesoriaus branduolio prioritetas negali buti nustatytas ant sios operacines sistemos."); 
        }
    }
}
