using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedParser;

public partial class Parser {
    private Lexer Lexer { get; set; }
    private LogLevel LogLevel { get; }
    public List<string> Errors { get; } = [];
    private Token CurrentToken { get; set; } = null!;
    private Token PeekToken { get; set; } = null!;

    private Dictionary<TokenType, Func<Expression?>> PrefixParseFunctions { get; } = new();
    private Dictionary<TokenType, Func<Expression, Expression?>> InfixParseFunctions { get; } = new();
    private Dictionary<TokenType, Precedence> PrecedenceDictionary { get; } = new() {
        { TokenType.EQ, Precedence.EQUALS },
        { TokenType.NOT_EQ, Precedence.EQUALS },
        { TokenType.LT, Precedence.LESS_GREATER },
        { TokenType.GT, Precedence.LESS_GREATER },
        { TokenType.PLUS, Precedence.SUM },
        { TokenType.MINUS, Precedence.SUM },
        { TokenType.SLASH, Precedence.PRODUCT },
        { TokenType.ASTRIX, Precedence.PRODUCT },
        { TokenType.LPAREN, Precedence.CALL },
    };

    public Parser(Lexer lexer, LogLevel logLevel = LogLevel.NONE) {
        Lexer = lexer;
        LogLevel = logLevel;
        NextToken();
        NextToken();
        InitParseMaps();
    }
    
}