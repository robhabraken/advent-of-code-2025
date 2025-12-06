var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\06\\input.txt");

var problems = new List<(int position, char op)>();

var grandTotal = 0L;
for (var i = 0; i < lines[^1].Length; i++)
    if (lines[^1][i] != ' ')
        problems.Add((i, lines[^1][i]));
problems.Add((lines[0].Length + 1, 'x'));

for (var p = 0; p < problems.Count - 1; p++)
{
    var problemResult = problems[p].op.Equals('*') ? 1L : 0L;
    for (var pos = problems[p].position; pos < problems[p + 1].position - 1; pos++)
    {
        var numberString = string.Empty;
        for (var i = 0; i < lines.Length - 1; i++)
            numberString += lines[i][pos];

        if (problems[p].op.Equals('*'))
            problemResult *= int.Parse(numberString.Trim());
        else
            problemResult += int.Parse(numberString.Trim());
    }
    grandTotal += problemResult;
}

Console.WriteLine(grandTotal);