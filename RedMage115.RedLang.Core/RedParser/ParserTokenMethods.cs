using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedParser;

public partial class Parser {
    private void NextToken() {
        CurrentToken = PeekToken;
        PeekToken = Lexer.NextToken();
    }

    private bool CurrentTokenIs(TokenType tokenType) {
        return CurrentToken.Type == tokenType;
    }

    private Precedence CurrentPrecedence() {
        return PrecedenceDictionary.GetValueOrDefault(CurrentToken.Type, Precedence.LOWEST);
    }
    
    private bool PeekTokenIs(TokenType tokenType) {
        return PeekToken.Type == tokenType;
    }

    private Precedence PeekPrecedence() {
        return PrecedenceDictionary.GetValueOrDefault(PeekToken.Type, Precedence.LOWEST);
    }
    
    private bool ExpectPeek(TokenType tokenType) {
        if (!PeekTokenIs(tokenType)) {
            PeekError(tokenType);
            return false;
        }
        NextToken();
        return true;
    }
}