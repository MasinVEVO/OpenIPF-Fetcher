# OpenIPF Lifter Analyzer (C# Console App)

A C# console application that downloads lifter data from the OpenIPF API, processes the CSV file, and outputs a clean summary of a lifter’s best performances.

The application aggregates multiple competition entries per lifter into a single overview, showing only the best results.

---

## Features

- Download lifter data directly from the OpenIPF API
- Automatically store CSV files in a dedicated directory
- Parse CSV data without external libraries
- Aggregate multiple competition entries per lifter
- Display:
  - Name
  - Age
  - Bodyweight
  - Weight Class
  - Best Squat
  - Best Bench
  - Best Deadlift
  - Total
  - Goodlift score
- Proper number formatting (Dutch locale supported)

---

## How It Works

1. The user enters the name of a lifter.
2. The application downloads the corresponding CSV file from OpenIPF.
3. The CSV file is moved to the `lifters` directory.
4. All rows are grouped by lifter name.
5. The best values across all competitions are calculated.
6. A single, clean summary is printed to the console.

---
