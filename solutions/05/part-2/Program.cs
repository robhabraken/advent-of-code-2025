var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\05\\input.txt");

var freshIngredients = 0L;
var ranges = new List<(long from, long to)>();

foreach (var line in lines)
{
    if (string.IsNullOrEmpty(line)) break;

    var range = line.Split('-').Select(long.Parse).ToArray();
    ranges.Add((range[0], range[1]));
}

while (merge(ref ranges)) ;

foreach (var (from, to) in ranges.Distinct())
    freshIngredients += to - from + 1;

Console.WriteLine(freshIngredients);

bool merge(ref List<(long from, long to)> ranges)
{
    var modified = false;
    var result = new List<(long from, long to)>();
    foreach (var range in ranges)
    {
        var overlaps = false;
        foreach (var compare in ranges)
            if (!range.Equals(compare) && isOverlapping(range, compare))
            {
                overlaps = modified = true;
                result.Add((Math.Min(range.from, compare.from), Math.Max(range.to, compare.to)));
                break;
            }

        if (!overlaps)
            result.Add(range);
    }
    ranges = result;
    return modified;
}

bool isOverlapping((long from, long to) a, (long from, long to) b) => (a.from >= b.from && a.from <= b.from && a.to > b.to) ||
                                                                        (a.from < b.from && a.to <= b.to && a.to >= b.from) ||
                                                                        (a.from < b.from && a.to > b.to) || (a.from >= b.from && a.to <= b.to);