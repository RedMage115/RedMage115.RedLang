namespace RedMage115.RedLang.Core.RedObject;

public struct Error : Object {
    public string Message { get; set; }

    public Error(string message) {
        Message = message;
    }

    public ObjectType GetObjectType() {
        return ObjectType.ERROR;
    }

    public string InspectObject() {
        return $"ERROR: {Message}";
    }
}