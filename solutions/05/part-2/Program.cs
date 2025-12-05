var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\05\\input.txt");

var answer = 0L;
var ranges = new List<Range>();

foreach (var line in lines)
{
    if (string.IsNullOrEmpty(line)) break;

    var range = line.Split('-').Select(long.Parse).ToArray();
    ranges.Add(new Range(range[0], range[1]));
}

while (merge(ref ranges));

foreach (var range in ranges.Distinct())
    answer += range.to - range.from + 1;

Console.WriteLine(answer);

bool merge(ref List<Range> ranges)
{
    var modified = false;
    var result = new List<Range>();
    foreach (var range in ranges)
    {
        var overlaps = false;
        foreach (var compare in ranges)
        {
            if (!range.Equals(compare))
            {
                if (isOverlapping(range, compare))
                {
                    overlaps = modified = true;
                    result.Add(new Range(Math.Min(range.from, compare.from), Math.Max(range.to, compare.to)));
                    break;
                }
            }
        }
        if (!overlaps)
            result.Add(range);
    }
    ranges = result;
    return modified;
}

bool isOverlapping(Range a, Range b) => (a.from >= b.from && a.from <= b.from && a.to > b.to) ||
                                        (a.from < b.from && a.to <= b.to && a.to >= b.from) ||
                                        (a.from < b.from && a.to > b.to) ||
                                        (a.from >= b.from && a.to <= b.to);

struct Range
{
    public long from;
    public long to;

    public Range(long from, long to)
    {
        this.from = from;
        this.to = to;
    }
}