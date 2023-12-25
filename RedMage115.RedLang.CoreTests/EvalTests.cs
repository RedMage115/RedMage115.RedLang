using System.Diagnostics;
using RedMage115.RedLang.Core.RedEvaluator;
using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedObject;
using RedMage115.RedLang.Core.RedParser;
using Xunit.Abstractions;
using Boolean = RedMage115.RedLang.Core.RedObject.Boolean;
using Environment = RedMage115.RedLang.Core.RedObject.Environment;
using Object = RedMage115.RedLang.Core.RedObject.Object;

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
            var environment = new Environment();
            var program = parser.ParseProgram();
            Assert.NotNull(program);
            Assert.Empty(parser.Errors);
            var actual = (Integer?)Evaluator.Eval(program,environment);
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
            var environment = new Environment();
            var program = parser.ParseProgram();
            Assert.NotNull(program);
            Assert.Empty(parser.Errors);
            var actual = (Boolean?)Evaluator.Eval(program,environment);
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
            var environment = new Environment();
            Assert.NotNull(program);
            Assert.Empty(parser.Errors);
            var actual = (Boolean?)Evaluator.Eval(program, environment);
            Assert.NotNull(actual);
            _testOutputHelper.WriteLine($"Expected: {test.expected}, got: {actual?.Value}");
            Assert.Equal(test.expected, actual?.Value);
        }
    }
    
    [Fact]
    private void TestIfElseExpressionEval() {
        var tests = new List<(string input, Object expected)>() {
            ("if (true) { 10 }", new Integer(10)),
            ("if (false) { 10 }", new Null()),
            ("if (1) { 10 }", new Integer(10)),
            ("if (1 < 2) { 10 }", new Integer(10)),
            ("if (1 > 2) { 10 }", new Null()),
            ("if (1 > 2) { 10 } else { 20 }", new Integer(20)),
            ("if (1 < 2) { 10 } else { 20 }", new Integer(10)),
        };
        foreach (var test in tests) {
            var parser = new Parser(new Lexer(test.input));
            var program = parser.ParseProgram();
            var environment = new Environment();
            Assert.NotNull(program);
            Assert.Empty(parser.Errors);
            var actual = Evaluator.Eval(program, environment);
            Assert.NotNull(actual);
            _testOutputHelper.WriteLine($"Expected: {test.expected}, got: {actual}");
            Assert.Equal(test.expected, actual);
        }
    }
    
    [Fact]
    private void TestReturnStatementEval() {
        var tests = new List<(string input, Object expected)>() {
            ("return 10;", new Integer(10)),
            ("return 10; 9;", new Integer(10)),
            ("return 2 * 5; 9;", new Integer(10)),
            ("9; return 2 * 5; 9;", new Integer(10)),
            ("if (1 > 2) { 10 }", new Null()),
        };
        foreach (var test in tests) {
            var parser = new Parser(new Lexer(test.input));
            var program = parser.ParseProgram();
            var environment = new Environment();
            Assert.NotNull(program);
            Assert.Empty(parser.Errors);
            var actual = Evaluator.Eval(program,environment);
            Assert.NotNull(actual);
            _testOutputHelper.WriteLine($"Expected: {test.expected}, got: {actual}");
            switch (actual) {
                case ReturnValue returnValue:
                    var integer = (Integer)returnValue.Value;
                    var expectedInt = (Integer)test.expected;
                    Assert.Equal(expectedInt.Value, integer.Value);
                    break;
                default:
                    break;
            }
            Assert.Equal(test.expected, actual);
        }
    }
    
    [Fact]
    private void TestErrorEval() {
        var tests = new List<(string input, string expected)>() {
            ("5 + true", "ERROR: expected (int operator int) or (bool operator bool), got: INTEGER+BOOLEAN"),
            ("foobar", "ERROR: identifier not found: foobar"),
        };
        foreach (var test in tests) {
            var parser = new Parser(new Lexer(test.input));
            var program = parser.ParseProgram();
            var environment = new Environment();
            Assert.NotNull(program);
            Assert.Empty(parser.Errors);
            var actual = Evaluator.Eval(program, environment);
            Assert.NotNull(actual);
            _testOutputHelper.WriteLine($"Expected: [{test.expected}],\nGot: [{actual.InspectObject()}]");
            Assert.Equal(test.expected, actual.InspectObject());
        }
    }
    
    [Fact]
    private void TestLetStatementEval() {
        var tests = new List<(string input, Object expected)>() {
            ("let a = 5; a;", new Integer(5)),
            ("let a = 5 * 5; a;", new Integer(25)),
            ("let a = 5; let b = a; b;", new Integer(5)),
            ("let a = 5; let b = a; let c = a + b + 5; c;", new Integer(15)),
        };
        foreach (var test in tests) {
            var parser = new Parser(new Lexer(test.input));
            var program = parser.ParseProgram();
            var environment = new Environment();
            Assert.NotNull(program);
            Assert.Empty(parser.Errors);
            var actual = Evaluator.Eval(program, environment);
            Assert.NotNull(actual);
            _testOutputHelper.WriteLine($"Expected: [{test.expected}],\nGot: [{actual.InspectObject()}]");
            Assert.IsType<Integer>(actual);
            if (actual is Integer integer) {
                Assert.Equal(test.expected, integer);
            }
            
        }
    }
    
    [Fact]
    private void TestFunctionEval() {
        var tests = new List<string>() {
            "fn (x){x+1;};",
        };
        foreach (var test in tests) {
            var parser = new Parser(new Lexer(test));
            var program = parser.ParseProgram();
            var environment = new Environment();
            Assert.NotNull(program);
            Assert.Empty(parser.Errors);
            var actual = Evaluator.Eval(program, environment);
            Assert.NotNull(actual);
            _testOutputHelper.WriteLine($"Expected: [{test}],\nGot: [{actual.InspectObject()}]");
            Assert.IsType<Function>(actual);
            if (actual is Function function) {
                Assert.Single(function.Parameters);
                function.Environment.TryGetValue("x", out var arg);
                _testOutputHelper.WriteLine($"Args: {arg}");
            }
        }
    }
    
    [Fact]
    private void TestFunctionCallEval() {
        var tests = new List<(string input, Object expected)>() {
            ("let identity = fn(x) { x; }; identity(5);", new Integer(5)),
            ("let identity = fn(x) { return x; }; identity(5);", new Integer(5)),
            ("fn(x) { x; }(5)", new Integer(5)),
            ("let add = fn(x, y) { x + y; }; add(5 + 5, add(5, 5));", new Integer(20)),
        };
        foreach (var test in tests) {
            var parser = new Parser(new Lexer(test.input));
            var program = parser.ParseProgram();
            var environment = new Environment();
            Assert.NotNull(program);
            Assert.Empty(parser.Errors);
            var actual = Evaluator.Eval(program, environment);
            Assert.NotNull(actual);
            _testOutputHelper.WriteLine($"Expected: [{test.expected}],\nGot: [{actual}]");
            if (actual is Error error) {
                _testOutputHelper.WriteLine(program.GetNodeTypeString());
                Assert.Fail(error.Message);
            }
            if (actual is Integer integer) {
                _testOutputHelper.WriteLine($"Expected: {test.expected.InspectObject()}, got: {integer.Value}");
                Assert.Equal(test.expected, integer);
            }
            
            
        }
    }
    
    [Fact]
    private void TestClosuresEval() {
        var input = """
                    let newAdder = fn(x) {
                        fn(y) { x + y };
                    };
                    let addTwo = newAdder(2);
                    addTwo(2);
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        Assert.NotNull(program);
        var env = new Environment();
        var eval = Evaluator.Eval(program, env);
        _testOutputHelper.WriteLine($"Expected: 4, got: {eval.InspectObject()}");
        Assert.Equal("4", eval.InspectObject());
    }
    [Fact]
    private void TestMemUsageEval() {
        var pageInitial = Process.GetCurrentProcess().PagedMemorySize64;
        var privInitial = Process.GetCurrentProcess().WorkingSet64;
        var virtInitial = Process.GetCurrentProcess().VirtualMemorySize64;
        var input = """
                    let counter = fn(x) {
                        if (x > 100) {
                            return true;
                        } 
                        else {
                            let foobar = 9999;
                            counter(x + 1);
                        }
                    };
                    counter(0);
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        Assert.NotNull(program);
        var env = new Environment();
        var eval = Evaluator.Eval(program, env);
        var pageEnd = Process.GetCurrentProcess().PeakPagedMemorySize64;
        var privEnd = Process.GetCurrentProcess().PeakWorkingSet64;
        var virtEnd = Process.GetCurrentProcess().PeakVirtualMemorySize64;

        _testOutputHelper.WriteLine($"Paged Initial: {pageInitial}, End: {pageEnd}, Diff: {pageEnd-pageInitial}");
        _testOutputHelper.WriteLine($"Working Set Initial: {privInitial}, End: {privEnd}, Diff: {privEnd-privInitial}");
        _testOutputHelper.WriteLine($"Virtual Initial: {virtInitial}, End: {virtEnd}, Diff: {virtEnd-virtInitial}");
        _testOutputHelper.WriteLine(eval.InspectObject());
    }
}