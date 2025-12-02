# Solutions to Day 2: Gift Shop

*For the puzzle description, see [Advent of Code 2025 - Day 2](https://adventofcode.com/2025/day/2).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

More parsing than logic today. It took a while for me to see the simplicity of part 1, because the pattern may only occur twice. So in other words, the string value of each ID must be even, and the first half of the string value must equal the second half. And in that case, add the numeric value to the puzzle answer.

## Part 2

For the second part, with an undefined number of pattern occurrences, I loop over the first half of the string value of each ID, increasing the substring length from 1 to half of the total length. And then I just replace that substring in the original string with an empty string. If the result then is empty, I assume that the string value of the ID only consists of sequences of this substring pattern. It's import to bail out the moment we know an ID is invalid, to avoid counting the same ID multiple times, as some IDs contain multiple repeating patterns (like `1111` contains both the repeating sequences `1` and `11`).