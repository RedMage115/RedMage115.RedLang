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
                else {
                    if (!Compile(ex)) {
                        Errors.Add($"Failed to parse expression: {ex}");
                    } 
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
            case PrefixExpression prefixExpression:
                if (!Compile(prefixExpression.Right)) {
                    return false;
                }

                switch (prefixExpression.Operator) {
                    case "!":
                        Emit(OpCode.OP_BANG, []);
                        break;
                    case "-":
                        Emit(OpCode.OP_MINUS, []);
                        break;
                }
                break;
            case IntegerLiteral integerLiteral:
                var integer = new Integer(integerLiteral.Value);
                Emit(OpCode.OP_CONSTANT, [AddConstant(integer)]);
                break;
            case Boolean boolean:
                Emit(boolean.Value ? OpCode.OP_TRUE : OpCode.OP_FALSE, []);
                break;
            case IfExpression ifExpression:
                if (!Compile(ifExpression.Condition)) {
                    return false;
                }
                var jntPos = Emit(OpCode.OP_JUMP_NOT_TRUE, [9999]);
                if (!Compile(ifExpression.Consequence)) {
                    return false;
                }
                if (LastInstructionIsPop()) {
                    RemoveLastPop();
                }
                var jumpPos = Emit(OpCode.OP_JUMP, [9999]);
                var afterConsequencePos = Instructions.Count;
                ChangeOperand(jntPos, afterConsequencePos);
                if (ifExpression.Alternative is null) {
                    Emit(OpCode.OP_NULL, []);
                }
                else {
                    if (!Compile(ifExpression.Alternative)) {
                        return false;
                    }
                    if (LastInstructionIsPop()) {
                        RemoveLastPop();
                    }
                }
                var afterAltPos = Instructions.Count;
                ChangeOperand(jumpPos, afterAltPos);
                break;
            case BlockStatement blockStatement:
                foreach (var blkStatement in blockStatement.Statements) {
                    if (!Compile(blkStatement)) {
                        return false;
                    }
                }
                break;
        }

        return true;
    }

    private int Emit(byte opCode, List<int> operands) {
        var ins = OpCode.Make(opCode, operands);
        var pos = AddInstruction(ins);
        SetLastInstruction(opCode ,pos);
        return pos;
    }

    private void SetLastInstruction(byte opCode, int pos) {
        var prev = LastInstruction;
        var last = new EmittedInstruction(opCode, pos);
        PreviousInstruction = prev;
        LastInstruction = last;
    }
    
    private int AddInstruction(IEnumerable<byte> instructions) {
        var pos = Instructions.Count;
        Instructions.AddRange(instructions);
        return pos;
    }

    private bool LastInstructionIsPop() {
        return LastInstruction?.Opcode == OpCode.OP_POP;
    }

    private void ReplaceInstruction(int position, List<byte> newInstruction) {
        for (var i = 0; i < newInstruction.Count; i++) {
            Instructions[position + i] = newInstruction[i];
        }
    }

    private void ChangeOperand(int position, int operand) {
        var op = Instructions[position];
        var newInst = OpCode.Make(op, [operand]);
        ReplaceInstruction(position, newInst);
    }

    private void RemoveLastPop() {
        if (LastInstruction is null) {
            return;
        }
        Instructions = Instructions[..LastInstruction.Position];
        LastInstruction = PreviousInstruction;
    }
    
    private int AddConstant(Object constant) {
        Constants.Add(constant);
        return Constants.Count - 1;
    }
    
    public ByteCode ByteCode() {
        return new ByteCode(Instructions, Constants);
    }
}