namespace RedMage115.RedLang.Core.RedObject;

public struct Option<T> {

    public OptionResult Result { get; set; }
    public T? Value { get; set; }
    

    public Option(OptionResult result, T? value) {
        Result = result;
        Value = value;
    }

    public Option(OptionResult result) {
        Result = result;
    }
}
