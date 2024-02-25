namespace Basm;

public class Parser
{
    private List<Token> tokens = [];
    private int tokenIndex = 0;
    private SymbolTable symbolTable;
    private Stack<int> stack = [];
    private Lexer lexer;
    private Dictionary<string, int> labelMappings = [];
    private List<string> program = [];
    private int pc = 0;

    public Parser()
    {
        symbolTable = new SymbolTable();
        lexer = new Lexer();
    }

    public void ParseProgram(string programString)
    {
        try
        {
            // Split the input string into lines
            var lines = programString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                // NOTE: keep empty lines for line numbers, dont delete any lines!
                program.Add(line);
                var tokens = getTokens(line);
                if (tokens.Count > 0 && tokens[0].Type == TokenType.LABEL)
                {
                    if (tokens.Count > 2) // LABEL and ENTER token
                    {
                        Console.WriteLine("label must appear on a seperate line");
                        Environment.Exit(1);
                    }
                    labelMappings[tokens[0].Value] = program.Count;
                }
            }

            // Reset program counter before execution
            pc = 0;

            while (pc < program.Count)
            {
                tokenIndex = 0;
                var line = program[pc];
                tokens = getTokens(line);
                pc++;
                parseStatement();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing program string: {ex.Message}");
        }
    }


    private void advance()
    {
        tokenIndex++;
    }

    private Token eat(TokenType type)
    {
        var tokenType = tokens[tokenIndex].Type;
        if (tokenType == type)
        {
            advance();
            return tokens[tokenIndex - 1];
        }
        else
        {
            throw new Exception("Syntax error");
        }
    }

    private List<Token> getTokens(string input)
    {
        var toks = lexer.GetTokens(input);
        return toks;
    }

    private int getLabelPos(string label)
    {
        if (labelMappings.ContainsKey(label))
        {
            return labelMappings[label];
        }
        return -1;
    }

    private bool evaluateCondition(RuntimeValue value1, TokenType operatorType, RuntimeValue value2)
    {
        // TODO: implement other value types
        if (value1.Type != ValueType.INT && value1.Type != ValueType.FLOAT)
        {
            Console.WriteLine("value types of logical expression must be of type int or float");
            Environment.Exit(1);
        }
        if (value2.Type != ValueType.INT && value2.Type != ValueType.FLOAT)
        {
            Console.WriteLine("value types of logical expression must be of type int or float");
            Environment.Exit(1);
        }

        var val1 = value1.Type == ValueType.INT ? value1.IntValue : value1.FloatValue;
        var val2 = value2.Type == ValueType.INT ? value1.IntValue : value1.FloatValue;

        switch (operatorType)
        {
            case TokenType.EQUALTO:
                return val1 == val2;
            case TokenType.GT:
                return val1 > val2;
            case TokenType.LT:
                return val1 < val2;
            case TokenType.NOTEQUALTO:
                return val1 != val2;
            case TokenType.GEQUAL:
                return val1 >= val2;
            case TokenType.LEQUAL:
                return val1 <= val2;
            default:
                return false;
        }
    }

    private TokenType getRelational(TokenType tokenType)
    {
        switch (tokenType)
        {
            case TokenType.EQUALTO:
            case TokenType.GT:
            case TokenType.LT:
            case TokenType.NOTEQUALTO:
            case TokenType.GEQUAL:
            case TokenType.LEQUAL:
                return tokenType;
            default:
                return TokenType.INVALID; // Assuming INVALID is a defined value for unsupported types
        }
    }

    public void parseStatement()
    {
        var type = tokens[tokenIndex].Type;
        switch (type)
        {
            case TokenType.LET:
                {

                    eat(TokenType.LET);
                    eat(TokenType.ID);
                    eat(TokenType.ASSIGN);
                    var rtValue = parseExpression();
                    symbolTable.CreateVariable(rtValue);
                    eat(TokenType.ENTER);
                    break;
                }

            case TokenType.SET:
                {

                    eat(TokenType.LET);
                    eat(TokenType.ID);
                    eat(TokenType.ASSIGN);
                    var rtValue = parseExpression();
                    symbolTable.UpdateVariable(rtValue);
                    eat(TokenType.ENTER);
                    break;
                }

            case TokenType.PRINT:
                {

                    eat(TokenType.PRINT);
                    string val = parseExprList();
                    Console.WriteLine(val);
                    eat(TokenType.ENTER);
                    break;
                }

            case TokenType.IF:
                {

                    eat(TokenType.IF);
                    var op1 = parseExpression();
                    var operatorType = eat(getRelational(tokens[tokenIndex].Type)).Type;
                    var op2 = parseExpression();
                    bool condition = evaluateCondition(op1, operatorType, op2);
                    if (condition)
                    {
                        eat(TokenType.THEN);
                        parseStatement();
                    }
                    break;
                }

            case TokenType.INPUT:
                {

                    eat(TokenType.INPUT);
                    var variableToken = tokens[tokenIndex];
                    if (variableToken.Type != TokenType.STRING)
                    {
                        Console.WriteLine($"input variable ${variableToken.Value} must be of type string");
                    }
                    advance();
                    while (variableToken.Type != TokenType.ENTER)
                    {
                        var input = Console.ReadLine();
                        var rtValue = new RuntimeValue(ValueType.STRING, variableToken.Value, input ?? "");
                        symbolTable.UpdateVariable(rtValue);
                        if (tokens[tokenIndex].Type != TokenType.COMMA) break;
                        eat(TokenType.COMMA);
                        variableToken = tokens[tokenIndex];
                        advance();
                    }
                    eat(TokenType.ENTER);
                    break;
                }

            case TokenType.GOTO:
                {

                    eat(TokenType.GOTO);
                    var gotoValue = parseExpression();
                    if (gotoValue.Type != ValueType.INT)
                    {
                        Console.WriteLine("GOTO target not an integer");
                        Environment.Exit(1);
                    }
                    pc = gotoValue.IntValue;
                    if (pc == -1)
                    {
                        Console.WriteLine("segfault GOTO target out of bounds");
                        Environment.Exit(1);
                    }
                    eat(TokenType.ENTER);
                    break;
                }

            case TokenType.GOSUB:
                {

                    stack.Push(pc);
                    var gotoValue = parseExpression();
                    if (gotoValue.Type != ValueType.INT)
                    {
                        Console.WriteLine("GOSUB target not an integer");
                        Environment.Exit(1);
                    }
                    pc = gotoValue.IntValue;
                    if (pc == -1)
                    {
                        Console.WriteLine("segfault GOSUB target out of bounds");
                        Environment.Exit(1);
                    }
                    eat(TokenType.ENTER);
                    break;
                }

            case TokenType.RETURN:
                {

                    eat(TokenType.RETURN);
                    if (stack.Count > 0)
                    {
                        pc = stack.Pop();
                    }
                    eat(TokenType.ENTER);
                    break;
                }

            case TokenType.LABEL:
            case TokenType.ENTER:
                // Processing comments, assuming no action is needed here
                break;

            case TokenType.END:
                Environment.Exit(0);
                break;

            default:
                throw new Exception("Unsupported token type.");
        }
    }

    public string parseExprList()
    {
        string expr = string.Empty;
        if (tokens[tokenIndex].Type == TokenType.S_LITERAL)
        {
            expr += tokens[tokenIndex].Value;
            eat(TokenType.S_LITERAL);
        }
        else
        {
            var val = parseExpression();
            expr += val.ToString();
        }

        if (tokens[tokenIndex].Type == TokenType.COMMA)
        {
            eat(TokenType.COMMA);
            expr += ", " + parseExprList();
        }

        return expr;
    }

    public RuntimeValue parseExpression()
    {
        var operand = parseTerm();
        while (tokenIndex < tokens.Count &&
               (tokens[tokenIndex].Type == TokenType.PLUS || tokens[tokenIndex].Type == TokenType.SUB))
        {
            var tokenType = tokens[tokenIndex].Type;
            eat(tokenType);
            var rightOperand = parseTerm();
            operand = tokenType == TokenType.PLUS ? evaluatePlusExpression(operand, rightOperand) : evaluateMinusExpression(operand, rightOperand);
        }
        return operand;
    }

    public RuntimeValue parseTerm()
    {
        var operand = parseFactor();
        while (tokenIndex < tokens.Count &&
               (tokens[tokenIndex].Type == TokenType.MUL || tokens[tokenIndex].Type == TokenType.DIV))
        {
            var tokenType = tokens[tokenIndex].Type;
            eat(tokenType);
            var rightOperand = parseFactor();
            operand = tokenType == TokenType.MUL ? evaluateMulExpression(operand, rightOperand) : evaluateDivExpression(operand, rightOperand);
        }
        return operand;
    }

    public RuntimeValue parseFactor()
    {
        // LABEL
        if (tokens[tokenIndex].Type == TokenType.LABEL)
        {
            var token = eat(TokenType.LABEL);
            var variableName = token.Value;
            var labelPos = new RuntimeValue(ValueType.INT, variableName, intval: getLabelPos(variableName));
            return labelPos;
        }
        else if (tokens[tokenIndex].Type == TokenType.ID)
        {
            var token = eat(TokenType.ID);
            var variableName = token.Value;
            var rtValue = symbolTable.GetSymbol(variableName);
            if (rtValue == null)
            {
                Console.WriteLine($"identifier ${variableName} not found");
            }
            else
            {
                return rtValue;
            }
        }

        // INT
        else if (tokens[tokenIndex].Type == TokenType.INT)
        {
            var numberToken = eat(TokenType.INT);
            var intValue = int.Parse(numberToken.Value);
            var rtValue = new RuntimeValue(ValueType.INT, "", intval: intValue);
            return rtValue;
        }

        // FLOAT
        else if (tokens[tokenIndex].Type == TokenType.FLOAT)
        {
            var numberToken = eat(TokenType.FLOAT);
            var floatValue = float.Parse(numberToken.Value);
            var rtValue = new RuntimeValue(ValueType.FLOAT, "", floatval: floatValue);
            return rtValue;
        }

        // (
        else if (tokens[tokenIndex].Type == TokenType.LEFTPAR)
        {
            eat(TokenType.LEFTPAR);
            var op = parseExpression();
            eat(TokenType.RIGHTPAR);
            return op;
        }
        throw new Exception("Unexpected token in factor.");
    }

    private RuntimeValue evaluatePlusExpression(RuntimeValue leftOperand, RuntimeValue rightOperand)
    {
        switch (leftOperand.Type)
        {
            case ValueType.INT:
                switch (rightOperand.Type)
                {
                    case ValueType.INT:
                        return new RuntimeValue(ValueType.INT, "", intval: leftOperand.IntValue + rightOperand.IntValue);
                    case ValueType.FLOAT:
                        return new RuntimeValue(ValueType.FLOAT, "", floatval: leftOperand.IntValue + rightOperand.FloatValue);
                }
                break;
            case ValueType.FLOAT:
                return new RuntimeValue(ValueType.FLOAT, "", floatval: leftOperand.FloatValue +
                    (rightOperand.Type == ValueType.INT ? rightOperand.IntValue : rightOperand.FloatValue));
            case ValueType.STRING:
                return new RuntimeValue(ValueType.STRING, "", strval: leftOperand.StringValue + rightOperand.StringValue);
        }

        throw new InvalidOperationException($"Operation + not supported between {leftOperand.Type} and {rightOperand.Type}");
    }



    private RuntimeValue evaluateMinusExpression(RuntimeValue leftOperand, RuntimeValue rightOperand)
    {
        switch (leftOperand.Type)
        {
            case ValueType.INT:
                if (rightOperand.Type == ValueType.INT)
                    return new RuntimeValue(ValueType.INT, "", intval: leftOperand.IntValue - rightOperand.IntValue);
                else if (rightOperand.Type == ValueType.FLOAT)
                    return new RuntimeValue(ValueType.FLOAT, "", floatval: leftOperand.IntValue - rightOperand.FloatValue);
                break;
            case ValueType.FLOAT:
                return new RuntimeValue(ValueType.FLOAT, "", floatval: leftOperand.FloatValue -
                    (rightOperand.Type == ValueType.INT ? rightOperand.IntValue : rightOperand.FloatValue));
        }

        throw new InvalidOperationException($"Operation - not supported between {leftOperand.Type} and {rightOperand.Type}");
    }


    private RuntimeValue evaluateMulExpression(RuntimeValue leftOperand, RuntimeValue rightOperand)
    {
        switch (leftOperand.Type)
        {
            case ValueType.INT:
                switch (rightOperand.Type)
                {
                    case ValueType.INT:
                        return new RuntimeValue(ValueType.INT, "", intval: leftOperand.IntValue * rightOperand.IntValue);
                    case ValueType.FLOAT:
                        return new RuntimeValue(ValueType.FLOAT, "", floatval: leftOperand.IntValue * rightOperand.FloatValue);
                }
                break;
            case ValueType.FLOAT:
                return new RuntimeValue(ValueType.FLOAT, "", floatval: leftOperand.FloatValue *
                    (rightOperand.Type == ValueType.INT ? rightOperand.IntValue : rightOperand.FloatValue));
        }

        throw new InvalidOperationException($"Operation * not supported between {leftOperand.Type} and {rightOperand.Type}");
    }

    private RuntimeValue evaluateDivExpression(RuntimeValue leftOperand, RuntimeValue rightOperand)
    {
        switch (leftOperand.Type)
        {
            case ValueType.INT:
                if (rightOperand.Type == ValueType.INT)
                {
                    if (rightOperand.IntValue == 0) throw new DivideByZeroException("Division by zero.");
                    return new RuntimeValue(ValueType.INT, "", intval: leftOperand.IntValue / rightOperand.IntValue);
                }
                else if (rightOperand.Type == ValueType.FLOAT)
                {
                    if (rightOperand.FloatValue == 0) throw new DivideByZeroException("Division by zero.");
                    return new RuntimeValue(ValueType.FLOAT, "", floatval: leftOperand.IntValue / rightOperand.FloatValue);
                }
                break;
            case ValueType.FLOAT:
                if ((rightOperand.Type == ValueType.INT && rightOperand.IntValue == 0) ||
                    (rightOperand.Type == ValueType.FLOAT && rightOperand.FloatValue == 0))
                    throw new DivideByZeroException("Division by zero.");
                return new RuntimeValue(ValueType.FLOAT, "", floatval: leftOperand.FloatValue /
                    (rightOperand.Type == ValueType.INT ? rightOperand.IntValue : rightOperand.FloatValue));
        }

        throw new InvalidOperationException($"Operation / not supported between {leftOperand.Type} and {rightOperand.Type}");
    }





    private RuntimeValue castValue(RuntimeValue rtValue, ValueType to)
    {
        switch (to)
        {
            case ValueType.INT:
                if (rtValue.Type == ValueType.FLOAT)
                {
                    return new RuntimeValue(to, rtValue.Name, intval: (int)rtValue.FloatValue);
                }
                else if (rtValue.Type == ValueType.STRING)
                {
                    int intValue;
                    if (int.TryParse(rtValue.StringValue, out intValue))
                    {
                        return new RuntimeValue(to, rtValue.Name, intval: intValue);
                    }
                }
                else if (rtValue.Type == ValueType.BOOL)
                {
                    return new RuntimeValue(to, rtValue.Name, intval: rtValue.BoolValue ? 1 : 0);
                }
                break;

            case ValueType.FLOAT:
                if (rtValue.Type == ValueType.INT)
                {
                    return new RuntimeValue(to, rtValue.Name, floatval: rtValue.IntValue);
                }
                else if (rtValue.Type == ValueType.STRING)
                {
                    double floatValue;
                    if (double.TryParse(rtValue.StringValue, out floatValue))
                    {
                        return new RuntimeValue(to, rtValue.Name, floatval: floatValue);
                    }
                }
                else if (rtValue.Type == ValueType.BOOL)
                {
                    return new RuntimeValue(to, rtValue.Name, floatval: rtValue.BoolValue ? 1.0 : 0.0);
                }
                break;

            case ValueType.STRING:
                if (rtValue.Type == ValueType.INT)
                {
                    return new RuntimeValue(to, rtValue.Name, strval: rtValue.IntValue.ToString());
                }
                else if (rtValue.Type == ValueType.FLOAT)
                {
                    return new RuntimeValue(to, rtValue.Name, strval: rtValue.FloatValue.ToString());
                }
                else if (rtValue.Type == ValueType.BOOL)
                {
                    return new RuntimeValue(to, rtValue.Name, strval: rtValue.BoolValue.ToString());
                }
                break;

            case ValueType.BOOL:
                if (rtValue.Type == ValueType.INT)
                {
                    return new RuntimeValue(to, rtValue.Name, boolval: rtValue.IntValue != 0);
                }
                else if (rtValue.Type == ValueType.FLOAT)
                {
                    return new RuntimeValue(to, rtValue.Name, boolval: rtValue.FloatValue != 0.0);
                }
                else if (rtValue.Type == ValueType.STRING)
                {
                    bool boolValue;
                    if (bool.TryParse(rtValue.StringValue, out boolValue))
                    {
                        return new RuntimeValue(to, rtValue.Name, boolval: boolValue);
                    }
                }
                break;
        }

        throw new InvalidOperationException($"Cannot cast from {rtValue.Type} to {to}");
    }

}
