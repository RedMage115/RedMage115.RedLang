using RedMage115.RedLang.Core.RedCode;
using RedMage115.RedLang.Core.RedCompiler;
using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedObject;
using RedMage115.RedLang.Core.RedParser;
using Xunit.Abstractions;
using Object = RedMage115.RedLang.Core.RedObject.Object;

namespace RedMage115.RedLang.CoreTests;

public class CompilerTests {
    private readonly ITestOutputHelper _testOutputHelper;
    public CompilerTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    private void TestIntegerArithmetic() {
        var tests = new List<TestCase>() {
            new TestCase("1 + 2",
                [new Integer(1), new Integer(2)],
                [
                    OpCode.Make(OpCode.OP_CONSTANT, new List<int>() { 0 }),
                    OpCode.Make(OpCode.OP_CONSTANT, new List<int>() { 1 }),
                    OpCode.Make(OpCode.OP_ADD, new List<int>() {})
                ])
        };

        foreach (var testCase in tests) {
            var program = new Parser(new Lexer(testCase.Input)).ParseProgram();
            var compiler = new Compiler();
            compiler.Compile(program);
            var actual = compiler.ByteCode();
            Assert.NotEmpty(actual.Instructions);
            Assert.NotEmpty(actual.Constants);
        }
        
    }
    
    [Fact]
    private void TestIfStatement() {
        var tests = new List<TestCase>() {
            new TestCase("if(1<2){10}",
                [new Integer(1), new Integer(2), new Integer(10)],
                [
                    OpCode.Make(OpCode.OP_CONSTANT, new List<int>() { 0 }),
                    OpCode.Make(OpCode.OP_CONSTANT, new List<int>() { 1 }),
                    OpCode.Make(OpCode.OP_ADD, new List<int>() {})
                ])
        };

        foreach (var testCase in tests) {
            var program = new Parser(new Lexer(testCase.Input)).ParseProgram();
            var compiler = new Compiler();
            compiler.Compile(program);
            var actual = compiler.ByteCode();
            Assert.NotEmpty(actual.Instructions);
            Assert.NotEmpty(actual.Constants);
            foreach (var instruction in actual.Instructions) {
                var def = Definition.Lookup(instruction);
                if (def is not null) {
                    _testOutputHelper.WriteLine($"{def.Name} - {instruction}");
                }
            }

            foreach (var constant in actual.Constants) {
                _testOutputHelper.WriteLine(constant.InspectObject());
            }
        }
        
    }
    
    [Fact]
    private void TestFunctionExplicit() {
        var tests = new List<TestCase>() {
            new TestCase("fn(){return 10+5;}",
                [],
                [
                ])
        };

        foreach (var testCase in tests) {
            var program = new Parser(new Lexer(testCase.Input)).ParseProgram();
            var compiler = new Compiler();
            compiler.Compile(program);
            var actual = compiler.ByteCode();
            _testOutputHelper.WriteLine($"Expected Compiled Function, got: {actual.Constants.Last().GetObjectType()}");
            Assert.IsType<CompiledFunction>(actual.Constants.Last());
            if (actual.Constants.Last() is CompiledFunction compiledFunction) {
                _testOutputHelper.WriteLine($"Expected last instruction: {OpCode.OP_RETURN_VALUE}, got: {compiledFunction.Instructions.Last()}");
                Assert.Equal(OpCode.OP_RETURN_VALUE, compiledFunction.Instructions.Last());
            }
            Assert.NotEmpty(actual.Instructions);
            Assert.NotEmpty(actual.Constants);
            _testOutputHelper.WriteLine("Dumping Instructions");
            foreach (var instruction in actual.Instructions) {
                var def = Definition.Lookup(instruction);
                if (def is not null) {
                    _testOutputHelper.WriteLine($"{def.Name} - {instruction}");
                }
            }

            _testOutputHelper.WriteLine("Dumping Constants");
            foreach (var constant in actual.Constants) {
                _testOutputHelper.WriteLine(constant.InspectObject());
            }
        }
        
    }
    
    [Fact]
    private void TestFunctionImplicit() {
        var tests = new List<TestCase>() {
            new TestCase("fn(){10+5}",
                [],
                [
                ])
        };

        foreach (var testCase in tests) {
            var program = new Parser(new Lexer(testCase.Input)).ParseProgram();
            var compiler = new Compiler();
            compiler.Compile(program);
            var actual = compiler.ByteCode();
            _testOutputHelper.WriteLine($"Expected Compiled Function, got: {actual.Constants.Last().GetObjectType()}");
            Assert.IsType<CompiledFunction>(actual.Constants.Last());
            if (actual.Constants.Last() is CompiledFunction compiledFunction) {
                _testOutputHelper.WriteLine($"Expected last instruction: {OpCode.OP_RETURN_VALUE}, got: {compiledFunction.Instructions.Last()}");
                Assert.Equal(OpCode.OP_RETURN_VALUE, compiledFunction.Instructions.Last());
            }
            Assert.NotEmpty(actual.Instructions);
            Assert.NotEmpty(actual.Constants);
            _testOutputHelper.WriteLine("Dumping Instructions");
            foreach (var instruction in actual.Instructions) {
                var def = Definition.Lookup(instruction);
                if (def is not null) {
                    _testOutputHelper.WriteLine($"{def.Name} - {instruction}");
                }
            }

            _testOutputHelper.WriteLine("Dumping Constants");
            foreach (var constant in actual.Constants) {
                _testOutputHelper.WriteLine(constant.InspectObject());
            }
        }
        
    }
    
    [Fact]
    private void TestFunctionImplicitWithPops() {
        var tests = new List<TestCase>() {
            new TestCase("fn(){10; 5+5}",
                [],
                [
                ])
        };

        foreach (var testCase in tests) {
            var program = new Parser(new Lexer(testCase.Input)).ParseProgram();
            var compiler = new Compiler();
            compiler.Compile(program);
            var actual = compiler.ByteCode();
            _testOutputHelper.WriteLine($"Expected Compiled Function, got: {actual.Constants.Last().GetObjectType()}");
            Assert.IsType<CompiledFunction>(actual.Constants.Last());
            if (actual.Constants.Last() is CompiledFunction compiledFunction) {
                _testOutputHelper.WriteLine($"Expected last instruction: {OpCode.OP_RETURN_VALUE}, got: {compiledFunction.Instructions.Last()}");
                Assert.Equal(OpCode.OP_RETURN_VALUE, compiledFunction.Instructions.Last());
            }
            Assert.NotEmpty(actual.Instructions);
            Assert.NotEmpty(actual.Constants);
            _testOutputHelper.WriteLine("Dumping Instructions");
            foreach (var instruction in actual.Instructions) {
                var def = Definition.Lookup(instruction);
                if (def is not null) {
                    _testOutputHelper.WriteLine($"{def.Name} - {instruction}");
                }
            }

            _testOutputHelper.WriteLine("Dumping Constants");
            foreach (var constant in actual.Constants) {
                _testOutputHelper.WriteLine(constant.InspectObject());
            }
        }
        
    }
    
    [Fact]
    private void TestFunctionNoBody() {
        var tests = new List<TestCase>() {
            new TestCase("fn(){}",
                [],
                [
                ])
        };

        foreach (var testCase in tests) {
            var program = new Parser(new Lexer(testCase.Input)).ParseProgram();
            var compiler = new Compiler();
            compiler.Compile(program);
            var actual = compiler.ByteCode();
            _testOutputHelper.WriteLine($"Expected Compiled Function, got: {actual.Constants.Last().GetObjectType()}");
            Assert.IsType<CompiledFunction>(actual.Constants.Last());
            if (actual.Constants.Last() is CompiledFunction compiledFunction) {
                _testOutputHelper.WriteLine($"Expected last instruction: {OpCode.OP_RETURN}, got: {compiledFunction.Instructions.Last()}");
                Assert.Equal(OpCode.OP_RETURN, compiledFunction.Instructions.Last());
            }
            Assert.NotEmpty(actual.Instructions);
            Assert.NotEmpty(actual.Constants);
            _testOutputHelper.WriteLine("Dumping Instructions");
            foreach (var instruction in actual.Instructions) {
                var def = Definition.Lookup(instruction);
                if (def is not null) {
                    _testOutputHelper.WriteLine($"{def.Name} - {instruction}");
                }
            }

            _testOutputHelper.WriteLine("Dumping Constants");
            foreach (var constant in actual.Constants) {
                _testOutputHelper.WriteLine(constant.InspectObject());
            }
        }
        
    }
    
    [Fact]
    private void TestFunctionCall() {
        var tests = new List<TestCase>() {
            new TestCase("""
                            let x = fn(){5+10};
                            x(); 
                         """,
                [],
                [
                ])
        };

        foreach (var testCase in tests) {
            var program = new Parser(new Lexer(testCase.Input)).ParseProgram();
            var compiler = new Compiler();
            compiler.Compile(program);
            var actual = compiler.ByteCode();
            _testOutputHelper.WriteLine($"Expected Compiled Function, got: {actual.Constants.Last().GetObjectType()}");
            Assert.IsType<CompiledFunction>(actual.Constants.Last());
            _testOutputHelper.WriteLine($"Expected last instruction: {OpCode.OP_CALL}, got: {actual.Instructions[^2]}");
            Assert.Equal(OpCode.OP_CALL, actual.Instructions[^2]);
            Assert.NotEmpty(actual.Instructions);
            Assert.NotEmpty(actual.Constants);
            _testOutputHelper.WriteLine("Dumping Instructions");
            foreach (var instruction in actual.Instructions) {
                var def = Definition.Lookup(instruction);
                if (def is not null) {
                    _testOutputHelper.WriteLine($"{def.Name} - {instruction}");
                }
            }

            _testOutputHelper.WriteLine("Dumping Constants");
            foreach (var constant in actual.Constants) {
                _testOutputHelper.WriteLine(constant.InspectObject());
            }
        }
        
    }
    
    private struct TestCase {
        internal string Input { get; set; }
        internal List<Object> ExpectedConstants { get; set; }
        internal List<byte> ExpectedInstructions { get; set; }
        public TestCase(string input, List<Object> expectedConstants, List<List<byte>> expectedInstructions) {
            Input = input;
            ExpectedConstants = expectedConstants;
            var inst = new List<byte>();
            foreach (var expectedInstruction in expectedInstructions) {
                inst.AddRange(expectedInstruction);
            }
            ExpectedInstructions = inst;
        }
    }
    
}