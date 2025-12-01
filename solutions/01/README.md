# Solutions to Day 1: Secret Entrance

*For the puzzle description, see [Advent of Code 2025 - Day 1](https://adventofcode.com/2025/day/1).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

This almost was a first time right, if I would've looked at the actual puzzle input before running my solution... because of course the example only uses moves of less than 100 rotations, while the real puzzle input also uses rotations that go well over a 100. So we need to take the modulus of the number of rotations, to stay within the right bandwidth for my -100 and +100 corrections to stay within the dial range of `0..99`. Other than that, parsing the input and looping over the rotations is most of the work. Then we only need to count every occassion we land on `0` after execution that rotation.. 

_After sending in my answer I slightly optimized my solution by moving the modules and the bound checks (to keep my dial in the range of `0..99`) to just counting all dial positions that were a multiple of 100 either way, which is just a tad cleaner and faster._

## Part 2

This was a bit more complex than I initally envisioned, because you also need to take into account the edge cases of leaving from `0` and ending on `0` to be careful not to count those events twice.

To start, I added `rotations / 100` to the total, because that rounds down to the exact number of times the dial will pass `0`. Then again I only rotate the amount that's left after division by using `rotations %= 100` again. Then, I've added increasing the total by one each time I need to correct my outcome (below 0, or 100 or higher). The trick here though is that you don't want to count if leaving from `0` because you already did count that when landing on it after the previous move. Also, when landing on `100` exactly, we also shouldn't count this to avoid counting this event twice. And eventually, we can always safely again increase our total when we landed on `0` at the end of this move.