namespace RedMage115.RedLang.Core.RedToken;

public enum TokenType {
    //Special
    ILLEGAL,
    EOF,
    
    IDENT,
    //Numbers
    INT,
    
    //Types
    STRING,
    
    //Operators
    ASSIGN,
    PLUS,
    MINUS,
    BANG,
    SLASH,
    ASTRIX,
    
    LT,
    GT,
    
    COMMA,
    SEMICOLON,
    
    EQ,
    NOT_EQ,
    
    LPAREN,
    RPAREN,
    LBRACE,
    RBRACE,
    LBRACKET,
    RBRACKET,
    
    //Keywords
    FUNCTION,
    LET,
    TRUE,
    FALSE,
    IF,
    ELSE,
    RETURN
}