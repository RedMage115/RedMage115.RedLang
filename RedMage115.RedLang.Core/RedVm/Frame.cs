using RedMage115.RedLang.Core.RedObject;

namespace RedMage115.RedLang.Core.RedVm;

public class Frame {
    public Closure Closure { get; set; }
    public int InstructionPointer { get; set; }
    public int BasePointer { get; set; }
    public List<byte> Instructions => Closure.Function.Instructions;

    public Frame(Closure closure, int basePointer) {
        Closure = closure;
        BasePointer = basePointer;
        InstructionPointer = -1;
    }
    public Frame(Closure closure, int instructionPointer, int basePointer) {
        Closure = closure;
        InstructionPointer = instructionPointer;
        BasePointer = basePointer;
    }
    
}