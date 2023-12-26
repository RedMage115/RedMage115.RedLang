namespace RedMage115.RedLang.Core.RedObject;

public struct Integer : Object, Hashable {
    public long Value { get; set; }
    public HashKey HashKey => GetCachedKey();
    private (long cachedValue, HashKey cachedKey) CachedHashKey { get; set; }
    
    public Integer(long value) {
        Value = value;
        CachedHashKey = (value, new HashKey(GetObjectType(), this));
    }

    public ObjectType GetObjectType() {
        return ObjectType.INTEGER;
    }

    public string InspectObject() {
        return Value.ToString();
    }
    
    private HashKey GetCachedKey() {
        if (Value == CachedHashKey.cachedValue) {
            return CachedHashKey.cachedKey;
        }
        CachedHashKey = (Value, new HashKey(GetObjectType(), this));
        return CachedHashKey.cachedKey;
    }
}