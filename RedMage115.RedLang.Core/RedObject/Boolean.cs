namespace RedMage115.RedLang.Core.RedObject;

public struct Boolean : Object, Hashable {
    public bool Value { get; set; }
    public HashKey HashKey => GetCachedKey();
    private (bool cachedValue, HashKey cachedKey) CachedHashKey { get; set; }

    public Boolean(bool value) {
        Value = value;
        CachedHashKey = (value, new HashKey(GetObjectType(), this));
    }

    public ObjectType GetObjectType() {
        return ObjectType.BOOLEAN;
    }

    public string InspectObject() {
        return Value ? "True" : "False";
    }

    private HashKey GetCachedKey() {
        if (Value == CachedHashKey.cachedValue) {
            return CachedHashKey.cachedKey;
        }
        CachedHashKey = (Value, new HashKey(GetObjectType(), this));
        return CachedHashKey.cachedKey;
    }
    
}