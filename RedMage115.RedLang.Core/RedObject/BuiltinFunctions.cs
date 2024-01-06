namespace RedMage115.RedLang.Core.RedObject;

public static class BuiltinFunctions {
    public static Object Len(List<Object> args) {
        if (args.Count != 1) {
            return new Error($"wrong number of arguments, expected: 1, got: {args.Count}");
        }
        var incorrectTypeErr = new Error(
            $"argument for len not supported, expected: string or array, got: {args.First().GetObjectType()}");
        return args.First() switch {
            String @string => new Integer(@string.Value.Length),
            Array array => new Integer(array.Elements.Count),
            _ => incorrectTypeErr
        };
    }

    public static Object First(List<Object> args) {
        if (args.Count != 1) {
            return new Error($"wrong number of arguments, expected: 1, got: {args.Count}");
        }
        var len = Len(args);
        if (len is Error) {
            return new Error("no elements in object");
        }
        if (len is Integer{Value: < 1}) {
            return new Error("no elements in object");
        }
        var incorrectTypeErr = new Error(
            $"argument for first not supported, expected: string or array, got: {args.First().GetObjectType()}");
        return args.First() switch {
            String @string => new String(@string.Value.First().ToString()),
            Array array => array.Elements.First(),
            _ => incorrectTypeErr
        };
    }
    
    public static Object Last(List<Object> args) {
        if (args.Count != 1) {
            return new Error($"wrong number of arguments, expected: 1, got: {args.Count}");
        }
        var len = Len(args);
        if (len is Error) {
            return new Error("no elements in object");
        }
        if (len is Integer{Value: < 1}) {
            return new Error("no elements in object");
        }
        var incorrectTypeErr = new Error(
            $"argument for first not supported, expected: string or array, got: {args.First().GetObjectType()}");
        return args.First() switch {
            String @string => new String(@string.Value.Last().ToString()),
            Array array => array.Elements.Last(),
            _ => incorrectTypeErr
        };
    }
    
    public static Object Tail(List<Object> args) {
        if (args.Count != 1) {
            return new Error($"wrong number of arguments, expected: 1, got: {args.Count}");
        }
        var len = Len(args);
        if (len is Error) {
            return new Error("no elements in object");
        }
        if (len is Integer{Value: < 1}) {
            return new Error("no elements in object");
        }
        var incorrectTypeErr = new Error(
            $"argument for first not supported, expected: string or array, got: {args.First().GetObjectType()}");
        return args.First() switch {
            String @string => new String(@string.Value[1..]),
            Array array => new Array(array.Elements[1..]),
            _ => incorrectTypeErr
        };
    }

    public static Object Push(List<Object> args) {
        if (args.Count != 2) {
            return new Error($"wrong number of arguments, expected: 2, got: {args.Count}");
        }
        var incorrectTypeErr = new Error(
            $"argument for first not supported, expected: string or array, got: {args.First().GetObjectType()}");
        return args[0] switch {
            String @string => new String(@string.Value + args[1]),
            Array array => new Array(array.Elements.Append(args[1]).ToList()),
            _ => incorrectTypeErr
        };
    }

    public static Object Print(List<Object> args) {
        foreach (var o in args) {
            Console.WriteLine(o.InspectObject());
        }
        return new Null();
    }
}