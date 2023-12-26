namespace RedMage115.RedLang.Core.RedObject;

public struct String : Object, Hashable {
    public string Value { get; set; }

    public HashKey HashKey => GetCachedKey();
    private (string cachedValue, HashKey cachedKey) CachedHashKey { get; set; }
    

    public String(string value) {
        Value = value;
    }

    public ObjectType GetObjectType() {
        return ObjectType.STRING;
    }

    public string InspectObject() {
        return Value;
    }
    
    private HashKey GetCachedKey() {
        if (Value == CachedHashKey.cachedValue) {
            return CachedHashKey.cachedKey;
        }
        CachedHashKey = (Value, new HashKey(GetObjectType(), this));
        return CachedHashKey.cachedKey;
    }
    
}