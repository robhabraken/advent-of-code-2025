# Solutions to Day 8: Playground

*For the puzzle description, see [Advent of Code 2025 - Day 8](https://adventofcode.com/2025/day/8).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

_I've committed my 'draft' solution, which I've used to submit my answer, but I may look into finding a better or faster way to solve today's puzzle._

First, I parse all boxes storing them as a `Junction` object in a dictionary, giving them an `id` for easy reference. This class also contains a `Distance()` method to calculate the distance between a box and another box. Then I loop over all boxes, calculate their distance to each other box and store those pairs in a `Dictionary<string, double>`. This initially was a quick solution to filter out duplicates using `TryAdd()` and to store the pairs with a combined ID using the `$"{id1}-{id2}"`-format, but I kept it for now, and it even turned out to be marginally faster than checking for a duplicate before calculating each pair (so now it loops over each pair from either direction).

Then I sort the pair dictionary `distances` on the distances (their value) and take the first 1000 items. I split the string identifier and start looping over all existing circuits to see in how many circuits any of the two IDs occur. If none of the existing circuits contains either of the IDs, I need to create a new circuit with these two boxes. If only one of the IDs is present in one of the circuits, I can safely add both IDs (it's a `HashSet<int>` so duplicates are taken care of anyway) to extend the existing circuit. If there are two circuits that contain either one of the IDs of this pair of boxes, I have found a connection between two circuits and I need to merge them. I do that by looping over the junction boxes of one of the circuits, adding them to the other, and then removing the first one.

Now, I have my circuits. I still need to count their sizes, sort that list descending and take the first 3 items, and then multiply them with each other to find my answer.

## Part 2

For the second part, finding the last pair that creates one big circuit, I added a `boxesLeft` collection (a `List<int>`) to store all boxes initially and them remove them from the list once I've used them. Then, at the end of the loop that connects the pairs, I check the number of circuits, and the number of boxes left. If those are respectively `1` and `0` I know I now have one circuit _and_ used all boxes. I then multiply the x-coordinates of both boxes of that current pair and have my answer. The code that counts the circuit sizes can be removed.