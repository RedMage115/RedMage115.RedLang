using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class ExpressionStatement : Statement {

    public Token Token { get; set; }
    public Expression? Expression { get; set; }

    public ExpressionStatement(Token token, Expression? expression) {
        Token = token;
        Expression = expression;
    }

    public string GetTokenLiteral() {
        return Token.Literal;
    }

    public string GetNodeTypeString() {
        return $"<Expression Statement [{Expression?.GetNodeTypeString()}]>";
    }

    public string GetRawStatement() {
        return $"{Expression?.GetRawExpression()}";
    }
}