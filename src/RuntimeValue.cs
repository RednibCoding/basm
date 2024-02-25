namespace Basm;

public enum ValueType
{
    INT,
    FLOAT,
    STRING,
    BOOL,
}

public class RuntimeValue
{
    public string Name { get; private set; }
    public ValueType Type { get; set; }
    public string StringValue { get; set; }
    public int IntValue { get; set; }
    public double FloatValue { get; set; }
    public bool BoolValue { get; set; }

    public RuntimeValue(ValueType type, string name, string strval = "", int intval = 0, double floatval = 0.0, bool boolval = false)
    {
        Type = type;
        Name = name;
        StringValue = strval;
        IntValue = intval;
        FloatValue = floatval;
        BoolValue = boolval;
    }
}