namespace RedMage115.RedLang.Core.RedObject;

public struct Boolean : Object {
    public bool Value { get; set; }
    public Boolean(bool value) {
        Value = value;
    }

    public ObjectType GetObjectType() {
        return ObjectType.BOOLEAN;
    }

    public string InspectObject() {
        return Value ? "True" : "False";
    }
}