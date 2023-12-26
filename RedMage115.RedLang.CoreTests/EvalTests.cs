using System.Diagnostics;
using RedMage115.RedLang.Core.RedEvaluator;
using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedObject;
using RedMage115.RedLang.Core.RedParser;
using Xunit.Abstractions;
using Array = RedMage115.RedLang.Core.RedObject.Array;
using Boolean = RedMage115.RedLang.Core.RedObject.Boolean;
using Environment = RedMage115.RedLang.Core.RedObject.Environment;
using Object = RedMage115.RedLang.Core.RedObject.Object;
using String = RedMage115.RedLang.Core.RedObject.String;

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
    
    [Fact]
    private void TestStringEval() {
        var input = """
                    "Hello, World"
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        Assert.NotNull(program);
        var env = new Environment();
        var eval = Evaluator.Eval(program, env);
        _testOutputHelper.WriteLine($"Expected: \"Hello, World\", got: {eval.InspectObject()}");
        Assert.Equal("Hello, World", eval.InspectObject());
    }
    
    [Fact]
    private void TestStringConcatEval() {
        var input = """
                    "Hello" + ", " + "World";
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        Assert.NotNull(program);
        var env = new Environment();
        var eval = Evaluator.Eval(program, env);
        _testOutputHelper.WriteLine($"Expected: \"Hello, World\", got: {eval.InspectObject()}");
        Assert.Equal("Hello, World", eval.InspectObject());
    }
    
    [Fact]
    private void TestStringConcatFailEval() {
        var input = """
                    "Hello" - ", " - "World";
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        Assert.NotNull(program);
        var env = new Environment();
        var eval = Evaluator.Eval(program, env);
        Assert.IsType<Error>(eval);
    }
    
    [Fact]
    private void TestBuiltinFunctionEval() {
        var inputs = new List<(string input, Object expected)>() {
            ("""len("");""", new Error()),
            ("""len("Hello, World");""", new Integer(12)),
            ("""len("Hello", "World");""", new Error()),
            ("""len(1);""", new Error()),
            ("""len(true);""", new Error()),
            ("""len([1,2]);""", new Integer(2)),
            ("""first([1,2]);""", new Integer(1)),
            ("""first([]);""", new Error()),
            ("""last([1,2]);""", new Integer(2)),
            ("""last([]);""", new Error()),
            ("""first(["one","two"]);""", new String("one")),
            ("""last(["one","two"]);""", new String("two")),
            ("""tail(["one","two","three"]);""", new Array([new String("two"), new String("three")])),
            ("""tail([1,2,3]);""", new Array([new Integer(2), new Integer(3)])),
            ("""push([1,2,3], 4);""", new Array([new Integer(1), new Integer(2), new Integer(3), new Integer(4)])),
            ("""push(["one","two","three"], "four");""", new Array([new String("one"), new String("two"), new String("three"), new String("four")])),
            
        };
        foreach (var input in inputs) {
            var parser = new Parser(new Lexer(input.input));
            var program = parser.ParseProgram();
            Assert.NotNull(program);
            var env = new Environment();
            var eval = Evaluator.Eval(program, env);
            var expectedType = input.expected.GetObjectType();
            _testOutputHelper.WriteLine($"Expected: {expectedType}, got: {eval.GetObjectType()}");
            if (eval is Error error) {
                _testOutputHelper.WriteLine($"Error: {error.Message}");
            }
            Assert.Equal(expectedType, eval.GetObjectType());
            if (eval is Integer integer && input.expected is Integer expectedInteger) {
                _testOutputHelper.WriteLine($"Expected: {expectedInteger.Value}, got: {integer.Value}");
                Assert.Equal(expectedInteger, integer);
            }
            if (eval is String str && input.expected is String expectedStr) {
                _testOutputHelper.WriteLine($"Expected: {expectedStr.Value}, got: {str.Value}");
                Assert.Equal(expectedStr.Value, str.Value);
            }
            if (eval is Array array && input.expected is Array expectedArray) {
                for (var i = 0; i < array.Elements.Count; i++) {
                    _testOutputHelper.WriteLine($"Expected: {array.Elements[i].InspectObject()}, got: {expectedArray.Elements[i].InspectObject()}");
                    Assert.Equal(expectedArray.Elements[i], array.Elements[i]);
                }
            }
        }
    }
    [Fact]
    private void TestArrayEval() {
        var input = """
                    [1,2*2,3+3] 
                    """;
        var expected = new List<Integer>() {
            new (1),
            new (4),
            new (6),
        };
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        Assert.NotNull(program);
        var env = new Environment();
        var eval = Evaluator.Eval(program, env);
        if (eval is Error error) {
            _testOutputHelper.WriteLine($"Error: {error.Message}");
        }
        Assert.IsType<Array>(eval);
        if (eval is Array array) {
            for (int i = 0; i < expected.Count; i++) {
                if (array.Elements[i] is Integer actualInteger) {
                    _testOutputHelper.WriteLine($"Expected: {expected[i].Value}, got: {actualInteger.Value}");
                    Assert.Equal(expected[i].Value, actualInteger.Value);
                }
                else {
                    Assert.Fail($"Array[{i}] was not an Integer, got: {array.Elements[i].GetObjectType()}");
                }
            }
        }
    }
    [Fact]
    private void TestArrayIndexEval() {
        var input = """
                    [1,2*2,3+3][0]
                    """;
        var expected = new List<Integer>() {
            new (1),
        };
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        Assert.NotNull(program);
        var env = new Environment();
        var eval = Evaluator.Eval(program, env);
        if (eval is Error error) {
            _testOutputHelper.WriteLine($"Error: {error.Message}");
        }
        Assert.IsType<Integer>(eval);
        if (eval is Integer integer) {
            _testOutputHelper.WriteLine($"Expected: {expected[0].Value}, got: {integer.Value}");
            Assert.Equal(expected[0].Value, integer.Value);
        }
    }
    
    [Fact]
    private void TestArrayIndexOutOfBoundsEval() {
        var input = """
                    [1,2*2,3+3][3]
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        Assert.NotNull(program);
        var env = new Environment();
        var eval = Evaluator.Eval(program, env);
        if (eval is Error error) {
            _testOutputHelper.WriteLine($"Error: {error.Message}");
        }

        _testOutputHelper.WriteLine($"Expected: NULL, got: {eval.GetObjectType()}");
        Assert.IsType<Null>(eval);
    }
    
    [Fact]
    private void TestArrayIndexOutOfBoundsNegativeEval() {
        var input = """
                    [1,2*2,3+3][-1]
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        Assert.NotNull(program);
        var env = new Environment();
        var eval = Evaluator.Eval(program, env);
        if (eval is Error error) {
            _testOutputHelper.WriteLine($"Error: {error.Message}");
        }

        _testOutputHelper.WriteLine($"Expected: NULL, got: {eval.GetObjectType()}");
        Assert.IsType<Null>(eval);
    }
    
    [Fact]
    private void TestHashLiteralEval() {
        var input = """
                    let two = "two";
                    {
                    "one": 10 - 9,
                    two: 1 + 1,
                    "thr" + "ee": 6 / 2,
                    4: 4,
                    true: 5,
                    false: 6
                    }
                    """;
        var parser = new Parser(new Lexer(input));
        var program = parser.ParseProgram();
        Assert.NotNull(program);
        var env = new Environment();
        var eval = Evaluator.Eval(program, env);
        if (eval is Error error) {
            _testOutputHelper.WriteLine($"Error: {error.Message}");
        }

        _testOutputHelper.WriteLine($"Expected: Hash, got: {eval.GetObjectType()}");
        _testOutputHelper.WriteLine($"Hash: {eval.InspectObject()}");
        Assert.IsNotType<Null>(eval);
        Assert.IsType<Hash>(eval);
        if (eval is Hash hash) {
            Assert.Collection(hash.Pairs,
                pair => {
                    Assert.Equal("one", pair.Value.Key.InspectObject());
                    Assert.Equal("1", pair.Value.Value.InspectObject());
                },
                pair => {
                    Assert.Equal("two", pair.Value.Key.InspectObject());
                    Assert.Equal("2", pair.Value.Value.InspectObject());
                },
                pair => {
                    Assert.Equal("three", pair.Value.Key.InspectObject());
                    Assert.Equal("3", pair.Value.Value.InspectObject());
                },
                pair => {
                    Assert.Equal("4", pair.Value.Key.InspectObject());
                    Assert.Equal("4", pair.Value.Value.InspectObject());
                },
                pair => {
                    Assert.Equal("True", pair.Value.Key.InspectObject());
                    Assert.Equal("5", pair.Value.Value.InspectObject());
                },
                pair => {
                    Assert.Equal("False", pair.Value.Key.InspectObject());
                    Assert.Equal("6", pair.Value.Value.InspectObject());
                });
        }
        
    }
}