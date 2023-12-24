namespace RedMage115.RedLang.Core.RedAst;

public class Program : Node {
    public List<Statement> Statements { get; } = [];
    
    public string GetTokenLiteral() {
        return Statements.First().GetTokenLiteral();
    }
    
    public string GetNodeTypeString() {
        return Statements.Aggregate(string.Empty, (current, statement) => current + (statement.GetNodeTypeString() + " "));
    }
}