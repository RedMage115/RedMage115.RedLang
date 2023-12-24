using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedParser;
using Xunit.Abstractions;

namespace RedMage115.RedLang.CoreTests;

public class ParserTests {
    private readonly ITestOutputHelper _testOutputHelper;
    public ParserTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    private void TestLetStatement() {
        var input = """
                    let x = 5;
                    let y = 10;
                    let foobar = 838383; 
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        _testOutputHelper.WriteLine(program.GetNodeTypeString());
        Assert.NotNull(program);
        foreach (var statement in program.Statements) {
            _testOutputHelper.WriteLine(statement.GetNodeTypeString());
        }
        Assert.Collection(program.Statements, 
            statement => Assert.Equal("<Let Statement [let x ] >",statement.GetNodeTypeString()),
            statement => Assert.Equal("<Let Statement [let y ] >",statement.GetNodeTypeString()),
            statement => Assert.Equal("<Let Statement [let foobar ] >",statement.GetNodeTypeString()));
        ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 0);
    }
    
    [Fact]
    private void TestLetStatementFail() {
        var input = """
                    let x 5;
                    let = 10;
                    let 838383;
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        _testOutputHelper.WriteLine(program.GetNodeTypeString());
        Assert.NotNull(program);
        foreach (var statement in program.Statements) {
            _testOutputHelper.WriteLine(statement.GetNodeTypeString());
        }
        ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 4);
    }
    
    [Fact]
    private void TestReturnStatement() {
        var input = """
                    return 5;
                    return 10;
                    return 993322;
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        _testOutputHelper.WriteLine(program.GetNodeTypeString());
        Assert.NotNull(program);
        foreach (var statement in program.Statements) {
            _testOutputHelper.WriteLine(statement.GetNodeTypeString());
        }
        Assert.Collection(program.Statements, 
            statement => Assert.Equal("<Return Statement [return ] >",statement.GetNodeTypeString()),
            statement => Assert.Equal("<Return Statement [return ] >",statement.GetNodeTypeString()),
            statement => Assert.Equal("<Return Statement [return ] >",statement.GetNodeTypeString()));
        ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 0);
    }
    
    [Fact]
    private void TestIdentifierExpression() {
        var input = """
                    foobar;
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        _testOutputHelper.WriteLine($"Full AST: {program.GetNodeTypeString()}");
        Assert.NotNull(program);
        Assert.Collection(program.Statements, 
            statement => Assert.Equal("<Expression Statement [<Identifier [foobar foobar]> (foobar)]>",statement.GetNodeTypeString()));
        ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 0);
    }
    
    [Fact]
    private void TestIntegerLiteralExpression() {
        var input = """
                    5;
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        _testOutputHelper.WriteLine($"Full AST: {program.GetNodeTypeString()}");
        Assert.NotNull(program);
        Assert.Collection(program.Statements, 
            statement => Assert.Equal("<Expression Statement [<Integer Literal [5]> (5)]>",statement.GetNodeTypeString()));
        ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 0);
    }
    
    [Fact]
    private void TestPrefixExpression() {
        var input = """
                    !5;
                    -15;
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        _testOutputHelper.WriteLine($"Full AST: {program.GetNodeTypeString()}");
        Assert.NotNull(program);
        ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 0);
        Assert.Collection(program.Statements,
            statement => {
                Assert.Equal("<Expression Statement [<Prefix Expression [55]> (!)]>", statement.GetNodeTypeString());
            },
            statement => {
                Assert.Equal("<Expression Statement [<Prefix Expression [1515]> (-)]>", statement.GetNodeTypeString());
            });
    }
    
    [Fact]
    private void TestInfixExpression() {
        var input = """
                    5 + 5;
                    5 - 5;
                    5 * 5;
                    5 / 5;
                    5 > 5;
                    5 < 5;
                    5 == 5;
                    5 != 5;
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        _testOutputHelper.WriteLine($"Full AST: {program.GetNodeTypeString()}");
        Assert.NotNull(program);
        ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 0);
    }
    
    [Fact]
    private void TestInfixExpressionMultiple() {
        var inputs = new List<string>() {
            "((-a) * b)",
            "(!(-a))",
            "((a + b) + c)",
            "((a + b) - c)",
            "((a * b) * c)",
            "((a * b) / c)",
            "(a + (b / c))",
            "(((a + (b * c)) + (d / e)) - f)",
            "(3 + 4)((-5) * 5)",
            "((5 > 4) == (3 < 4))",
            "((5 < 4) != (3 > 4))",
            "((3 + (4 * 5)) == ((3 * 1) + (4 * 5)))",

        };
        var expected = new List<string>() {
            "-a * b",
            "!-a",
            "a + b + c",
            "a + b - c",
            "a * b * c",
            "a * b / c",
            "a + b / c",
            "a + b * c + d / e - f",
            "3 + 4; -5 * 5",
            "5 > 4 == 3 < 4",
            "5 < 4 != 3 > 4",
            "3 + 4 * 5 == 3 * 1 + 4 * 5",
        };

        foreach (var input in inputs) {
            var parser = new Parser(new Lexer(input));
            var program = parser.ParseProgram();
            Assert.NotNull(program);
            _testOutputHelper.WriteLine($"Full AST: {program.GetNodeTypeString()}");
            //ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 0);
        }
    }
}