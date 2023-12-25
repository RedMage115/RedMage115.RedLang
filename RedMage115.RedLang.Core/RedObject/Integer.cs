namespace RedMage115.RedLang.Core.RedObject;

public struct Integer : Object {
    public long Value { get; set; }

    public Integer(long value) {
        Value = value;
    }

    public ObjectType GetObjectType() {
        return ObjectType.INTEGER;
    }

    public string InspectObject() {
        return Value.ToString();
    }
}