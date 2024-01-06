using RedMage115.RedLang.Core.RedCompiler;
using RedMage115.RedLang.Core.RedEvaluator;
using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedObject;
using RedMage115.RedLang.Core.RedParser;
using RedMage115.RedLang.Core.RedToken;
using RedMage115.RedLang.Core.RedVm;
using Environment = RedMage115.RedLang.Core.RedObject.Environment;
using Object = RedMage115.RedLang.Core.RedObject.Object;

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
            var symbolTable = new SymbolTable();
            var globals = new Object[65536];
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
                var compiler = new Compiler(symbolTable);
                compiler.Compile(program);
                var byteCode = compiler.ByteCode();
                var vm = new VirtualMachine(byteCode, globals);
                vm.Run();
                var returnVal = vm.GetLastPopped();
                if (vm.Errors.Count > 0) {
                    Console.WriteLine("ERRORS:");
                }
                foreach (var vmError in vm.Errors) {
                    Console.WriteLine(vmError);
                }
                if (vm.Log.Count > 0) {
                    Console.WriteLine("LOG:");
                }
                foreach (var vmLog in vm.Log) {
                    Console.WriteLine(vmLog);
                }
                if (returnVal is null) {
                    returnVal = new Null();
                }

                Console.WriteLine("Dumping Stack");
                foreach (var s in vm.Stack) {
                    if (s is null) {
                        break;
                    }
                    Console.WriteLine(s.InspectObject());
                }
                if (vm.Errors.Count > 0 || vm.Log.Count > 0) {
                    Console.WriteLine("==========");
                }
                Console.WriteLine(returnVal.InspectObject());
                symbolTable = compiler.SymbolTable;
                globals = vm.Globals;
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