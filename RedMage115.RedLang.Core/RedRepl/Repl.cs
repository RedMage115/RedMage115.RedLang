using RedMage115.RedLang.Core.RedCompiler;
using RedMage115.RedLang.Core.RedEvaluator;
using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedParser;
using RedMage115.RedLang.Core.RedToken;
using RedMage115.RedLang.Core.RedVm;
using Environment = RedMage115.RedLang.Core.RedObject.Environment;

namespace RedMage115.RedLang.Core.RedRepl;

public static class Repl {
    private const string Prompt = ">> ";

    public static void Start(ReplMode mode = ReplMode.None) {
        Console.WriteLine("=======================================");
        Console.WriteLine("""REDLANG REPL - Enter "@exit" to exit """);
        Console.WriteLine("""REDLANG REPL - Enter "@lex" to lex """);
        Console.WriteLine("""REDLANG REPL - Enter "@ast" to get ast """);
        Console.WriteLine("""REDLANG REPL - Enter "@parse" to parse """);
        Console.WriteLine("""REDLANG REPL - Enter "@full" to enter REPL """);
        Console.WriteLine("=======================================");
        while (mode == ReplMode.None) {
            Console.WriteLine("Enter Mode:");
            Console.Write(Prompt);
            var input = Console.ReadLine();
            switch (input?.ToLower()) {
                case "@lex":
                    mode = ReplMode.Lex;
                    break;
                case "@ast":
                    mode = ReplMode.Ast;
                    break;
                case "@parse":
                    mode = ReplMode.Parse;
                    break;
                case "@full":
                    mode = ReplMode.Full;
                    break;
                default:
                    Console.WriteLine("Entered mode is invalid");
                    break;
            }
        }

        var environment = new Environment();
        Console.WriteLine($"Mode is: {mode}");
        if (mode == ReplMode.Lex) {
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
        else if (mode == ReplMode.Ast) {
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
        else if (mode == ReplMode.Parse) {
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
        else if (mode == ReplMode.Full) {
            while (true) {
                Console.Write(Prompt);
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
                var compiler = new Compiler();
                compiler.Compile(program);
                var byteCode = compiler.ByteCode();
                var vm = new VirtualMachine(byteCode);
                vm.Run();
                var top = vm.StackTop();
                Console.WriteLine(top is null
                    ? "NULL"
                    : top.InspectObject());
            }
        }
    }
    
    public enum ReplMode {
        None,
        Lex,
        Ast,
        Parse,
        Full
    }
    
}