using RedMage115.RedLang.Core.RedObject;

namespace RedMage115.RedLang.Core.RedVm;

public class Frame {
    public CompiledFunction Function { get; set; }
    public int InstructionPointer { get; set; }
    public List<byte> Instructions => Function.Instructions;

    public Frame(CompiledFunction function) {
        Function = function;
        InstructionPointer = 0;
    }
    public Frame(CompiledFunction function, int instructionPointer) {
        Function = function;
        InstructionPointer = instructionPointer;
    }
    
}