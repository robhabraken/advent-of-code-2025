using System.Diagnostics;

var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\10\\input.txt");

var buttonPresses = 0;

var sw = new Stopwatch();
sw.Start();

foreach (var line in lines)
{
    var buttonList = line.Split(' ')[1..^1];
    var target = line.Split(' ')[^1][1..^1].Split(',').Select(int.Parse).ToArray();

    // create buttons
    var buttons = new Button[buttonList.Length];
    for (var i = 0; i < buttonList.Length; i++)
        buttons[i] = new Button(buttonList[i], (char)(i + 97), target);

    // analyze lights (affected by buttons)
    var lights = new List<Button>[target.Length];
    for (var i = 0; i < target.Length; i++)
    {
        lights[i] = [];
        foreach (var button in buttons)
            if (button.affectsLights[i])
                lights[i].Add(button);
    }

    var deltas = new Dictionary<(Button a, Button b), int>();

    // set known values, narrow ranges, solve variables, remove duplicates, replace subsets, subtract overlap
    simplify(buttons, lights, deltas, target);

    // if the configuration is already determined after simplifying, use that, otherwise continue solving the equations
    if (buttons.All(b => b.value.HasValue))
        buttonPresses += buttons.Sum(b => b.value!.Value);
    else
        buttonPresses += solve(buttons, lights, deltas, target);
}

sw.Stop();
Console.WriteLine(sw.ElapsedMilliseconds + "ms");
Console.WriteLine("Answer: " + buttonPresses);

static bool simplify(Button[] buttons, List<Button>[] lights, Dictionary<(Button a, Button b), int> deltas, int[] target)
{
    // if only one button influences a light, we know the correct amount of pushes for that button right away
    for (var i = 0; i < target.Length; i++)
        if (lights[i].Count == 1)
            lights[i][0].value = lights[i][0].lowerBound = lights[i][0].upperBound = target[i];

    // if one light is only influenced by two buttons, adjust lower bounds, because this dictates minimum pushes of other button
    for (var i = 0; i < lights.Length; i++)
    {
        if (lights[i].Count == 2)
        {
            if (lights[i][0].upperBound < target[i])
                lights[i][1].lowerBound = Math.Max(lights[i][1].lowerBound, target[i] - lights[i][0].upperBound);

            if (lights[i][1].upperBound < target[i])
                lights[i][0].lowerBound = Math.Max(lights[i][0].lowerBound, target[i] - lights[i][1].upperBound);
        }
    }

    // adjust upper bounds with the cumulative of lower bounds of other buttons for the same light (new lower bounds limits range of other buttons)
    for (var i = 0; i < lights.Length; i++)
    {
        var cumulativeLowerBounds = 0;
        foreach (var button in lights[i])
            cumulativeLowerBounds += button.lowerBound;

        foreach (var button in lights[i])
            button.upperBound = Math.Min(button.upperBound, target[i] - (cumulativeLowerBounds - button.lowerBound));
    }

    // if the bounds closed in on one value set that value, for the number of pushes this button requires is now known
    for (var i = 0; i < buttons.Length; i++)
    {
        if (buttons[i].lowerBound == buttons[i].upperBound)
            buttons[i].value = buttons[i].lowerBound;
    }

    // remove duplicates (multiple occurrences of the same list of buttons with the same target do not influence the outcome)
    for (var i = 0; i < lights.Length; i++)
    {
        for (var j = i + 1; j < lights.Length; j++)
        {
            if (target[i] == target[j] && lights[i].Count == lights[j].Count)
            {
                var same = true;
                for (var k = 0; k < lights[i].Count; k++)
                    if (!lights[i][k].Equals(lights[j][k]))
                        same = false;

                if (same)
                {
                    lights[j].Clear();
                    target[j] = 0;
                }
            }
        }
    }

    // remove all buttons and target values from equation if they are known to simplify equations
    var changed = false;
    for (var i = 0; i < buttons.Length; i++)
    {
        if (buttons[i].value.HasValue)
        {
            // remove button and subtract target value from other lights
            for (var j = 0; j < lights.Length; j++)
            {
                if (lights[j].Contains(buttons[i]))
                {
                    lights[j].Remove(buttons[i]);
                    target[j] -= buttons[i].value!.Value;

                    changed = true;
                }
            }
        }
    }

    // find subsets and remove them to further simplify the equations and ranges
    for (var i = 0; i < lights.Length; i++)
    {
        for (var j = i + 1; j < lights.Length; j++)
        {
            // if the button lists are of equal lengths, subsets won't help solving, and also, check if lists aren't empty (after removing duplicates)
            if (lights[i].Count == 0 || lights[j].Count == 0)
                continue;

            List<Button> diffLeft = [], diffRight = [], same = [];
            foreach (var button in lights[i])
                if (!lights[j].Contains(button))
                    diffLeft.Add(button);
                else
                    same.Add(button);

            foreach (var button in lights[j])
                if (!lights[i].Contains(button))
                    diffRight.Add(button);

            if (lights[i].Count != lights[j].Count && diffLeft.Count > 0 && diffRight.Count == 0)
            {
                if (target[j] > target[i]) return false; // would result in negative value, not possible!

                lights[i] = [];
                lights[i].AddRange(diffLeft);
                target[i] -= target[j];

                changed = true;
            }
            else if (lights[i].Count != lights[j].Count && diffLeft.Count == 0 && diffRight.Count > 0)
            {
                if (target[i] > target[j]) return false; // would result in negative value, not possible!

                lights[j] = [];
                lights[j].AddRange(diffRight);
                target[j] -= target[i];

                changed = true;
            }

            // extract deltas between buttons from overlap between lists (a+b+f=20 and a+b+g=15 means f-g=5)
            else if (same.Count > 1 && diffLeft.Count == 1 && diffRight.Count == 1)
            {
                changed |= deltas.TryAdd((diffLeft[0], diffRight[0]), target[i] - target[j]);
                changed |= deltas.TryAdd((diffRight[0], diffLeft[0]), target[j] - target[i]);
            }
        }
    }

    // apply deltas to further narrow bounds
    foreach (var ((a, b), delta) in deltas)
    {
        var newLowerBound = Math.Max(0, b.lowerBound + delta);
        var newUpperBound = b.upperBound + delta;

        if (newLowerBound > a.lowerBound)
        {
            a.lowerBound = newLowerBound;
            changed = true;
        }

        if (newUpperBound < a.upperBound)
        {
            a.upperBound = newUpperBound;
            changed = true;
        }

        if (a.lowerBound == a.upperBound && !a.value.HasValue)
        {
            a.value = a.lowerBound;
            changed = true;
        }
    }

    foreach (var button in buttons)
        if (button.lowerBound > button.upperBound)
            return false; // this branch isn't possible

    // repeat process until range and equations are stable
    if (changed)
        return simplify(buttons, lights, deltas, target);

    return true;
}

static int solve(Button[] buttons, List<Button>[] lights, Dictionary<(Button a, Button b), int> deltas, int[] target)
{
    // if all buttons have a value, we have either found a valid solution, or an impossible configuration, in which case we return 0
    var presses = 0;
    var complete = true;
    foreach(var button in buttons)
        if (button.value.HasValue)
            presses += button.value!.Value;
        else
        {
            complete = false;
            break;
        }

    if (complete)
        return isValid(lights, target) ? presses : 0;

    // find optimal start
    Button start = null;
    var smallestRange = int.MaxValue;
    for (var i = 0; i < buttons.Length; i++)
    {
        if (buttons[i].value.HasValue)
            continue;

        var range = buttons[i].upperBound - buttons[i].lowerBound;
        if (range < smallestRange)
        {
            start = buttons[i];
            smallestRange = range;
        }
    }

    var fewestPresses = int.MaxValue;

    for (var i = start.lowerBound; i <= start.upperBound; i++)
    {
        try
        {
            foreach (var button in buttons)
                button.Save();

            start.value = start.lowerBound = start.upperBound = i;

            var lightsClone = new List<Button>[lights.Length];
            for (var j = 0; j < lights.Length; j++)
                lightsClone[j] = new List<Button>(lights[j]);
            var deltasClone = new Dictionary<(Button a, Button b), int>(deltas);
            var targetClone = (int[])target.Clone();

            // continue if the tested value isn't possible for the equations of this configuration
            if (!simplify(buttons, lightsClone, deltasClone, targetClone))
                continue;

            var testNextButton = solve(buttons, lightsClone, deltasClone, targetClone);
            if (testNextButton > 0 && testNextButton < fewestPresses)
                fewestPresses = testNextButton;
        }
        finally
        {
            foreach (var button in buttons)
                button.Reset();
        }
    }

    if (fewestPresses == int.MaxValue)
        fewestPresses = 0;

    return fewestPresses;
}

static bool isValid(List<Button>[] lights, int[] target)
{
    for (var i = 0; i < lights.Length; i++)
    {
        var sum = 0;
        for (var b = 0; b < lights[i].Count; b++)
            sum += lights[i][b].value!.Value;

        if (sum != target[i])
            return false;
    }
    return true;
}

class Button
{
    public char name;
    public string input;
    public bool[] affectsLights;
    public int lowerBound;
    public int upperBound;
    public int? value;

    private readonly Stack<(int lowerBound, int upperBound, int? value)> stack = [];

    public Button(string input, char name, int[] target)
    {
        this.name = name;
        this.input = input;

        lowerBound = 0;
        upperBound = int.MaxValue;

        affectsLights = new bool[target.Length];
        var indexes = input[1..^1].Split(',').Select(int.Parse).ToArray();
        foreach (var index in indexes)
        {
            affectsLights[index] = true;
            upperBound = Math.Min(upperBound, target[index]);
        }
    }

    public void Save()
    {
        stack.Push((lowerBound, upperBound, value));
    }

    public void Reset()
    {
        var memory = stack.Pop();
        lowerBound = memory.lowerBound;
        upperBound = memory.upperBound;
        value = memory.value;
    }

    public override string ToString()
    {
        var stringValue = value.HasValue ? $"value: {value.Value}" : string.Empty;
        return $"{name}{input} [{lowerBound} - {upperBound}] {stringValue}";
    }
}