using System.Buffers.Binary;

namespace RedMage115.RedLang.Core.RedCode;

public partial class OpCode {
    private const byte OP_CONSTANT = 0000;
    
    public static byte GetCodeFromType(OpCodeType codeType) {
        return codeType switch {
            OpCodeType.OP_CONSTANT => OP_CONSTANT,
            _ => throw new ArgumentOutOfRangeException(nameof(codeType), codeType, null)
        };
    }
    
    public static OpCodeType GetTypeFromCode(byte opCode) {
        return opCode switch {
            0 => OpCodeType.OP_CONSTANT,
            _ => throw new ArgumentOutOfRangeException(nameof(opCode), opCode, null)
        };
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
            }
        }
        return instruction.ToList();
    }
    
    
}