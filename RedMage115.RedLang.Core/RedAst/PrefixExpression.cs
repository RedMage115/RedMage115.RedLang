using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class PrefixExpression : Expression {
    public Token Token { get; set; }
    public string Operator { get; set; }
    public Expression Right { get; set; }

    public PrefixExpression(Token token, string @operator, Expression right) {
        Token = token;
        Operator = @operator;
        Right = right;
    }

    public string GetTokenLiteral() {
        return Token.Literal;
    }

    public string GetNodeTypeString() {
        return $"<Prefix Expression [{Operator}{Right.GetRawExpression()}]>";
    }

    public string GetRawExpression() {
        return $"{Operator}{Right.GetRawExpression()}";
    }
}