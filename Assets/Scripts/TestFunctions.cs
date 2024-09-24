
using System;

public static class TestingFunctions{ 
public static bool AreCloseEnough(float value1, float value2)
{
    return Math.Abs(value1 - value2) <= 0.0000001f;
}
}
