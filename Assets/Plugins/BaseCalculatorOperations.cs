using UnityEngine;
using UnityEngine.UIElements;

public class BaseCalculatorOperations
{
    public virtual float? Add(float a, float b) => null;
    public virtual float? Subtract(float a, float b) => null;
    public virtual float? Multiply(float a, float b) => null;
    public virtual float? Divide(float a, float b) => null;
    public virtual float? Modulus(float a, float b) => null;
} 