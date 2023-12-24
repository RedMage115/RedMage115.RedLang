using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class CallExpression : Expression {
    public Token Token { get; set; }
    public Expression Function { get; set; }
    public List<Expression> Arguments { get; set; }

    public CallExpression(Token token, Expression function, List<Expression>? arguments) {
        Token = token;
        Function = function;
        Arguments = arguments ?? [];
    }
    
    public string GetArgumentListRaw() {
        if (Arguments.Count == 0) {
            return "";
        } 
        if (Arguments.Count == 1) {
            return Arguments.First().GetRawExpression();
        }

        return Arguments.Aggregate(string.Empty,
            (s, identifier) => s += identifier.GetRawExpression() + ", ").Trim().Trim(',');
    }
    
    public string GetArgumentListNodes() {
        if (Arguments.Count == 0) {
            return "";
        } 
        if (Arguments.Count == 1) {
            return Arguments.First().GetNodeTypeString();
        }

        return Arguments.Aggregate(string.Empty,
            (s, identifier) => s += identifier.GetNodeTypeString() + " ");
    }

    public string GetTokenLiteral() {
        return Token.Literal;
    }

    public string GetNodeTypeString() {
        return $"<Call Expression [{Function.GetNodeTypeString()} ({GetArgumentListNodes()})]>";
    }

    public string GetRawExpression() {
        return $"{Function.GetRawExpression()} ({GetArgumentListRaw()})";
    }
}