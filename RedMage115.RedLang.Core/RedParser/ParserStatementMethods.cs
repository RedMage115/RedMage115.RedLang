using System.Diagnostics;
using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedParser;

public partial class Parser {
    private Statement? ParseStatement() {
        if (LogLevel > 0) {
            Debug.WriteLine("PARSE STATEMENT");
        }
        switch (CurrentToken.Type) {
            case TokenType.LET:
                return ParseLetStatement();
            case TokenType.RETURN:
                return ParseReturnStatement();
            default:
                return ParseExpressionStatement();
        }
    }

    private LetStatement? ParseLetStatement() {
        if (LogLevel > 0) {
            Debug.WriteLine("BEGIN PARSE LET STATEMENT");
        }
        var token = CurrentToken;
        if (!ExpectPeek(TokenType.IDENT)) return null;
        var name = CurrentToken;
        if (!ExpectPeek(TokenType.ASSIGN)) return null;
        NextToken();
        var value = ParseExpression(Precedence.LOWEST);
        while (!CurrentTokenIs(TokenType.SEMICOLON) && !CurrentTokenIs(TokenType.EOF)) {
            NextToken();
        }
        var statement = new LetStatement(token, new Identifier(name, name.Literal), value);
        if (LogLevel > 0) {
            Debug.WriteLine("END PARSE LET STATEMENT");
        }
        return statement;
    }

    private ReturnStatement? ParseReturnStatement() {
        if (LogLevel > 0) {
            Debug.WriteLine("BEGIN PARSE RETURN STATEMENT");
        }
        var token = CurrentToken;
        NextToken();

        var returnExpression = ParseExpression(Precedence.LOWEST);
        while (!CurrentTokenIs(TokenType.SEMICOLON) && !CurrentTokenIs(TokenType.EOF)) {
            NextToken();
        }

        var statement = new ReturnStatement(token, returnExpression);
        if (LogLevel > 0) {
            Debug.WriteLine("END PARSE RETURN STATEMENT");
        }
        return statement;
    }

    private ExpressionStatement ParseExpressionStatement() {
        if (LogLevel > 0) {
            Debug.WriteLine("BEGIN PARSE EXPRESSION STATEMENT");
        }
        var token = CurrentToken;
        var expression = ParseExpression(Precedence.LOWEST);
        if (PeekTokenIs(TokenType.SEMICOLON)) {
            NextToken();
        }
        if (LogLevel > 0) {
            Debug.WriteLine("END PARSE EXPRESSION STATEMENT");
        }
        return new ExpressionStatement(token, expression);
    }

    private BlockStatement ParseBlockStatement() {
        var statement = new BlockStatement(CurrentToken);
        NextToken();
        while (!CurrentTokenIs(TokenType.RBRACE) && !CurrentTokenIs(TokenType.EOF)) {
            var nextStatement = ParseStatement();
            if (nextStatement is not null) {
                statement.Statements.Add(nextStatement);
            }
            NextToken();
        }
        return statement;
    }
}