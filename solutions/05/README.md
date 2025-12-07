# Solutions to Day 5: Cafeteria

*For the puzzle description, see [Advent of Code 2025 - Day 5](https://adventofcode.com/2025/day/5).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

The first part was straight forward again today, looping over the ingredients and checking if they are present in any of the ranges. Without squashing or merging the ranges this already was super fast (0.78ms) so I didn't bother to optimize anything, but I knew what was coming...

The input consists of two different part: first a list of ranges, then a list of ingredients, separated by a whiteline. We want to loop over the input just once, so we need to know where we are within the input, that's why I used a boolean `readingRanges` I can flip once I encounter the whiteline. Until then, I add each range to my `List<long[]>` collection, a list of `long` arrays containing the boundaries of all ranges. I can do that in one line by splitting the input range at the `-` character, selecting the result and applying the `long.Parse` function to all elements and then converting the output back to an array again.

Once we're past reading the ranges, I start processing the ingredients. For each ingredient, which I parse to a `long` value too, I check if it is in any of the ranges. And once I know it's fresh, I bail out right away to save myself from doing unneccessary checks.

## Part 2

For part two, I don't need the second part of the input anymore, so I just break out of loop reading the input once I hit the whiteline. I also changed the `long[]` array to a `(long from, long to)` Tuple for readability to store my ranges.

The fastest way (and the only way frankly) to count the number of fresh ingredients is to add the sum of each range to the total, but a lot of ranges in the input are actually overlapping. So we need to squash them, or merge if you like. There are four ways a range can overlap another: on the left side, on the right side, falling fully inbetween another range, or enclosing the other range fully. I created a helper method `isOverlapping()` to check those conditions.

Then, I wrote a `merge()` function that loops over all ranges and then for each range again loops over all (other) ranges. I don't want to compare myself to myself, so I first change `!range.Equals(compare)` and then check if the ranges are overlapping. If they overlap, I created a new range with the outer boundaries of both ranges (using `Math.Min` and `Math.Max`) and add that to a new list. If the current range isn't overlapping with any other range, I add the range itself to the new list too. Mind that I didn't remove the other range that this range overlaps with (if so), because I cannot mutate a collections I'm looping over without a lot of extra checks and code, and I don't need to because it will get filtered out eventually anyway. My code runs in `3ms` so there isn't any real performance penalty, and removing the other range I'm comparing with also takes time.

So now I have a new list and I replace the orignal one with that. And I loop over this `merge()` function until I don't see any modifications in my list anymore, knowing I have merged all overlapping ranges. This algorithm goes introduce a few duplicate ranges, but a simple `Distinct()` filters those out very fast and effectively. Lastly I loop over the resulting list of distinct merged ranges, and add the amount of fresh ingredients per range to the grand total (don't forget the `+ 1` because we need to include both the boundaries of the range as well (`3-5` refers to a list of `3` ingredients, not `2`)).