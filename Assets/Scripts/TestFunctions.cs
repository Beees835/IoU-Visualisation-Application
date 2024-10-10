using System;

public static class TestingFunctions
{
    public static bool AreCloseEnough(float value1, float value2)
    {
        // Need to account for floating point innaccuracy
        float errorVal = 0.0001f;
        return Math.Abs(value1 - value2) <= errorVal;
    }
}
