using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1;

public class LifterData
{
    private static readonly CultureInfo NlCulture = new("nl-NL");
    public async Task LifterInformation()
    {
        string csvDirectory = Path.Combine(AppContext.BaseDirectory, "lifters");
        var csvFile = Directory.GetFiles(csvDirectory, "*.csv").FirstOrDefault();

        if (csvFile == null)
        {
            Console.WriteLine("No information found");
            return;
        }

        var lines = await File.ReadAllLinesAsync(csvFile);
        if (lines.Length < 2)
        {
            Console.WriteLine("CSV contains no data");
            return;
        }

        var header = lines[0].Split(',');

        int nameIndex  = GetIndex(header, "Name");
        int ageIndex   = GetIndex(header, "Age");
        int bwIndex    = GetIndex(header, "BodyweightKg");
        int wcIndex    = GetIndex(header, "WeightClassKg");
        int squatIndex = GetIndex(header, "Best3SquatKg");
        int benchIndex = GetIndex(header, "Best3BenchKg");
        int deadIndex  = GetIndex(header, "Best3DeadliftKg");
        int totalIndex = GetIndex(header, "TotalKg");
        int glIndex    = GetIndex(header, "Goodlift");

        int[] indices =
        {
            nameIndex, ageIndex, bwIndex, wcIndex,
            squatIndex, benchIndex, deadIndex, totalIndex, glIndex
        };
        
        var lifter = lines
            .Skip(1)
            .Select(l => l.Split(','))
            .GroupBy(c => c[nameIndex])
            .Select(g => new
            {
                Name = g.Key,
                Age = g.Max(c => ToInt(c[ageIndex])),
                Bodyweight = g.Max(c => ToDouble(c[bwIndex])),
                WeightClass = g.Last()[wcIndex],
                BestSquat = g.Max(c => ToInt(c[squatIndex])),
                BestBench = g.Max(c => ToInt(c[benchIndex])),
                BestDeadlift = g.Max(c => ToInt(c[deadIndex])),
                Total = g.Max(c => ToInt(c[totalIndex])),
                GL = g.Max(c => ToDouble(c[glIndex]))
            })
            .First();

        PrintLifter(lifter);
    }
    
    private static int GetIndex(string[] header, string columnName)
    {
        return Array.FindIndex(
            header,
            h => h.Trim().Equals(columnName, StringComparison.OrdinalIgnoreCase)
        );
    }
    private static int ToInt(string value)
        => int.TryParse(value, out var r) ? r : 0;

    private static double ToDouble(string value)
        => double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var r) ? r : 0;

    private static void PrintLifter(dynamic lifter)
    {
        Console.WriteLine();
        Console.WriteLine("===== LIFTER SUMMARY =====");
        Console.WriteLine($"Name         : {lifter.Name}");
        Console.WriteLine($"Age          : {lifter.Age}");
        Console.WriteLine($"Bodyweight   : {lifter.Bodyweight.ToString("0.0", NlCulture)} kg");
        Console.WriteLine($"WeightClass  : {lifter.WeightClass} kg");
        Console.WriteLine($"Best Squat   : {lifter.BestSquat.ToString("N0", NlCulture)} kg");
        Console.WriteLine($"Best Bench   : {lifter.BestBench.ToString("N0", NlCulture)} kg");
        Console.WriteLine($"Best Deadlift: {lifter.BestDeadlift.ToString("N0", NlCulture)} kg");
        Console.WriteLine($"Total        : {lifter.Total.ToString("N0", NlCulture)} kg");
        Console.WriteLine($"Goodlift     : {lifter.GL.ToString("0.00", NlCulture)}");
        Console.WriteLine("==========================");
    }
}
