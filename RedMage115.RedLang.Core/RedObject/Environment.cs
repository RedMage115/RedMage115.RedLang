namespace RedMage115.RedLang.Core.RedObject;

public class Environment {
    public Dictionary<string, Object> Store { get; } = [];

    public Option<Object> TryGetValue(string name) {
        if (Store.TryGetValue(name, out var value)) {
            return new Option<Object>(OptionResult.Ok, value);
        }
        return new Option<Object>(OptionResult.Err);
    }

    public Option<Object> TrySetValue(string name, Object value) {
        if (Store.TryAdd(name, value)) {
            return new Option<Object>(OptionResult.Ok, value);
        }
        return new Option<Object>(OptionResult.Err);
    }
    
}