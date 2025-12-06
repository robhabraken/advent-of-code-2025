# Solutions to Day 3: Lobby

*For the puzzle description, see [Advent of Code 2025 - Day 3](https://adventofcode.com/2025/day/3).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

Finding the highest number that consists of two digits while not changing the order of the given digits in the bank simply means first finding the highest digit in all values except the last one, because we need two digits and thus we cannot use the last digit within a bank for the first digit of our joltage rating. For the second digit, we start from the position after the highest value and then find the highest value. So if we have a bank of 15 digits, we first find the highest value in the range `1..14` and if that for example turns out to be at position `3`, then we find the highest value for our second digit within the range `4..15`.

To achieve this, we loop through all of the characters of the bank but the last (`Length - 1`), convert them to an integer value, and store both the digit and its position if it is higher than our reference value (starting at `0` to be sure to pick up any value as soon as we start looping). Instead of using `int.Parse` we can use a trick that is even a little quicker: `bank[i] - '0'` gives the integer value too, as the characters `0` to `9` are represented by their ASCII values `48` to `57`. So if the character at `bank[i]` is `3` for example, it's integer value actually is `51` and subtracting the value of `0` gives `51 - 48 = 3`.

Then for the second digit, we do the same, but we start at `position + 1` and we don't need to store the position of that second digit anymore because it isn't relevant. Lastly, multiplying the first digit by `10` and adding the second digit gives us the integer value of the total joltage rating of that row of batteries, and we can then add that to the total output joltage rating.

## Part 2

For the second part, I kept the first part to find the first digit, but now added a loop around it counting down from the first to the last digit backwards, using the multiple of 10 as my index. So the first digit to find is the twelfth digit from the right, but because we're zero-based that is digit `11`. The result of the digit can be added to the outputJoltage by multiplying it with `10^11`, and we can use all the digits from the bank except the last `11`. So that's quite a handy trick counting down from `11` to `0` to find all the digits of the joltage rating of the current bank. Then I just increase my starting index with the current position for each iteration and I'm done.

To save some time, I don't bother calculating the joltage rating of a single bank first, but I just add each digit with its own multiple to the total output joltage right away.