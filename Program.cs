using System;
using System.IO;
using System.Linq;
using System.Net;
using ConsoleApp1;

class FileDownloader
{
    void FileHandling()
    {
        Console.WriteLine("Put in a name of a lifter");
        string Lifter = Console.ReadLine().Replace(" ", "").ToLower();
        try
        {
            string OpenIPFUrl = $"https://www.openipf.org/api/liftercsv/{Lifter}";
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(OpenIPFUrl, $"{Lifter}.csv");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Person has not been found in the database.");
            throw;
        }
    }
    
    public void CSVHandler()
    {
        string Extension = "*.csv"; 
        
        string Path = AppContext.BaseDirectory;
        string DestinationDirectory = "lifters"; 
        
        string fullDestinationPath = System.IO.Path.Combine(Path, DestinationDirectory);
        bool csvExists = Directory.EnumerateFiles(Path, Extension).Any();
        
        try
        {
            if (!Directory.Exists(fullDestinationPath)) 
            {
                Directory.CreateDirectory(fullDestinationPath);
            }
            else if (csvExists)  
            {
                Console.WriteLine("CSV file is found!"); 
                foreach (string file in Directory.EnumerateFiles(Path, Extension))
                {
                    string destinationPath = System.IO.Path.Combine(fullDestinationPath, System.IO.Path.GetFileName(file));
                    File.Move(file, destinationPath, true);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    internal static async Task Main()
    {
        FileDownloader fileDownloader = new FileDownloader();
        
        fileDownloader.FileHandling();
        fileDownloader.CSVHandler();

        LifterData lifterData = new LifterData();
        await lifterData.LifterInformation();
    }
}