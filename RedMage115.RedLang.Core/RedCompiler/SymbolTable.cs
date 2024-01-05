using System.Diagnostics.CodeAnalysis;

namespace RedMage115.RedLang.Core.RedCompiler;

public class SymbolTable {
    public SymbolTable? Outer { get; set; }
    public Dictionary<string, Symbol> Store { get; set; } = [];
    public int NumDefinitions => Store.Count;


    public SymbolTable() { }
    public SymbolTable(SymbolTable? outer) {
        Outer = outer;
    }

    public Symbol Define(string name) {
        var scope = SymbolScope.LOCAL;
        if (Outer is null) {
            scope = SymbolScope.GLOBAL;
        }
        var symbol = new Symbol(name, scope, NumDefinitions);
        Store.TryAdd(name, symbol);
        return symbol;
    }

    public bool Resolve(string name, [MaybeNullWhen(false)]out Symbol symbol) {
        if (Store.TryGetValue(name, out symbol)) {
            return true;
        }
        if (Outer is null) {
            return false;
        }
        return Outer.Resolve(name, out symbol);
    }
}