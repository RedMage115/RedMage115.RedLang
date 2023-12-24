namespace RedMage115.RedLang.Core.RedParser;

public enum Precedence {
    NONE,
    LOWEST,
    EQUALS,
    LESS_GREATER,
    SUM,
    PRODUCT,
    PREFIX,
    CALL
}