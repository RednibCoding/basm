

namespace Basm;

public enum TokenType
{
    ID,
    // Keywords
    PRINT,
    IF,
    GOTO,
    INPUT,
    LET,
    SET,
    GOSUB,
    RETURN,
    CLEAR,
    LIST,
    RUN,
    END,
    THEN,
    ENTER,

    // Operators
    PLUS,
    SUB,
    MUL,
    DIV,
    MOD,
    INCR,
    DECR,

    // Assignment
    ASSIGN,
    PLUS_ASSIGN,
    SUB_ASSIGN,
    MUL_ASSIGN,
    DIV_ASSIGN,
    MOD_ASSIGN,

    // Relational
    EQUALTO,
    GT,
    LT,
    NOTEQUALTO,
    GEQUAL,
    LEQUAL,

    // Logical
    AND_L,
    OR_L,
    NOT_L,

    // Bitwise
    AND,
    OR,
    XOR,
    COMP,
    SHIFTL,
    SHIFTR,

    // Others
    LEFTPAR,
    RIGHTPAR,
    LEFTCUR,
    RIGHTCUR,
    COMMA,
    LEFTSQR,
    RIGHTSQR,
    POINTR,
    QUOTE,
    APST,
    SEMICOLON,
    DOT,
    INT,
    FLOAT,
    BOOL,
    STRING,
    FUNC,
    LABEL,

    INVALID,
}
