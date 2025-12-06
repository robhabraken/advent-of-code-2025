# Solutions to Day 4: Printing Department

*For the puzzle description, see [Advent of Code 2025 - Day 4](https://adventofcode.com/2025/day/4).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

It's time to pull out my `deltaMap` again! A trick I came up with previously and one that came in handy for a lot of map puzzles. It consists of an array of delta's of `x,y`-positions that represent a relative position on a map, which I can more easily iterate over to move around on a map with the need of using multiple nested loops or using a lot of if-statements.

I then wrote an `isAccessible()` method that checks all of the surrounding positions of a location on the map, counting the number of rolls and if that number is less than `4` I increase the total number of accessible rolls. Now I only need to loop over the input and check each location on the map, calling that method.

## Part 2

For part two, I needed to copy the input to a `bool[,]` to allow for mutations to be able to remove rolls. I then moved my logic from part one into a separate method named `removeRolls()`, looping over the map to find the accessible rolls by calling `isAccessible()` for every roll on the map and then removing it right away once I'm able to. I then simply loop over this method until I no longer detect any changes, as I need multiple iterations to remove rolls as the maps keeps changing constantly and rolls that weren't accessible first can eventually become reachable by the forklifts.