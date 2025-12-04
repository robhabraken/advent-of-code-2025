var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\04\\input.txt");

var deltaMap = new int[8, 2] { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

var answer = 0;
for (var y = 0; y < lines.Length; y++)
    for (var x = 0; x < lines[y].Length; x++)
        if (lines[y][x].Equals('@'))
            isAccessible(y, x);

Console.WriteLine(answer);

void isAccessible(int y, int x)
{
    int dY, dX, rollsAround = 0;
    for (int i = 0; i < 8; i++)
    {
        dY = y + deltaMap[i, 0];
        dX = x + deltaMap[i, 1];

        if (dY >= 0 && dY < lines.Length && dX >= 0 && dX < lines[0].Length)
            if (lines[dY][dX].Equals('@'))
                rollsAround++;
    }
    if (rollsAround < 4)
        answer++;
}