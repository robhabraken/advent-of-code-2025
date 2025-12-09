var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\09\\input.txt");

var answer = 0L;
var points = new Point[lines.Length];
for (var i = 0; i < lines.Length; i++)
    points[i] = new Point(lines[i]);

for (var a = 0; a < points.Length; a++)
    for (var b = a + 1; b < points.Length; b++)
        if (points[a].x != points[b].x && points[a].y != points[b].y)
        {
            var sq = (Math.Abs(points[a].x - points[b].x) + 1) * (Math.Abs(points[a].y - points[b].y + 1));
            if (sq > answer)
                answer = sq;
        }

Console.WriteLine(answer);

class Point
{
    public long x;
    public long y;

    public Point(string input)
    {
        var t = input.Split(',').Select(long.Parse).ToArray();
        x = t[0];
        y = t[1];
    }
}