var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\05\\input.txt");

var fresh = 0;
var ranges = new List<long[]>();

var readingRanges = true;
foreach (var line in lines)
{
    if (string.IsNullOrEmpty(line))
        readingRanges = false;
    else if (readingRanges)
        ranges.Add(line.Split('-').Select(long.Parse).ToArray());
    else
    {
        var ingredient = long.Parse(line);
        foreach (var range in ranges)
        {
            if (ingredient >= range[0] && ingredient <= range[1])
            {
                fresh++;
                break;
            }
        }
    }
}

Console.WriteLine(fresh);