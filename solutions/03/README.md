# Solutions to Day 3: Lobby

*For the puzzle description, see [Advent of Code 2025 - Day 3](https://adventofcode.com/2025/day/3).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

Finding the highest number that exists of two digits while not changing the order of the given digits in the bank simply means first finding the highest digit in all values except the last one, because we need two digits and thus we cannot use the last digit within a bank for the first digit of our joltage rating. For the second digit, we start from the position after the highest value and then find the highest value. So if we have a bank of 15 digits, we first find the highest value in the range `1..14` and if that for example turns out to be at position `3`, then we find the highest value for our second digit within the range `4..15`.

## Part 2

For the second part, we need to move our code from part one into a generic function so we can loop over the digits, but the algorithm stays the same. To be as efficient as possible, I do not use string concatenation, and I also add all of the digits right away, instead of first calculating the joltate rating per bank and then adding those again. We can pretty much do this whole algorithm in one go only touching each highest digit once.