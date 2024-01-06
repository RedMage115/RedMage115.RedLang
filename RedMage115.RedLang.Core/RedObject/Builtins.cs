using System.Diagnostics.CodeAnalysis;

namespace RedMage115.RedLang.Core.RedObject;

public static class Builtins {
    public static readonly SortedList<string, Builtin> BuiltinFunctionList = new() {
        {"len", new Builtin(BuiltinFunctions.Len)},
        {"first", new Builtin(BuiltinFunctions.First)},
        {"last", new Builtin(BuiltinFunctions.Last)},
        {"tail", new Builtin(BuiltinFunctions.Tail)},
        {"push", new Builtin(BuiltinFunctions.Push)},
        {"print", new Builtin(BuiltinFunctions.Print)},
    };

    public static bool GetBuiltinByName(string name, [MaybeNullWhen(false)]out Builtin builtin) {
        return BuiltinFunctionList.TryGetValue(name, out builtin);
    }
}