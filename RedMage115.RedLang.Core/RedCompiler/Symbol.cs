namespace RedMage115.RedLang.Core.RedCompiler;

public class Symbol {
    public string Name { get; set; }
    public SymbolScope Scope { get; set; }
    public int Index { get; set; }
    public Symbol(string name, SymbolScope scope, int index) {
        Name = name;
        Scope = scope;
        Index = index;
    }
}