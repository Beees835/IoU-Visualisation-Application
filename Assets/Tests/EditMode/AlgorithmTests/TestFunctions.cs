using System;

/// <summary>
/// Functions used within the test suite
/// </summary>
public static class TestingFunctions
{
    /// <summary>
    /// Determine whether two values are equivalent to an epsilon
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns>Whether the values are equivalent</returns>
    public static bool AreCloseEnough(float value1, float value2, float errorVal = 0.0001f)
    {
        return Math.Abs(value1 - value2) <= errorVal;
    }
}
