using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;


// define starting values
int xVaule = 1;
int nrOfCycles = 0;
int nrOfSignalStrength = 0;
int signalStrength = 0;
int firstCalculationCycle = 20;
int cycleInterval = 40;

// create basic CRT with dots
int rowSize = 6;
int columnSize = 40;
String[,] CRT = new String[rowSize, columnSize];

for (int i = 0; i < rowSize; i++)
{
    for (int j = 0; j < columnSize; j++)
    {
        CRT[i, j] = ".";
    }
}

// path to input file
string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\resources", "input.txt");

// read the input, make calculations and present the result
string[] inputs = ReadInput(filePath);
HandleInput(inputs);
PresentResult();


string[] ReadInput(string filePath)
{
    string[] readInput = new string[] {};

    if (File.Exists(filePath))
    {
        readInput = File.ReadAllLines(filePath);
    }
    else
    {
        Console.WriteLine("readInput, file not found");
    }
    return readInput;
}

void HandleInput(string[] inputs)
{
    foreach (string input in inputs)
    {
        if (input == "noop")
        {
            nrOfCycles++;
            DrawPixel();
            if (ShouldCalculateSignalStrength())
            {
                signalStrength += CalculateSignalStrength();
                nrOfSignalStrength++;
            }
        }
        else if (input.StartsWith("addx"))
        {
            int value = 0;
            try
            {
                string[] splitInput = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                value = int.Parse(splitInput[1]);
            }
            catch (Exception ex) { 
                Console.WriteLine("The input: ", input, " is invalid");
                continue;
            }

            for (int i = 0; i < 2; i++)
            {
                nrOfCycles++;
                if (ShouldCalculateSignalStrength())
                {
                    signalStrength += CalculateSignalStrength();
                    nrOfSignalStrength++;
                }

                if (i == 1)
                {
                    xVaule += value;
                }

                DrawPixel();
            }
        }
        else
        {
            Console.WriteLine("The input: ", input, " is invalid");
        }
    }
}

void DrawPixel()
{
    int row = nrOfCycles / columnSize;
    int normalizedNrCycles = nrOfCycles - columnSize * row;

    if ((xVaule - 1) <= normalizedNrCycles && normalizedNrCycles <= (xVaule + 1))
    {
        int column = normalizedNrCycles % columnSize;
        CRT[row, column] = "#";
    }
}

bool ShouldCalculateSignalStrength()
{
    if (nrOfCycles == firstCalculationCycle)
        return true;

    int mod = nrOfCycles % cycleInterval;
    if (mod == firstCalculationCycle)
    {
        return true;
    }
    return false;
}

int CalculateSignalStrength()
{
    return ((firstCalculationCycle + cycleInterval * nrOfSignalStrength) * xVaule);
}

void PresentResult()
{
    Console.WriteLine("Part 1: Final signal strength value: {0}", signalStrength);
    Console.WriteLine("Part 2:");
    for (int i = 0; i < rowSize; i++)
    {
        var row = new string[columnSize];
        for (int j = 0; j < columnSize; j++)
        {
            row[j] = CRT[i, j];
        }

        Console.WriteLine(string.Join(" ", row));
    }
}
