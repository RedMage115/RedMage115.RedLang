using System.Diagnostics;
using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedParser;

public partial class Parser {
    public Program ParseProgram() {
        if (LogLevel > 0) {
            Debug.WriteLine("BEGIN PARSE PROGRAM");
        }
        var program = new Program();

        while (CurrentToken.Type != TokenType.EOF) {
            var statement = ParseStatement();
            if (statement is not null) {
                program.Statements.Add(statement);
            }
            NextToken();
        }
        if (LogLevel > 0) {
            Debug.WriteLine("END PARSE PROGRAM");
        }
        return program;
    }
    
    private void InitParseMaps() {
        PrefixParseFunctions.Add(TokenType.IDENT, ParseIdentifier);
        PrefixParseFunctions.Add(TokenType.INT, ParseIntegerLiteral);
        PrefixParseFunctions.Add(TokenType.BANG, ParsePrefixExpression);
        PrefixParseFunctions.Add(TokenType.MINUS, ParsePrefixExpression);
        PrefixParseFunctions.Add(TokenType.TRUE, ParseBoolean);
        PrefixParseFunctions.Add(TokenType.FALSE, ParseBoolean);
        PrefixParseFunctions.Add(TokenType.LPAREN, ParseGroupedExpression);
        PrefixParseFunctions.Add(TokenType.IF, ParseIfExpression);
        PrefixParseFunctions.Add(TokenType.FUNCTION, ParseFunctionLiteral);
        PrefixParseFunctions.Add(TokenType.STRING, ParseStringLiteral);
        
        InfixParseFunctions.Add(TokenType.PLUS, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.MINUS, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.SLASH, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.ASTRIX, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.EQ, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.NOT_EQ, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.LT, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.GT, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.LPAREN, ParseCallExpression);
    }
    
    private void PeekError(TokenType tokenType) {
        Errors.Add($"expected next token to be: {tokenType}, got {PeekToken} instead");
    }

    private void NoPrefixParseFunctionError(TokenType tokenType) {
        Errors.Add($"no prefix parse function found for {tokenType}");
    }
}