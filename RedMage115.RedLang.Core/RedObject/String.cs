namespace RedMage115.RedLang.Core.RedObject;

public struct String : Object {
    public string Value { get; set; }
    public String(string value) {
        Value = value;
    }

    public ObjectType GetObjectType() {
        return ObjectType.STRING;
    }

    public string InspectObject() {
        return Value;
    }
}