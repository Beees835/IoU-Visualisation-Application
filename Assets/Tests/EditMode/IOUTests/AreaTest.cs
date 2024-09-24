using NUnit.Framework;
using UnityEngine;
using static IoUManager;
using static TestingFunctions;
using System;

public class AreaTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void CalculateAreaSquare()
    {

        Vector2[] polygon = new Vector2[4];

        int length = 2;
        int width = 2;


        // Create shape points
        polygon[0] = new Vector2(0, 0);
        polygon[1] = new Vector2(length, 0);
        polygon[2] = new Vector2(length, width);
        polygon[3] = new Vector2(0, width);

        float realArea = length * width;

        float calcArea = IoUManager.CalculatePolygonArea(polygon);

        bool closeEnough = AreCloseEnough(realArea, calcArea);
        Assert.IsTrue(closeEnough);
    }

    [Test]
    public void CalculateAreaRectangle()
    {

        Vector2[] polygon = new Vector2[4];

        int length = 2;
        int width = 4;


        // Create shape points
        polygon[0] = new Vector2(0, 0);
        polygon[1] = new Vector2(length, 0);
        polygon[2] = new Vector2(length, width);
        polygon[3] = new Vector2(0, width);

        float realArea = length * width;

        float calcArea = IoUManager.CalculatePolygonArea(polygon);

        bool closeEnough = AreCloseEnough(realArea, calcArea);
        Assert.IsTrue(closeEnough);
    }

    [Test]
    public void CalculateAreaTriangle()
    {
        Vector2[] polygon = new Vector2[3];

        int width = 3;
        int height = 2;


        // Create shape points
        polygon[0] = new Vector2(0, 0);
        polygon[1] = new Vector2(width, 0);
        polygon[2] = new Vector2(width - 0.5f * width, height);

        float realArea = 0.5f * width * height;

        float calcArea = IoUManager.CalculatePolygonArea(polygon);


        bool closeEnough = AreCloseEnough(realArea, calcArea);
        Assert.IsTrue(closeEnough);
    }

}
