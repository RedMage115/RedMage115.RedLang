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
                    !-/*5;
                    5 < 10 > 5;
                    if (5 < 10) {
                        return true;
                    } else {
                        return false;
                    }
                    10 == 10;
                    10 != 9;
                    "foobar";
                    "foo bar";
                    [1,2];
                    {"foo":"bar"};
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
            new(TokenType.BANG, "!"),
            new(TokenType.MINUS, "-"),
            new(TokenType.SLASH, "/"),
            new(TokenType.ASTRIX, "*"),
            new(TokenType.INT, "5"),
            new(TokenType.SEMICOLON, ";"),
            new(TokenType.INT, "5"),
            new(TokenType.LT, "<"),
            new(TokenType.INT, "10"),
            new(TokenType.GT, ">"),
            new(TokenType.INT, "5"),
            new(TokenType.SEMICOLON, ";"),
            new(TokenType.IF, "if"),
            new(TokenType.LPAREN, "("),
            new(TokenType.INT, "5"),
            new(TokenType.LT, "<"),
            new(TokenType.INT, "10"),
            new(TokenType.RPAREN, ")"),
            new(TokenType.LBRACE, "{"),
            new(TokenType.RETURN, "return"),
            new(TokenType.TRUE, "true"),
            new(TokenType.SEMICOLON, ";"),
            new(TokenType.RBRACE, "}"),
            new(TokenType.ELSE, "else"),
            new(TokenType.LBRACE, "{"),
            new(TokenType.RETURN, "return"),
            new(TokenType.FALSE, "false"),
            new(TokenType.SEMICOLON, ";"),
            new(TokenType.RBRACE, "}"),
            new(TokenType.INT, "10"),
            new(TokenType.EQ, "=="),
            new(TokenType.INT, "10"),
            new(TokenType.SEMICOLON, ";"),
            new(TokenType.INT, "10"),
            new(TokenType.NOT_EQ, "!="),
            new(TokenType.INT, "9"),
            new(TokenType.SEMICOLON, ";"),
            new(TokenType.STRING, "foobar"),
            new(TokenType.SEMICOLON, ";"),
            new(TokenType.STRING, "foo bar"),
            new(TokenType.SEMICOLON, ";"),
            new(TokenType.LBRACKET, "["),
            new(TokenType.INT, "1"),
            new(TokenType.COMMA, ","),
            new(TokenType.INT, "2"),
            new(TokenType.RBRACKET, "]"),
            new(TokenType.SEMICOLON, ";"),
            new(TokenType.LBRACE, "{"),
            new(TokenType.STRING, "foo"),
            new(TokenType.COLON, ":"),
            new(TokenType.STRING, "bar"),
            new(TokenType.RBRACE, "}"),
            new(TokenType.SEMICOLON, ";"),
            new(TokenType.EOF, ""),
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