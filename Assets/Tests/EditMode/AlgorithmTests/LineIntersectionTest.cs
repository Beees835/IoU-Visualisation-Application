using NUnit.Framework;
using UnityEngine;
using static IoUManager;
using static TestingFunctions;

public class LineIntersectionTest
{
    [Test]
    public void Intersection()
    {
        Vector2 l1Start = new Vector2(0, 0);
        Vector2 l1End = new Vector2(3, 0);
        Vector2 l2Start = new Vector2(2, 1);
        Vector2 l2End = new Vector2(2, -1);

        Vector2 output = (Vector2)GetIntersectionPoint(l1Start, l1End, l2Start, l2End);

        Assert.NotNull(output);

        AreCloseEnough(2, output.x);
        AreCloseEnough(0, output.y);
    }


    [Test]
    public void NoIntersection()
    {
        Vector2 l1Start = new Vector2(0, 0);
        Vector2 l1End = new Vector2(2, 0);
        Vector2 l2Start = new Vector2(3, 4);
        Vector2 l2End = new Vector2(5, 2);

        Assert.Null(GetIntersectionPoint(l1Start, l1End, l2Start, l2End));
    }


    [Test]
    public void ParallelNoIntersection()
    {
        // Lines are parallel
        Vector2 l1Start = new Vector2(0, 0);
        Vector2 l1End = new Vector2(2, 0);
        Vector2 l2Start = new Vector2(0, 1);
        Vector2 l2End = new Vector2(2, 1);

        Assert.Null(GetIntersectionPoint(l1Start, l1End, l2Start, l2End));
    }

    [Test]
    public void ParallelIntersection()
    {
        // Lines are same
        Vector2 l1Start = new Vector2(0, 0);
        Vector2 l1End = new Vector2(1, 1);

        // Infinite Intersections not useful
        Assert.Null(GetIntersectionPoint(l1Start, l1End, l1Start, l1End));
    }
}