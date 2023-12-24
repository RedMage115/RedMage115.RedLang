using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class ReturnStatement : Statement {

    public Token Token { get; set; }
    public Expression? ReturnValue { get; set; }

    public ReturnStatement(Token token, Expression? returnValue) {
        Token = token;
        ReturnValue = returnValue;
    }
    
    public string GetTokenLiteral() {
        return Token.Literal;
    }

    public string GetNodeTypeString() {
        return $"<Return Statement [{Token.Literal} {ReturnValue?.GetTokenLiteral()}] >";
    }

    public string GetRawStatement() {
        return $"return {ReturnValue?.GetRawExpression()}";
    }
}