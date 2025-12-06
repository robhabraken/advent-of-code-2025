# Solutions to Day 1: Secret Entrance

*For the puzzle description, see [Advent of Code 2025 - Day 1](https://adventofcode.com/2025/day/1).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

We need to variables, keeping track of the position of the `dial` and counting the zeroes resulting in the `password`. I loop through the instructions (one per line) and only read the number (position `1` and beyond), and multiply the given rotation value by `-1` in case the line starts with an `L` (ignoring the `R`, which isn't relevant). Then I increment the value of the dial with the rotation from the instruction. At first, I then did some checks and corrections to keep the value of the dial in the `0..99` range but I thought that wasn't necessary at all, because instead of checking for a value of `0`, we can also check whether the modulus of `100` of the dial is equal to `0`, saving even more logic and code.

## Part 2

For the second part, I brought back the check and corrections to keep the dial within the range of `0..99` and I also took the modulus of `100` of the given rotation before turning the dail to simplify this check. Because we need to count the number of `0` that occur within each rotation, it's easier to stay within the desired range to avoid edge cases going into negative values and back up into positive values of the dial otherwise.

If the rotation value is higher than 100 we count the times we pass the `0` within that rotation (before applying the modulus), which is as easy as `rotation / 100`, and then for the outcome we correct the value of the dial to fall within the `0..99` range again and count passing the `0` value too while doing so, except when we started at `0` going counterclockwise (because we already counted ending on the `0` at the end of the previous turn) which I check by comparing the value of the dial after the turn with the value of the current rotation, or when ending at `0` going clockwise (because we will count that at the end of the turn). And then we count every time we end on `0` after completing the rotation of that single instruction.