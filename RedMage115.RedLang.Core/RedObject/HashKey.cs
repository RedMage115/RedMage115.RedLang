namespace RedMage115.RedLang.Core.RedObject;

public struct HashKey {
    public ObjectType Type { get; set; }
    public long Value { get; set; }
    
    public HashKey(ObjectType type, Boolean boolean) {
        Type = type;
        Value = boolean.Value ? 1 : 0;
    }
    
    public HashKey(ObjectType type, Integer integer) {
        Type = type;
        Value = integer.Value;
    }
    
    public HashKey(ObjectType type, String str) {
        Type = type;
        Value = str.Value.Select(c => (byte)c).Aggregate(0L, (l, b) => l += b);
    }
    
    
}