using NUnit.Framework;
using UnityEngine;
using static IoUManager;
using static TestingFunctions;
using System;

public class AreaTest
{
    [Test]
    public void RectangleArea()
    {
        Vector2[] polygon = new Vector2[4];

        float length, width;
        float realArea, calcArea;
        int max = 10;
        bool closeEnough = true;

        for (float i=1;i<max;i=i+0.1f)
        {
            for (float j =1;j<max; j = j + 0.1f)
            {
                length = i;
                width = j;
                // Create shape points
                polygon[0] = new Vector2(0, 0);
                polygon[1] = new Vector2(length, 0);
                polygon[2] = new Vector2(length, width);
                polygon[3] = new Vector2(0, width);

                realArea = length * width;

                calcArea = CalculatePolygonArea(polygon);

                closeEnough &= AreCloseEnough(realArea, calcArea);
            }
        }
        Assert.IsTrue(closeEnough);
    }

    [Test]
    public void TriangleArea()
    {
        Vector2[] polygon = new Vector2[3];

        float height, width;
        float realArea, calcArea;
        int max = 10;
        bool closeEnough = true;

        // Isosceles
        for (float i = 1; i < max; i = i + 0.1f)
        {
            for (float j = 1; j < max; j = j + 0.1f)
            {
                width = i;
                height = j;

                // Create shape points
                polygon[0] = new Vector2(0, 0);
                polygon[1] = new Vector2(width, 0);
                polygon[2] = new Vector2(width - 0.5f * width, height);

                realArea = 0.5f * width * height;

                calcArea = CalculatePolygonArea(polygon);

                closeEnough &= AreCloseEnough(realArea, calcArea);
            }
        }

        // Equilateral
        for (float i = 1; i < max; i = i + 0.1f)
        {
            width = i;
            height = (float)(Math.Sqrt(3) / 2) * width;

            // Create shape points
            polygon[0] = new Vector2(0, 0);
            polygon[1] = new Vector2(width, 0);
            polygon[2] = new Vector2(width - 0.5f * width, height);

            realArea = 0.5f * width * height;

            calcArea = CalculatePolygonArea(polygon);

            closeEnough &= AreCloseEnough(realArea, calcArea);
        }

        Assert.IsTrue(closeEnough);
    }

    [Test]
    public void ComplexAre()
    {
        // Triangle stacked on a square

        Vector2[] polygon = new Vector2[5];

        float width, squareHeight, triangleHeight;
        float realArea, calcArea;
        float max = 5;
        bool closeEnough = true;

        for (float i = 1; i < max; i = i + 0.1f)
        {
            for (float j = 1; j < max; j = j + 0.1f)
            {
                for (float k = 1; j < max; j = j + 0.1f)
                {
                    width = i;
                    squareHeight = j;
                    triangleHeight = k;

                    // Create shape points
                    polygon[0] = new Vector2(0, 0);
                    polygon[1] = new Vector2(width, 0);
                    polygon[2] = new Vector2(width, squareHeight);
                    polygon[3] = new Vector2(width - 0.5f * width, squareHeight + triangleHeight);
                    polygon[4] = new Vector2(0, squareHeight);

                    float squareArea = width * squareHeight;
                    float triangleArea = 0.5f * width * triangleHeight;

                    realArea = squareArea + triangleArea;

                    calcArea = CalculatePolygonArea(polygon);

                    closeEnough &= AreCloseEnough(realArea, calcArea);
                }
            }
            Assert.IsTrue(closeEnough);
        }
    }
}
