var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\02\\input.txt");

var answer = 0L;
foreach (var range in lines[0].Split(','))
{
    var edges = range.Split('-');
    for (var id = long.Parse(edges[0]); id <= long.Parse(edges[1]); id++)
    {
        var strId = id.ToString();
        for (var i = 1; i <= strId.Length / 2; i++)
            if (strId.Replace(strId[0..i], string.Empty).Equals(string.Empty))
            {
                answer += id;
                break;
            }
    }
}

Console.WriteLine(answer);