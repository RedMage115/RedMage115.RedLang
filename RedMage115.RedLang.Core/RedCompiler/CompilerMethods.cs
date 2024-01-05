using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedCode;
using RedMage115.RedLang.Core.RedObject;
using Boolean = RedMage115.RedLang.Core.RedAst.Boolean;
using Object = RedMage115.RedLang.Core.RedObject.Object;
using String = RedMage115.RedLang.Core.RedObject.String;

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
                if (LastInstructionIs(OpCode.OP_POP)) {
                    RemoveLastPop();
                }
                var jumpPos = Emit(OpCode.OP_JUMP, [9999]);
                var afterConsequencePos = CurrentScope.Instructions.Count;
                ChangeOperand(jntPos, afterConsequencePos);
                if (ifExpression.Alternative is null) {
                    Emit(OpCode.OP_NULL, []);
                }
                else {
                    if (!Compile(ifExpression.Alternative)) {
                        return false;
                    }
                    if (LastInstructionIs(OpCode.OP_POP)) {
                        RemoveLastPop();
                    }
                }
                var afterAltPos = CurrentScope.Instructions.Count;
                ChangeOperand(jumpPos, afterAltPos);
                break;
            case BlockStatement blockStatement:
                foreach (var blkStatement in blockStatement.Statements) {
                    if (!Compile(blkStatement)) {
                        return false;
                    }
                }
                break;
            case LetStatement letStatement:
                if (letStatement.Value is null) {
                    return false;
                }
                if (!Compile(letStatement.Value)) {
                    return false;
                }
                var letSymbol = SymbolTable.Define(letStatement.Name.Value);
                Emit(OpCode.OP_SET_GLOBAL, [letSymbol.Index]);
                break;
            case Identifier identifier:
                if (!SymbolTable.Resolve(identifier.Value, out var identSymbol)) {
                    return false;
                }
                Emit(OpCode.OP_GET_GLOBAL, [identSymbol.Index]);
                break;
            case StringLiteral stringLiteral:
                var strLit = new String(stringLiteral.Value);
                Emit(OpCode.OP_CONSTANT, [AddConstant(strLit)]);
                break;
            case ArrayLiteral arrayLiteral:
                foreach (var arrayLiteralElement in arrayLiteral.Elements) {
                    if (!Compile(arrayLiteralElement)) {
                        return false;
                    }
                }
                Emit(OpCode.OP_ARRAY, [arrayLiteral.Elements.Count]);
                break;
            case HashLiteral hashLiteral:
                var hashKeys = new List<Expression>();
                foreach (var hashLiteralPair in hashLiteral.Pairs) {
                    hashKeys.Add(hashLiteralPair.Key);
                }
                hashKeys.Sort((expression, expression1) => string.CompareOrdinal(expression.GetTokenLiteral(), expression1.GetTokenLiteral()));
                foreach (var hk in hashKeys) {
                    if (!Compile(hk)) {
                        return false;
                    }
                    if (!Compile(hashLiteral.Pairs[hk])) {
                        return false;
                    }
                }

                Emit(OpCode.OP_HASH, [hashLiteral.Pairs.Count * 2]);
                break;
            case IndexExpression indexExpression:
                if (!Compile(indexExpression.Left)) {
                    return false;
                }    
                if (!Compile(indexExpression.Index)) {
                    return false;
                }

                Emit(OpCode.OP_INDEX, []);
                break;
            case FunctionLiteral functionLiteral:
                EnterScope();
                if (!Compile(functionLiteral.Body)) {
                    return false;
                }

                if (LastInstructionIs(OpCode.OP_POP)) {
                    ReplaceLastPopWithReturn();
                }

                if (!LastInstructionIs(OpCode.OP_RETURN_VALUE)) {
                    Emit(OpCode.OP_RETURN, []);
                }
                var instructions = LeaveScope();
                var compiledFunction = new CompiledFunction(instructions);
                Emit(OpCode.OP_CONSTANT, [AddConstant(compiledFunction)]);
                break;
            case ReturnStatement returnStatement:
                if (returnStatement.ReturnValue != null && !Compile(returnStatement.ReturnValue)) {
                    return false;
                }
                Emit(OpCode.OP_RETURN_VALUE, []);
                break;
            case CallExpression callExpression:
                if (!Compile(callExpression.Function)) {
                    return false;
                }
                Emit(OpCode.OP_CALL, []);
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
        var prev = CurrentScope.LastInstruction;
        var last = new EmittedInstruction(opCode, pos);
        CurrentScope.PreviousInstruction = prev;
        CurrentScope.LastInstruction = last;
    }
    
    private int AddInstruction(IEnumerable<byte> instructions) {
        var pos = CurrentScope.Instructions.Count;
        CurrentScope.Instructions.AddRange(instructions);
        return pos;
    }

    private bool LastInstructionIs(byte opCode) {
        if (CurrentScope.Instructions.Count == 0) {
            return false;
        }
        return CurrentScope.LastInstruction?.Opcode == opCode;
    }

    private void ReplaceInstruction(int position, IReadOnlyList<byte> newInstruction) {
        for (var i = 0; i < newInstruction.Count; i++) {
            CurrentScope.Instructions[position + i] = newInstruction[i];
        }
    }

    private void ChangeOperand(int position, int operand) {
        var op = CurrentScope.Instructions[position];
        var newInst = OpCode.Make(op, [operand]);
        ReplaceInstruction(position, newInst);
    }

    private void RemoveLastPop() {
        if (CurrentScope.LastInstruction is null) {
            return;
        }
        CurrentScope.Instructions = CurrentScope.Instructions[..CurrentScope.LastInstruction.Position];
        CurrentScope.LastInstruction = CurrentScope.PreviousInstruction;
    }
    
    private int AddConstant(Object constant) {
        Constants.Add(constant);
        return Constants.Count - 1;
    }
    
    public ByteCode ByteCode() {
        return new ByteCode(CurrentScope.Instructions, Constants);
    }

    private void EnterScope() {
        var scope = new CompilationScope();
        Scopes.Add(scope);
        ScopeIndex++;
    }

    private void ReplaceLastPopWithReturn() {
        if (CurrentScope.LastInstruction is null ) return;
        var lastPos = CurrentScope.LastInstruction.Position;
        ReplaceInstruction(lastPos, OpCode.Make(OpCode.OP_RETURN_VALUE, []));
        CurrentScope.LastInstruction.Opcode = OpCode.OP_RETURN_VALUE;
    }
    
    private List<byte> LeaveScope() {
        var scope = CurrentScope;
        Scopes.RemoveAt(ScopeIndex);
        ScopeIndex--;
        return scope.Instructions;
    }
    
    
    
}