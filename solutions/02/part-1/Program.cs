var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\02\\input.txt");

var answer = 0L;
foreach (var range in lines[0].Split(','))
{
    var edges = range.Split('-');
    for (var id = long.Parse(edges[0]); id <= long.Parse(edges[1]); id++)
    {
        var strId = id.ToString();
        if (strId.Length % 2 == 0 && strId[0..(strId.Length / 2)] == strId[(strId.Length / 2)..(strId.Length)])
            answer += id;
    }
}

Console.WriteLine(answer);