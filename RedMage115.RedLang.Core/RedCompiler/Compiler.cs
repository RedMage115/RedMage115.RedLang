using RedMage115.RedLang.Core.RedCode;
using Object = RedMage115.RedLang.Core.RedObject.Object;

namespace RedMage115.RedLang.Core.RedCompiler;

public partial class Compiler {
    private List<byte> Instructions { get; set; }
    private List<Object> Constants { get; set; }

    public SymbolTable SymbolTable { get; set; }

    private EmittedInstruction? LastInstruction { get; set; }
    private EmittedInstruction? PreviousInstruction { get; set; }


    public List<string> Errors = [];

    public Compiler() {
        Instructions = [];
        Constants = [];
        SymbolTable = new SymbolTable();
    }
    
    public Compiler(SymbolTable symbolTable) {
        Instructions = [];
        Constants = [];
        SymbolTable = symbolTable;
    }
}