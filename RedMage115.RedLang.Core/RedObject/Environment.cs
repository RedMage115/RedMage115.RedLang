namespace RedMage115.RedLang.Core.RedObject;

public class Environment {
    public Dictionary<string, Object> Store { get; } = [];
    public Environment? OuterEnvironment { get; }

    public Environment(Environment outerEnvironment) {
        OuterEnvironment = outerEnvironment;
    }
    
    public Environment() {
        OuterEnvironment = null;
    }


    public bool TryGetValue(string name, out Object? result) {
        if (Store.TryGetValue(name, out var value)) {
            result = value;
            return true;
        }
        if (OuterEnvironment is null) {
            result = null;
            return false;
        }
        if (OuterEnvironment.TryGetValue(name, out var outerValue)) {
            result = outerValue;
            return true;
        }
        result = null;
        return false;
    }

    public bool TrySetValue(string name, Object value) {
        return Store.TryAdd(name, value);
    }
    
}