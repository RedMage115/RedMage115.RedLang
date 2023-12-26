using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class HashLiteral : Expression {
    public Token Token { get; set; }
    public Dictionary<Expression, Expression> Pairs { get; set; }
    
    public HashLiteral(Token token, Dictionary<Expression, Expression> pairs) {
        Token = token;
        Pairs = pairs;
    }

    public HashLiteral(Token token) {
        Token = token;
        Pairs = [];
    }

    public string GetTokenLiteral() {
        return Token.Literal;
    }

    public string GetNodeTypeString() {
        return $"<Hash Literal [{Pairs.Aggregate("{", (s, pair) => s+=$"{pair.Key.GetNodeTypeString()}:{pair.Value.GetNodeTypeString()} ").Trim() + "}"}]>";
    }

    public string GetRawExpression() {
        return $"{Pairs.Aggregate("{", (s, pair) => s+=$"{pair.Key.GetRawExpression()}:{pair.Value.GetRawExpression()} ").Trim() + "}"}";
    }
}