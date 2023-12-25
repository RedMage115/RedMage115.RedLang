using System.Diagnostics;
using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedToken;
using Boolean = RedMage115.RedLang.Core.RedAst.Boolean;

namespace RedMage115.RedLang.Core.RedParser;

public partial class Parser {
    
    private Expression? ParseExpression(Precedence precedence) {
        if (LogLevel > 0) {
            Debug.WriteLine("BEGIN PARSE EXPRESSION");
        }
        if (!PrefixParseFunctions.TryGetValue(CurrentToken.Type, out var prefix)) {
            NoPrefixParseFunctionError(CurrentToken.Type);
            return null;
        }
        var leftExpression = prefix();
        
        while (!PeekTokenIs(TokenType.SEMICOLON) && precedence < PeekPrecedence()) {
            if (leftExpression is null) {
                return null;
            }
            if (!InfixParseFunctions.TryGetValue(PeekToken.Type, out var infix)) {
                if (LogLevel > 0) {
                    Debug.WriteLine("END PARSE EXPRESSION");
                }
                return leftExpression;
            }
            NextToken();
            leftExpression = infix(leftExpression);
        }
        if (LogLevel > 0) {
            Debug.WriteLine("END PARSE EXPRESSION");
        }
        return leftExpression;
    }

    private Expression ParseIdentifier() {
        if (LogLevel > 0) {
            Debug.WriteLine("PARSE INDENT");
        }
        return new Identifier(CurrentToken, CurrentToken.Literal);
    }

    private Expression? ParseIntegerLiteral() {
        if (LogLevel > 0) {
            Debug.WriteLine("PARSE INT LITERAL");
        }
        if (!long.TryParse(CurrentToken.Literal, out var value)) {
            Errors.Add($"could not parse {CurrentToken.Literal} as integer");
            return null;
        }
        return new IntegerLiteral(CurrentToken, value);
    }

    private Expression? ParsePrefixExpression() {
        if (LogLevel > 0) {
            Debug.WriteLine("BEGIN PARSE PREFIX EXPRESSION");
        }
        var token = CurrentToken;
        NextToken();
        var right = ParseExpression(Precedence.PREFIX);
        if (right is null) {
            Errors.Add($"failed to parse right side of {token.Literal}{CurrentToken.Literal}");
            return null;
        }
        if (LogLevel > 0) {
            Debug.WriteLine("END PARSE PREFIX EXPRESSION");
        }
        return new PrefixExpression(token, token.Literal, right);
    }

    private Expression? ParseInfixExpression(Expression left) {
        if (LogLevel > 0) {
            Debug.WriteLine("BEGIN PARSE INFIX EXPRESSION");
        }
        var token = CurrentToken;
        var precedence = CurrentPrecedence();
        NextToken();

        Expression? right;
        if (CurrentToken.Literal == "+") {
            right = ParseExpression(precedence);
        }
        else {
            right = ParseExpression(precedence);
        }
        
        
        if (right is null) {
            Errors.Add($"Could not parse right side of {left.GetTokenLiteral()} {token.Literal} {CurrentToken.Literal}");
            return null;
        }
        if (LogLevel > 0) {
            Debug.WriteLine("END PARSE INFIX EXPRESSION");
        }
        return new InfixExpression(token, left, token.Literal, right);
    }

    private Expression ParseBoolean() {
        return new Boolean(CurrentToken, CurrentTokenIs(TokenType.TRUE));
    }

    private Expression? ParseGroupedExpression() {
        NextToken();
        var exp = ParseExpression(Precedence.LOWEST);
        if (!ExpectPeek(TokenType.RPAREN)) {
            return null;
        }

        return exp;
    }

    private Expression? ParseIfExpression() {
        var token = CurrentToken;
        if (!ExpectPeek(TokenType.LPAREN)) {
            return null;
        }
        NextToken();
        var condition = ParseExpression(Precedence.LOWEST);
        if (condition is null) {
            return null;
        }
        if (!ExpectPeek(TokenType.RPAREN)) {
            return null;
        }
        if (!ExpectPeek(TokenType.LBRACE)) {
            return null;
        }

        var consequence = ParseBlockStatement();
        BlockStatement? alt = null;
        if (PeekTokenIs(TokenType.ELSE)) {
            NextToken();
            if (!ExpectPeek(TokenType.LBRACE)) {
                return null;
            }

            alt = ParseBlockStatement();
        }
        

        return new IfExpression(token, condition, consequence, alt);
    }

    private Expression? ParseFunctionLiteral() {
        var token = CurrentToken;
        if (!ExpectPeek(TokenType.LPAREN)) {
            return null;
        }
        var parameters = ParseFunctionParameters();
        if (!ExpectPeek(TokenType.LBRACE)) {
            return null;
        }
        if (parameters is null) {
            return null;
        }
        var body = ParseBlockStatement();
        return new FunctionLiteral(token, parameters, body);
    }

    private List<Identifier>? ParseFunctionParameters() {
        var identList = new List<Identifier>();
        if (PeekTokenIs(TokenType.RPAREN)) {
            NextToken();
            return identList;
        }
        NextToken();
        identList.Add(new Identifier(CurrentToken, CurrentToken.Literal));
        while (PeekTokenIs(TokenType.COMMA)) {
            NextToken();
            NextToken();
            identList.Add(new Identifier(CurrentToken, CurrentToken.Literal));
        }
        if (!ExpectPeek(TokenType.RPAREN)) {
            return null;
        }
        return identList;
    }

    private Expression ParseCallExpression(Expression function) {
        var token = CurrentToken;
        var args = ParseCallArguments();
        return new CallExpression(token, function, args);
    }
    
    private List<Expression>? ParseCallArguments() {
        var argList = new List<Expression>();
        if (PeekTokenIs(TokenType.RPAREN)) {
            NextToken();
            return argList;
        }
        NextToken();
        var initialExpression = ParseExpression(Precedence.LOWEST);
        if (initialExpression is not null) {
            argList.Add(initialExpression);
        }
        while (PeekTokenIs(TokenType.COMMA)) {
            NextToken();
            NextToken();
            var nextExpression = ParseExpression(Precedence.LOWEST);
            if (nextExpression is null) {
                break;
            }
            argList.Add(nextExpression);
        }
        if (!ExpectPeek(TokenType.RPAREN)) {
            return null;
        }
        return argList;
    }

}