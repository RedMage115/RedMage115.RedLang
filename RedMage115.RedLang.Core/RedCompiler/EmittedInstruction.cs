

namespace RedMage115.RedLang.Core.RedCompiler;

public class EmittedInstruction {
    public byte Opcode { get; set; }
    public int Position { get; set; }
    public EmittedInstruction(byte opcode, int position) {
        Opcode = opcode;
        Position = position;
    }
}