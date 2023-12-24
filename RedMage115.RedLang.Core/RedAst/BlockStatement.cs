using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedAst;

public class BlockStatement : Statement {

    public Token Token { get; set; }
    public List<Statement> Statements { get; set; } = [];

    public BlockStatement(Token token) {
        Token = token;
    }


    public string GetTokenLiteral() {
        return Token.Literal;
    }

    public string GetNodeTypeString() {
        return $"<Block Statement [{Statements.First().GetRawStatement()}] >";
    }

    public string GetRawStatement() {
        return Statements.Aggregate(string.Empty,
            (s, statement) => s += statement.GetRawStatement());
    }
}