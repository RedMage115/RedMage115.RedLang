using System.Diagnostics;
using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedObject;
using Array = RedMage115.RedLang.Core.RedObject.Array;
using Boolean = RedMage115.RedLang.Core.RedObject.Boolean;
using Environment = RedMage115.RedLang.Core.RedObject.Environment;
using Object = RedMage115.RedLang.Core.RedObject.Object;
using String = RedMage115.RedLang.Core.RedObject.String;
namespace RedMage115.RedLang.Core.RedEvaluator;

public static partial class Evaluator {
    public static Object Eval(Node? node, Environment environment) {
            switch (node) {
                case Program program:
                    return EvalProgram(program.Statements, environment);
                case ExpressionStatement expressionStatement:
                    return Eval(expressionStatement.Expression, environment);
                case IntegerLiteral integerLiteral:
                    return new Integer(integerLiteral.Value);
                case RedAst.Boolean boolean:
                    return boolean.Value switch {
                        true => True,
                        false => False
                    };
                case PrefixExpression prefixExpression:
                    var prefixRight = Eval(prefixExpression.Right, environment);
                    if (IsError(prefixRight)) return prefixRight;
                    return EvalPrefixExpression(prefixExpression.Operator, prefixRight);
                case InfixExpression infixExpression:
                    var infixLeft = Eval(infixExpression.Left, environment);
                    if (IsError(infixLeft)) return infixLeft;
                    var infixRight = Eval(infixExpression.Right, environment);
                    if (IsError(infixRight)) return infixRight;
                    return EvalInfixExpression(infixExpression.Operator, infixLeft, infixRight);
                case BlockStatement blockStatement:
                    return EvalBlockStatement(blockStatement, environment);
                case IfExpression ifExpression:
                    return EvalIfExpression(ifExpression, environment);
                case ReturnStatement returnStatement:
                    var returnValue = Eval(returnStatement.ReturnValue, environment);
                    return IsError(returnValue) ? returnValue : new ReturnValue(returnValue);
                case LetStatement letStatement:
                    var letValue = Eval(letStatement.Value, environment);
                    if (IsError(letValue)) return letValue;
                    var letBindResult = environment.TrySetValue(letStatement.Name.Value, letValue);
                    break;
                case Identifier identifier:
                    return EvalIdentifier(identifier, environment);
                case FunctionLiteral functionLiteral:
                    var functionParams = functionLiteral.Parameters;
                    var functionBody = functionLiteral.Body;
                    var function = new Function(functionParams, functionBody, environment);
                    return function;
                case CallExpression callExpression:
                    var callFunction = Eval(callExpression.Function, environment);
                    if (IsError(callFunction)) return callFunction;
                    var callArgs = EvalExpressions(callExpression.Arguments, environment);
                    if (callArgs.Count == 1 && callArgs.First() is Error) {
                        return callArgs.First();
                    }
                    return ApplyFunction(callFunction, callArgs);
                case StringLiteral stringLiteral:
                    return new String(stringLiteral.Value);
                case ArrayLiteral arrayLiteral:
                    var arrayElements = EvalExpressions(arrayLiteral.Elements, environment);
                    if (arrayElements.Count == 1 && arrayElements.First() is Error) {
                        return arrayElements.First();
                    }
                    return new Array(arrayElements);
                case IndexExpression indexExpression:
                    var left = Eval(indexExpression.Left, environment);
                    if (left is Error) {
                        return left;
                    }
                    var index = Eval(indexExpression.Index, environment);
                    if (index is Error) {
                        return index;
                    }
                    return EvalIndexExpression(left, index);
                case HashLiteral hashLiteral:
                    return EvalHashLiteral(hashLiteral, environment);
                    
            }
            return Null;
    }
    
    private static Object EvalProgram(List<Statement> statements, Environment environment) {
        Object result = Null;
        foreach (var statement in statements) {
            result = Eval(statement, environment);
            if (result is ReturnValue returnValue) {
                return returnValue.Value;
            }
            if (result is Error error) {
                return error;
            }
        }
        return result;
    }
    
    private static Object EvalBlockStatement(BlockStatement blockStatement, Environment environment) {
        Object result = Null;
        foreach (var statement in blockStatement.Statements) {
            result = Eval(statement, environment);
            if (result is ReturnValue returnValue) {
                return returnValue;
            }
            if (result is Error error) {
                return error;
            }
        }
        return result;
    }

    private static Object EvalPrefixExpression(string @operator, Object right) {
        return @operator switch {
            "!" => EvalBangOperator(right),
            "-" => EvalMinusPrefixOperatorExpression(right),
            _ => new Error($"unknown operator: {@operator}{right.GetObjectType()}")
        };
    }

    private static Object EvalBangOperator(Object right) {
        return right switch {
            Boolean boolean => boolean.Value switch {
                true => False,
                false => True
            },
            Null _ => True,
            _ => False
        };
    }

    private static Object EvalMinusPrefixOperatorExpression(Object right) {
        if (right is not Integer) {
            return new Error($"int expected, got: {right.GetObjectType()}");
        }
        var value = long.Parse(right.InspectObject());
        return new Integer(-value);
    }

    private static Object EvalInfixExpression(string @operator, Object left, Object right) {
        if (left is Integer leftInteger && right is Integer rightInteger) {
            return EvalIntegerInfixExpression(@operator, leftInteger, rightInteger);
        }
        if (left is Boolean leftBoolean && right is Boolean rightBoolean) {
            switch (@operator) {
                case "==":
                    return leftBoolean.Value == rightBoolean.Value ? True : False;
                case "!=":
                    return leftBoolean.Value != rightBoolean.Value ? True : False;
            }
        }
        if (left is String leftString && right is String rightString) {
            return EvalStringInfixExpression(@operator, leftString, rightString);
        }
        
        return new Error($"expected (int operator int) or (bool operator bool), got: {left.GetObjectType()}{@operator}{right.GetObjectType()}");
    }

    private static Object EvalIntegerInfixExpression(string @operator, Integer left, Integer right) {
        return @operator switch {
            "+" => new Integer(left.Value + right.Value),
            "-" => new Integer(left.Value - right.Value),
            "*" => new Integer(left.Value * right.Value),
            "/" => new Integer(left.Value / right.Value),
            "<" => left.Value < right.Value ? True : False,
            ">" => left.Value > right.Value ? True : False,
            "==" => left.Value == right.Value ? True : False,
            "!=" => left.Value != right.Value ? True : False,
            _ => new Error($"unknown infix operator: {@operator}")
        };
    }
    
    private static Object EvalStringInfixExpression(string @operator, String left, String right) {
        return @operator switch {
            "+" => new String(left.Value + right.Value),
            _ => new Error($"unknown infix operator: {@operator}")
        };
    }

    private static Object EvalIfExpression(IfExpression ifExpression, Environment environment) {
        var condition = Eval(ifExpression.Condition, environment);
        if (condition is Null) {
            return new Error($"if condition is empty: if ({ifExpression.Condition.GetRawExpression()}) {{...}}");
        }
        if (condition is Error) {
            return condition;
        }
        if (IsTruthy(condition)) {
            return Eval(ifExpression.Consequence, environment);
        }
        if (ifExpression.Alternative is null) return Null;
        return Eval(ifExpression.Alternative, environment);
    }

    private static Object EvalIdentifier(Identifier node, Environment environment) {
        if (environment.TryGetValue(node.Value, out var identValue) && identValue is not null) {
            return identValue;
        }
        if (Builtins.TryGetValue(node.Value, out var builtin)) {
            return builtin;
        }
        return new Error($"identifier not found: {node.Value}");
    }

    private static List<Object> EvalExpressions(List<Expression> expressions,
        Environment environment) {
        var resultList = new List<Object>();
        foreach (var expression in expressions) {
            var eval = Eval(expression, environment);
            resultList.Add(eval);
            if (IsError(eval)) {
                return resultList;
            }
        }
        return resultList;
    }

    private static Object ApplyFunction(Object callFunction, List<Object> args) {
        if (callFunction is Function function) {
            var extendedEnv = ExtendFunctionEnv(function, args);
            var evaluated = Eval(function.Body, extendedEnv);
            return UnwrapReturnValue(evaluated);
        }
        if (callFunction is Builtin builtin) {
            return builtin.Fn(args);
        }
        return new Error($"not a function: {callFunction.GetObjectType()}");
    }

    private static Environment ExtendFunctionEnv(Function function, List<Object> args) {
        var enclosedEnv = new Environment(function.Environment);
        for (var i = 0; i < args.Count; i++) {
            var ident = function.Parameters[i].Value;
            var value = args[i];
            enclosedEnv.TrySetValue(ident, value);
        }
        return enclosedEnv;
    }

    private static Object EvalIndexExpression(Object left, Object index) {

        switch (left) {
            case Array array:
                if (index is Integer integer) {
                    return EvalArrayIndexExpression(array, integer);
                }
                break;
            case Hash hash:
                if (index is Hashable hashable) {
                    return EvalHashIndexExpression(hash, hashable);
                }
                break;
            default:
                break;
        }
        
        return new Error($"index operator not supported for {left.GetObjectType()}[{index.GetObjectType()}]");
    }

    private static Object EvalArrayIndexExpression(Array left, Integer indexInteger) {
        var index = indexInteger.Value;
        var max = left.Elements.Count - 1L;
        if (index < 0 || index > max) {
            return Null;
        }
        return left.Elements[(int)index];
    }
    
    private static Object EvalHashIndexExpression(Hash left, Hashable index) {
        if (left.Pairs.TryGetValue(index.HashKey, out var pair)) {
            return pair.Value;
        }
        return Null;
    }

    private static Object EvalHashLiteral(HashLiteral hashLiteral, Environment environment) {
        var pairs = new Dictionary<HashKey, HashPair>();
        foreach (var hashLiteralPair in hashLiteral.Pairs) {
            var key = Eval(hashLiteralPair.Key, environment);
            if (IsError(key)) {
                return key;
            }
            if (key is not Hashable hashable) {
                return new Error($"unusable as hash key: {key.GetObjectType()}");
            }
            var value = Eval(hashLiteralPair.Value, environment);
            if (IsError(key)) {
                return value;
            }

            var keyHash = hashable.HashKey;
            pairs.Add(keyHash, new HashPair(key, value));
        }

        return new Hash(pairs);
    }
    
    private static Object UnwrapReturnValue(Object returnObject) {
        if (returnObject is ReturnValue returnValue) {
            return returnValue.Value;
        }
        return returnObject;
    }
    
    
    private static bool IsTruthy(Object @object) {
        return @object switch {
            RedObject.Null => false,
            Boolean boolean => boolean.Value,
            _ => true
        };
    }

    private static bool IsError(Object @object) {
        return @object is Error;
    }
}