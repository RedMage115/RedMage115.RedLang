using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedCode;
using RedMage115.RedLang.Core.RedObject;
using Boolean = RedMage115.RedLang.Core.RedAst.Boolean;
using Object = RedMage115.RedLang.Core.RedObject.Object;

namespace RedMage115.RedLang.Core.RedCompiler;

public partial class Compiler {
    public bool Compile(Node node) {
        switch (node) {
            case Program program:
                foreach (var statement in program.Statements) {
                    if (!Compile(statement)) {
                        Errors.Add($"Failed to parse statement: {statement}");
                    }
                }
                break;
            case ExpressionStatement expressionStatement:
                var ex = expressionStatement.Expression;
                if (ex is null) {
                    Errors.Add($"Failed to parse expression: expression is null");
                }
                if (!Compile(ex)) {
                    Errors.Add($"Failed to parse expression: {ex}");
                }
                Emit(OpCode.OP_POP, []);
                break;
            case InfixExpression infixExpression:
                if (!Compile(infixExpression.Left)) {
                    Errors.Add($"Failed to parse expression: {infixExpression.Left}");
                }
                if (!Compile(infixExpression.Right)) {
                    Errors.Add($"Failed to parse expression: {infixExpression.Right}");
                }
                switch (infixExpression.Operator) {
                    case "+":
                        Emit(OpCode.OP_ADD, []);
                        break;
                    case "-":
                        Emit(OpCode.OP_SUB, []);
                        break;
                    case "*":
                        Emit(OpCode.OP_MUL, []);
                        break;
                    case "/":
                        Emit(OpCode.OP_DIV, []);
                        break;
                    case "==":
                        Emit(OpCode.OP_EQUAL, []);
                        break;
                    case "!=":
                        Emit(OpCode.OP_NOT_EQUAL, []);
                        break;
                    case "<":
                        Emit(OpCode.OP_LESS_THAN, []);
                        break;
                    case ">":
                        Emit(OpCode.OP_GREATER_THAN, []);
                        break;
                    default:
                        return false;
                }
                break;
            case IntegerLiteral integerLiteral:
                var integer = new Integer(integerLiteral.Value);
                Emit(OpCode.OP_CONSTANT, [AddConstant(integer)]);
                break;
            case Boolean boolean:
                Emit(boolean.Value ? OpCode.OP_TRUE : OpCode.OP_FALSE, []);
                break;
        }

        return true;
    }

    private int Emit(byte opCode, List<int> operands) {
        var ins = OpCode.Make(opCode, operands);
        var pos = AddInstruction(ins);
        return pos;
    }

    private int AddInstruction(IEnumerable<byte> instructions) {
        var pos = Instructions.Count;
        Instructions.AddRange(instructions);
        return pos;
    }
    
    private int AddConstant(Object constant) {
        Constants.Add(constant);
        return Constants.Count - 1;
    }
    
    public ByteCode ByteCode() {
        return new ByteCode(Instructions, Constants);
    }
}