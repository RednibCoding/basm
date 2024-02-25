namespace Basm;

public class Lexer
{
    public List<Token> GetTokens(string input)
    {
        List<Token> tokens = new List<Token>();
        string token = string.Empty;
        TokenType retType;

        // If set, treat the input as code else it could be comment or string literal
        bool isCode = true;

        // Try to match tokens character by character
        for (int i = 0; i < input.Length; i++)
        {
            // Skip comments
            if (input[i] == ';')
            {
                while (i < input.Length && input[i] != '\n')
                {
                    i++;
                }
                continue;
            }

            // Skip if whitespace
            if (char.IsWhiteSpace(input[i]) && isCode) continue;

            // If the first character of the token is alphabet or underscore
            if ((char.IsLetter(input[i]) || input[i] == '_') && isCode)
            {
                while (i < input.Length && (char.IsLetterOrDigit(input[i]) || input[i] == '_'))
                {
                    token += input[i++];
                }

                if (i < input.Length && input[i] == ':')
                {
                    // LABEL
                    tokens.Add(new Token(TokenType.LABEL, token));
                    if (tokens.Count > 1)
                    {
                        Console.WriteLine("label must appear on a seperate line");
                        Environment.Exit(1);
                    }
                    token = string.Empty;
                    i--; // Adjust for the increment in the loop
                }
                else
                {
                    Token? last = null;
                    if (tokens.Count > 0)
                    {
                        last = tokens.Last();
                    }
                    if (last != null && last.Type == TokenType.GOTO)
                    {
                        retType = TokenType.LABEL;
                    }
                    else
                    {
                        retType = GetType(token);
                    }
                    tokens.Add(new Token(retType != TokenType.INVALID ? retType : TokenType.ID, token));
                    token = string.Empty;
                    i--; // Adjust for the increment in the loop
                }

            }

            // If the first character is a number, it must be a float or int
            else if (char.IsDigit(input[i]) && isCode)
            {
                var hasDot = false;
                while (i < input.Length && (char.IsDigit(input[i]) || input[i] == '.'))
                {
                    if (input[i] == '.')
                    {
                        if (hasDot)
                        {
                            Console.WriteLine("number can only have one dot");
                            Environment.Exit(1);
                        }
                        hasDot = true;
                    }
                    token += input[i++];
                }

                tokens.Add(new Token(hasDot ? TokenType.FLOAT : TokenType.INT, token));
                token = string.Empty;
                i--; // Adjust for the increment in the loop
            }

            // String and character literals
            else if (input[i] == '\"')
            {
                i++; // Skip the initial quote
                while (i < input.Length && input[i] != '\"')
                {
                    token += input[i++];
                }

                tokens.Add(new Token(TokenType.S_LITERAL, token));
                token = string.Empty;
            }

            // If the first character is a special character
            else if (isPunctuation(input[i]) && input[i] != '\'' && input[i] != '\"' && isCode)
            {
                token += input[i];
                if (input.Length > i + 1 && isPunctuation(input[i + 1]))
                {
                    token += input[++i];
                }
                TokenType type = GetType(token);
                if (type != TokenType.INVALID)
                {
                    tokens.Add(new Token(type, token));
                    token = string.Empty;
                }
            }
        }

        // Finalize by adding a token indicating the end of input
        tokens.Add(new Token(TokenType.ENTER, "CR"));

        return tokens;
    }

    private TokenType GetType(string token)
    {
        switch (token)
        {
            // Keywords
            case "PRINT": return TokenType.PRINT;
            case "IF": return TokenType.IF;
            case "THEN": return TokenType.THEN;
            case "GOTO": return TokenType.GOTO;
            case "INPUT": return TokenType.INPUT;
            case "LET": return TokenType.LET;
            case "GOSUB": return TokenType.GOSUB;
            case "RETURN": return TokenType.RETURN;
            case "CLEAR": return TokenType.CLEAR;
            case "LIST": return TokenType.LIST;
            case "RUN": return TokenType.RUN;
            case "END": return TokenType.END;

            // Operators
            case "+": return TokenType.PLUS;
            case "-": return TokenType.SUB;
            case "*": return TokenType.MUL;
            case "/": return TokenType.DIV;
            case "%": return TokenType.MOD;
            case "++": return TokenType.INCR;
            case "--": return TokenType.DECR;

            // Assignment
            case "=": return TokenType.ASSIGN;
            case "+=": return TokenType.PLUS_ASSIGN;
            case "-=": return TokenType.SUB_ASSIGN;
            case "*=": return TokenType.MUL_ASSIGN;
            case "/=": return TokenType.DIV_ASSIGN;
            case "%=": return TokenType.MOD_ASSIGN;

            // Relational
            case "==": return TokenType.EQUALTO;
            case ">": return TokenType.GT;
            case "<": return TokenType.LT;
            case "!=": return TokenType.NOTEQUALTO;
            case ">=": return TokenType.GEQUAL;
            case "<=": return TokenType.LEQUAL;

            // Miscellaneous
            case ",": return TokenType.COMMA;

            default: return TokenType.INVALID;
        }
    }

    private bool isPunctuation(char ch)
    {
        return "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~".Contains(ch);
    }

}
