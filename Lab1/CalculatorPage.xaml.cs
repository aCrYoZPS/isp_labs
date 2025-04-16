using System.Globalization;
using Lab1.Entities;

namespace Lab1;

public partial class CalculatorPage : ContentPage
{
    private double? operand1 = null;
    private double? operand2 = null;
    private double? operand2Cache = null;
    private double? memory = null;
    private OperationType operation = OperationType.None;
    private OperationType operationCache = OperationType.None;
    private bool isAnswer = false;
    private bool isAfterOperation = true;

    public CalculatorPage()
    {
        InitializeComponent();
    }

    private void OnNumberButtonClicked(object? sender, EventArgs args)
    {
        if (NumberEntry.Text == "0" || isAnswer || isAfterOperation)
        {
            NumberEntry.Text = (sender as Button)?.Text!;
            isAnswer = false;
            isAfterOperation = false;
            return;
        }

        NumberEntry.Text += (sender as Button)?.Text;
    }

    private void OnClearButtonClicked(object? sender, EventArgs args)
    {
        OnClearEntryButtonClicked(sender, args);
        OnMemoryClearButtonClicked(sender, args);
    }

    private void OnClearEntryButtonClicked(object? sender, EventArgs args)
    {
        NumberEntry.Text = string.Empty;
        HistoryLabel.Text = string.Empty;
        operand1 = null;
        operand2 = null;
        operation = OperationType.None;
    }

    private void CalculateResult()
    {
        if (operand1 == null) return;
        var result = 0.0;

        if (operation is OperationType.Square or OperationType.SquareRoot or OperationType.Inversion)
        {
            result = operation switch
            {
                OperationType.Square => Math.Pow(operand1.Value, 2),
                OperationType.SquareRoot => Math.Sqrt(operand1.Value),
                OperationType.Inversion => 1 / operand1.Value,
            };

            if (double.IsNaN(result))
            {
                return;
            }

            HistoryLabel.Text = CreateOperationString(operand1, operation, operand2);
            NumberEntry.Text = NumberFormatter.Format(result);

            operand1 = result;
            operation = OperationType.None;
            isAnswer = true;
            return;
        }


        if (operand2 == null)
        {
            if (double.TryParse(NumberEntry.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var tmp))
            {
                operand2 = tmp;
            }
            else
            {
                return;
            }
        }

        result = operation switch
        {
            OperationType.Addition => operand1.Value + operand2.Value,
            OperationType.Subtraction => operand1.Value - operand2.Value,
            OperationType.Multiplication => operand1.Value * operand2.Value,
            OperationType.Division => operand1.Value / operand2.Value,
            // OperationType.Exponentiation => ,
            _ => throw new ArgumentOutOfRangeException()
        };

        HistoryLabel.Text = CreateOperationString(operand1, operation, operand2);
        NumberEntry.Text = NumberFormatter.Format(result);

        operand1 = result;
        operand2Cache = operand2;
        operand2 = null;
        operationCache = operation;
        operation = OperationType.None;
        isAnswer = true;
    }

    private void OnOperatorButtonClicked(object? sender, EventArgs args)
    {
        var operationSign = (sender as Button)?.Text;

        if (operation != OperationType.None && operationSign != "%")
        {
            CalculateResult();
        }

        if (operationSign == "%")
        {
            if (operand1 == null)
            {
                return;
            }

            operand2 = double.Parse(NumberEntry.Text, NumberStyles.Float, CultureInfo.InvariantCulture);
            operand2 = operand1 * (operand2 / 100);
            CalculateResult();
            return;
        }


        operation = operationSign switch
        {
            "+" => OperationType.Addition,
            "-" => OperationType.Subtraction,
            "×" => OperationType.Multiplication,
            "÷" => OperationType.Division,
            "x²" => OperationType.Square,
            "√x" => OperationType.SquareRoot,
            "1/x" => OperationType.Inversion,
            _ => OperationType.None,
        };

        operand1 ??= double.Parse(NumberEntry.Text, NumberStyles.Float, CultureInfo.InvariantCulture);

        switch (operation)
        {
            case OperationType.Square or OperationType.Inversion or OperationType.SquareRoot:
                CalculateResult();
                return;
        }

        HistoryLabel.Text = $"{operand1} {operationSign}";
        isAfterOperation = true;
    }

    private void OnSignChangeButtonClicked(object? sender, EventArgs args)
    {
        NumberEntry.Text = NumberEntry.Text.StartsWith('-') ? NumberEntry.Text[1..] : $"-{NumberEntry.Text}";
    }

    private void OnPointButtonClicked(object? sender, EventArgs args)
    {
        if (!NumberEntry.Text.Contains('.'))
        {
            NumberEntry.Text += '.';
        }
    }

    private void OnBackspaceButtonClicked(object? sender, EventArgs args)
    {
        if (NumberEntry.Text.ToLower().Contains('e'))
        {
            return;
        }

        NumberEntry.Text = NumberEntry.Text.Length switch
        {
            1 => "0",
            >= 2 => NumberEntry.Text[..^1],
            _ => NumberEntry.Text
        };
    }

    private void OnEqualsButtonClicked(object? sender, EventArgs args)
    {
        if (operand2 == null && operand2Cache != null && operationCache is OperationType.Addition
                or OperationType.Subtraction or OperationType.Multiplication or OperationType.Division && isAnswer)
        {
            operand2 = operand2Cache;
            operation = operationCache;
        }
        else if (operand1 == null || isAnswer)
        {
            if (!double.TryParse(NumberEntry.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var tmp))
            {
                return;
            }

            operand1 = tmp;
            isAnswer = false;
        }
        else
        {
            if (double.TryParse(NumberEntry.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var tmp))
            {
                operand2 ??= tmp;
            }
        }

        CalculateResult();
    }


    private static string CreateOperationString(double? operand1, OperationType operationType, double? operand2)
    {
        return operationType switch
        {
            OperationType.None => $"{operand1} = ",
            OperationType.Addition => $"{operand1} + {operand2} =",
            OperationType.Subtraction => $"{operand1} - {operand2} =",
            OperationType.Multiplication => $"{operand1} × {operand2} =",
            OperationType.Division => $"{operand1} ÷ {operand2} =",
            OperationType.Inversion => $"inverse({operand1}) =",
            OperationType.SquareRoot => $"sqrt({operand1}) =",
            OperationType.Square => $"sqr({operand1}) =",
            _ => string.Empty,
        };
    }

    private void OnMemoryClearButtonClicked(object? sender, EventArgs args)
    {
        memory = null;
        MemoryClearButton.IsEnabled = false;
        MemoryAddButton.IsEnabled = false;
        MemorySubtractButton.IsEnabled = false;
        MemoryRecallButton.IsEnabled = false;
    }

    private void OnMemoryRecallButtonClicked(object? sender, EventArgs args)
    {
        if (memory == null) return;
        NumberEntry.Text = NumberFormatter.Format(memory.Value);
    }

    private void OnMemoryAddButtonClicked(object? sender, EventArgs args)
    {
        if (memory == null) return;
        var operand = double.Parse(NumberEntry.Text, NumberStyles.Float, CultureInfo.InvariantCulture);
        memory += operand;
    }

    private void OnMemorySubtractButtonClicked(object? sender, EventArgs args)
    {
        if (memory == null) return;
        var operand = double.Parse(NumberEntry.Text, NumberStyles.Float, CultureInfo.InvariantCulture);
        memory -= operand;
    }

    private void OnMemoryStoreButtonClicked(object? sender, EventArgs args)
    {
        if (string.IsNullOrEmpty(NumberEntry.Text)) return;
        memory = double.Parse(NumberEntry.Text, NumberStyles.Float, CultureInfo.InvariantCulture);
        MemoryClearButton.IsEnabled = true;
        MemoryAddButton.IsEnabled = true;
        MemorySubtractButton.IsEnabled = true;
        MemoryRecallButton.IsEnabled = true;
    }
}