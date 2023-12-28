using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedObject;
using RedMage115.RedLang.Core.RedCode;
using RedMage115.RedLang.Core.RedCompiler;
using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedParser;
using RedMage115.RedLang.Core.RedVm;
using Xunit.Abstractions;
using Object = RedMage115.RedLang.Core.RedObject.Object;

namespace RedMage115.RedLang.CoreTests;

public class VirtualMachineTests {
    private readonly ITestOutputHelper _testOutputHelper;
    public VirtualMachineTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    private void TestIntegerArithmetic() {
        var tests = new List<TestCase>() {
            new TestCase("10+2", 
                new List<Object>(){new Integer(1),new Integer(2)},
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
            _testOutputHelper.WriteLine("Dumping stack");
            foreach (var obj in vm.Stack[0..vm.StackPointer]) {
                _testOutputHelper.WriteLine($"{obj.GetObjectType()} {obj.InspectObject()}");
            }
            if (vm.Stack[vm.StackPointer-1] is Integer integer) {
                
                Assert.Equal(12, integer.Value);
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