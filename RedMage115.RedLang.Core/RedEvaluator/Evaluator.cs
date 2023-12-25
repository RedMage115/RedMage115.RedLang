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
                    return EvalStatements(prog.Statements);
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
                    return EvalPrefixExpression(prefixExpression.Operator, prefixRight);
                case InfixExpression infixExpression:
                    var infixLeft = Eval(infixExpression.Left);
                    var infixRight = Eval(infixExpression.Right);
                    return EvalInfixExpression(infixExpression.Operator, infixLeft, infixRight);
                    
            }
            return null;
        
    }

    private static Object? EvalStatements(List<Statement> statements) {
        Object? result = null;
        foreach (var statement in statements) {
            result = Eval(statement);
        }
        return result;
    }

    private static Object EvalPrefixExpression(string @operator, Object right) {
        return @operator switch {
            "!" => EvalBangOperator(right),
            "-" => EvalMinusPrefixOperatorExpression(right),
            _ => Null
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
            return Null;
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
        
        return Null;
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
            _ => Null
        };
    }
}