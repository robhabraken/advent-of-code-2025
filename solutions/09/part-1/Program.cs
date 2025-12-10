var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\09\\input.txt");

var largestArea = 0L;

var tiles = new Tile[lines.Length];
for (var i = 0; i < lines.Length; i++)
    tiles[i] = new Tile(lines[i]);

for (var a = 0; a < tiles.Length; a++)
    for (var b = a + 1; b < tiles.Length; b++)
        if (tiles[a].x != tiles[b].x && tiles[a].y != tiles[b].y)
        {
            var area = (Math.Abs(tiles[a].x - tiles[b].x) + 1) * (Math.Abs(tiles[a].y - tiles[b].y + 1));
            if (area > largestArea)
                largestArea = area;
        }

Console.WriteLine(largestArea);

class Tile
{
    public long x;
    public long y;

    public Tile(string input)
    {
        var coords = input.Split(',').Select(long.Parse).ToArray();
        x = coords[0];
        y = coords[1];
    }
}