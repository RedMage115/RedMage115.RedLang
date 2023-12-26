using System.Diagnostics;
using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedLexer;

public partial class Lexer {
    /// <summary>
    /// Set Ch to the next char, or EOF if no more chars
    /// </summary>
    private void ReadChar() {
        Ch = ReadPosition >= Length ? '\0' : Input[ReadPosition];
        Position = ReadPosition;
        ReadPosition++;
    }
/// <summary>
/// Get the next char without advancing the positions
/// </summary>
/// <returns>The char in ReadPosition or EOF</returns>
    private char PeekChar() {
        return ReadPosition >= Length ? '\0' : Input[ReadPosition];
    }
    
    /// <summary>
    /// Gets the next token from the current position
    /// </summary>
    /// <returns>Token, or ILLEGAL if token is bad</returns>
    public Token NextToken() {
        Token token;
        SkipWhitespace();
        switch (Ch) {
            case '=':
                if (PeekChar() == '=') {
                    var ch = Ch;
                    ReadChar();
                    token = new Token(TokenType.EQ, $"{ch}{Ch}");
                }
                else {
                    token = new Token(TokenType.ASSIGN, Ch);
                }
                break;
            case '+':
                token = new Token(TokenType.PLUS, Ch);
                break;
            case '-':
                token = new Token(TokenType.MINUS, Ch);
                break;
            case '!':
                if (PeekChar() == '=') {
                    var ch = Ch;
                    ReadChar();
                    token = new Token(TokenType.NOT_EQ, $"{ch}{Ch}");
                }
                else {
                    token = new Token(TokenType.BANG, Ch);
                }
                break;
            case '/':
                token = new Token(TokenType.SLASH, Ch);
                break;
            case '*':
                token = new Token(TokenType.ASTRIX, Ch);
                break;
            case '<':
                token = new Token(TokenType.LT, Ch);
                break;
            case '>':
                token = new Token(TokenType.GT, Ch);
                break;
            case ';':
                token = new Token(TokenType.SEMICOLON, Ch);
                break;
            case '(':
                token = new Token(TokenType.LPAREN, Ch);
                break;
            case ')':
                token = new Token(TokenType.RPAREN, Ch);
                break;
            case ',':
                token = new Token(TokenType.COMMA, Ch);
                break;
            case '{':
                token = new Token(TokenType.LBRACE, Ch);
                break;
            case '}':
                token = new Token(TokenType.RBRACE, Ch);
                break;
            case '[':
                token = new Token(TokenType.LBRACKET, Ch);
                break;
            case ']':
                token = new Token(TokenType.RBRACKET, Ch);
                break;
            case '"':
                token = new Token(TokenType.STRING, ReadString());
                break;
            case '\0':
                token = new Token(TokenType.EOF, "");
                break;
            default:
                if (IsLetter(Ch)) {
                    var literal = ReadIdentifier();
                    token = new Token(Token.LookupIdentifier(literal), literal);
                    return token;
                }
                if (IsDigit(Ch)) {
                    token = new Token(TokenType.INT, ReadNumber());
                    return token;
                }
                token = new Token(TokenType.ILLEGAL, "");
                break;
                
        }
        ReadChar();
        return token;
    }

    private string ReadIdentifier() {
        var initialPos = Position;
        while (IsLetter(Ch)) {
            ReadChar();
        }

        return Input[initialPos..Position];
    }
    
    private string ReadNumber() {
        var initialPos = Position;
        while (IsDigit(Ch)) {
            ReadChar();
        }
        return Input[initialPos..Position];
    }
    
    private string ReadString() {
        var initialPos = Position+1;
        if (PeekChar() is '"' or '\0') {
            return "";
        }
        ReadChar();
        while (true) {
            ReadChar();
            if (Ch is '"' or '\0') {
                break;
            }
        }
        return Input[initialPos..Position];
    }

    private void SkipWhitespace() {
        while (Ch is ' ' or '\t' or '\n' or '\r') {
            ReadChar();
        }
    }
    
    private static bool IsLetter(char ch) {
        return ch is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_';
    }
    
    private static bool IsDigit(char ch) {
        return ch is >= '0' and <= '9';
    }

}