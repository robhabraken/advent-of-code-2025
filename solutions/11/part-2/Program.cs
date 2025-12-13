var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\11\\input.txt");

var nodes = new Dictionary<string, Node>();

var index = 0;
foreach (var line in lines)
    nodes.Add(line.Split(' ')[0].Trim(':'), new Node(index++, line.Split(' ')[0].Trim(':')));
nodes.Add("out", new Node(index, "out"));

foreach (var line in lines)       
    foreach (var connection in line.Split(' ')[1..])
        nodes[line.Split(' ')[0].Trim(':')].connections.Add(nodes[connection]);

nodes["out"].paths[0] = 1;
findPaths([nodes["out"]], 1);

var answer = 0L;
foreach (var opt in nodes["svr"].visitBoth)
    answer += opt;

Console.WriteLine(answer);

void findPaths(HashSet<Node> targets, int depth)
{
    var newTargets = new HashSet<Node>();
    foreach (var target in targets)
        foreach (var node in nodes.Values)
            foreach (var connection in node.connections)
                if (connection.id == target.id && !node.visitedConnections[depth - 1].Contains(target))
                {
                    node.paths[depth] += connection.paths[depth - 1];
                    node.visitDac[depth] += connection.visitDac[depth - 1];
                    node.visitFft[depth] += connection.visitFft[depth - 1];
                    node.visitBoth[depth] += connection.visitBoth[depth - 1];

                    if (node.name.Equals("dac"))
                    {
                        node.visitDac[depth] = node.paths[depth];
                        node.visitBoth[depth] = node.visitFft[depth];
                    }
                    else if (node.name.Equals("fft"))
                    {
                        node.visitFft[depth] = node.paths[depth];
                        node.visitBoth[depth] = node.visitDac[depth];
                    }

                    node.visitedConnections[depth - 1].Add(connection);
                    newTargets.Add(node);
                }
    
    if (newTargets.Count > 0)
        findPaths(newTargets, depth + 1);
}

class Node
{
    public static readonly int MAX_DEPTH = 40;

    public int id;
    public string name;

    public List<Node> connections = [];
    public List<Node>[] visitedConnections = new List<Node>[MAX_DEPTH];

    public long[] paths = new long[MAX_DEPTH];
    public long[] visitDac = new long[MAX_DEPTH];
    public long[] visitFft = new long[MAX_DEPTH];
    public long[] visitBoth = new long[MAX_DEPTH];

    public Node(int id, string name)
    {
        this.id = id;
        this.name = name;
        for (var i = 0; i < MAX_DEPTH; i++)
            visitedConnections[i] = [];
    }
}