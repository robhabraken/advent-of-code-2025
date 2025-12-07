# Solutions to Day 7: Laboratories

*For the puzzle description, see [Advent of Code 2025 - Day 7](https://adventofcode.com/2025/day/7).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

This was a great puzzle! It looked like a map puzzle but it actually wasn't. We need to keep track of the beams, but they may overlap, so actually each position on the x-axis is unique: the tachyons ending up in the same location are seen as one beam. This means we can keep track of the beams on the x-axis using a `HashSet<int>` collection, automatically filtering out duplicates. And the y-value doesn't need to be stored because we only go over the diagram once from top to bottom.

Next trick is that we don't need to look at those empty lines at all, as each row that contains splitters is separated by a line of emptyness, that wouldn't change anything for the beams, so let's ignore those and start at `y = 2` and increment with `y += 2`.

Then, we just loop over the x-values on the diagram and each time a location contains a splitter I check if my collection of beams also contains a beam on that exact same x-position, and if so, I remove it from the list and add two others (one on the left and one of the right), but I count this splitting event only once, as the amount of splitters reached is what we are looking for!

That's actually all we need to do, I don't draw a map or keep track of the beams in an array, I just store the number of beams horizontally for one row at a time and loop over the input just once!

## Part 2

The second part is a little more difficult. Using recursion to follow all the beams doesn't work because there's too many of them, but also search algorithms like DFS aren't really required, as everything in the same direction and we still need to go over the diagram once from top to bottom. But now, we do need to keep track of duplicates, but I don't want to store each beam individually, just count the number of overlapping beams as they represent timelines.

So to achieve this I changed my collection into a `Dictionary<int, long>` that keeps track of the number of beams on each x-position. Then I loop over my diagram again with the same `y += 2` increments. But now, instead of looping over the x-positions in the diagram, I use the list of beam locations in my collection as a reference. Then if this horiziontal line contains a splitter on the location of that beam location (could be 1 or many beams overlapping) I split this collection of beams into two, storing it into a new collection for the end result of this row. If there isn't a split, I just carry the same amount of beams over to the new collection.

When we reach the end of the diagram, the sum of all beams in all x-locations is the amount of timelines. So only one loop from top to bottom, keeping track of the amount of beams on each x-location for every row, one row at a time.