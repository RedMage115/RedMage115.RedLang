using Object = RedMage115.RedLang.Core.RedObject.Object;

namespace RedMage115.RedLang.Core.RedCompiler;

public class ByteCode {
    public List<byte> Instructions { get; set; }
    public List<Object> Constants { get; set; }
    public ByteCode(List<byte> instructions, List<Object> constants) {
        Instructions = instructions;
        Constants = constants;
    }
}