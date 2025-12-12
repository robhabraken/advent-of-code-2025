# Solutions to Day 12: Christmas Tree Farm

*For the puzzle description, see [Advent of Code 2025 - Day 12](https://adventofcode.com/2025/day/12).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

What isn't the case for the example input actually is for the real input: the total area of all presents collectively is either in the 63 to 73% range of a region, or above 100%. Logically, if the sum of the area of all presents is higher than the total available area in a region, it doesn't fit. In all other cases, given the low coverage / percentage, it always seems to fit - we don't even have to actually fit the presents into the region. A little unsatisfactory, but I only check if `presentArea < regionArea`, that's all the logic I used to find the answer.