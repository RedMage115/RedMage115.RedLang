using System.Diagnostics;
using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedObject;
using Boolean = RedMage115.RedLang.Core.RedObject.Boolean;
using Object = RedMage115.RedLang.Core.RedObject.Object;

namespace RedMage115.RedLang.Core.RedEvaluator;

public static class Evaluator {

    public static readonly Boolean True = new(true);
    public static readonly Boolean False = new(false);
    public static readonly Null Null = new();
    
    public static Object? Eval(Node? node) {
        
            switch (node) {
                case Program prog:
                    return EvalProgram(prog.Statements);
                case ExpressionStatement expressionStatement:
                    return Eval(expressionStatement.Expression);
                case IntegerLiteral integerLiteral:
                    return new Integer(integerLiteral.Value);
                case RedAst.Boolean boolean:
                    return boolean.Value switch {
                        true => True,
                        false => False
                    };
                case PrefixExpression prefixExpression:
                    var prefixRight = Eval(prefixExpression.Right);
                    if (IsError(prefixRight)) return prefixRight;
                    return EvalPrefixExpression(prefixExpression.Operator, prefixRight);
                case InfixExpression infixExpression:
                    var infixLeft = Eval(infixExpression.Left);
                    if (IsError(infixLeft)) return infixLeft;
                    var infixRight = Eval(infixExpression.Right);
                    if (IsError(infixRight)) return infixRight;
                    return EvalInfixExpression(infixExpression.Operator, infixLeft, infixRight);
                case BlockStatement blockStatement:
                    return EvalBlockStatement(blockStatement);
                case IfExpression ifExpression:
                    return EvalIfExpression(ifExpression);
                case ReturnStatement returnStatement:
                    var value = Eval(returnStatement.ReturnValue);
                    if (value is null) {
                        return new Error($"return value is null, {returnStatement.GetRawStatement()}");
                    }
                    if (IsError(value)) return value;
                    return new ReturnValue(value);
                    
            }
            return null;
        
    }
    
    private static Object? EvalProgram(List<Statement> statements) {
        Object? result = null;
        foreach (var statement in statements) {
            result = Eval(statement);
            if (result is ReturnValue returnValue) {
                return returnValue.Value;
            }
            if (result is Error error) {
                return error;
            }
        }
        return result;
    }
    
    private static Object? EvalBlockStatement(BlockStatement blockStatement) {
        Object? result = null;
        foreach (var statement in blockStatement.Statements) {
            result = Eval(statement);
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

    private static Object? EvalIfExpression(IfExpression ifExpression) {
        var condition = Eval(ifExpression.Condition);
        if (condition is null) {
            return new Error($"if condition is empty: if ({ifExpression.Condition.GetRawExpression()}) {{...}}");
        }

        if (condition is Error) {
            return condition;
        }
        if (IsTruthy(condition)) {
            return Eval(ifExpression.Consequence);
        }
        if (ifExpression.Alternative is null) return Null;
        return Eval(ifExpression.Alternative);
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