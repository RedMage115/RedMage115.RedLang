namespace RedMage115.RedLang.Core.RedCode;

public partial class Definition {
    public string Name { get; set; }
    public List<int> OperandWidths { get; set; }

    public Definition(string name, List<int> operandWidths) {
        Name = name;
        OperandWidths = operandWidths;
    }
}