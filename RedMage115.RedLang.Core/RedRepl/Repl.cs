using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedRepl;

public static class Repl {
    private const string Prompt = ">> ";

    public static void Start() {
        Console.WriteLine("=======================================");
        Console.WriteLine("""REDLANG REPL - Enter "@exit" to exit """);
        Console.WriteLine("=======================================");
        while (true) {
            Console.Write(Prompt);
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;
            if (input == "@exit") {
                Console.WriteLine("Exiting now...");
                break;
            }
            var lexer = new Lexer(input);
            var token = lexer.NextToken();
            while (token.Type != TokenType.EOF) {
                Console.WriteLine(token);
                token = lexer.NextToken();
            }
        }
    }
    
}