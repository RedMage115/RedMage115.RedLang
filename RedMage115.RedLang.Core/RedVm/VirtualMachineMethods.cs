using System.Runtime.Intrinsics.X86;
using RedMage115.RedLang.Core.RedCode;
using RedMage115.RedLang.Core.RedObject;
using Boolean = RedMage115.RedLang.Core.RedObject.Boolean;
using Object = RedMage115.RedLang.Core.RedObject.Object;

namespace RedMage115.RedLang.Core.RedVm;

public partial class VirtualMachine {
    public Object? StackTop() {
        return StackPointer > 0 ? Stack[StackPointer - 1] : null;
    }

    public bool Run() {
        for (var ip = 0; ip < Instructions.Count; ip++) {
            var op = Instructions[ip];
            switch (op) {
                case OpCode.OP_CONSTANT:
                    var constIndex = Definition.ReadUint16(Instructions[(ip + 1)..]);
                    ip += 2;
                    if (!Push(Constants[constIndex])) {
                        return false;
                    }
                    break;
                case OpCode.OP_ADD or OpCode.OP_SUB or OpCode.OP_MUL or OpCode.OP_DIV:
                    var resultBin = ExecuteBinaryOperation(op);
                    if (!resultBin) {
                        return false;
                    }
                    break;
                case OpCode.OP_POP:
                    Pop();
                    break;
                case OpCode.OP_TRUE:
                    if (!Push(True)) {
                        return false;
                    }
                    break;
                case OpCode.OP_FALSE:
                    if (!Push(False)) {
                        return false;
                    }
                    break;
                case OpCode.OP_EQUAL or OpCode.OP_NOT_EQUAL or OpCode.OP_LESS_THAN or OpCode.OP_GREATER_THAN:
                    var resultComp = ExecuteCompareOperation(op);
                    if (!resultComp) {
                        return false;
                    }
                    break;
                case OpCode.OP_BANG:
                    var resultBang = ExecuteBangOperator();
                    if (!resultBang) {
                        return false;
                    }
                    break;
                case OpCode.OP_MINUS:
                    var resultMinus = ExecuteMinusOperator();
                    if (!resultMinus) {
                        return false;
                    }
                    break;
            }
        }

        return true;
    }

    private bool Push(Object obj) {
        if (StackPointer >= StackSize) {
            return false;
        }
        Stack[StackPointer] = obj;
        StackPointer++;
        return true;
    }

    private Object Pop() {
        var obj = Stack[StackPointer - 1];
        StackPointer--;
        return obj;
    }

    public Object GetLastPopped() {
        var obj = Stack[StackPointer];
        return obj;
    }

    private bool ExecuteBinaryOperation(byte opCode) {
        var right = Pop();
        var left = Pop();
        if (left is Integer leftInteger && right is Integer rightInteger) {
            try {
                var result = opCode switch {
                    OpCode.OP_ADD => leftInteger.Value + rightInteger.Value,
                    OpCode.OP_SUB => leftInteger.Value - rightInteger.Value,
                    OpCode.OP_MUL => leftInteger.Value * rightInteger.Value,
                    OpCode.OP_DIV => leftInteger.Value / rightInteger.Value,
                    _ => throw new ArgumentOutOfRangeException(nameof(opCode), opCode, null)
                };
                Push(new Integer(result));
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        return false;
    }
    
    private bool ExecuteCompareOperation(byte opCode) {
        var right = Pop();
        var left = Pop();
        if (left is Integer leftInteger && right is Integer rightInteger) {
            try {
                var result = opCode switch {
                    OpCode.OP_EQUAL => leftInteger.Value == rightInteger.Value,
                    OpCode.OP_NOT_EQUAL => leftInteger.Value != rightInteger.Value,
                    OpCode.OP_LESS_THAN => leftInteger.Value < rightInteger.Value,
                    OpCode.OP_GREATER_THAN => leftInteger.Value > rightInteger.Value,
                    _ => throw new ArgumentOutOfRangeException(nameof(opCode), opCode, null)
                };
                Push(new Boolean(result));
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        else if (left is Boolean leftBoolean && right is Boolean rightBoolean) {
            try {
                var result = opCode switch {
                    OpCode.OP_EQUAL => leftBoolean.Value == rightBoolean.Value,
                    OpCode.OP_NOT_EQUAL => leftBoolean.Value != rightBoolean.Value,
                    _ => throw new ArgumentOutOfRangeException(nameof(opCode), opCode, null)
                };
                switch (result) {
                    case true:
                        Push(True);
                        break;
                    case false:
                        Push(False);
                        break;
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        return false;
    }

    private bool ExecuteBangOperator() {
        var operand = Pop();
        if (operand is Boolean boolean) {
            switch (boolean.Value) {
                case true:
                    Push(False);
                    break;
                case false:
                    Push(True);
                    break;
            }
            return true;
        }
        else {
            return false;
        }
    }
    
    private bool ExecuteMinusOperator() {
        var operand = Pop();
        if (operand is Integer integer) {
            var value = integer.Value;
            Push(new Integer(-value));
            return true;
        }
        else {
            return false;
        }
    }
}