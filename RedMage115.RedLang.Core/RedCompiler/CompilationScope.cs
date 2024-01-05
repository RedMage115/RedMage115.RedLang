namespace RedMage115.RedLang.Core.RedCompiler;

public class CompilationScope {
    public List<byte> Instructions { get; set; } = [];
    public EmittedInstruction? LastInstruction { get; set; }
    public EmittedInstruction? PreviousInstruction { get; set; }

    public CompilationScope() { }
}