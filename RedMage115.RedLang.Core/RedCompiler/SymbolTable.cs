using System.Diagnostics.CodeAnalysis;

namespace RedMage115.RedLang.Core.RedCompiler;

public class SymbolTable {
    public SymbolTable? Outer { get; set; }
    public Dictionary<string, Symbol> Store { get; set; } = [];
    public List<Symbol> FreeSymbols { get; set; } = [];
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
    
    public Symbol DefineBuiltin(int index, string name) {
        var symbol = new Symbol(name, SymbolScope.BUILTIN, index);
        Store.TryAdd(name, symbol);
        return symbol;
    }

    private Symbol DefineFree(Symbol original) {
        FreeSymbols.Add(original);

        var symbol = new Symbol(original.Name, SymbolScope.FREE, FreeSymbols.Count - 1);
        Store[original.Name] = symbol;
        return symbol;
    }

    public bool Resolve(string name, [MaybeNullWhen(false)]out Symbol symbol) {
        if (Store.TryGetValue(name, out symbol)) {
            return true;
        }
        if (Outer is not null) {
            if (!Outer.Resolve(name, out symbol)) return false;
            if (symbol.Scope is SymbolScope.GLOBAL or SymbolScope.BUILTIN) {
                return true;
            }
            var free = DefineFree(symbol);
            symbol = free;
            return true;
        }
        return false;
    }
}