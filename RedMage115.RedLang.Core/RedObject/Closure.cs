namespace RedMage115.RedLang.Core.RedObject;

public class Closure : Object {
    public CompiledFunction Function { get; set; }
    public List<Object> Free { get; set; }

    public Closure(CompiledFunction function, List<Object> free) {
        Function = function;
        Free = free;
    }


    public ObjectType GetObjectType() {
        return ObjectType.CLOSURE;
    }

    public string InspectObject() {
        return $"{Function.InspectObject()} {Free.Aggregate("", (s, o) => s+=o.InspectObject()+", ").Trim().Trim(',')}";
    }
}