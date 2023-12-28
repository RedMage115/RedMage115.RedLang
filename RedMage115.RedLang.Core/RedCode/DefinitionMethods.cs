using System.Buffers.Binary;

namespace RedMage115.RedLang.Core.RedCode;

public partial class Definition {
    private static Dictionary<byte, Definition> _definitionMap = new() {
        { OpCode.OP_CONSTANT, new Definition("OpConstant", [2])},
        { OpCode.OP_ADD, new Definition("OpAdd", [])},
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
                    var result = ReadUint16(instructions[offset..]);
                    operandList[i] = result;
                    break;
            }
            offset += operandWidth;
            i++;
        }

        return (operandList.ToList(), offset);
    }

    public static ushort ReadUint16(List<byte> bytes) {
        var span = new Span<byte>(bytes.ToArray());
        return BinaryPrimitives.ReadUInt16BigEndian(span);
    }
}