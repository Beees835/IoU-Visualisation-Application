using System;

/*
 * Functions used within the test suite 
 */
public static class TestingFunctions
{
    /*
     * Check if two values are the same within a given epsilon 
     */
    public static bool AreCloseEnough(float value1, float value2)
    {
        // Need to account for floating point innaccuracy
        float errorVal = 0.0001f;
        return Math.Abs(value1 - value2) <= errorVal;
    }
}
