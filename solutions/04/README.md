# Solutions to Day 4: Printing Department

*For the puzzle description, see [Advent of Code 2025 - Day 4](https://adventofcode.com/2025/day/4).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

A bit of a warming up exercise for the upcoming map challenges I guess. The first part of today just involved looping over the map once, checking and counting all neighbors.

## Part 2

For part 2, I moved the code into a generic function that I call until it returns `false`, indicating there are no more rolls to remove. So it basically loops over part 1 while updating the map (using a copy of the original input as a `bool[,]` to be able to easily remove rolls) until the answer of the part 1 algorithm is `0`. There could maybe be a smarter and more efficient way to do it, but this is reasonably fast (about 28 ms). Maybe I look into this when I have more time.