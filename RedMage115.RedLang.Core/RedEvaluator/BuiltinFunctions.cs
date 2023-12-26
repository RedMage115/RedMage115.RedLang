using RedMage115.RedLang.Core.RedObject;
using Object = RedMage115.RedLang.Core.RedObject.Object;
using String = RedMage115.RedLang.Core.RedObject.String;

namespace RedMage115.RedLang.Core.RedEvaluator;

public static partial class Evaluator {
    public static Object Len(List<Object> args) {
        if (args.Count != 1) {
            return new Error($"wrong number of arguments, expected: 1, got: {args.Count}");
        }

        if (args.First() is String @string) {
            return new Integer(@string.Value.Length);
        }
        return new Error($"argument for len not supported, expected: string, got: {args.First().GetObjectType()}");
    }
}