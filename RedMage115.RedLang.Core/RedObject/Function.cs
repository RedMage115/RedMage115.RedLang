using RedMage115.RedLang.Core.RedAst;

namespace RedMage115.RedLang.Core.RedObject;

public class Function : Object {
    public List<Identifier> Parameters { get; set; } = [];
    public BlockStatement Body { get; set; }
    public Environment Environment { get; set; }

    public Function(BlockStatement body, Environment environment) {
        Body = body;
        Environment = environment;
    }
    
    public Function(List<Identifier> parameters, BlockStatement body, Environment environment) {
        Parameters = parameters;
        Body = body;
        Environment = environment;
    }

    public ObjectType GetObjectType() {
        return ObjectType.FUNCTION;
    }

    public string InspectObject() {
        var funParams = Parameters.Count switch {
            0 => "",
            1 => Parameters.First().Value,
            _ => Parameters.Aggregate(string.Empty, (s, identifier) => s += identifier.Value + " ")
                .Trim()
        };
        return $"fn ({funParams}) {{\n {Body.GetRawStatement()}\n}}";
    }
}