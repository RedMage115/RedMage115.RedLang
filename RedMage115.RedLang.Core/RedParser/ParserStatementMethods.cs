using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedParser;

public partial class Parser {
    private Statement? ParseStatement() {
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
        var token = CurrentToken;
        if (!ExpectPeek(TokenType.IDENT)) return null;
        var name = CurrentToken;
        if (!ExpectPeek(TokenType.ASSIGN)) return null;
        while (!CurrentTokenIs(TokenType.SEMICOLON)) {
            //TODO Handle value here
            NextToken();
        }
        var statement = new LetStatement(token, new Identifier(name, name.Literal), null);
        return statement;
    }

    private ReturnStatement? ParseReturnStatement() {
        var token = CurrentToken;
        NextToken();
        while (!CurrentTokenIs(TokenType.SEMICOLON)) {
            //TODO Handle value here
            NextToken();
        }

        var statement = new ReturnStatement(token, null);
        return statement;
    }

    private ExpressionStatement ParseExpressionStatement() {
        var token = CurrentToken;
        var expression = ParseExpression(Precedence.LOWEST);
        if (PeekTokenIs(TokenType.SEMICOLON)) {
            NextToken();
        }
        return new ExpressionStatement(token, expression);
    }
}