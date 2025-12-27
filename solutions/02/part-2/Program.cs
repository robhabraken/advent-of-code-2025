var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\02\\input.txt");

var invalidIDs = 0L;

foreach (var range in lines[0].Split(','))
{
    var edges = range.Split('-').Select(long.Parse).ToArray();
    for (var id = edges[0]; id <= edges[1]; id++)
    {
        var strId = id.ToString();
        for (var i = 1; i <= strId.Length / 2; i++)
            if (strId.Replace(strId[0..i], string.Empty).Length == 0)
            {
                invalidIDs += id;

                // skip to the increment of the pattern found if possible
                if (i >= 2 && strId[0..i].Distinct().Count() > 2)
                    id = long.Parse($"{int.Parse(strId[0..i]) + 1}".PadRight(strId.Length, '0'));

                break;
            }
    }
}

Console.WriteLine(invalidIDs);