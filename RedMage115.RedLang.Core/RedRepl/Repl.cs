using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedParser;
using RedMage115.RedLang.Core.RedToken;

namespace RedMage115.RedLang.Core.RedRepl;

public static class Repl {
    private const string Prompt = ">> ";

    public static void Start() {
        Console.WriteLine("=======================================");
        Console.WriteLine("""REDLANG REPL - Enter "@exit" to exit """);
        Console.WriteLine("""REDLANG REPL - Enter "@lex" to lex """);
        Console.WriteLine("""REDLANG REPL - Enter "@ast" to get ast """);
        Console.WriteLine("""REDLANG REPL - Enter "@parse" to parse """);
        Console.WriteLine("=======================================");
        var mode = Mode.None;
        while (mode == Mode.None) {
            Console.WriteLine("Enter Mode:");
            Console.Write(Prompt);
            var input = Console.ReadLine();
            switch (input.ToLower()) {
                case "@lex":
                    mode = Mode.Lex;
                    break;
                case "@ast":
                    mode = mode = Mode.Ast;
                    break;
                case "@parse":
                    mode = mode = Mode.Parse;
                    break;
                default:
                    Console.WriteLine("Entered mode is invalid");
                    break;
            }
        }

        if (mode == Mode.Lex) {
            while (true) {
                Console.Write("LEX " + Prompt);
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
        else if (mode == Mode.Ast) {
            while (true) {
                Console.Write("AST " + Prompt);
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;
                if (input == "@exit") {
                    Console.WriteLine("Exiting now...");
                    break;
                }
                var parser = new Parser(new Lexer(input));
                var program = parser.ParseProgram();
                foreach (var error in parser.Errors) {
                    Console.WriteLine($"ERROR: {error}");
                }

                foreach (var statement in program.Statements) {
                    Console.WriteLine(statement.GetNodeTypeString());
                }
            }
        }
        else if (mode == Mode.Parse) {
            while (true) {
                Console.Write("PARSE " + Prompt);
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;
                if (input == "@exit") {
                    Console.WriteLine("Exiting now...");
                    break;
                }
                var parser = new Parser(new Lexer(input));
                var program = parser.ParseProgram();
                foreach (var error in parser.Errors) {
                    Console.WriteLine($"ERROR: {error}");
                }

                foreach (var statement in program.Statements) {
                    Console.WriteLine(statement.GetRawStatement());
                }
            }
        }
    }
    
    private enum Mode {
        None,
        Lex,
        Ast,
        Parse
    }
    
}