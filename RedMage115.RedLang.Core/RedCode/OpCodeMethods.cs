using System.Buffers.Binary;

namespace RedMage115.RedLang.Core.RedCode;

public static class OpCode {
    public const byte OP_CONSTANT = 0;
    public const byte OP_ADD = 1;
    public const byte OP_POP = 2;
    public const byte OP_SUB = 3;
    public const byte OP_MUL = 4;
    public const byte OP_DIV = 5;
    public const byte OP_TRUE = 6;
    public const byte OP_FALSE = 7;
    public const byte OP_EQUAL = 8;
    public const byte OP_NOT_EQUAL = 9;
    public const byte OP_GREATER_THAN = 10;
    public const byte OP_LESS_THAN = 11;
    public const byte OP_MINUS = 12;
    public const byte OP_BANG = 13;
    public const byte OP_JUMP_NOT_TRUE = 14;
    public const byte OP_JUMP = 15;
    public const byte OP_NULL = 16;
    public const byte OP_GET_GLOBAL = 17;
    public const byte OP_SET_GLOBAL = 18;
    public const byte OP_ARRAY = 19;
    public const byte OP_HASH = 20;
    public const byte OP_INDEX = 21;
    public const byte OP_CALL = 22;
    public const byte OP_RETURN_VALUE = 23;
    public const byte OP_RETURN = 24;
    public const byte OP_GET_LOCAL = 25;
    public const byte OP_SET_LOCAL = 26;


    public static string OpCodeToString(List<byte> opcode) {
        var op = opcode.First();
        var inst = opcode[1..].Aggregate("[", (s, b) => s += b.ToString()) + "]";
        return $"[{op}]{inst}";
    }

    public static List<byte> Make(byte opCode, List<int> operands) {
        var def = Definition.Lookup(opCode);
        if (def is null) {
            return [];
        }
        var instructionLen = 1 + def.OperandWidths.Sum();

        var instruction = new byte[instructionLen];
        instruction[0] = opCode;

        var offset = 1;
        for (var i = 0; i < operands.Count; i++) {
            var width = def.OperandWidths[i];
            switch (width) {
                case 2:
                    var span = new Span<byte>(new byte[width]);
                    BinaryPrimitives.WriteUInt16BigEndian(span, (ushort)operands[i]);
                    foreach (var b in span) {
                        instruction[offset] = b;
                        offset++;
                    }
                    break;
                case 1:
                    instruction[offset] = (byte)i;
                    offset += width;
                    break;
            }
        }
        return instruction.ToList();
    }
    
    
}