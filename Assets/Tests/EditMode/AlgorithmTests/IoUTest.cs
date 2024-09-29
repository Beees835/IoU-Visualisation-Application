using NUnit.Framework;
using UnityEngine;
using static IoUManager;
using static TestingFunctions;

public class IoUTest
{
    [Test]
    public void FullOverlap()
    {
        Vector2[] polygon = new Vector2[4];
        polygon[0] = new Vector2(0, 0);
        polygon[1] = new Vector2(3, 0);
        polygon[2] = new Vector2(3, 3);
        polygon[3] = new Vector2(0, 3);

        float area = CalculatePolygonArea(polygon);

        float output = CalculateIoU(area, area, polygon)[2];
        AreCloseEnough(1, output);
    }

    [Test]
    public void NoOverlap()
    {
        Vector2[] polygon = new Vector2[0];

        // Placeholder value shouldn't matter
        float area = 5;

        float output = CalculateIoU(area, area, polygon)[2];
        AreCloseEnough(0, output);
    }

    [Test]
    public void PartialOverlap()
    {
        // Simulate an Equilateral Triangle fully inside a rectangle

        int width = 4;

        Vector2[] polygon = new Vector2[3];
        polygon[0] = new Vector2(0, 0);
        polygon[1] = new Vector2(width, 0);
        polygon[2] = new Vector2(0.5f * width, width);

        float rectangleArea = width * width;
        float triangleArea = 0.5f * width * width;

        float output = CalculateIoU(rectangleArea, triangleArea, polygon)[2];

        AreCloseEnough(0.5f, output);
    }
}