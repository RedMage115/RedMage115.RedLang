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
    public List<byte> Instructions { get; set; }

    public Object[] Stack { get; set; }
    public int StackPointer { get; set; }

    private const int StackSize = 2048;

    private readonly Boolean True = new(true);
    private readonly Boolean False = new(false);
    private readonly Null Null = new();
    
    
    public VirtualMachine(List<Object> constants, List<byte> instructions) {
        Constants = constants;
        Instructions = instructions;
        Stack = new Object[StackSize];
        StackPointer = 0;
    }
    
    public VirtualMachine(ByteCode byteCode) {
        Constants = byteCode.Constants;
        Instructions = byteCode.Instructions;
        Stack = new Object[StackSize];
        StackPointer = 0;
    }
    
    
}