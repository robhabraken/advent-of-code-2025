# Solutions to Day 2: Gift Shop

*For the puzzle description, see [Advent of Code 2025 - Day 2](https://adventofcode.com/2025/day/2).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

To find the invalid IDs, we need to loop through all of the given ranges, splitting the input on `,` characters, and then splitting the ranges on the `-` character and parsing them into `long` values (this is important to be able to store the large sum of all invalid IDs).

Then we just loop through each given range, and check for each ID if it is a sequence of digits that is repeated twice. Or in other words, if the ID consists of two equal sequences of digits. To check this, I convert the ID into a `string` value again, and check if it has an even length (not necessary, but saves unnecessary string comparisons because strings of uneven length cannot be a repetition of two of the exact same substrings). Then, I simply compare the first half of the string to the second half, and if they're equal, I can add the value of the ID to the total of all invalid IDs. 

## Part 2

For the second part, with an undefined number of pattern occurrences, instead of simply comparing the first half to the second half of the string, I loop over the first half of the string value of each ID, incrementally increasing the substring length from 1 to half of the total length. And then I just replace that substring in the original string with an empty string. If the result then is empty, I assume that the string value of the ID only consists of sequences of this substring pattern. It's import to bail out the moment we know an ID is invalid, to avoid counting the same ID multiple times, as some IDs contain multiple repeating patterns (like `1111` contains both the repeating sequences `1` and `11`, but at `1` we already know it is invalid).

I have added a bit of a performance boost, which makes my solution roughly 10% faster: after finding a pattern, I skip a lot of numbers by incrementing the pattern. So if I find a pattern like `302` and the number is `302302`, it doesn't make sense to test anything up until `303000`, as no patterns can occur that start with `302` when we already checked for that pattern and it turned out to be one. So I increment the pattern with `1` and fill the number out with zeroes. Though, this doesn't work when the pattern exists of only one character of course, or when there are only two different numbers in the pattern (because when we find `303` as a pattern in `363363`, the next pattern match actually is `363636` which is below `364000` - so if there are only two different characters in the pattern we better treat it as being a pattern of two and thus not apply our shortcut).