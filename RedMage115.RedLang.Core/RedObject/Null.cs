namespace RedMage115.RedLang.Core.RedObject;

public struct Null : Object {
    public ObjectType GetObjectType() {
        return ObjectType.NULL;
    }

    public string InspectObject() {
        return "null";
    }
}