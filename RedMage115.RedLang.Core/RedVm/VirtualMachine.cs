using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedCompiler;
using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedObject;
using RedMage115.RedLang.Core.RedParser;
using Boolean = RedMage115.RedLang.Core.RedObject.Boolean;
using Object = RedMage115.RedLang.Core.RedObject.Object;

namespace RedMage115.RedLang.Core.RedVm;

public partial class VirtualMachine {

    public List<Object> Constants { get; set; }

    public Object[] Stack { get; set; }
    public Object[] Globals { get; set; }

    public Frame[] Frames { get; set; } = new Frame[MaxFrames];
    private int FrameIndex { get; set; }
    public int StackPointer { get; set; }

    private const int StackSize = 2048;
    private const int GlobalsSize = 65536;
    private const int MaxFrames = 1024;

    private readonly Boolean True = new(true);
    private readonly Boolean False = new(false);
    private readonly Null Null = new();
    private Frame CurrentFrame => Frames[FrameIndex-1];
    
    public VirtualMachine(List<Object> constants, List<byte> instructions) {
        Constants = constants;
        Stack = new Object[StackSize];
        Globals = new Object[GlobalsSize];
        StackPointer = 0;
        Frames[0] = new Frame(new CompiledFunction(instructions));
        FrameIndex = 1;
    }
    
    public VirtualMachine(ByteCode byteCode) {
        Constants = byteCode.Constants;
        Stack = new Object[StackSize];
        Globals = new Object[GlobalsSize];
        StackPointer = 0;
        Frames[0] = new Frame(new CompiledFunction([]));
        FrameIndex = 1;
    }
    
    public VirtualMachine(ByteCode byteCode, Object[] globals) {
        Constants = byteCode.Constants;
        Stack = new Object[StackSize];
        Globals = globals;
        StackPointer = 0;
        Frames[0] = new Frame(new CompiledFunction([]));
        FrameIndex = 1;
    }
    
    
}