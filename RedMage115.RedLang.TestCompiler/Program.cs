// ReSharper disable StringLiteralTypo

using System.Xml.Serialization;
using RedMage115.RedLang.Core.RedCompiler;
using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedParser;

namespace RedMage115.RedLang.TestCompiler;

public static class Program {
    public static void Main(string[] args) {
        if (string.IsNullOrWhiteSpace(args[0])) {
            Exit("Please pass a file as arg 0");
        }
        var inputFile = args[0];
        if (!File.Exists(inputFile)) {
            Exit($"Failed to read {inputFile}");
        }

        var outputFile = "out.redcode";
        if (!string.IsNullOrWhiteSpace(args[1])) {
            outputFile = args[1];
        }
        var input = File.ReadAllText(inputFile);
        var lexer = new Lexer(input);
       
        var parser = new Parser(lexer);
        foreach (var parserError in parser.Errors) {
            Console.WriteLine(parserError);
        }
        var program = parser.ParseProgram();
        var compiler = new Compiler();
        foreach (var parserError in compiler.Errors) {
            Console.WriteLine(parserError);
        }
        compiler.Compile(program);
        var byteCode = compiler.ByteCode();
        File.WriteAllText(outputFile,"");
        var x = 0;
        foreach (var instruction in byteCode.Instructions) {
            File.AppendAllText(outputFile,instruction.ToString());
            if (x != byteCode.Instructions.Count-1) {
                File.AppendAllText(outputFile,"|");
            }
            x++;
        }
        File.AppendAllText(outputFile,"\n");
        File.AppendAllText(outputFile,"----");
        File.AppendAllText(outputFile,"\n");
        foreach (var constant in byteCode.Constants) {
            File.AppendAllText(outputFile,constant.GetObjectType() + "|" + constant.InspectObject() + '\n');
        }
        Exit($"Output written to: {outputFile}", 0);
    }

    private static void Exit(string message, int code = 1) {
        Console.WriteLine(message);
        Environment.Exit(code);
    }
}