namespace RedMage115.RedLang.Core.RedObject;

public class CompiledFunction : Object {
    public List<byte> Instructions { get; init; } = [];
    public int NumberOfLocals { get; set; }
    public int NumberOfArguments { get; set; }

    public CompiledFunction(int numberOfArguments) {
        NumberOfArguments = numberOfArguments;
    }

    public CompiledFunction(List<byte> instructions, int numberOfLocals, int numberOfArguments) {
        Instructions = instructions;
        NumberOfLocals = numberOfLocals;
        NumberOfArguments = numberOfArguments;
    }

    public ObjectType GetObjectType() {
        return ObjectType.COMPILED_FUNCTION;
    }

    public string InspectObject() {
        return Instructions.Aggregate("Compiled Function [", (s, b) => s += $"{b},").TrimEnd(',') + "]";
    }
}