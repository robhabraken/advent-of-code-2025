# Solutions to Day 5: Cafeteria

*For the puzzle description, see [Advent of Code 2025 - Day 5](https://adventofcode.com/2025/day/5).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

The first part was straight forward again today, looping over the ingredients and checking if they are present in any of the ranges. Without squashing or merging the ranges this already was super fast (0.78ms) so I didn't bother to optimize anything, but I knew what was coming...

## Part 2

Yep, now we do need to merge the ranges so we can count the number of fresh ingredients without counting duplicates as a lot of the given ranges are overlapping (partially). I have created a helper method first that can check if a range overlaps with another range on either side, or falling fully within or fully over the other range. Then I wrote a merge function that loops over the given ranges and checks one by one against all others. If it finds an overlapping range, it will not add the original range to the output, but a new one that spans both ranges (using the most outward boundaries) - it doesn't remove the other range that it was checking against because modifying a collection while looping over it isn't possible or adds a lot of complexity code-wise. It doens't really matter because I will be iterating over this merge function anyway to filter out overlapping ranges anyway. To be able to signal when there are no more overlapping ranges, I return a `modified` boolean, and than just loop over this function until nothing changes anymore. I do introduce a few duplicate ranges this way, but a simple `Distinct()` filters those out very fast. At 3ms it is a pretty fast solution still and I like the simplicity of the code over adding more checks to decrease the amount of loops required.