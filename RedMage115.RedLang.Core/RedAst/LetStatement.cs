using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class LetStatement : Statement {

    public Token Token { get; set; }
    public Identifier Name { get; set; }
    public Expression? Value { get; set; }

    public LetStatement(Token token, Identifier name, Expression? value) {
        Token = token;
        Name = name;
        Value = value;
    }

    public string GetTokenLiteral() {
        return Token.Literal;
    }
    
    public string GetNodeTypeString() {
        return $"<Let Statement [{Token.Literal} {Name.GetTokenLiteral()} {Value?.GetTokenLiteral()}] >";
    }
}