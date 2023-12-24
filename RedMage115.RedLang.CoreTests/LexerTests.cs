using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedToken;
using Xunit.Abstractions;

namespace RedMage115.RedLang.CoreTests;

public class LexerTests {
    private readonly ITestOutputHelper _testOutputHelper;
    public LexerTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TestNextTokenSimple() {
        var input = "=+(){},;";
        var expectedTokens = new List<Token>() {
            new(TokenType.ASSIGN, "="),
            new(TokenType.PLUS, "+"),
            new(TokenType.LPAREN, "("),
            new(TokenType.RPAREN, ")"),
            new(TokenType.LBRACE, "{"),
            new(TokenType.RBRACE, "}"),
            new(TokenType.COMMA, ","),
            new(TokenType.SEMICOLON, ";")
        };
        var lexer = new Lexer(input);
        foreach (var expected in expectedTokens) {
            var actual = lexer.NextToken();
            _testOutputHelper.WriteLine($"Expected: {expected}, Got: {actual}");
            Assert.Equal(expected.Literal, actual.Literal);
            Assert.Equal(expected.Type, actual.Type);
        }
    }
    [Fact]
    public void TestNextToken() {
        var input = """
                    let five = 5;
                    let ten = 10;
                    let add = fn(x, y) {
                    x + y;
                    };
                    let result = add(five, ten);
                    """;
        var expectedTokens = new List<Token>() {
            new(TokenType.LET, "let"),
            new(TokenType.IDENT, "five"),
            new(TokenType.ASSIGN, "="),
            new(TokenType.INT, "5"),
            new(TokenType.SEMICOLON, ";"),
            new(TokenType.LET, "let"),
            new(TokenType.IDENT, "ten"),
            new(TokenType.ASSIGN, "="),
            new(TokenType.INT, "10"),
            new(TokenType.SEMICOLON, ";"),
            new(TokenType.LET, "let"),
            new(TokenType.IDENT, "add"),
            new(TokenType.ASSIGN, "="),
            new(TokenType.FUNCTION, "fn"),
            new(TokenType.LPAREN, "("),
            new(TokenType.IDENT, "x"),
            new(TokenType.COMMA, ","),
            new(TokenType.IDENT, "y"),
            new(TokenType.RPAREN, ")"),
            new(TokenType.LBRACE, "{"),
            new(TokenType.IDENT, "x"),
            new(TokenType.PLUS, "+"),
            new(TokenType.IDENT, "y"),
            new(TokenType.SEMICOLON, ";"),
            new(TokenType.RBRACE, "}"),
            new(TokenType.SEMICOLON, ";"),
            new(TokenType.LET, "let"),
            new(TokenType.IDENT, "result"),
            new(TokenType.ASSIGN, "="),
            new(TokenType.IDENT, "add"),
            new(TokenType.LPAREN, "("),
            new(TokenType.IDENT, "five"),
            new(TokenType.COMMA, ","),
            new(TokenType.IDENT, "ten"),
            new(TokenType.RPAREN, ")"),
            new(TokenType.SEMICOLON, ";"),
        };
        var lexer = new Lexer(input);
        foreach (var expected in expectedTokens) {
            var actual = lexer.NextToken();
            _testOutputHelper.WriteLine($"Expected: {expected}, Got: {actual}");
            Assert.Equal(expected.Literal, actual.Literal);
            Assert.Equal(expected.Type, actual.Type);
        }
    }
}