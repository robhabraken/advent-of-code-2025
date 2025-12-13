var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\11\\input.txt");

var devices = new Dictionary<string, string[]>();
foreach (var line in lines)
    devices.Add(line.Split(' ')[0].Trim(':'), line.Split(' ')[1..]);

var paths = 0;
findPaths("you");

Console.WriteLine(paths);

void findPaths(string device)
{
    foreach (var connection in devices[device])
        if (connection.Equals("out"))
            paths++;
        else
            findPaths(connection);
}