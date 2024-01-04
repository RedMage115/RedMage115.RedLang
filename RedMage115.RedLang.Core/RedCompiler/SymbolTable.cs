using System.Diagnostics.CodeAnalysis;

namespace RedMage115.RedLang.Core.RedCompiler;

public class SymbolTable {
    private Dictionary<string, Symbol> Store { get; set; } = [];
    private int NumDefinitions => Store.Count;

    public Symbol Define(string name) {
        var symbol = new Symbol(name, SymbolScope.GLOBAL, NumDefinitions);
        Store.TryAdd(name, symbol);
        return symbol;
    }

    public bool Resolve(string name, [MaybeNullWhen(false)]out Symbol symbol) {
        return Store.TryGetValue(name, out symbol);
    }
}