using System.Numerics;
using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedObject;
using RedMage115.RedLang.Core.RedCode;
using RedMage115.RedLang.Core.RedCompiler;
using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedParser;
using RedMage115.RedLang.Core.RedVm;
using Xunit.Abstractions;
using Array = RedMage115.RedLang.Core.RedObject.Array;
using Boolean = RedMage115.RedLang.Core.RedObject.Boolean;
using Object = RedMage115.RedLang.Core.RedObject.Object;
using String = RedMage115.RedLang.Core.RedObject.String;

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
            DumpStack(vm);
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
            DumpStack(vm);
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
            DumpStack(vm);
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
            DumpStack(vm);
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
            DumpStack(vm);
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
            DumpStack(vm);
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
            DumpStack(vm);
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
            DumpStack(vm);
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
            DumpStack(vm);
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
            DumpStack(vm);
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
    
    [Fact]
    private void TestIfStatementTrue() {
        var tests = new List<TestCase>() {
            new TestCase("if(1<2){10}", 
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
            DumpStack(vm);
            const int expectedResult = 10;
            if (vm.GetLastPopped() is Integer integer) {
                _testOutputHelper.WriteLine($"Last Popped Expected: {expectedResult}, got: {integer.Value}");
                Assert.Equal(expectedResult, integer.Value);
                const int expectedSp = 0;
                _testOutputHelper.WriteLine($"Stack Pointer Expected: {expectedSp}, got: {vm.StackPointer}");
                Assert.Equal(expectedSp, vm.StackPointer);
                
            }
            
        }
        
    }
    
    [Fact]
    private void TestIfStatementFalse() {
        var tests = new List<TestCase>() {
            new TestCase("if(1>2){10}else{11}", 
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
            DumpStack(vm);
            const int expectedResult = 11;
            if (vm.GetLastPopped() is Integer integer) {
                _testOutputHelper.WriteLine($"Last Popped Expected: {expectedResult}, got: {integer.Value}");
                Assert.Equal(expectedResult, integer.Value);
                const int expectedSp = 0;
                _testOutputHelper.WriteLine($"Stack Pointer Expected: {expectedSp}, got: {vm.StackPointer}");
                Assert.Equal(expectedSp, vm.StackPointer);
                
            }
            
        }
        
    }
    
    [Fact]
    private void TestIfStatementFalseNoAlt() {
        var tests = new List<TestCase>() {
            new TestCase("if(1>2){10}", 
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
            DumpStack(vm);
            _testOutputHelper.WriteLine($"Expected NULL, got: {vm.GetLastPopped().InspectObject()}");
            Assert.IsType<Null>(vm.GetLastPopped());
            
        }
        
    }
    
    [Fact]
    private void TestLetStatement() {
        var tests = new List<TestCase>() {
            new TestCase("let x = 10; x", 
                new List<Object>(){},
                new List<List<byte>>() {
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
            DumpStack(vm);
            const int expected = 10;
            _testOutputHelper.WriteLine($"Expected {expected}, got: {vm.GetLastPopped().InspectObject()}");
            var lastPopped = vm.GetLastPopped();
            
            if (lastPopped is Integer integer) {
                Assert.Equal(expected, integer.Value);
            }

        }
        
    }
    
    [Fact]
    private void TestLetStatementMultiple() {
        var tests = new List<TestCase>() {
            new TestCase("let x = 10; let y = 11; let z = x + y; z", 
                new List<Object>(){},
                new List<List<byte>>() {
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
            DumpStack(vm);
            const int expected = 21;
            _testOutputHelper.WriteLine($"Expected {expected}, got: {vm.GetLastPopped().InspectObject()}");
            var lastPopped = vm.GetLastPopped();
            
            if (lastPopped is Integer integer) {
                Assert.Equal(expected, integer.Value);
            }

        }
        
    }
    
    [Fact]
    private void TestStringLit() {
        var tests = new List<TestCase>() {
            new TestCase(""" let x = "hello"; x """, 
                new List<Object>(){},
                new List<List<byte>>() {
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
            DumpStack(vm);
            const string expected = "hello";
            _testOutputHelper.WriteLine($"Expected {expected}, got: {vm.GetLastPopped().InspectObject()}");
            var lastPopped = vm.GetLastPopped();
            
            if (lastPopped is String @string) {
                Assert.Equal(expected, @string.Value);
            }

        }
        
    }
    
    [Fact]
    private void TestStringLitAdd() {
        var tests = new List<TestCase>() {
            new TestCase(""" let x = "hello "; let y = "world"; x+y """, 
                new List<Object>(){},
                new List<List<byte>>() {
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
            DumpStack(vm);
            const string expected = "hello world";
            _testOutputHelper.WriteLine($"Expected {expected}, got: {vm.GetLastPopped().InspectObject()}");
            var lastPopped = vm.GetLastPopped();
            
            if (lastPopped is String @string) {
                Assert.Equal(expected, @string.Value);
            }

        }
        
    }
    
    [Fact]
    private void TestArrayLit() {
        var tests = new List<TestCase>() {
            new TestCase(""" let x = [1,2,3]; x[0]""", 
                new List<Object>(){},
                new List<List<byte>>() {
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
            DumpStack(vm);
            const string expected = "1";
            _testOutputHelper.WriteLine($"Expected Array of [1,2,3], got: {vm.GetLastPopped().InspectObject()}");
            var lastPopped = vm.GetLastPopped();
            
            if (lastPopped is Array val) {
                _testOutputHelper.WriteLine($"Expected: {expected}, got: {val.Elements.First().InspectObject()}");
                Assert.Equal(expected, val.Elements.First().InspectObject());
            }

        }
        
    }
    
    [Fact]
    private void TestArrayLitTwo() {
        var tests = new List<TestCase>() {
            new TestCase(""" let x = [1,2,3]; x[1]""", 
                new List<Object>(){},
                new List<List<byte>>() {
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
            DumpStack(vm);
            const string expected = "2";
            _testOutputHelper.WriteLine($"Expected Array of [1,2,3], got: {vm.GetLastPopped().InspectObject()}");
            var lastPopped = vm.GetLastPopped();
            
            if (lastPopped is Array val) {
                _testOutputHelper.WriteLine($"Expected: {expected}, got: {val.Elements.First().InspectObject()}");
                Assert.Equal(expected, val.Elements.First().InspectObject());
            }

        }
        
    }
    
    [Fact]
    private void TestHashLit() {
        var tests = new List<TestCase>() {
            new TestCase(""" let x = {"a":5,"b":10,"c":15}; x["a"]""", 
                new List<Object>(){},
                new List<List<byte>>() {
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
            DumpStack(vm);
            const string expected = "5";
            _testOutputHelper.WriteLine($"Expected Hash of {{\"a\":5,\"b\":10,\"c\":15}}, got: {vm.GetLastPopped().InspectObject()}");
            var lastPopped = vm.GetLastPopped();
            
            if (lastPopped is Hash val) {
                _testOutputHelper.WriteLine($"Expected: {expected}, got: {val.Pairs.First().Value.Value.InspectObject()}");
                Assert.Equal(expected, val.Pairs.First().Value.Value.InspectObject());
            }

        }
        
    }
    
    [Fact]
    private void TestHashLitTwo() {
        var tests = new List<TestCase>() {
            new TestCase(""" let x = {"a":5,"b":10,"c":15}; x["b"]""", 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            const string expected = "10";
            _testOutputHelper.WriteLine($"Expected Hash of {{\"a\":5,\"b\":10,\"c\":15}}, got: {vm.GetLastPopped().InspectObject()}");
            var lastPopped = vm.GetLastPopped();
            
            if (lastPopped is Hash val) {
                _testOutputHelper.WriteLine($"Expected: {expected}, got: {val.Pairs.First().Value.Value.InspectObject()}");
                Assert.Equal(expected, val.Pairs.First().Value.Value.InspectObject());
            }

        }
        
    }
    
    [Fact]
    private void TestFunctionSimpleImplReturn() {
        var tests = new List<TestCase>() {
            new TestCase(""" let x = fn() {5+10}; x(); """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            var lastPopped = vm.GetLastPopped();
            Assert.NotNull(lastPopped);
            const string expected = "15";
            _testOutputHelper.WriteLine($"Expected a function of 5+10, got: {lastPopped.InspectObject()}");
            
            Assert.IsType<Integer>(lastPopped);
            if (lastPopped is Integer val) {
                _testOutputHelper.WriteLine($"Expected: {expected}, got: {val.InspectObject()}");
                Assert.Equal(expected, val.InspectObject());
            }

        }
        
    }
    
    [Fact]
    private void TestFunctionSimpleExpReturn() {
        var tests = new List<TestCase>() {
            new TestCase(""" let x = fn() {return 5+10;}; x(); """, 
                new List<Object>(){},
                new List<List<byte>>() {
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
            DumpStack(vm);
            var lastPopped = vm.GetLastPopped();
            Assert.NotNull(lastPopped);
            const string expected = "15";
            _testOutputHelper.WriteLine($"Expected a function of 5+10, got: {lastPopped.InspectObject()}");
            Assert.IsType<Integer>(lastPopped);
            if (lastPopped is Integer val) {
                _testOutputHelper.WriteLine($"Expected: {expected}, got: {val.InspectObject()}");
                Assert.Equal(expected, val.InspectObject());
            }

        }
        
    }
    
    [Fact]
    private void TestFunctionSimpleExpReturn2() {
        var tests = new List<TestCase>() {
            new TestCase("""fn() {return 5+10;}();""", 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            var lastPopped = vm.GetLastPopped();
            Assert.NotNull(lastPopped);
            const string expected = "15";
            _testOutputHelper.WriteLine($"Expected a function of 5+10, got: {lastPopped.InspectObject()}");
            Assert.IsType<Integer>(lastPopped);
            if (lastPopped is Integer val) {
                _testOutputHelper.WriteLine($"Expected: {expected}, got: {val.InspectObject()}");
                Assert.Equal(expected, val.InspectObject());
            }

        }
        
    }
    
    [Fact]
    private void TestFunctionFirstClass() {
        var tests = new List<TestCase>() {
            new TestCase("""
                         let x = fn(){return 10;};
                         let y = fn(){x;};
                         y()();
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            var lastPopped = vm.GetLastPopped();
            Assert.NotNull(lastPopped);
            const string expected = "10";
            _testOutputHelper.WriteLine($"Expected a function of 5+10, got: {lastPopped.InspectObject()}");
            Assert.IsType<Integer>(lastPopped);
            if (lastPopped is Integer val) {
                _testOutputHelper.WriteLine($"Expected: {expected}, got: {val.InspectObject()}");
                Assert.Equal(expected, val.InspectObject());
            }

        }
        
    }
    
    [Fact]
    private void TestFunctionSimpleNoReturn() {
        var tests = new List<TestCase>() {
            new TestCase("""
                          let fivePlusTen = fn() { let x = 10; };
                          fivePlusTen();
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            var lastPopped = vm.GetLastPopped();
            const string expected = "null";
            _testOutputHelper.WriteLine($"Expected a function of null, got: {lastPopped}");
            
            
            Assert.IsType<Null>(lastPopped);
            _testOutputHelper.WriteLine($"Expected: {expected}, got: {lastPopped.InspectObject()}");
            Assert.Equal(expected, lastPopped.InspectObject());
            

        }
        
    }

    [Fact]
    private void TestLocalVars() {
        var tests = new List<TestCase>() {
            new TestCase("""
                         let glob = 1;
                         let x = fn() {
                            let num = 1;
                            glob+num
                         };
                         let y = fn() {
                            let num = 2;
                            glob+num
                         };
                         x() + y()
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            DumpScopes(compiler, vm);
            DumpLogs(vm);
            var lastPopped = vm.GetLastPopped();
            Assert.NotNull(lastPopped);
            const string expected = "5";
            _testOutputHelper.WriteLine($"Expected a function, got: {lastPopped.InspectObject()}");
            Assert.IsType<Integer>(lastPopped);
            if (lastPopped is Integer val) {
                _testOutputHelper.WriteLine($"Expected: {expected}, got: {val.InspectObject()}");
                Assert.Equal(expected, val.InspectObject());
            }

        }
        
    }
    
    [Fact]
    private void TestLocalVarsSimple() {
        var tests = new List<TestCase>() {
            new TestCase("""
                         let globalVar = 100;
                         let x = fn(){return globalVar;};
                         x();
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            var lastPopped = vm.GetLastPopped();
            Assert.NotNull(lastPopped);
            const string expected = "100";
            _testOutputHelper.WriteLine($"Expected a function of {expected}, got: {lastPopped.InspectObject()}");
            Assert.IsType<Integer>(lastPopped);
            if (lastPopped is Integer val) {
                _testOutputHelper.WriteLine($"Expected: {expected}, got: {val.InspectObject()}");
                Assert.Equal(expected, val.InspectObject());
            }

        }
        
    }
    
    
    [Fact]
    private void TestFunctionParams() {
        var tests = new List<TestCase>() {
            new TestCase("""
                         let manyArg = fn(a, b, c) { a; b; c };
                         manyArg(24, 25, 26);
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            DumpScopes(compiler, vm);
            DumpLogs(vm);
            var lastPopped = vm.GetLastPopped();
            Assert.NotNull(lastPopped);
            const string expected = "26";
            _testOutputHelper.WriteLine($"Expected a function of {expected}, got: {lastPopped.InspectObject()}");
            Assert.IsType<Integer>(lastPopped);
            if (lastPopped is Integer val) {
                _testOutputHelper.WriteLine($"Expected: {expected}, got: {val.InspectObject()}");
                Assert.Equal(expected, val.InspectObject());
            }

        }
        
    }
    
    [Fact]
    private void TestFunctionParamsWithGlobal() {
        var tests = new List<TestCase>() {
            new TestCase("""
                         let globalVar = 100;
                         let manyArg = fn(a, b, c) { a + b + c };
                         manyArg(10, 5, globalVar);
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            DumpScopes(compiler, vm);
            DumpLogs(vm);
            var lastPopped = vm.GetLastPopped();
            Assert.NotNull(lastPopped);
            const string expected = "115";
            _testOutputHelper.WriteLine($"Expected a function of {expected}, got: {lastPopped.InspectObject()}");
            Assert.IsType<Integer>(lastPopped);
            if (lastPopped is Integer val) {
                _testOutputHelper.WriteLine($"Expected: {expected}, got: {val.InspectObject()}");
                Assert.Equal(expected, val.InspectObject());
            }

        }
        
    }
    
    [Fact]
    private void TestFunctionParamsWithGlobalAndLocal() {
        var tests = new List<TestCase>() {
            new TestCase("""
                         let globalVar = 100;
                         let manyArg = fn(a) {
                            let localVar = a + 10;
                            a + localVar + globalVar 
                         };
                         manyArg(10);
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            DumpScopes(compiler, vm);
            DumpLogs(vm);
            var lastPopped = vm.GetLastPopped();
            Assert.NotNull(lastPopped);
            const string expected = "130";
            _testOutputHelper.WriteLine($"Expected a function of {expected}, got: {lastPopped.InspectObject()}");
            Assert.IsType<Integer>(lastPopped);
            if (lastPopped is Integer val) {
                _testOutputHelper.WriteLine($"Expected: {expected}, got: {val.InspectObject()}");
                Assert.Equal(expected, val.InspectObject());
            }

        }
        
    }
    
    [Fact]
    private void TestFunctionParamsWithMultipleCalls() {
        var tests = new List<TestCase>() {
            new TestCase("""
                            let x = fn(a){a+10};
                            let y = fn(a){a+5};
                            let z = x(0) + y(0);
                            z
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            DumpScopes(compiler, vm);
            DumpLogs(vm);
            var lastPopped = vm.GetLastPopped();
            Assert.NotNull(lastPopped);
            const string expected = "15";
            _testOutputHelper.WriteLine($"Expected a function of 15, got: {lastPopped.InspectObject()}");
            Assert.IsType<Integer>(lastPopped);
            if (lastPopped is Integer val) {
                _testOutputHelper.WriteLine($"Expected: {expected}, got: {val.InspectObject()}");
                Assert.Equal(expected, val.InspectObject());
            }

        }
        
    }
    
    [Fact]
    private void TestFunctionParamsWithWrongArgs() {
        var tests = new List<TestCase>() {
            new TestCase("""
                            let x = fn(a,b){a+10+b};
                            let z = x(0);
                            z
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            DumpScopes(compiler, vm);
            DumpLogs(vm);
            _testOutputHelper.WriteLine($"Expected errors, got: {vm.Errors.Count} errors");
            Assert.Contains("ERROR", vm.Errors.First());

        }
        
    }
    
    [Fact]
    private void TestBuiltinFunctionLen() {
        var tests = new List<TestCase>() {
            new TestCase("""
                            let x = [1,2,3];
                            let z = len(x);
                            z
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            DumpScopes(compiler, vm);
            DumpLogs(vm);

            var top = vm.GetLastPopped();
            Assert.Equal("3",top?.InspectObject());

        }
        
    }
    
    [Fact]
    private void TestEdgeCase() {
        var tests = new List<TestCase>() {
            new TestCase("""
                            let x = fn(a){a+10};
                            x(5)
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            DumpScopes(compiler, vm);
            DumpLogs(vm);

            var top = vm.GetLastPopped();
            Assert.Equal("15",top?.InspectObject());

        }
        
    }
    
    [Fact]
    private void TestClosure() {
        var tests = new List<TestCase>() {
            new TestCase("""
                            let newClosure = fn(a) {
                                fn() { a; };
                            };
                            let closure = newClosure(99);
                            closure();
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            DumpScopes(compiler, vm);
            DumpLogs(vm);

            var top = vm.GetLastPopped();
            Assert.Equal("99",top?.InspectObject());

        }
        
    }
    
    [Fact]
    private void TestClosureBig() {
        var tests = new List<TestCase>() {
            new TestCase("""
                            let newAdder = fn(a, b) {
                                fn(c) { a + b + c };
                            };
                            let adder = newAdder(1, 2);
                            adder(8);
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            DumpScopes(compiler, vm);
            DumpLogs(vm);

            var top = vm.GetLastPopped();
            Assert.Equal("11",top?.InspectObject());

        }
        
    }
    
    [Fact]
    private void TestClosureRecursive() {
        var tests = new List<TestCase>() {
            new TestCase("""
                         let countDown = fn(x) {
                             if (x == 0) {
                                return 0;
                             } else {
                                countDown(x - 1);
                             }
                         };
                         countDown(1);
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            DumpScopes(compiler, vm);
            DumpLogs(vm);

            var top = vm.GetLastPopped();
            Assert.Equal("0",top?.InspectObject());

        }
        
    }
    
    [Fact]
    private void TestClosureRecursive2() {
        var tests = new List<TestCase>() {
            new TestCase("""
                         let countDown = fn(x) {
                         if (x == 0) {
                         return 0;
                         } else {
                         countDown(x - 1);
                         }
                         };
                         let wrapper = fn() {
                         countDown(1);
                         };
                         wrapper();
                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            DumpScopes(compiler, vm);
            DumpLogs(vm);

            var top = vm.GetLastPopped();
            Assert.Equal("0",top?.InspectObject());

        }
        
    }
    
    [Fact]
    private void TestClosureRecursive3() {
        var tests = new List<TestCase>() {
            new TestCase("""
                         let wrapper = fn() {
                         let countDown = fn(x) {
                         if (x == 0) {
                         return 0;
                         } else {
                         countDown(x - 1);
                         }
                         };
                         countDown(1);
                         };
                         wrapper();

                         """, 
                new List<Object>(){},
                new List<List<byte>>() {
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

            DumpStack(vm);
            DumpScopes(compiler, vm);
            DumpLogs(vm);

            var top = vm.GetLastPopped();
            Assert.Equal("0",top?.InspectObject());

        }
        
    }
    
    private Program GetProgram(string input) {
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);
        var program = parser.ParseProgram();
        return program;
    }

    private void DumpStack(VirtualMachine vm ,string test = "") {
        _testOutputHelper.WriteLine(
            string.IsNullOrWhiteSpace(test)
            ? "Dumping stack"
            : $"Dumping stack for {test}");
        var x = 0;
        foreach (var obj in vm.Stack) {
            if (obj == null!) {
                break;
            }
            _testOutputHelper.WriteLine($"[{x}]{obj.GetObjectType()} {obj.InspectObject()}");
            x++;
        }

        _testOutputHelper.WriteLine($"Stack Pointer: {vm.StackPointer}");
        _testOutputHelper.WriteLine(
            string.IsNullOrWhiteSpace(test)
                ? "Dumping frames"
                : $"Dumping frames for {test}");
        foreach (var frame in vm.Frames) {
            if (frame == null!) {
                break;
            }
            _testOutputHelper.WriteLine($"{frame.Closure.Function.InspectObject()} - {frame.Instructions.Aggregate("", (s, b) => s+=b.ToString())} - {frame.InstructionPointer}");
        }
    }

    private void DumpScopes(Compiler compiler, VirtualMachine virtualMachine) {
        _testOutputHelper.WriteLine("Dumping Scopes");
        foreach (var def in compiler.SymbolTable.Store) {
            _testOutputHelper.WriteLine($"{def.Key} - {def.Value.Name} - {def.Value.Scope} {def.Value.Index}");
        }
        
    }

    private void DumpLogs(VirtualMachine virtualMachine) {
        _testOutputHelper.WriteLine("Dumping Logs");
        foreach (var line in virtualMachine.Log) {
            _testOutputHelper.WriteLine(line);
        }
        _testOutputHelper.WriteLine("Log Dump Finished");
        _testOutputHelper.WriteLine("Dumping Errors");
        foreach (var line in virtualMachine.Errors) {
            _testOutputHelper.WriteLine(line);
        }
        _testOutputHelper.WriteLine("Error Dump Finished");
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