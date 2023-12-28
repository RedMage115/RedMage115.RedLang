using Object = RedMage115.RedLang.Core.RedObject.Object;

namespace RedMage115.RedLang.Core.RedCompiler;

public partial class Compiler {
    private List<byte> Instructions { get; }
    private List<Object> Constants { get; }

    public Compiler() {
        Instructions = [];
        Constants = [];
    }
}