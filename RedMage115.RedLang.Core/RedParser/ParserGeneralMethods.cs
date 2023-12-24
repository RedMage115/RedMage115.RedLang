using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedParser;

public partial class Parser {
    public Program ParseProgram() {
        var program = new Program();

        while (CurrentToken.Type != TokenType.EOF) {
            var statement = ParseStatement();
            if (statement is not null) {
                program.Statements.Add(statement);
            }
            NextToken();
        }

        return program;
    }
    
    private void PeekError(TokenType tokenType) {
        Errors.Add($"expected next token to be: {tokenType}, got {PeekToken} instead");
    }

    private void NoPrefixParseFunctionError(TokenType tokenType) {
        Errors.Add($"no prefix parse function found for {tokenType}");
    }
}