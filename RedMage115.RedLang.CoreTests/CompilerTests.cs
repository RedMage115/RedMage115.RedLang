﻿using RedMage115.RedLang.Core.RedCode;
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