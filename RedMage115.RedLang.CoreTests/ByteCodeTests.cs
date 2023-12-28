using RedMage115.RedLang.Core.RedCode;
using Xunit.Abstractions;

namespace RedMage115.RedLang.CoreTests;

public class ByteCodeTests {
    private readonly ITestOutputHelper _testOutputHelper;
    public ByteCodeTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    private void TestMake() {
        var tests = new List<TestCase>() {
            new TestCase(OpCode.GetCodeFromType(OpCodeType.OP_CONSTANT), new List<int>(1){65534}, new List<byte>(){OpCode.GetCodeFromType(OpCodeType.OP_CONSTANT), 255, 254})
        };
        foreach (var testCase in tests) {
            var instruction = OpCode.Make(testCase.OpCode, testCase.Operands);
            for (var i = 0; i < instruction.Count; i++) {
                _testOutputHelper.WriteLine($"Expected: {testCase.Expected[i]}, got: {instruction[i]}");
                Assert.Equal(testCase.Expected[i], instruction[i]);
            }
        }
    }

    private struct TestCase {
        internal byte OpCode { get; set; }
        internal List<int> Operands { get; set; }
        internal List<byte> Expected { get; set; }
        public TestCase(byte opCode, List<int> operands, List<byte> expected) {
            OpCode = opCode;
            Operands = operands;
            Expected = expected;
        }
    }
}