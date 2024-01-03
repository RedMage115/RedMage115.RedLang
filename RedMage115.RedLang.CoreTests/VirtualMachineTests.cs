using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedObject;
using RedMage115.RedLang.Core.RedCode;
using RedMage115.RedLang.Core.RedCompiler;
using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedParser;
using RedMage115.RedLang.Core.RedVm;
using Xunit.Abstractions;
using Boolean = RedMage115.RedLang.Core.RedObject.Boolean;
using Object = RedMage115.RedLang.Core.RedObject.Object;

namespace RedMage115.RedLang.CoreTests;

public class VirtualMachineTests {
    private readonly ITestOutputHelper _testOutputHelper;
    public VirtualMachineTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    private void TestIntegerArithmeticAdd() {
        var tests = new List<TestCase>() {
            new TestCase("10+2", 
                new List<Object>(){new Integer(10),new Integer(2)},
                new List<List<byte>>() {
                    OpCode.Make(OpCode.OP_CONSTANT, [10]),
                    OpCode.Make(OpCode.OP_CONSTANT, [2]),
                    OpCode.Make(OpCode.OP_ADD, []),
                }
            )
        };

        foreach (var testCase in tests) {
            var compiler = new Compiler();
            var program = GetProgram(testCase.Input);
            var result = compiler.Compile(program);
            Assert.True(result);
            var byteCode = compiler.ByteCode();

            var vm = new VirtualMachine(byteCode.Constants, byteCode.Instructions);
            vm.Run();
            foreach (var compilerError in compiler.Errors) {
                _testOutputHelper.WriteLine(compilerError);
            }
            _testOutputHelper.WriteLine("Dumping stack");
            foreach (var obj in vm.Stack[..vm.StackPointer]) {
                _testOutputHelper.WriteLine($"{obj.GetObjectType()} {obj.InspectObject()}");
            }
            if (vm.GetLastPopped() is Integer integer) {
                const int expectedResult = 12;
                Assert.Equal(expectedResult, integer.Value);
                if (vm.GetLastPopped() is Integer lastPopped) {
                    _testOutputHelper.WriteLine($"Last Popped Expected: {expectedResult}, got: {lastPopped.Value}");
                    Assert.Equal(expectedResult, lastPopped.Value);
                    const int expectedSp = 0;
                    _testOutputHelper.WriteLine($"Stack Pointer Expected: {expectedSp}, got: {vm.StackPointer}");
                    Assert.Equal(expectedSp, vm.StackPointer);
                }
            }
            
        }
        
    }
    
    [Fact]
    private void TestIntegerArithmeticSubtract() {
        var tests = new List<TestCase>() {
            new TestCase("10-2", 
                new List<Object>(){new Integer(10),new Integer(2)},
                new List<List<byte>>() {
                    OpCode.Make(OpCode.OP_CONSTANT, [10]),
                    OpCode.Make(OpCode.OP_CONSTANT, [2]),
                    OpCode.Make(OpCode.OP_SUB, []),
      
                }
            )
        };

        foreach (var testCase in tests) {
            var compiler = new Compiler();
            var program = GetProgram(testCase.Input);
            var result = compiler.Compile(program);
            Assert.True(result);
            var byteCode = compiler.ByteCode();

            var vm = new VirtualMachine(byteCode.Constants, byteCode.Instructions);
            vm.Run();
            foreach (var compilerError in compiler.Errors) {
                _testOutputHelper.WriteLine(compilerError);
            }
            _testOutputHelper.WriteLine("Dumping stack");
            foreach (var obj in vm.Stack[..vm.StackPointer]) {
                _testOutputHelper.WriteLine($"{obj.GetObjectType()} {obj.InspectObject()}");
            }
            if (vm.GetLastPopped() is Integer integer) {
                const int expectedResult = 8;
                Assert.Equal(expectedResult, integer.Value);
                if (vm.GetLastPopped() is Integer lastPopped) {
                    
                    _testOutputHelper.WriteLine($"Last Popped Expected: {expectedResult}, got: {lastPopped.Value}");
                    Assert.Equal(expectedResult, lastPopped.Value);
                    const int expectedSp = 0;
                    _testOutputHelper.WriteLine($"Stack Pointer Expected: {expectedSp}, got: {vm.StackPointer}");
                    Assert.Equal(expectedSp, vm.StackPointer);
                }
            }
            
        }
        
    }
    
    [Fact]
    private void TestIntegerArithmeticMultiply() {
        var tests = new List<TestCase>() {
            new TestCase("10*2", 
                new List<Object>(){new Integer(10),new Integer(2)},
                new List<List<byte>>() {
                    OpCode.Make(OpCode.OP_CONSTANT, [10]),
                    OpCode.Make(OpCode.OP_CONSTANT, [2]),
                    OpCode.Make(OpCode.OP_MUL, []),

                }
            )
        };

        foreach (var testCase in tests) {
            var compiler = new Compiler();
            var program = GetProgram(testCase.Input);
            var result = compiler.Compile(program);
            Assert.True(result);
            var byteCode = compiler.ByteCode();

            var vm = new VirtualMachine(byteCode.Constants, byteCode.Instructions);
            vm.Run();
            foreach (var compilerError in compiler.Errors) {
                _testOutputHelper.WriteLine(compilerError);
            }
            _testOutputHelper.WriteLine("Dumping stack");
            foreach (var obj in vm.Stack[..vm.StackPointer]) {
                _testOutputHelper.WriteLine($"{obj.GetObjectType()} {obj.InspectObject()}");
            }
            if (vm.GetLastPopped() is Integer integer) {
                const int expectedResult = 20;
                Assert.Equal(expectedResult, integer.Value);
                if (vm.GetLastPopped() is Integer lastPopped) {
                    _testOutputHelper.WriteLine($"Last Popped Expected: {expectedResult}, got: {lastPopped.Value}");
                    Assert.Equal(expectedResult, lastPopped.Value);
                    const int expectedSp = 0;
                    _testOutputHelper.WriteLine($"Stack Pointer Expected: {expectedSp}, got: {vm.StackPointer}");
                    Assert.Equal(expectedSp, vm.StackPointer);
                }
            }
            
        }
        
    }
    
    [Fact]
    private void TestIntegerArithmeticDivide() {
        var tests = new List<TestCase>() {
            new TestCase("10/2", 
                new List<Object>(){new Integer(10),new Integer(2)},
                new List<List<byte>>() {
                    OpCode.Make(OpCode.OP_CONSTANT, [10]),
                    OpCode.Make(OpCode.OP_CONSTANT, [2]),
                    OpCode.Make(OpCode.OP_DIV, []),
                }
            )
        };

        foreach (var testCase in tests) {
            var compiler = new Compiler();
            var program = GetProgram(testCase.Input);
            var result = compiler.Compile(program);
            Assert.True(result);
            var byteCode = compiler.ByteCode();

            var vm = new VirtualMachine(byteCode.Constants, byteCode.Instructions);
            vm.Run();
            foreach (var compilerError in compiler.Errors) {
                _testOutputHelper.WriteLine(compilerError);
            }
            _testOutputHelper.WriteLine("Dumping stack");
            foreach (var obj in vm.Stack[..vm.StackPointer]) {
                _testOutputHelper.WriteLine($"{obj.GetObjectType()} {obj.InspectObject()}");
            }
            const int expectedResult = 5;
            if (vm.GetLastPopped() is Integer integer) {
                Assert.Equal(expectedResult, integer.Value);
                if (vm.GetLastPopped() is Integer lastPopped) {
                    _testOutputHelper.WriteLine($"Last Popped Expected: {expectedResult}, got: {lastPopped.Value}");
                    Assert.Equal(expectedResult, lastPopped.Value);
                    const int expectedSp = 0;
                    _testOutputHelper.WriteLine($"Stack Pointer Expected: {expectedSp}, got: {vm.StackPointer}");
                    Assert.Equal(expectedSp, vm.StackPointer);
                }
            }
            
        }
        
    }
    
    [Fact]
    private void TestIntegerBooleanTrue() {
        var tests = new List<TestCase>() {
            new TestCase("10==2", 
                new List<Object>(){new Integer(10),new Integer(2)},
                new List<List<byte>>() {
                    OpCode.Make(OpCode.OP_CONSTANT, [10]),
                    OpCode.Make(OpCode.OP_CONSTANT, [2]),
                    OpCode.Make(OpCode.OP_TRUE, []),
                }
            )
        };

        foreach (var testCase in tests) {
            var compiler = new Compiler();
            var program = GetProgram(testCase.Input);
            var result = compiler.Compile(program);
            Assert.True(result);
            var byteCode = compiler.ByteCode();

            var vm = new VirtualMachine(byteCode.Constants, byteCode.Instructions);
            vm.Run();
            foreach (var compilerError in compiler.Errors) {
                _testOutputHelper.WriteLine(compilerError);
            }
            _testOutputHelper.WriteLine("Dumping stack");
            foreach (var obj in vm.Stack[..vm.StackPointer]) {
                _testOutputHelper.WriteLine($"{obj.GetObjectType()} {obj.InspectObject()}");
            }
            const bool expectedResult = false;
            if (vm.GetLastPopped() is Boolean boolean) {
                _testOutputHelper.WriteLine($"Last Popped Expected: {expectedResult}, got: {boolean.Value}");
                Assert.Equal(expectedResult, boolean.Value);
                const int expectedSp = 0;
                _testOutputHelper.WriteLine($"Stack Pointer Expected: {expectedSp}, got: {vm.StackPointer}");
                Assert.Equal(expectedSp, vm.StackPointer);
                
            }
            
        }
        
    }
    
    [Fact]
    private void TestIntegerBooleanFalse() {
        var tests = new List<TestCase>() {
            new TestCase("10!=2", 
                new List<Object>(){new Integer(10),new Integer(2)},
                new List<List<byte>>() {
                    OpCode.Make(OpCode.OP_CONSTANT, [10]),
                    OpCode.Make(OpCode.OP_CONSTANT, [2]),
                    OpCode.Make(OpCode.OP_FALSE, []),
                }
            )
        };

        foreach (var testCase in tests) {
            var compiler = new Compiler();
            var program = GetProgram(testCase.Input);
            var result = compiler.Compile(program);
            Assert.True(result);
            var byteCode = compiler.ByteCode();
            var vm = new VirtualMachine(byteCode.Constants, byteCode.Instructions);
            vm.Run();
            foreach (var compilerError in compiler.Errors) {
                _testOutputHelper.WriteLine(compilerError);
            }
            _testOutputHelper.WriteLine("Dumping stack");
            foreach (var obj in vm.Stack[..vm.StackPointer]) {
                _testOutputHelper.WriteLine($"{obj.GetObjectType()} {obj.InspectObject()}");
            }
            const bool expectedResult = true;
            if (vm.GetLastPopped() is Boolean boolean) {
                _testOutputHelper.WriteLine($"Last Popped Expected: {expectedResult}, got: {boolean.Value}");
                Assert.Equal(expectedResult, boolean.Value);
                const int expectedSp = 0;
                _testOutputHelper.WriteLine($"Stack Pointer Expected: {expectedSp}, got: {vm.StackPointer}");
                Assert.Equal(expectedSp, vm.StackPointer);
                
            }
            
        }
        
    }
    
    [Fact]
    private void TestBooleanTrue() {
        var tests = new List<TestCase>() {
            new TestCase("false != true", 
                new List<Object>(){new Boolean(false)},
                new List<List<byte>>() {
                    OpCode.Make(OpCode.OP_CONSTANT, [10]),
                    OpCode.Make(OpCode.OP_CONSTANT, [2]),
                    OpCode.Make(OpCode.OP_FALSE, []),
                }
            )
        };

        foreach (var testCase in tests) {
            var compiler = new Compiler();
            var program = GetProgram(testCase.Input);
            var result = compiler.Compile(program);
            Assert.True(result);
            var byteCode = compiler.ByteCode();
            var vm = new VirtualMachine(byteCode.Constants, byteCode.Instructions);
            vm.Run();
            foreach (var compilerError in compiler.Errors) {
                _testOutputHelper.WriteLine(compilerError);
            }
            _testOutputHelper.WriteLine("Dumping stack");
            foreach (var obj in vm.Stack[..vm.StackPointer]) {
                _testOutputHelper.WriteLine($"{obj.GetObjectType()} {obj.InspectObject()}");
            }
            const bool expectedResult = true;
            if (vm.GetLastPopped() is Boolean boolean) {
                _testOutputHelper.WriteLine($"Last Popped Expected: {expectedResult}, got: {boolean.Value}");
                Assert.Equal(expectedResult, boolean.Value);
                const int expectedSp = 0;
                _testOutputHelper.WriteLine($"Stack Pointer Expected: {expectedSp}, got: {vm.StackPointer}");
                Assert.Equal(expectedSp, vm.StackPointer);
                
            }
            
        }
        
    }
    
    [Fact]
    private void TestBooleanFalse() {
        var tests = new List<TestCase>() {
            new TestCase("false == true", 
                new List<Object>(){new Boolean(false)},
                new List<List<byte>>() {
                    OpCode.Make(OpCode.OP_CONSTANT, [10]),
                    OpCode.Make(OpCode.OP_CONSTANT, [2]),
                    OpCode.Make(OpCode.OP_FALSE, []),
                }
            )
        };

        foreach (var testCase in tests) {
            var compiler = new Compiler();
            var program = GetProgram(testCase.Input);
            var result = compiler.Compile(program);
            Assert.True(result);
            var byteCode = compiler.ByteCode();
            var vm = new VirtualMachine(byteCode.Constants, byteCode.Instructions);
            vm.Run();
            foreach (var compilerError in compiler.Errors) {
                _testOutputHelper.WriteLine(compilerError);
            }
            _testOutputHelper.WriteLine("Dumping stack");
            foreach (var obj in vm.Stack[..vm.StackPointer]) {
                _testOutputHelper.WriteLine($"{obj.GetObjectType()} {obj.InspectObject()}");
            }
            const bool expectedResult = false;
            if (vm.GetLastPopped() is Boolean boolean) {
                _testOutputHelper.WriteLine($"Last Popped Expected: {expectedResult}, got: {boolean.Value}");
                Assert.Equal(expectedResult, boolean.Value);
                const int expectedSp = 0;
                _testOutputHelper.WriteLine($"Stack Pointer Expected: {expectedSp}, got: {vm.StackPointer}");
                Assert.Equal(expectedSp, vm.StackPointer);
                
            }
            
        }
        
    }
    
    [Fact]
    private void TestBooleanFalseBang() {
        var tests = new List<TestCase>() {
            new TestCase("!true", 
                new List<Object>(){new Boolean(false)},
                new List<List<byte>>() {
                    OpCode.Make(OpCode.OP_CONSTANT, [10]),
                    OpCode.Make(OpCode.OP_CONSTANT, [2]),
                    OpCode.Make(OpCode.OP_FALSE, []),
                }
            )
        };

        foreach (var testCase in tests) {
            var compiler = new Compiler();
            var program = GetProgram(testCase.Input);
            var result = compiler.Compile(program);
            Assert.True(result);
            var byteCode = compiler.ByteCode();
            var vm = new VirtualMachine(byteCode.Constants, byteCode.Instructions);
            vm.Run();
            foreach (var compilerError in compiler.Errors) {
                _testOutputHelper.WriteLine(compilerError);
            }
            _testOutputHelper.WriteLine("Dumping stack");
            foreach (var obj in vm.Stack[..vm.StackPointer]) {
                _testOutputHelper.WriteLine($"{obj.GetObjectType()} {obj.InspectObject()}");
            }
            const bool expectedResult = false;
            if (vm.GetLastPopped() is Boolean boolean) {
                _testOutputHelper.WriteLine($"Last Popped Expected: {expectedResult}, got: {boolean.Value}");
                Assert.Equal(expectedResult, boolean.Value);
                const int expectedSp = 0;
                _testOutputHelper.WriteLine($"Stack Pointer Expected: {expectedSp}, got: {vm.StackPointer}");
                Assert.Equal(expectedSp, vm.StackPointer);
                
            }
            
        }
        
    }

    
    [Fact]
    private void TestIntegerArithmetic() {
        var tests = new List<TestCase>() {
            new TestCase("-1", 
                new List<Object>(){new Boolean(false)},
                new List<List<byte>>() {
                    OpCode.Make(OpCode.OP_CONSTANT, [0]),
                    OpCode.Make(OpCode.OP_MINUS, []),
                    OpCode.Make(OpCode.OP_POP, []),
                }
            )
        };

        foreach (var testCase in tests) {
            var compiler = new Compiler();
            var program = GetProgram(testCase.Input);
            var result = compiler.Compile(program);
            Assert.True(result);
            var byteCode = compiler.ByteCode();
            var vm = new VirtualMachine(byteCode.Constants, byteCode.Instructions);
            vm.Run();
            foreach (var compilerError in compiler.Errors) {
                _testOutputHelper.WriteLine(compilerError);
            }
            _testOutputHelper.WriteLine("Dumping stack");
            foreach (var obj in vm.Stack[..vm.StackPointer]) {
                _testOutputHelper.WriteLine($"{obj.GetObjectType()} {obj.InspectObject()}");
            }
            const int expectedResult = -1;
            if (vm.GetLastPopped() is Integer integer) {
                _testOutputHelper.WriteLine($"Last Popped Expected: {expectedResult}, got: {integer.Value}");
                Assert.Equal(expectedResult, integer.Value);
                const int expectedSp = 0;
                _testOutputHelper.WriteLine($"Stack Pointer Expected: {expectedSp}, got: {vm.StackPointer}");
                Assert.Equal(expectedSp, vm.StackPointer);
                
            }
            
        }
        
    }

    private Program GetProgram(string input) {
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);
        var program = parser.ParseProgram();
        return program;
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