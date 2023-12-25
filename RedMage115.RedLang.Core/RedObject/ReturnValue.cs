namespace RedMage115.RedLang.Core.RedObject;

public struct ReturnValue : Object {
    public Object Value { get; set; }
    public ReturnValue(Object value) {
        Value = value;
    }

    public ObjectType GetObjectType() {
        return ObjectType.RETURN;
    }
    public string InspectObject() {
        return Value.InspectObject();
    }
}