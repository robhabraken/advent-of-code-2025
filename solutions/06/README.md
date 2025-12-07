# Solutions to Day 6: Trash Compactor

*For the puzzle description, see [Advent of Code 2025 - Day 6](https://adventofcode.com/2025/day/6).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

This is my kind of fun, just a bit of a clever parsing challenge to be honest. Where first I thought we would need to find mutual spaces between the lines with the numbers to find the columns, I quickly noticed that actually the operators on the last line give away the index of each column, so I loop over all characters of the last line first, and then store each operator with their respective column index (called `position`) in a `List<(int position, char op)>`. To prevent the need of handling the last problem at the end of the input differently, I just added a faux problem position to the end of the list with an index that would make my actual last problem work exactly the same as all others. This is way more elegant to me than needing if-statements to deal with the exception of the last problem in the collection.

Now I can just loop over all problems. I know the operator of each set and I know its position. The result of each problem `problemResult` starts of with a `1L` for multiplications (operator `*`) and with a `0L` for additions (operator `+`). Then I loop through the input lines from top to bottom, except the last line with the operator because I already have that information, and then simply take the substring at the position of the current operator to the next one in my problem collection (the tuple list) like so: `lines[i][problems[p].position..problems[p + 1].position]`. Using `Trim()` takes care of the unwanted whitespace I collect this way, and then I parse the result to an `int` and simply perform the math that fits the current operator.

This way, I only loop over each character of the input once and do the math right away, which should be the fastest solution possible and gives me an execution time of only `0.33ms`.

## Part 2

I expected really really big numbers that would make life difficult mathematically, but luckily part two is just a variation of the parsing challenge. And my method would work equally well for this slight re-orientation. So the first part stays exactly the same, until I start looping over the problems, because their meaning and position didn't change. I only need to change my loops: instead of looping over the lines and taking a substring of each line at the given position of the operator, I now loop over the positions between the current operator and the next one, and then take the individual characters from all lines top to bottom. Which is basically rotating the loop - or reading direction if you like - by 90 degrees. The only extra thing I have to do now is concatenate the separate digits as characters to a string and then parse that to an integer value. The rest again, stays the same.