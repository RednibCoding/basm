
namespace Basm;


public class SymbolTable
{
    private List<RuntimeValue> symbols = new();

    public void CreateVariable(RuntimeValue rtValue)
    {
        var existingSymbol = GetSymbol(rtValue.Name);
        if (existingSymbol == null)
        {
            // Add new symbol
            symbols.Add(rtValue);
        }
        else
        {
            Console.WriteLine($"symbol ${rtValue.Name} already defined");
            Environment.Exit(1);
        }
    }

    public void UpdateVariable(RuntimeValue rtValue)
    {
        var existingSymbol = GetSymbol(rtValue.Name);
        if (existingSymbol != null)
        {
            existingSymbol.StringValue = rtValue.StringValue;
            existingSymbol.IntValue = rtValue.IntValue;
            existingSymbol.FloatValue = rtValue.FloatValue;
            existingSymbol.BoolValue = rtValue.BoolValue;
        }
        else
        {
            Console.WriteLine($"symbol ${rtValue.Name} not found");
            Environment.Exit(1);
        }
    }

    public RuntimeValue? GetSymbol(string name)
    {
        return symbols.FirstOrDefault(s => s.Name == name);
    }

    private int GetSymbolPos(string name)
    {
        for (int i = 0; i < symbols.Count; i++)
        {
            if (symbols[i].Name == name)
            {
                return i;
            }
        }
        return -1;
    }
}

