using RedMage115.RedLang.Core.RedObject;

namespace RedMage115.RedLang.Core.RedVm;

public class Frame {
    public CompiledFunction Function { get; set; }
    public int InstructionPointer { get; set; }
    public int BasePointer { get; set; }
    public List<byte> Instructions => Function.Instructions;

    public Frame(CompiledFunction function, int basePointer) {
        Function = function;
        BasePointer = basePointer;
        InstructionPointer = -1;
    }
    public Frame(CompiledFunction function, int instructionPointer, int basePointer) {
        Function = function;
        InstructionPointer = instructionPointer;
        BasePointer = basePointer;
    }
    
}