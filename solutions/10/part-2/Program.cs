using System.Numerics;

var lines = File.ReadAllLines("..\\..\\..\\..\\..\\..\\..\\advent-of-code-2025-io\\10\\input.txt");

var buttonPresses = 0;

foreach (var line in lines)
{
    var buttonList = line.Split(' ')[1..^1];
    var target = line.Split(' ')[^1][1..^1].Split(',').Select(int.Parse).ToArray();

    // create buttons
    var buttons = new Button[buttonList.Length];
    for (var i = 0; i < buttonList.Length; i++)
        buttons[i] = new Button(buttonList[i], target);

    // analyze lights (affected by buttons)
    var lights = new int[target.Length];
    for (var i = 0; i < target.Length; i++)
        for (var b = 0; b < buttons.Length; b++)
            if (buttons[b].affectsLights[i])
                lights[i] |= 1 << b; // store buttons affecting this light in an integer using bit masking

    // storage for deltas in values between sets of buttons
    var deltas = new Dictionary<(int a, int b), int>();

    // set known values, narrow ranges, solve variables, remove duplicates, replace subsets, subtract overlap
    simplify(buttons, lights, deltas, target);

    // if the configuration is already determined after simplifying, use that, otherwise continue solving the equations
    if (buttons.All(b => b.value.HasValue))
        buttonPresses += buttons.Sum(b => b.value!.Value);
    else
        buttonPresses += solve(buttons, lights, deltas, target);
}

Console.WriteLine(buttonPresses);

static bool simplify(Button[] buttons, int[] lights, Dictionary<(int a, int b), int> deltas, int[] target)
{
    // if only one button influences a light, we know the correct amount of pushes for that button right away
    for (var i = 0; i < target.Length; i++)
        if (containsOneBit(lights[i]))
        {
            var b = BitOperations.TrailingZeroCount(lights[i]);
            buttons[b].value = buttons[b].lowerBound = buttons[b].upperBound = target[i];
        }

    // if one light is only influenced by two buttons, adjust lower bounds, because this dictates minimum pushes of other button
    for (var i = 0; i < lights.Length; i++)
        if (containsTwoBits(lights[i]))
        {
            var mask = lights[i];
            var b0 = BitOperations.TrailingZeroCount(mask);

            mask &= mask - 1; // clear the lowest set bit
            var b1 = BitOperations.TrailingZeroCount(mask);

            if (buttons[b0].upperBound < target[i])
                buttons[b1].lowerBound = Math.Max(buttons[b1].lowerBound, target[i] - buttons[b0].upperBound);

            if (buttons[b1].upperBound < target[i])
                buttons[b0].lowerBound = Math.Max(buttons[b0].lowerBound, target[i] - buttons[b1].upperBound);
        }

    // adjust upper bounds with the cumulative of lower bounds of other buttons for the same light (new lower bounds limits range of other buttons)
    for (var i = 0; i < lights.Length; i++)
    {
        var cumulativeLowerBounds = 0;

        var mask = lights[i];
        while (mask != 0)
        {
            var b = BitOperations.TrailingZeroCount(mask);
            cumulativeLowerBounds += buttons[b].lowerBound;
            mask &= mask - 1; // clear the lowest set bit
        }

        mask = lights[i];
        while (mask != 0)
        {
            var b = BitOperations.TrailingZeroCount(mask);
            buttons[b].upperBound = Math.Min(buttons[b].upperBound, target[i] - (cumulativeLowerBounds - buttons[b].lowerBound));
            mask &= mask - 1; // clear the lowest set bit
        }
    }

    // if the bounds closed in on one value, set that value, for the number of pushes this button requires is now known
    for (var i = 0; i < buttons.Length; i++)
        if (buttons[i].lowerBound == buttons[i].upperBound)
            buttons[i].value = buttons[i].lowerBound;

    // remove duplicates (multiple occurrences of the same list of buttons with the same target do not influence the outcome)
    for (var i = 0; i < lights.Length; i++)
        for (var j = i + 1; j < lights.Length; j++)
            if (target[i] == target[j] && lights[i] == lights[j])
            {
                lights[j] = 0;
                target[j] = 0;
            }

    // remove all buttons and target values from equation if they are known to simplify equations
    var changed = false;
    for (var b = 0; b < buttons.Length; b++)
        if (buttons[b].value.HasValue)
        {
            // remove button and subtract target value from other lights
            var bit = 1 << b;

            for (var i = 0; i < lights.Length; i++)
                if ((lights[i] & bit) != 0)
                {
                    lights[i] &= ~bit; // remove button
                    target[i] -= buttons[b].value!.Value;
                    changed = true;
                }
        }

    // find subsets and remove them to further simplify the equations and ranges
    for (var i = 0; i < lights.Length; i++)
        for (var j = i + 1; j < lights.Length; j++)
        {
            // check if lists aren't empty (after removing duplicates)
            if (lights[i] == 0 || lights[j] == 0)
                continue;

            var same = lights[i] & lights[j];
            var diffLeft = lights[i] & ~lights[j];
            var diffRight = lights[j] & ~lights[i];

            if (BitOperations.PopCount((uint)lights[i]) != BitOperations.PopCount((uint)lights[j]) && diffLeft != 0 && diffRight == 0)
            {
                if (target[j] > target[i]) return false; // would result in negative value, not possible!

                lights[i] = diffLeft;
                target[i] -= target[j];
                changed = true;
            }
            else if (BitOperations.PopCount((uint)lights[i]) != BitOperations.PopCount((uint)lights[j]) && diffLeft == 0 && diffRight != 0)
            {
                if (target[i] > target[j]) return false; // would result in negative value, not possible!

                lights[j] = diffRight;
                target[j] -= target[i];
                changed = true;
            }

            // extract deltas between buttons from overlap between lists (a+b+f=20 and a+b+g=15 means f-g=5)
            else if (BitOperations.PopCount((uint)same) > 1 && containsOneBit(diffLeft) && containsOneBit(diffRight))
            {
                var a = BitOperations.TrailingZeroCount(diffLeft);
                var b = BitOperations.TrailingZeroCount(diffRight);

                changed |= deltas.TryAdd((a, b), target[i] - target[j]);
                changed |= deltas.TryAdd((b, a), target[j] - target[i]);
            }
        }

    // apply deltas to further narrow bounds
    foreach (var ((a, b), delta) in deltas)
    {
        var newLowerBound = Math.Max(0, buttons[b].lowerBound + delta);
        var newUpperBound = buttons[b].upperBound + delta;

        if (newLowerBound > buttons[a].lowerBound)
        {
            buttons[a].lowerBound = newLowerBound;
            changed = true;
        }

        if (newUpperBound < buttons[a].upperBound)
        {
            buttons[a].upperBound = newUpperBound;
            changed = true;
        }

        if (buttons[a].lowerBound == buttons[a].upperBound && !buttons[a].value.HasValue)
        {
            buttons[a].value = buttons[a].lowerBound;
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

static int solve(Button[] buttons, int[] lights, Dictionary<(int a, int b), int> deltas, int[] target)
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
        return isValid(buttons, lights, target) ? presses : 0;

    // find optimal start (button with the smallest range)
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

            var lightsClone = (int[])lights.Clone();
            var deltasClone = new Dictionary<(int a, int b), int>(deltas);
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

    return fewestPresses;
}

static bool containsOneBit(int light) => light != 0 && (light & (light - 1)) == 0;

static bool containsTwoBits(int light) => containsOneBit(light & (light - 1));

static bool isValid(Button[] buttons, int[] lights, int[] target)
{
    for (var i = 0; i < lights.Length; i++)
    {
        var sum = 0;
        var mask = lights[i];
        while (mask != 0)
        {
            var b = BitOperations.TrailingZeroCount(mask);
            sum += buttons[b].value!.Value;
            mask &= mask - 1; // clear the lowest set bit
        }

        if (sum != target[i]) return false;
    }
    return true;
}

class Button
{
    public string input;
    public bool[] affectsLights;
    public int lowerBound;
    public int upperBound;
    public int? value;

    private readonly Stack<(int lowerBound, int upperBound, int? value)> stack = [];

    public Button(string input, int[] target)
    {
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
}