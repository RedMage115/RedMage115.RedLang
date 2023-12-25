using RedMage115.RedLang.Core.RedEvaluator;
using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedObject;
using RedMage115.RedLang.Core.RedParser;
using Xunit.Abstractions;
using Boolean = RedMage115.RedLang.Core.RedObject.Boolean;

namespace RedMage115.RedLang.CoreTests;

public class EvalTests {
    private readonly ITestOutputHelper _testOutputHelper;
    public EvalTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    private void TestIntegerExpressionEval() {
        var tests = new List<(string input, long expected)>() {
            ("5", 5),
            ("10", 10),
            ("5214312", 5214312),
            ("522", 522),
            ("05", 5),
            ("-5", -5),
            ("5+5", 10),
            ("5*5", 25),
            ("10/5", 2),
            ("-50 + 100 + -50", 0),
            ("3 * (3 * 3) + 10", 37),
            ("3 * 3 * 3 + 10", 37),
            ("(5 + 10 * 2 + 15 / 3) * 2 + -10", 50),
        };
        foreach (var test in tests) {
            var parser = new Parser(new Lexer(test.input));
            var program = parser.ParseProgram();
            Assert.NotNull(program);
            Assert.Empty(parser.Errors);
            var actual = (Integer?)Evaluator.Eval(program);
            Assert.NotNull(actual);
            _testOutputHelper.WriteLine($"Expected: {test.expected}, got: {actual?.Value}");
            Assert.Equal(test.expected, actual?.Value);
        }
    }
    
    [Fact]
    private void TestBoolExpressionEval() {
        var tests = new List<(string input, bool expected)>() {
            ("true", true),
            ("false", false),
            ("true;", true),
            ("false;", false),
            ("5<10;", true),
            ("10<5;", false),
            ("10==10", true),
            ("1==10", false),
            ("10!=10", false),
            ("1!=10", true),
            ("true == true", true),
            ("false == false", true),
            ("true == false", false),
            ("(10 > 1) == true", true),
        };
        foreach (var test in tests) {
            var parser = new Parser(new Lexer(test.input));
            var program = parser.ParseProgram();
            Assert.NotNull(program);
            Assert.Empty(parser.Errors);
            var actual = (Boolean?)Evaluator.Eval(program);
            Assert.NotNull(actual);
            _testOutputHelper.WriteLine($"Expected: {test.expected}, got: {actual?.Value}");
            Assert.Equal(test.expected, actual?.Value);
        }
    }
    
    [Fact]
    private void TestBangOperatorEval() {
        var tests = new List<(string input, bool expected)>() {
            ("!true", false),
            ("!false", true),
            ("!!true;", true),
            ("!!false;", false),
            ("!!5;", true),
            ("!5;", false),
        };
        foreach (var test in tests) {
            var parser = new Parser(new Lexer(test.input));
            var program = parser.ParseProgram();
            Assert.NotNull(program);
            Assert.Empty(parser.Errors);
            var actual = (Boolean?)Evaluator.Eval(program);
            Assert.NotNull(actual);
            _testOutputHelper.WriteLine($"Expected: {test.expected}, got: {actual?.Value}");
            Assert.Equal(test.expected, actual?.Value);
        }
    }
}