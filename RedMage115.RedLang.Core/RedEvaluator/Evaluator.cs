﻿using System.Diagnostics;
using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedObject;
using Boolean = RedMage115.RedLang.Core.RedObject.Boolean;
using Environment = RedMage115.RedLang.Core.RedObject.Environment;
using Object = RedMage115.RedLang.Core.RedObject.Object;
using String = RedMage115.RedLang.Core.RedObject.String;

namespace RedMage115.RedLang.Core.RedEvaluator;

public static partial class Evaluator {

    private static readonly Boolean True = new(true);
    private static readonly Boolean False = new(false);
    private static readonly Null Null = new();
    private static Dictionary<string, Builtin> Builtins { get; } = new() {
        {"len", new Builtin(Len)},
        {"first", new Builtin(First)},
        {"last", new Builtin(Last)},
        {"tail", new Builtin(Tail)},
        {"push", new Builtin(Push)},
        {"print", new Builtin(Print)},
    };

}