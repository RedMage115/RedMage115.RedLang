using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class FunctionLiteral : Expression {
    public Token Token { get; set; }
    public List<Identifier> Parameters { get; set; }
    public BlockStatement Body { get; set; }

    public FunctionLiteral(Token token, BlockStatement body) {
        Token = token;
        Parameters = new List<Identifier>();
        Body = body;
    }
    
    public FunctionLiteral(Token token, List<Identifier> parameters, BlockStatement body) {
        Token = token;
        Parameters = parameters;
        Body = body;
    }


    public string GetParameterListRaw() {
        if (Parameters.Count == 0) {
            return "";
        } 
        if (Parameters.Count == 1) {
            return Parameters.First().GetRawExpression();
        }

        return Parameters.Aggregate(string.Empty,
            (s, identifier) => s += identifier.GetRawExpression() + ", ").Trim().Trim(',');
    }
    
    public string GetParameterListNodes() {
        if (Parameters.Count == 0) {
            return "";
        } 
        if (Parameters.Count == 1) {
            return Parameters.First().GetNodeTypeString();
        }

        return Parameters.Aggregate(string.Empty,
            (s, identifier) => s += identifier.GetNodeTypeString() + " ");
    }
    

    public string GetTokenLiteral() {
        return Token.Literal;
    }

    public string GetNodeTypeString() {
        return $"<Function Literal [({GetParameterListNodes()}) {{{Body.GetNodeTypeString()}}}]>";
    }

    public string GetRawExpression() {
        return $"fn ({GetParameterListRaw()}) {{{Body.GetRawStatement()}}}";
    }
}