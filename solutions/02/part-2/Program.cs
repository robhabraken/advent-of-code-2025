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
                break;
            }
    }
}

Console.WriteLine(invalidIDs);