namespace RedMage115.RedLang.Core.RedObject;

public struct Hash : Object {
    public Dictionary<HashKey, HashPair> Pairs { get; set; }
    
    public Hash(Dictionary<HashKey, HashPair> pairs) {
        Pairs = pairs;
    }

    public Hash() {
        Pairs = [];
    }
    
    public ObjectType GetObjectType() {
        return ObjectType.HASH;
    }

    public string InspectObject() {
        return $"{{{Pairs.Aggregate(string.Empty, (s, pair) => s+=$"{pair.Value.Key.InspectObject()}:{pair.Value.Value.InspectObject()}" + " ").Trim()}}}";
    }
    
}