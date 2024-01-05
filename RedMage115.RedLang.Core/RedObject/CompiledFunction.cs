namespace RedMage115.RedLang.Core.RedObject;

public class CompiledFunction : Object {
    public List<byte> Instructions { get; init; } = [];

    public CompiledFunction() { }

    public CompiledFunction(List<byte> instructions) {
        Instructions = instructions;
    }

    public ObjectType GetObjectType() {
        return ObjectType.COMPILED_FUNCTION;
    }

    public string InspectObject() {
        return Instructions.Aggregate("Compiled Function [", (s, b) => s += $"{b},").TrimEnd(',') + "]";
    }
}