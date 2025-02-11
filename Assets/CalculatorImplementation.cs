using System;

public class CalculatorImplementation : BaseCalculatorOperations
{
    public override float? Add(float a, float b)
    {
        return a + b;
    }

    public override float? Multiply(float a, float b)
    {
        return a * b;
    }
}
