namespace RedMage115.RedLang.Core.RedObject;

public struct HashPair {
    public Object Key { get; set; }
    public Object Value { get; set; }
    
    public HashPair(Object key, Object value) {
        Key = key;
        Value = value;
    }
}