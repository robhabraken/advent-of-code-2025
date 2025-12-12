var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\11\\input.txt");

var paths = 0;

var devices = new Dictionary<string, string[]>();
foreach (var line in lines)
{
    var segments = line.Split(' ');
    devices.Add(segments[0].Trim(':'), segments[1..]);
}

follow("you");

Console.WriteLine(paths);

void follow(string device)
{
    foreach (var connection in devices[device])
    {
        if (connection.Equals("out"))
            paths++;
        else
            follow(connection);
    }
}