using RedMage115.RedLang.Core.RedAst;
using RedMage115.RedLang.Core.RedCode;
using RedMage115.RedLang.Core.RedObject;
using Object = RedMage115.RedLang.Core.RedObject.Object;

namespace RedMage115.RedLang.Core.RedCompiler;

public partial class Compiler {
    public bool Compile(Node node) {
        switch (node) {
            case Program program:
                foreach (var statement in program.Statements) {
                    if (!Compile(statement)) {
                        return false;
                    }
                }
                break;
            case ExpressionStatement expressionStatement:
                var ex = expressionStatement.Expression;
                if (ex is null) {
                    return false;
                }
                if (!Compile(ex)) {
                    return false;
                }
                break;
            case InfixExpression infixExpression:
                if (!Compile(infixExpression.Left)) {
                    return false;
                }
                if (!Compile(infixExpression.Right)) {
                    return false;
                }

                switch (infixExpression.Operator) {
                    case "+":
                        Emit(OpCode.OP_ADD, []);
                        break;
                    default:
                        return false;
                }
                
                break;
            case IntegerLiteral integerLiteral:
                var integer = new Integer(integerLiteral.Value);
                Emit(OpCode.OP_CONSTANT, [AddConstant(integer)]);
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