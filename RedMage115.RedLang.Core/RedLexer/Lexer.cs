namespace RedMage115.RedLang.Core.RedLexer;
/// <summary>
/// Lexer
/// </summary>
public partial class Lexer {
    /// <summary>
    /// The string to lex
    /// </summary>
    public string Input { get; set; }
    /// <summary>
    /// Length of the input string
    /// </summary>
    private int Length => Input.Length;
    /// <summary>
    /// Current Position
    /// </summary>
    private int Position { get; set; }
    /// <summary>
    /// Next Position
    /// </summary>
    private int ReadPosition { get; set; }
    /// <summary>
    /// Current Char
    /// </summary>
    private char Ch { get; set; }

    public Lexer(string input) {
        Input = input;
        ReadChar();
    }
}