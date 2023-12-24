using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedParser;

public partial class Parser {

    private void InitParseMaps() {
        PrefixParseFunctions.Add(TokenType.IDENT, ParseIdentifier);
        PrefixParseFunctions.Add(TokenType.INT, ParseIntegerLiteral);
        PrefixParseFunctions.Add(TokenType.BANG, ParsePrefixExpression);
        PrefixParseFunctions.Add(TokenType.MINUS, ParsePrefixExpression);
        
        InfixParseFunctions.Add(TokenType.PLUS, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.MINUS, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.SLASH, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.ASTRIX, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.EQ, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.NOT_EQ, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.LT, ParseInfixExpression);
        InfixParseFunctions.Add(TokenType.GT, ParseInfixExpression);
    }
    
    private Expression? ParseExpression(Precedence precedence) {
        if (!PrefixParseFunctions.TryGetValue(CurrentToken.Type, out var prefix)) {
            NoPrefixParseFunctionError(CurrentToken.Type);
            return null;
        }
        var leftExpression = prefix();
        
        while (!PeekTokenIs(TokenType.SEMICOLON) && precedence < PeekPrecedence()) {
            if (!InfixParseFunctions.TryGetValue(PeekToken.Type, out var infix)) {
                return leftExpression;
            }
            NextToken();
            if (leftExpression is null) {
                return null;
            }
            leftExpression = infix(leftExpression);
        }

        return leftExpression;
    }

    private Expression? ParseIdentifier() {
        return new Identifier(CurrentToken, CurrentToken.Literal);
    }

    private Expression? ParseIntegerLiteral() {
        if (!long.TryParse(CurrentToken.Literal, out var value)) {
            Errors.Add($"could not parse {CurrentToken.Literal} as integer");
            return null;
        }
        return new IntegerLiteral(CurrentToken, value);
    }

    private Expression? ParsePrefixExpression() {
        var token = CurrentToken;
        NextToken();
        var right = ParseExpression(Precedence.PREFIX);
        if (right is null) {
            Errors.Add($"failed to parse right side of {token.Literal}{CurrentToken.Literal}");
            return null;
        }
        return new PrefixExpression(token, CurrentToken.Literal, right);
    }

    private Expression? ParseInfixExpression(Expression left) {
        var token = CurrentToken;
        var precedence = CurrentPrecedence();
        NextToken();
        var right = ParseExpression(precedence);
        if (right is null) {
            Errors.Add($"Could not parse right side of {left.GetTokenLiteral()} {token.Literal} {CurrentToken.Literal}");
            return null;
        }
        return new InfixExpression(token, left, token.Literal, right);
    }

}