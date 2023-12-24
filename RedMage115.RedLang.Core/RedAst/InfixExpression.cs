using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class InfixExpression : Expression {

    public Token Token { get; set; }
    public Expression Left { get; set; }
    public string Operator { get; set; }
    public Expression Right { get; set; }

    public InfixExpression(Token token, Expression left, string @operator, Expression right) {
        Token = token;
        Left = left;
        Operator = @operator;
        Right = right;
    }

    public string GetTokenLiteral() {
        return Token.Literal;
    }

    public string GetNodeTypeString() {
        return $"<Infix Expression [{Left.GetNodeTypeString()} {Operator} {Right.GetNodeTypeString()}]>";
    }

    public string GetRawExpression() {
        return $"{Left.GetRawExpression()}{Operator}{Right.GetRawExpression()}";
    }
}