var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\10\\input.txt");

var buttonPresses = 0;

foreach (var line in lines)
{
    var segments = line.Split(' ');
    var lights = segments[0][1..^1];
    var buttonList = segments[1..^1];

    var buttons = new Button[buttonList.Length];
    for (var i = 0; i < buttons.Length; i++)
        buttons[i] = new Button(lights.Length, buttonList[i]);

    var targetState = new bool[lights.Length];
    for (var i = 0; i < lights.Length; i++)
            targetState[i] = lights[i].Equals('#');

    var fewestPresses = int.MaxValue;
    var total = 1 << buttons.Length;
    for (var i = 0; i < total; i++)
    {
        var bits = Convert.ToString(i, 2).PadLeft(buttons.Length, '0');
        var currentState = new bool[lights.Length];
        var presses = 0;
        for (var b = 0; b < buttons.Length; b++)
        {
            if (bits[b].Equals('1'))
            {
                currentState = buttons[b].Apply(currentState);
                presses++;
            }
        }

        var stateString = "";
        foreach (var s in currentState)
            stateString += s ? '#' : '.';

        if (stateString.Equals(lights) && presses < fewestPresses)
            fewestPresses = presses;
    }

    buttonPresses += fewestPresses;
}

Console.WriteLine(buttonPresses);

class Button
{
    public bool[] stateChange;

    public Button(int lights, string wiring)
    {
        stateChange = new bool[lights];
        var pushes = wiring[1..^1].Split(',').Select(int.Parse).ToArray();
        foreach (var push in pushes)
            stateChange[push] = true;
    }

    public bool[] Apply(bool[] state)
    {
        for (var i = 0; i < state.Length; i++)
            if (stateChange[i])
                state[i] = !state[i];

        return state;
    }
}