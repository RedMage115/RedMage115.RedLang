using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class IfExpression : Expression {
    public Token Token { get; set; }
    public Expression Condition { get; set; }
    public BlockStatement Consequence { get; set; }
    public BlockStatement? Alternative { get; set; }

    public IfExpression(Token token, Expression condition, BlockStatement consequence, BlockStatement? alternative) {
        Token = token;
        Condition = condition;
        Consequence = consequence;
        Alternative = alternative;
    }


    public string GetTokenLiteral() {
        return Token.Literal;
    }

    public string GetNodeTypeString() {
        var nodeString =
            $"<If Statement [({Condition.GetRawExpression()}) {{{Consequence.GetNodeTypeString()}}}";
        if (Alternative is not null) {
            nodeString += $" else {{{Alternative?.GetNodeTypeString()}}}]>";
        }
        else {
            nodeString += $"]>";
        }
        return nodeString;
    }

    public string GetRawExpression() {
        var rawExpression = $"if ({Condition.GetRawExpression()}) {{{Consequence.GetRawStatement()}}}";
        if (Alternative is not null) {
            rawExpression += $" else {{{Alternative?.GetRawStatement() ?? "NULL"}}}";
        }
        return rawExpression;
    }
}