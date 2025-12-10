*This is a copy of the original puzzle description. Also, the example input and my personal puzzle input are included in this folder as well as my answers to both parts of today's puzzle. If you want to learn more about my approach to solving these puzzles see [my solutions](https://github.com/robhabraken/advent-of-code-2025/blob/main/solutions/09/README.md).*

---
# Day 9: Movie Theater

The first part was a very easy warm-up. We just need to calculate the area of all squares that we can form between each possible pair of tiles in the given list. I created a `Tile` class to easily parse the input and reference the coordinates and then loop over the input to store the given tiles in a `Tile[]` array. Then I loop over all tiles, and then over every other tile from the index after that to find all unique pairs. For each pair, if the x- and y-coordinates aren't equal (I'm guestimating that single row or single column squares will never be the largest ones possible), I calculate the area, which is the product of the difference between the x- and y-coordinates. If the calculated area is bigger than the largest one we found, store the new largest area, until we've calculated them all.

The number of tiles is less than 500, so even calculating the area regardless of where they are isn't that expensive. This solution runs in 1ms. The size of the grid though is huge, so we have to stick to math and not create an actual map for this puzzle.

# Part Two

The second part was the hardest puzzle of 2025 yet I think. It took a while to envision even an approach 'on paper'. And when it clicked, it took quite some time to actually build the solution too. 

My translation of the puzzle:
- the given tiles are corners of line segments (I call them 'edges') and they form a polygon
- we need to find the biggest rectangle we can fit inside this polygon

Luckily, the coordinates are already in the right order (clockwise line segments). We do need to understand though that while the example is quite a simple polygon, the real puzzle input may very well be a very complex polygon with paths going into itself (large and complex shaped hollow spaces inside the outer edges of the polygon). But at least it is closed, and it never crosses itself, that's a safe assumption to make I think.

I have written an elaborate explanation below, but if you just want a very short explanation, here's my TL;DR: **I use raycasting to determine if the additional corners of the rectangle, and all intersections of the rectangle with the edges of the polygon, lie inside of the polygon, and if so, it fits**.

---

The approach I came up with:
- start exactly the same as with part 1, calculating the area of any given unique combination of two tiles from the input
- determine the other two corners of the rectangle that is defined by the two given tiles
- check if those other corners are both located inside the polygon (we of course know the given tiles are) which is actually the cheapest and fastest way to deprecate this rectangle if it protrudes over the edges of the polygon (because I only have to check two points, and if those other corners aren't inside of the polygon, the rectangle is never going to fit anyway)
- if the given area is larger than the largest one we found yet, and if all four corners are located inside of the polygon, we can start to do the more complex and expensive computations
- I then create four new `Edge` objects to describe the rectangle, I need those to calculate intersections
- now comes the 'smart' part: we actually don't have to check every tile inside of this rectangle, we only need to check the edges, and if all edges are fully inside of the polygon, all of the tiles within the edges automatically are as well, because the polygon could never have an open space inside this rectangle without crossing at least one of the edges
- to make my check even more efficient: I only need to concentrate on the tiles that are close to the intersections of my rectangle and the edges of the polygon
- in other words, if a side of my rectangle intersects with the polygon, I need to check the adjacent tiles, and if they are both still inside of the polygon, I can ignore this intersection - but if one of these tiles around an intersection lay outside of the polygon, the rectangle doesn't fit

I think the last statement needs some visual explanation, let's take the diagram from the example:
```
..............
.......#XXX#..
.......XXXXX..
..0OOOOOOOXX..
..OOOOOOOOXX..
..OOOOOOO0XX..
.........XXX..
.........#X#..
..............
```
You can see that the upper horizontal edge of the rectangle intersects with two edges of the polygon (we can safely ignore parallel or coincident lines between the polygon and the rectangle): the left one on the side (2,5-2,3) and the topleft one (7,3-7,1). So an intersection of the edge of the polygon could be alarmign, but in this case, because we are working with a very coarse grid of tiles and no decimal values, those intersecting 'lines' are fully overlapping with the edges of the rectangle, so all is okay.

But in this case for example, it is not:
```
..................
..#XXX#...#XXXX#..
..XXXXX...XXXXXX..
..XXXX#XXX#XXXXX..
..#XXXXXXXXXXXX#..
..................
```
If you would draw a rectangle over this polygon using the outside corners of the polygon, all four corners of the rectangle would be inside of the polygon, but it still wouldn't fit because of the indent. So let's take that top segment, that top edge of the imaginar rectangle again, then this are the intersecting lines:
```
..................
..#OOO#OOO#OOOO#..
..X...X...X....X..
..X...#...#....X..
..#............#..
..................
```
So the four vertical lines intersect with my horizontal line.

Now what my algorithm does, is it finds all of the _adjacent_ tiles to the intersections (the `#` marked tiles in the top row), and checks if they lay inside the edge of the rectangle, which are the tiles marked `A` to `F`:
```
..................
..#AOB#COD#EOOF#..
..X...X...X....X..
..X...#...#....X..
..#............#..
..................
```
Now if we are going to check for each of those tiles if they are inside of the polygon, I will find that `A`, `B`, `E` and `F` are, but `C` and `D` aren't. And as soon as my algorithm finds `C`, it aborts because this rectangle isn't valid.

To further illustrate, the next shape gives you the exact same intersection locations:
```
..................
......#XXX#.......
..#XXX#XXX#XXXX#..
..XXXXXXXXXXXXXX..
..XXXXXXXXXXXXXX..
..#XXXXXXXXXXXX#..
..................
```
But now, if you would check those `A` to `F` tiles in this case, they would all be inside the polygon. And that's correct, for the same rectangle would indeed fit into this polygon.

So, to summarize, for each reactangle:
- I compute the two other corners
- check if those 2 corners fit inside the polygon
- if so, calculate the intersections between the rectangle and the polygon, which would be 10 intersections in this case
- find the adjacent tiles within the line segments of the rectangle, which would be 12 tiles in this case
- only check those 12 tiles to see if they are inside of the polygon
- as soon as any of those checks fails, I skip this rectangle right away

Most rectangles only have 5 to 10 intersections, so the amount of checks I need to do per pair of tiles are relatively limited! My solution runs in 1332ms and I'm quite happy with that for now.

## Raycasting & odd-even rule
How do I determine if a tile lies inside of the polygon? That's surprisingly easy using raycasting. None of the given points is using the `0,0` coordinate, so I'm using that as a reference point. For any tile I want to check, I simple create a new line (edge) from that tile to my reference point. And then I reuse my method `Intersect` to find the intersection of that reference line with _all_ of the given edges! So, I check every edge of the polygon and test if it intersects with this reference line, this ray. I then simply count the number of intersections: an odd number of intersections is good, that means that tile is inside of the polygon. An even number of intersections means it is not. Simply draw a square, draw a line from inside that square to outside the square. You cross the edge of the square only once while doing that. If the shape (polygon) would be more complex, you could potentially cross the edge multiple times going in and out again, but as soon as you end up outside of your polygon, that number should still be uneven.

Now there's an exception to that rule in our case: if we leave from a tile that lies on one of the edges, you could count two intersections (at a corner tile). So, we first check for each edge if the given tile actually lies _on_ that edge, and if that's the case, we immediately abort any calculations for all of the edges, because we know for sure it lies inside of our polygon!