namespace RedMage115.RedLang.Core.RedCode;

public partial class Definition {
    private static Dictionary<OpCodeType, Definition> _definitionMap = new() {
        { OpCodeType.OP_CONSTANT, new Definition("OpConstant", [2])}
    };

    public static Definition? Lookup(byte opCode) {
        return _definitionMap.GetValueOrDefault(OpCode.GetTypeFromCode(opCode));
    }
    
    public static Definition? Lookup(OpCodeType opCode) {
        return _definitionMap.GetValueOrDefault(opCode);
    }
}