using System.Globalization;
using RedMage115.RedLang.Core.RedCompiler;
using RedMage115.RedLang.Core.RedLexer;
using RedMage115.RedLang.Core.RedObject;
using RedMage115.RedLang.Core.RedParser;
using RedMage115.RedLang.Core.RedVm;
using Environment = System.Environment;
using Object = RedMage115.RedLang.Core.RedObject.Object;
using String = RedMage115.RedLang.Core.RedObject.String;

namespace RedMage115.RedLang.TestVmRunner;


public static class Program {
    
    public static void Main(string[] args) {
        if (string.IsNullOrWhiteSpace(args[0])) {
            Exit("Please pass a file as arg 0");
        }
        var inputFile = args[0];
        if (!File.Exists(inputFile)) {
            Exit($"Failed to read {inputFile}");
        }
        var input = File.ReadAllLines(inputFile);
        var instStart = 0;
        var instEnd = 0;
        var constStart = 0;
        var constEnd = 0;
        var x = 0;
        foreach (var s in input) {
            if (s == "----") {
                instEnd = x - 1;
                constStart = x + 1;
                constEnd = input.Length - 1;
                break;
            }
            x++;
        }

        var instructions = new List<byte>();
        for (var i = instStart; i <= instEnd; i++) {
            var split = input[i].Split('|');
            foreach (var s in split) {
                instructions.Add(byte.Parse(s));
            }
        }
        var constants = new List<Object>();
        for (var i = constStart; i <= constEnd; i++) {
            var line = input[i].Split('|');
            var typeRaw = line.First();
            var value = line.Last();
            Object type = typeRaw switch {
                "INTEGER" => new Integer(long.Parse(value)),
                "STRING" => new String(value),
                _ => new Null()
            };
           constants.Add(type);
        }
        var vm = new VirtualMachine(constants, instructions);
        vm.Run();
        foreach (var vmError in vm.Errors) {
            Console.WriteLine(vmError);
        }
        Exit("", 0);
    }
    
    private static void Exit(string message, int code = 1) {
        Console.WriteLine(message);
        Console.WriteLine("Press any key exit...");
        Console.ReadLine();
        Environment.Exit(code);
    }
}

