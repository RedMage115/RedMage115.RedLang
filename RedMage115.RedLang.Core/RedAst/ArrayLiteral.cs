using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class ArrayLiteral : Expression {
    public Token Token { get; set; }
    public List<Expression> Elements { get; set; }
    public ArrayLiteral(Token token) {
        Token = token;
        Elements = [];
    }
    public ArrayLiteral(Token token, List<Expression> elements) {
        Token = token;
        Elements = elements;
    }

    public string GetTokenLiteral() {
        return Token.Literal;
    }

    public string GetNodeTypeString() {
        return $"<Array Literal [{Elements.Aggregate(string.Empty, (s, expression) => s+=expression.GetNodeTypeString() + ", ").Trim().Trim(',')}]>";
    }

    public string GetRawExpression() {
        return $"[{Elements.Aggregate(string.Empty, (s, expression) => s+=expression.GetRawExpression() + ", ").Trim().Trim(',')}]";
    }
}