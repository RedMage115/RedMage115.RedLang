using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class IndexExpression : Expression {
    public Token Token { get; set; }
    public Expression Left { get; set; }
    public Expression Index { get; set; }
    public IndexExpression(Token token, Expression left, Expression index) {
        Token = token;
        Left = left;
        Index = index;
    }

    public string GetTokenLiteral() {
        return Token.Literal;
    }

    public string GetNodeTypeString() {
        return $"<Index Expression [{Left.GetNodeTypeString()}[{Index.GetNodeTypeString()}]]>";
    }

    public string GetRawExpression() {
        return $"{Left.GetRawExpression()}[{Index.GetRawExpression()}]";
    }
}