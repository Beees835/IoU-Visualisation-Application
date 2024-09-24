using NUnit.Framework;
using UnityEngine;
using static IoUManager;
using static TestingFunctions;

public class PointInPolygonTest
{
    [Test]
    public void PointInsideRectangle()
    {
        Vector2[] polygon = new Vector2[4];
        polygon[0] = new Vector2(0, 0);
        polygon[1] = new Vector2(3, 0);
        polygon[2] = new Vector2(3, 3);
        polygon[3] = new Vector2(0, 3);

        Vector2 point = new Vector2(1, 1);

        Assert.IsTrue(IsPointInPolygon(point, polygon));
    }

    [Test]
    public void PointOutsideRectangle()
    {
        Vector2[] polygon = new Vector2[4];
        polygon[0] = new Vector2(0, 0);
        polygon[1] = new Vector2(3, 0);
        polygon[2] = new Vector2(3, 3);
        polygon[3] = new Vector2(0, 3);

        Vector2 point = new Vector2(4, 4);

        Assert.IsFalse(IsPointInPolygon(point, polygon));
    }

    [Test]
    public void PointOnEdge()
    {
        Vector2[] polygon = new Vector2[4];
        polygon[0] = new Vector2(0, 0);
        polygon[1] = new Vector2(3, 0);
        polygon[2] = new Vector2(3, 3);
        polygon[3] = new Vector2(0, 3);

        Vector2 point = new Vector2(2, 0);

        Assert.IsTrue(IsPointInPolygon(point, polygon));
    }

    [Test]
    public void PointInsideTriangle()
    {
        Vector2[] polygon = new Vector2[3];
        polygon[0] = new Vector2(0, 0);
        polygon[1] = new Vector2(3, 0);
        polygon[2] = new Vector2(1.5f, 3);

        Vector2 point = new Vector2(1, 1);

        Assert.IsTrue(IsPointInPolygon(point, polygon));
    }

    [Test]
    public void PointOutsideTriangle()
    {
        Vector2[] polygon = new Vector2[3];
        polygon[0] = new Vector2(0, 0);
        polygon[1] = new Vector2(3, 0);
        polygon[2] = new Vector2(1.5f, 3);

        Vector2 point = new Vector2(1.6f, 3);

        Assert.IsFalse(IsPointInPolygon(point, polygon));
    }
}