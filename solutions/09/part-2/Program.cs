var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\09\\input.txt");

var largestArea = 0L;

var tiles = new Tile[lines.Length];
for (var i = 0; i < lines.Length; i++)
    tiles[i] = new Tile(lines[i]);

var edges = new Edge[tiles.Length];
for (int a = 0, b = 1; a < tiles.Length; a++, b++)
    edges[a] = new Edge(tiles[a], tiles[b < tiles.Length ? b : 0]);

for (var a = 0; a < tiles.Length; a++)
{
    for (var b = a + 1; b < tiles.Length; b++)
    {
        if (tiles[a].x != tiles[b].x && tiles[a].y != tiles[b].y)
        {
            var area = (Math.Abs(tiles[a].x - tiles[b].x) + 1) * (Math.Abs(tiles[a].y - tiles[b].y + 1));
            if (area > largestArea)
            {
                // determine coordinates of the two other corners of the rectangle
                var c = new Tile(tiles[a].x, tiles[b].y);
                var d = new Tile(tiles[b].x, tiles[a].y);
                if ((tiles[a].x < tiles[b].x && tiles[a].y < tiles[b].y) ||
                    (tiles[a].x > tiles[b].x && tiles[a].y > tiles[b].y))
                {
                    c.x = tiles[b].x;
                    c.y = tiles[a].y;
                    d.x = tiles[a].x;
                    d.y = tiles[b].y;
                }

                // if the other corners are not in the given polygon, this rectangle isn't valid
                // this is the cheapest check to invalidate this rectangle and we can abort right away
                if (!liesInPolygon(c) || !liesInPolygon(d))
                    continue;

                // now define the edges of the rectangle to validate
                var rectangle = new Edge[4]
                {
                    new (tiles[a], c),
                    new (c, tiles[b]),
                    new (tiles[b], d),
                    new (d, tiles[a])
                };

                // build list of minimum tiles to be checked (both sides of each intersection with each polygon edge)
                var tilesToCheck = new List<Tile>();
                foreach (var edge in edges)
                {
                    for (var i = 0; i < 4; i++)
                    {
                        var intersection = edge.Intersect(rectangle[i]);
                        if (edge.Intersect(rectangle[i]) != null)
                        {
                            if (rectangle[i].a.x == rectangle[i].b.x)
                            {
                                var x = rectangle[i].a.x;
                                var yBefore = intersection.y - 1;
                                var yAfter = intersection.y + 1;
                                if (Edge.IsBetween(yBefore,rectangle[i].a.y, rectangle[i].b.y))
                                    tilesToCheck.Add(new Tile(x, yBefore));
                                if (Edge.IsBetween(yAfter,rectangle[i].a.y, rectangle[i].b.y))
                                    tilesToCheck.Add(new Tile(x, yAfter));
                            }
                            else
                            {
                                var xBefore = intersection.x - 1;
                                var xAfter = intersection.x + 1;
                                var y = rectangle[i].a.y;
                                if (Edge.IsBetween(xBefore,rectangle[i].a.x, rectangle[i].b.x))
                                    tilesToCheck.Add(new Tile(xBefore, y));
                                if (Edge.IsBetween(xAfter, rectangle[i].a.x, rectangle[i].b.x))
                                    tilesToCheck.Add(new Tile(xAfter, y));
                            }
                        }
                    }
                }

                // validate all tiles to be checked, as soon as we find one that is outside of the polygon, abort
                var valid = true;
                foreach (var tile in tilesToCheck)
                    if (!liesInPolygon(tile))
                    {
                        valid = false; break;
                    }

                if (valid)
                    largestArea = area;
            }
        }
    }
}

Console.WriteLine(largestArea);

bool liesInPolygon(Tile tile)
{
    // use raycasting and the odd-even rule to determine if the tile is inside of our polygon
    var ray = new Edge(tile, new Tile(0, 0));

    var count = 0;
    var onEdge = false;
    foreach (var edge in edges)
    {
        if (Edge.IsBetween(tile.x, edge.a.x, edge.b.x) && Edge.IsBetween(tile.y, edge.a.y, edge.b.y))
        {
            onEdge = true;
            break;
        }

        if (edge.Intersect(ray) != null)
            count++;
    }

    return onEdge || count % 2 != 0;
}

class Edge(Tile a, Tile b)
{
    public Tile a = a;
    public Tile b = b;

    public Tile Intersect(Edge other)
    {
        double denominator = (a.x - b.x) * (other.a.y - other.b.y) - (a.y - b.y) * (other.a.x - other.b.x);

        // edges are either parallel or coincident
        if (Math.Abs(denominator) == 0) return null;

        double px = ((a.x * b.y - a.y * b.x) * (other.a.x - other.b.x) -
                     (a.x - b.x) * (other.a.x * other.b.y - other.a.y * other.b.x)) / denominator;

        double py = ((a.x * b.y - a.y * b.x) * (other.a.y - other.b.y) -
                     (a.y - b.y) * (other.a.x * other.b.y - other.a.y * other.b.x)) / denominator;

        // check if the intersection lies on both edges (otherwise the intersection doesn't count as crossing an edge)
        if (!(IsBetween(px, a.x, b.x) && IsBetween(py, a.y, b.y)) ||
            !(IsBetween(px, other.a.x, other.b.x) && IsBetween(py, other.a.y, other.b.y)))
            return null;

        return new Tile((long)px, (long)py);
    }

    public static bool IsBetween(double value, double from, double to)
    {
        return value >= Math.Min(from, to) && value <= Math.Max(from, to);
    }
}

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

    public Tile(long x, long y)
    {
        this.x = x;
        this.y = y;
    }
}