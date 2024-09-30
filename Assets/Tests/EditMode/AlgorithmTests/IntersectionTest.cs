using NUnit.Framework;
using UnityEngine;
using static IoUCalculator;
using static TestingFunctions;

public class IntersectionTest
{
    [Test]
    public void RectangleIntersection()
    {

        Vector2[] polygon1 = new Vector2[4];
        Vector2[] polygon2 = new Vector2[4];

        float length, width;
        float realArea, calcArea;
        float offset;
        int max = 10;
        bool closeEnough = true;

        for (float i = 1; i < max; i = i + 0.1f)
        {
            for (float j = 1; j < max; j = j + 0.1f)
            {
                for (float k = 0; j < max; j = j + 0.1f)
                {
                    width = i;
                    length = j;
                    offset = k;

                    polygon1[0] = new Vector2(0, 0);
                    polygon1[1] = new Vector2(length, 0);
                    polygon1[2] = new Vector2(length, width);
                    polygon1[3] = new Vector2(0, width);

                    polygon2[0] = new Vector2(offset, 0);
                    polygon2[1] = new Vector2(offset + length, 0);
                    polygon2[2] = new Vector2(offset + length, width);
                    polygon2[3] = new Vector2(offset, width);

                    if (width <= offset)
                    {
                        realArea = 0;
                    }
                    else
                    {
                        realArea = (width - offset) * length;
                    }

                    calcArea = CalculatePolygonArea(GetIntersectionPoints(polygon1, polygon2));

                    closeEnough &= AreCloseEnough(realArea, calcArea);

                }
            }
        }
        Assert.IsTrue(closeEnough);
    }

}