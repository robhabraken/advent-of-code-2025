var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\12\\input.txt");

var presents = new List<int>();
var regions = new List<string>();
foreach (var line in lines)
    if (line.EndsWith(':'))
        presents.Add(0);
    else if (line.Contains('x'))
        regions.Add(line);
    else
        presents[^1] += line.Replace(".", "").Length;

var regionsFit = 0;
foreach (var region in regions)
{
    var dimensions = region[..region.IndexOf(':')].Split('x').Select(int.Parse).ToArray();
    var gifts = region[(region.IndexOf(' ') + 1)..].Split(' ').Select(int.Parse).ToArray();

    var regionArea = dimensions[0] * dimensions[1];
    var giftArea = 0;
    for (var i = 0; i < gifts.Length; i++)
        giftArea += gifts[i] * presents[i];

    if (giftArea < regionArea)
        regionsFit++;
}

Console.WriteLine(regionsFit);