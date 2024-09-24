using NUnit.Framework;
using UnityEngine;
using static IoUManager;

public class IOUTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void CalculateAreaSquare()
    {

        Vector2[] polygon = new Vector2[4];

        int length = 2;
        int height = 2;


        // Create shape points
        polygon[0] = new Vector2(0,0);
        polygon[1] = new Vector2(length, 0);
        polygon[2] = new Vector2(length, height);
        polygon[3] = new Vector2(0, height);

        double baseArea = length * height;

        double calcArea = IoUManager.CalculatePolygonArea(polygon);


        // Use the Assert class to test conditions
        Assert.AreEqual(baseArea, calcArea);
    }

    [Test]
    public void CalculateAreaRectangle()
    {

        Vector2[] polygon = new Vector2[4];

        // Use the Assert class to test conditions
        Assert.AreEqual(1, 1);

    }

    [Test]
    public void CalculateAreaTriangle()
    {
        Vector2[] polygon = new Vector2[3];

        // Use the Assert class to test conditions
    }

    [Test]
    public void CalculateAreaPentagon()
    {
        Vector2[] polygon = new Vector2[5];

        // Use the Assert class to test conditions
    }
}
