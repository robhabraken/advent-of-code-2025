# Solutions to Day 10: ...

*For the puzzle description, see [Advent of Code 2025 - Day 10](https://adventofcode.com/2025/day/10).*

Here are my solutions to the puzzles of today. Written chronologically so you can follow both my code and line of thought.

## Part 1

...

## Part 2

This was a proper brain teaser! After writing an optimized brute force (with self-correcting ranges of possibilities) which took over 8 minutes to run, I started from scratch again to find the 'proper' mathematical approach.

### Finding a suitable data format and approach
Let's start with the first line of the example input:
```
[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
```
The way this line is built up makes it difficult to see and makes you think there are a lot of possibilities, but we can rewrite this into a much better way that makes it easier to see what the actual structure of the puzzle is, and also will be a good start for our solver:

- The first button (3) only influences the last light, so if we push that it leads to `...#`, ot in other words `0001`
- The second button (1,3) influences both the second and the last light, so that leads to triggering the following lights: `0101`
- Et cetera...

If we list those in a table, and give our buttons names (starting with `a`), this would result in:
```
(3)   -> a(0001)
(1,3) -> b(0101)
(2)   -> c(0010)
(2,3) -> d(0011)
(0,2) -> e(1010)
(0,1) -> f(1100)
```
Now, if you would look vertically, you can easily see which buttons influence which light:

```
  vvvv
a(0001)
b(0101)
c(0010)
d(0011)
e(1010)
f(1100)
  3547 -> our target configuration
```
So, only button `e` and `f` can be used to set the first light to `3`. Which means that `e + f = 3`. The amount of presses on `e` and `f` combined should lead to the target configuration of `3`. If we do that for al lights we get:
```
e + f = 3
b + f = 5
c + d + e = 4
a + b + d = 7
```
And this immediately gives us a lot of information! The number of presses on a single button can never exceed its target configuration. So `e` can never be pressed more than `3` times, the same goes for `f`. And `b` can never be pressed more than `5` times, et cetera. But these equations do not stand on their own, they are a set that influence each other! So `f` can never be more than `3` based on our first equation, which means that for the next equation to be possible, `b` should at least be `2` to get to the sum of `5`. And because `a` and `b` are used in the last equation, while `a` intially would be limited to `7`, the fact that `b` should at least be pressed twice, `a` can actaully not be higher than `5` to not exceed our target configuration of `7`. So, when looking at these equations, we can first determine the maximum number of presses based on their own equation, creating a range (between the lower and the upper bound) of possible values, and then refine those ranges based on the information we get from other equations:
```
a[0-7]           -> a[0-5]
b[0-5] -> b[2-5]
c[0-4]
d[0-4]
e[0-3]
f[0-3]
```
Now, what we are looking for, is the least amount of presses required to make all of these equations valid. So we need to minimize:
```
S = a + b + c + d + e + f
```
And we can also start to solve some of the equations:
```
a + b + d = 7 -> a = 7 - (b + d) -> a = 7 - b - d
e + f = 3 -> e = 3 - f
```
So, the equation for `S` becomes:
```
S = 7 - b - d + b + c + d + 3 - f + f
```
Which we can simply to (because `b`, `d` and `f` equal each other out):
```
S = 7 + c + 3 -> S = 10 + c
```
So we now know that the least amount of pushes required is between `10` and `14` (because the range of `c` is `[0-4]`). And that is pretty much how far we can go mathematically, so we still have to test a few values, but our range is super small compared to trying all possible values, and we can also calculate them very quickly. _Let's start with testing the smallest range possible that is used in most equations_ (because this is the least expensive and has the most impact - which is a crucial part of my approach in code later on):
```
e = 0 means f = 3 (because f = 3 - e)
if f = 3, than b = 2 (beacuse b = 5 - f)
now we can say that c + d = 4 (as e = 0)
and also that a + b + d = 7 will become a + d = 5 (we can subtract b = 2 from both sides of the equation)
``` 
So with filling in only _one_ variable, having all of our equations and ranges, we can actually calculate other variables as well, but also further decrease our ranges:
```
a[1-5] (because d cannot exceed 4 anymore)
b = 2
c[0-4]
d[0-4]
e = 0
f = 3
```
In summary, having these equations and ranges, filling in only one variable, we narrowed our search area to only three small ranges. We don't have our answer yet, but we can also set another variable, let's set `d` to `0` (our next smallest range and biggest impact):
```
a + d = 5 -> a = 5 (because d = 0)
c + d = 4 -> c = 4 (beacuse d = 0)
```
So, we now have all of our values, being:
```
a: 5
b: 2
c: 4
d: 0
e: 0
f: 3
Total number of presses: 14
```
This isn't our answer _neccessarily_, but it at least is a valid answer. And you can see that testing out one variable rapidly narrows down the search space, and you actually don't have to try all combinations!

We need to test more though, so let's set `d` to `1`:
```
a + d = 5 -> a = 4
c + d = 4 -> c = 3
This gives a total number of presses of: 13
```
And, if you would set `d` to `2` the sum of presses would be `12`, while `d = 3` would lead to a total of `11` and `d = 4` gives `10`. I know that that is the right answer, but we also have to range test `e` to see if the result gets lower - but it doens't.

So to summarize the steps we've taken, being our blueprint for the code to write:
- Make a list of buttons that influence each light (those are our equations from above)
- Set the ranges of each button to the target configuration of each light it affects
- Narrow down the lower bounds of the ranges of buttons based on the upper bounds of other buttons that affect the same light
- Then narrow down the upper bounds of ubttons based on the lower bounds of other buttons that affect the same light
- Start testing a value of the button with the smallest range that occurs most (I eventually dropped the last condition as sorting the buttons was quite heavy and counter-productive)
- Loop over updating the ranges and solving the equations based on the new state
- If testing one value doesn't solve all equations, start looping over the range of the next button nested
- Continue until you have found a solution
- For each outcome, check if it is possible and if the number of presses is lower than what we already found

This seems quite heavy, but it isn't, because you can see that filling in a few values quickly narrows down your search space. And sometimes, you are very lucky and will find the answer almost immediately, even without testing certain values. Let's take this seemingly large and complex line from my real input:
```
[#.##...] (1,4,5,6) (3,5) (1,2,3,6) (0,1,2,3,6) (2,4,5) (4,5) {18,35,40,224,27,224,35}
```
Let's convert it to equations:
```
a(1,4,5,6)   -> a(0100111)
b(3,5)       -> b(0001010)
c(1,2,3,6)   -> c(0111001)
d(0,1,2,3,6) -> d(1111001)
e(2,4,5)     -> e(0010110)
f(4,5)       -> f(0000110)

d = 18
a + c + d = 35
c + d + e = 40
b + c + d = 224
a + e + f = 27
a + b + e + f = 224
a + c + d = 35
```
Now, what looked like a complex input line with large ranges, actually isn't so complex anymore when we rewrite it in our list of equations! There is a lot of great info in there. First of all, it turns out that the first light is only affected by button `d` so it's value is known right away! Secondly, the second and the last light in the configuration have the same target value, but also the same list of buttons which affect that light! (And this isn't always the case, as you can see that 4th and 6th light also have the same target value, but their list of buttons actually isn't the same).

So we can start to simplfiy, let's remove `d` from all equations, and `18` from the corresponding target values (yes we're just going to change those along the way in my algorithm), and also remove that duplicate equation. Our list now becomes:
```
a + c = 17
c + e = 22
b + c = 206
a + e + f = 27
a + b + e + f = 224
```
But there's even _another_ great case that we can catch in code: overlapping button lists! `a + e + f = 27` and `a + b + e + f = 224` are almost equal, except that the latter equation has a `b` in it. Wich means that `b` is also known, or at least can be derived from this. Because `(a + b + e + f) - (a + e + f) = b` so `b = 224 - 27`, which means `b = 197`. Let's see what we've got now, and remove `b` from all equations, together with its value:
```
d = 18
b = 197

a + c = 17
c + e = 22
b + c = 206 -> c = 206 - 197 -> c = 9
a + e + f = 27
a + b + e + f = 224 -> a + e + f = 27
```
Okay, so now we end up with a new duplicate (the last equation) and we suddenly also know the value of `c`. So let's do another round of removing `c` this time:
```
d = 18
b = 197
c = 9

a + c = 17 -> a = 17 - c -> a = 8
c + e = 22 -> e = 22 - c -> e = 13
a + e + f = 27
```
And now with this round, we know `a` and `e` as well, `f` in the last equation follows as well, that becomes `f = 27 - a - e = 27 - 8 - 13 = 6`. In other words:
```
S = a + b + c + d + e + f = 8 + 197 + 9 + 18 + 13 + 6 = 251
```
That's great! We have our answer! And this means, that there is only _one_ possible solution for this. What seemed to be a large search space (`3.378.412.800` possible combinations to be precise), only took a few basic calculations.

And this turns out to be the case for quite a lot of input lines. I wrote a method named `simplify()` to narrow down the search space, decreasing the ranges, and also find overlapping button lists that determines exact values (but also derived deltas like `e + f = 3` means that `e` is always `3 - f`), and that method was able to solve almost 40% of all input lines in under `40ms`. For my first example above, I wrote the method `solve()` that actually recursively starts testing specific values, which is way heavier of course, but it also uses `simplify()` a lot to constantly narrow the search space. With any crazy further optimizations, my algorithm now takes a little less than `900ms` to solve all machines, using the logic as explained above.

### The code

