using System;

public class CalculatorImplementation : BaseCalculatorOperations
{
    public override float? Add(float a, float b)
    {
        return a + b;
    }
    
    /// <summary>
    /// Multiplication function.
    /// </summary>
    /// <param name="a">First number</param>
    /// <param name="b">Second number</param>
    /// <returns></returns>
    public override float? Multiply(float a, float b)
    {
        return a * b;
    }
}
