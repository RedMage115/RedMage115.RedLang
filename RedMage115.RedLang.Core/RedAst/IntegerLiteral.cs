using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class IntegerLiteral : Expression{

    public Token Token { get; set; }
    public long Value { get; set; }

    public IntegerLiteral(Token token, long value) {
        Token = token;
        Value = value;
    }

    public string GetTokenLiteral() {
        return Token.Literal;
    }

    public string GetNodeTypeString() {
        return $"<Integer Literal [{Value}]>";
    }
}