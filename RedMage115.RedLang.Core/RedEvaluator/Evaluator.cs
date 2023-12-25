using System.Diagnostics;
using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedObject;
using Boolean = RedMage115.RedLang.Core.RedObject.Boolean;
using Environment = RedMage115.RedLang.Core.RedObject.Environment;
using Object = RedMage115.RedLang.Core.RedObject.Object;

namespace RedMage115.RedLang.Core.RedEvaluator;

public static class Evaluator {

    public static readonly Boolean True = new(true);
    public static readonly Boolean False = new(false);
    public static readonly Null Null = new();
    
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
                    if (letBindResult.Result == OptionResult.Err) { }
                    break;
                case Identifier identifier:
                    return EvalIdentifier(identifier, environment);
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
        var value = environment.TryGetValue(node.Value);
        if (value is { Result: OptionResult.Ok, Value: not null }) {
            return value.Value;
        }
        return new Error($"identifier not found: {node.Value}");
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