using RedMage115.RedLang.Core.RedCode;
using RedMage115.RedLang.Core.RedObject;
using Object = RedMage115.RedLang.Core.RedObject.Object;

namespace RedMage115.RedLang.Core.RedCompiler;

public partial class Compiler {
    public List<string> Errors { get; set; } = [];
    public SymbolTable SymbolTable { get; set; }
    private List<Object> Constants { get; set; }
    private List<CompilationScope> Scopes { get; set; } = [];

    private int ScopeIndex { get; set; } = 0;
    private CompilationScope CurrentScope => Scopes[ScopeIndex];

    public Compiler() {
        Constants = [];
        SymbolTable = new SymbolTable();
        var x = 0;
        foreach (var builtin in Builtins.BuiltinFunctionList) {
            SymbolTable.DefineBuiltin(x, builtin.Key);
            x++;
        }
        Scopes.Add(new CompilationScope());
    }
    
    public Compiler(SymbolTable symbolTable) {
        Constants = [];
        SymbolTable = symbolTable;
        var x = 0;
        foreach (var builtin in Builtins.BuiltinFunctionList) {
            SymbolTable.DefineBuiltin(x, builtin.Key);
            x++;
        }
        Scopes.Add(new CompilationScope());
    }
}