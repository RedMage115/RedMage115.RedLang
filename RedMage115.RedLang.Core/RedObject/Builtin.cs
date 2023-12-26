namespace RedMage115.RedLang.Core.RedObject;
using BuiltinFunction = Func<List<Object>, Object>;

public struct Builtin : Object {
    public BuiltinFunction Fn { get; set; }
    public Builtin(BuiltinFunction fn) {
        Fn = fn;
    }

    public ObjectType GetObjectType() {
        return ObjectType.BUILTIN;
    }

    public string InspectObject() {
        return "builtin function";
    }
}