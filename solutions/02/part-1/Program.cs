var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\02\\input.txt");

var answer = 0L;
foreach (var range in lines[0].Split(','))
{
    var edges = range.Split('-');
    for (var id = long.Parse(edges[0]); id <= long.Parse(edges[1]); id++)
    {
        var digitCount = (int)Math.Log10(id) + 1;
        if (digitCount % 2 == 0)
        {
            var factor = (long)Math.Pow(10, digitCount / 2);
            var firstHalf = id / factor;
            if (firstHalf * factor + firstHalf == id)
                answer += id;
        }
    }
}

Console.WriteLine(answer);