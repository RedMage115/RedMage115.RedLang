namespace RedMage115.RedLang.Core.RedObject;

public struct Array : Object {
    public List<Object> Elements { get; set; }
    public Array(List<Object> elements) {
        Elements = elements;
    }
    public Array() {
        Elements = [];
    }

    public ObjectType GetObjectType() {
        return ObjectType.ARRAY;
    }

    public string InspectObject() {
        return Elements.Aggregate("[", (s, o) => s+=o.InspectObject() + " ").Trim() + "]";
    }
}