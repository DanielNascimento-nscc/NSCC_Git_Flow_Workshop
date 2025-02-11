using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class App : MonoBehaviour
{
    private UIDocument uiDocument;
    private MenuCreator menuCreator;
    private float?[] storedNumbers = new float?[] { null, null };
    private string currentOperation;
    private BaseCalculatorOperations calculatorOperations;
    public Dictionary<string, string> operationDisplay = new Dictionary<string, string> { { "add", "+" },
            { "subtract", "-" },
            { "multiply", "×" },
            { "divide", "÷" },
            { "modulus", "%" }
    };
    
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        menuCreator = GetComponent<MenuCreator>();
        calculatorOperations = new CalculatorImplementation();
        SetupUI();
    }


    private void SetupUI()
    {

        // Create list of all buttons
        var allButtons = new List<MenuButton>();

        // Add number buttons (7-8-9, 4-5-6, 1-2-3, 0)
        for (int i = 7; i <= 9; i++)
        {
            int number = i;
            allButtons.Add(new MenuButton(
                i.ToString(),
                () => OnNumberPressed(number),
                GetButtonColorForNumber()
            ));
        }
        for (int i = 4; i <= 6; i++)
        {
            int number = i;
            allButtons.Add(new MenuButton(
                i.ToString(),
                () => OnNumberPressed(number),
                GetButtonColorForNumber()
            ));
        }
        for (int i = 1; i <= 3; i++)
        {
            int number = i;
            allButtons.Add(new MenuButton(
                i.ToString(),
                () => OnNumberPressed(number),
                GetButtonColorForNumber()
            ));
        }
        
        // Add 0 and operations in the last row
        allButtons.Add(new MenuButton(
            "0",
            () => OnNumberPressed(0),
            GetButtonColorForNumber()
        ));

        // Add operation buttons
        allButtons.Add(new MenuButton("+", () => OnOperationPressed("add"), 
            IsOperationOverridden("add") ? ButtonColors.ButtonBlue : ButtonColors.ButtonGrey));
        allButtons.Add(new MenuButton("-", () => OnOperationPressed("subtract"), 
            IsOperationOverridden("subtract") ? ButtonColors.ButtonBlue : ButtonColors.ButtonGrey));
        allButtons.Add(new MenuButton("×", () => OnOperationPressed("multiply"), 
            IsOperationOverridden("multiply") ? ButtonColors.ButtonBlue : ButtonColors.ButtonGrey));
        allButtons.Add(new MenuButton("÷", () => OnOperationPressed("divide"), 
            IsOperationOverridden("divide") ? ButtonColors.ButtonBlue : ButtonColors.ButtonGrey));
        allButtons.Add(new MenuButton("%", () => OnOperationPressed("modulus"), 
            IsOperationOverridden("modulus") ? ButtonColors.ButtonBlue : ButtonColors.ButtonGrey));

        allButtons.Add(new MenuButton(
            "C",
            Clear,
            ButtonColors.ButtonRed
        ));
        // Add equals and clear buttons
        allButtons.Add(new MenuButton(
            "=",
            ExecuteOperation,
            ButtonColors.ButtonGreen
        ));

        // Create the menu with three buttons side by side
        menuCreator.CreateMenuWithThreeButtonWhenPossible(allButtons.ToArray());

        menuCreator.m_MenuTitle.text = "";
    }


    private int currentNumberInputting = 0;
    private string[] numbers = new string[] { "" , ""};
    private string operation = "";
    private string previousOperation = "";

    private ButtonColors GetButtonColorForNumber()
    {
        return ButtonColors.ButtonNavyBlue;
    }

    private void OnNumberPressed(int number)
    {
        numbers[currentNumberInputting] += number.ToString();
        UpdateNumbersVisual();
        Debug.Log($"OnNumberPressed: {menuCreator.m_MenuTitle.text} added {number.ToString()}");
    }

    private void UpdateNumbersVisual()
    {
        menuCreator.m_MenuTitle.text = string.IsNullOrEmpty(previousOperation) ? "": $"<size=50%>{previousOperation}</size>\n";
        string showOperation = "";
        if (!string.IsNullOrEmpty(operation))
            operationDisplay.TryGetValue(operation, out showOperation);
        else if (string.IsNullOrEmpty(numbers[0]))
            menuCreator.m_MenuTitle.text += "|  ";
        
        menuCreator.m_MenuTitle.text += $"{numbers[0]} {showOperation} {numbers[1]}";
        
    }

    private void OnOperationPressed(string operation)
    {
        if (!string.IsNullOrEmpty(operation))
        {
            if (storedNumbers[0].HasValue) {
                ExecuteOperation();
            }
            
            storedNumbers[0] = float.Parse(numbers[currentNumberInputting]);
            currentOperation = operation;
            this.operation = operation;
            currentNumberInputting++;

            numbers[1] = "";
        }
        UpdateNumbersVisual();
    }

    private void ExecuteOperation()
    {
        if (!storedNumbers[0].HasValue || string.IsNullOrEmpty(numbers[1]))
            return;

        float secondNumber = float.Parse(numbers[1]);
        float? result = null;

        switch (operation)
        {
            case "add":
                result = calculatorOperations.Add(storedNumbers[0].Value, secondNumber);
                break;
            case "subtract":
                result = calculatorOperations.Subtract(storedNumbers[0].Value, secondNumber);
                break;
            case "multiply":
                result = calculatorOperations.Multiply(storedNumbers[0].Value, secondNumber);
                break;
            case "divide":
                result = calculatorOperations.Divide(storedNumbers[0].Value, secondNumber);
                break;
            case "modulus":
                result = calculatorOperations.Modulus(storedNumbers[0].Value, secondNumber);
                break;
        }

        if (result.HasValue)
        {
            numbers[0] = result.Value.ToString();
        } else
        {
            previousOperation = menuCreator.m_MenuTitle.text;
            menuCreator.m_MenuTitle.text = "";
            numbers[0] = "";
            numbers[1] = "";
        }

        operation = "";
        storedNumbers[0] = null;
        storedNumbers[1] = null;
        currentOperation = "";
        currentNumberInputting = 0;

        UpdateNumbersVisual();
    }

    private void Clear()
    {
        numbers[0] = "";
        numbers[1] = "";
        operation = "";
        storedNumbers[0] = null;
        storedNumbers[1] = null;
        currentOperation = "";
        currentNumberInputting = 0;
    }

    private bool IsOperationOverridden(string operation)
    {
        var type = calculatorOperations.GetType();
        var baseType = typeof(BaseCalculatorOperations);
        var method = type.GetMethod(char.ToUpper(operation[0]) + operation.Substring(1));
        return method.DeclaringType != baseType;
    }
}
