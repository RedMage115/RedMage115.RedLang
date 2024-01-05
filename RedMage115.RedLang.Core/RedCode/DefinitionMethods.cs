using System.Buffers.Binary;

namespace RedMage115.RedLang.Core.RedCode;

public partial class Definition {
    private static Dictionary<byte, Definition> _definitionMap = new() {
        { OpCode.OP_CONSTANT, new Definition("OpConstant", [2])},
        { OpCode.OP_ADD, new Definition("OpAdd", [])},
        { OpCode.OP_POP, new Definition("OpPop", [])},
        { OpCode.OP_SUB, new Definition("OpSub", [])},
        { OpCode.OP_MUL, new Definition("OpMul", [])},
        { OpCode.OP_DIV, new Definition("OpDiv", [])},
        { OpCode.OP_TRUE, new Definition("OpTrue", [])},
        { OpCode.OP_FALSE, new Definition("OpFalse", [])},
        { OpCode.OP_EQUAL, new Definition("OpEqual", [])},
        { OpCode.OP_NOT_EQUAL, new Definition("OpNotEqual", [])},
        { OpCode.OP_GREATER_THAN, new Definition("OpGreaterThan", [])},
        { OpCode.OP_LESS_THAN, new Definition("OpLessThan", [])},
        { OpCode.OP_MINUS, new Definition("OpMinus", [])},
        { OpCode.OP_BANG, new Definition("OpBang", [])},
        { OpCode.OP_JUMP_NOT_TRUE, new Definition("OpJumpNotTrue", [2])},
        { OpCode.OP_JUMP, new Definition("OpJump", [2])},
        { OpCode.OP_NULL, new Definition("OpNull", [])},
        { OpCode.OP_GET_GLOBAL, new Definition("OpGetGlobal", [2])},
        { OpCode.OP_SET_GLOBAL, new Definition("OpSetGlobal", [2])},
        { OpCode.OP_ARRAY, new Definition("OpArray", [2])},
        { OpCode.OP_HASH, new Definition("OpHash", [2])},
        { OpCode.OP_INDEX, new Definition("OpIndex", [])},
        { OpCode.OP_CALL, new Definition("OpCall", [])},
        { OpCode.OP_RETURN_VALUE, new Definition("OpReturnValue", [])},
        { OpCode.OP_RETURN, new Definition("OpReturn", [])},
        { OpCode.OP_GET_LOCAL, new Definition("OpGetLocal", [1])},
        { OpCode.OP_SET_LOCAL, new Definition("OpSetLocal", [1])},
    };

    public static Definition? Lookup(byte opCode) {
        return _definitionMap.GetValueOrDefault(opCode);
    }

    public static (List<int> operands, int offset) ReadOperands(Definition definition, List<byte> instructions) {
        var operandList = new int[definition.OperandWidths.Count];
        var offset = 0;
        var i = 0;
        foreach (var operandWidth in definition.OperandWidths) {
            switch (operandWidth) {
                case 2:
                    var resultTwo = ReadUint16(instructions[offset..]);
                    operandList[i] = resultTwo;
                    break;
                case 1:
                    var resultOne = ReadUint8(instructions[offset..]);
                    operandList[i] = resultOne;
                    break;
            }
            offset += operandWidth;
            i++;
        }

        return (operandList.ToList(), offset);
    }

    public static ushort ReadUint16(List<byte> bytes) {
        if (bytes.Count != 2) {
            throw new ArgumentException(null, nameof(bytes));
        }

        var byteArr = new byte[2];
        var x = 0;
        foreach (var b in bytes) {
            byteArr[x] = b;
            x++;
        }
        var span = new Span<byte>(byteArr);
        return BinaryPrimitives.ReadUInt16BigEndian(span);
    }

    public static byte ReadUint8(List<byte> bytes) {
        return bytes.First();
    }
}