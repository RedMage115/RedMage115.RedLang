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
                    let x = 1 * 2 * 3 * 4 * 5;
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        _testOutputHelper.WriteLine(program.GetNodeTypeString());
        Assert.NotNull(program);
        foreach (var statement in program.Statements) {
            _testOutputHelper.WriteLine(statement.GetNodeTypeString());
        }
        Assert.Collection(program.Statements, 
            statement => Assert.Equal("<Let Statement [let x <Integer Literal [5]>] >",statement.GetNodeTypeString()),
            statement => Assert.Equal("<Let Statement [let y <Integer Literal [10]>] >",statement.GetNodeTypeString()),
            statement => Assert.Equal("<Let Statement [let foobar <Integer Literal [838383]>] >",statement.GetNodeTypeString()),
            statement => Assert.Equal("<Let Statement [let x <Infix Expression [<Infix Expression [<Infix Expression [<Infix Expression [<Integer Literal [1]> * <Integer Literal [2]>]> * <Integer Literal [3]>]> * <Integer Literal [4]>]> * <Integer Literal [5]>]>] >",statement.GetNodeTypeString()));
        ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 0);
    }
    
    [Fact]
    private void TestLetStatementFail() {
        var input = """
                    let x 5;
                    let = 10;
                    let 838383;
                    let x = 1 * 2 * 3 * 4 * 5
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
            statement => Assert.Equal("<Return Statement [return 5] >",statement.GetNodeTypeString()),
            statement => Assert.Equal("<Return Statement [return 10] >",statement.GetNodeTypeString()),
            statement => Assert.Equal("<Return Statement [return 993322] >",statement.GetNodeTypeString()));
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
            statement => Assert.Equal("<Expression Statement [<Identifier [foobar foobar]>]>",statement.GetNodeTypeString()));
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
            statement => Assert.Equal("<Expression Statement [<Integer Literal [5]>]>",statement.GetNodeTypeString()));
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
                Assert.Equal("<Expression Statement [<Prefix Expression [!5]>]>", statement.GetNodeTypeString());
            },
            statement => {
                Assert.Equal("<Expression Statement [<Prefix Expression [-15]>]>", statement.GetNodeTypeString());
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
            "((a + add((b * c))) + d)"

        };
        var expected = new List<string>() {
            "-a*b",
            "!-a",
            "a+b+c",
            "a+b-c",
            "a*b*c",
            "a*b/c",
            "a+b/c",
            "a+b*c+d/e-f",
            "3+4 (-5*5)",
            "5>4==3<4",
            "5<4!=3>4",
            "3+4*5==3*1+4*5",
            "a+add (b*c)+d"
        };
        var cnt = 0;
        foreach (var input in inputs) {
            var parser = new Parser(new Lexer(input));
            var program = parser.ParseProgram();
            Assert.NotNull(program);
            foreach (var statement in program.Statements) {
                _testOutputHelper.WriteLine($"==========================================");
                _testOutputHelper.WriteLine($"Actual AST: {statement.GetNodeTypeString()}");
                _testOutputHelper.WriteLine($"Actual Raw: {statement.GetRawStatement()}");
                _testOutputHelper.WriteLine($"Expected Raw: {expected[cnt]}");
                Assert.Equal(expected[cnt], statement.GetRawStatement());
                cnt++;
            }
            ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 0);
        }
    }
    [Fact]
    private void TestBooleanExpression() {
        var input = """
                    true;
                    false;
                    let x = false;
                    return false;
                    x == false;
                    x != true;
                    !false;
                    !true;
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        _testOutputHelper.WriteLine($"Full AST: {program.GetNodeTypeString()}");
        Assert.NotNull(program);
        ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 0);
    }
    
    [Fact]
    private void TestIfExpression() {
        var input = """
                    if (x < y) { x }
                    if (x < y) { x + 1 }
                    if (x == true) { return false; }
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        _testOutputHelper.WriteLine($"Full AST: {program.GetNodeTypeString()}");
        foreach (var statement in program.Statements) {
            _testOutputHelper.WriteLine($"Raw: {statement.GetRawStatement()}");
        }
        Assert.NotNull(program);
        ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 0);
    }
    [Fact]
    private void TestIfElseExpression() {
        var input = """
                    if (x < y) { x } else { y }
                    if (x) { x } else { y }
                    if (x) { x + 1 } else { y - 1 }
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        _testOutputHelper.WriteLine($"Full AST: {program.GetNodeTypeString()}");
        foreach (var statement in program.Statements) {
            _testOutputHelper.WriteLine($"Raw: {statement.GetRawStatement()}");
        }
        Assert.NotNull(program);
        ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 0);
    }
    
    [Fact]
    private void TestFunctionExpression() {
        var input = """
                    fn() {return x;}
                    fn(x) {return x;}
                    fn(x, y) {return x + y;}
                    fn(x) {x}
                    fn(x, y) {x+y}
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        _testOutputHelper.WriteLine($"Full AST: {program.GetNodeTypeString()}");
        foreach (var statement in program.Statements) {
            _testOutputHelper.WriteLine($"Raw: {statement.GetRawStatement()}");
        }
        Assert.NotNull(program);
        ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 0);
    }
    
    [Fact]
    private void TestCallExpression() {
        var input = """
                    let x = add(x,y);
                    add(x,y);
                    inc(x);
                    add(1, 2 * 3, 4 + 5);
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        _testOutputHelper.WriteLine($"Full AST: {program.GetNodeTypeString()}");
        foreach (var statement in program.Statements) {
            _testOutputHelper.WriteLine($"Raw: {statement.GetRawStatement()}");
        }
        Assert.NotNull(program);
        ParserErrorTests.TestParserErrors(_testOutputHelper, parser, 0);
    }
}