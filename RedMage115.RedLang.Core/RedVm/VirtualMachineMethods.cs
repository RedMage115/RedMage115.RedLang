using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using RedMage115.RedLang.Core.RedCode;
using RedMage115.RedLang.Core.RedObject;
using Array = RedMage115.RedLang.Core.RedObject.Array;
using Boolean = RedMage115.RedLang.Core.RedObject.Boolean;
using Object = RedMage115.RedLang.Core.RedObject.Object;
using String = RedMage115.RedLang.Core.RedObject.String;

namespace RedMage115.RedLang.Core.RedVm;

public partial class VirtualMachine {
    public Object? StackTop() {
        return StackPointer > 0 ? Stack[StackPointer - 1] : null;
    }

    public bool Run() {
        int ip;
        List<byte> instructions;
        byte op;
        
        while (true) {
            var curFrame = CurrentFrame;
            ip = CurrentFrame.InstructionPointer;
            instructions = CurrentFrame.Instructions;
            op = instructions[ip];
            
            switch (op) {
                case OpCode.OP_CONSTANT:
                    var constStart = ip + 1;
                    var constEnd = ip + 3;
                    var constByte = instructions[constStart..constEnd];
                    var constIndex = Definition.ReadUint16(constByte);
                    CurrentFrame.InstructionPointer += 2;
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
                case OpCode.OP_JUMP:
                    var jmpPos = (int)Definition.ReadUint16(instructions[(ip + 1)..(ip + 3)]);
                    CurrentFrame.InstructionPointer = jmpPos - 1;
                    break;
                case OpCode.OP_JUMP_NOT_TRUE:
                    var jntPos = (int)Definition.ReadUint16(instructions[(ip + 1)..(ip + 3)]);
                    CurrentFrame.InstructionPointer += 2;
                    var jntCondition = Pop();
                    if (!IsTrue(jntCondition)) {
                        CurrentFrame.InstructionPointer = jntPos - 1;
                    }
                    break;
                case OpCode.OP_NULL:
                    Push(Null);
                    break;
                case OpCode.OP_SET_GLOBAL:
                    var globalIndexSet = (int)Definition.ReadUint16(instructions[(ip + 1)..(ip + 3)]);
                    CurrentFrame.InstructionPointer += 2;
                    Globals[globalIndexSet] = Pop();
                    break;
                case OpCode.OP_GET_GLOBAL:
                    var globalIndexGet = (int)Definition.ReadUint16(instructions[(ip + 1)..(ip + 3)]);
                    CurrentFrame.InstructionPointer += 2;
                    Push(Globals[globalIndexGet]);
                    break;
                case OpCode.OP_ARRAY:
                    var numOfArrayElements = (int)Definition.ReadUint16(instructions[(ip + 1)..(ip + 3)]);
                    CurrentFrame.InstructionPointer += 2;
                    var array = BuildArray(StackPointer-numOfArrayElements, StackPointer);
                    StackPointer -= numOfArrayElements;
                    Push(array);
                    break;
                case OpCode.OP_HASH:
                    var numOfHashElements = (int)Definition.ReadUint16(instructions[(ip + 1)..(ip + 3)]);
                    CurrentFrame.InstructionPointer += 2;
                    var hash = BuildHash(StackPointer-numOfHashElements, StackPointer);
                    StackPointer -= numOfHashElements;
                    if (hash is not null) {
                        Push(hash);
                    }
                    break;
                case OpCode.OP_INDEX:
                    var index = Pop();
                    var left = Pop();
                    if (!ExecuteIndexExpression(left, index)) {
                        return false;
                    }
                    break;
                case OpCode.OP_CALL:
                    if (Stack[StackPointer-1] is not CompiledFunction compiledFunction) {
                        return false;
                    }
                    var frame = new Frame(compiledFunction);
                    PushFrame(frame);
                    break;
                case OpCode.OP_RETURN_VALUE:
                    var returnValue = Pop();
                    PopFrame();
                    Pop();
                    Push(returnValue);
                    break;
                case OpCode.OP_RETURN:
                    PopFrame();
                    Pop();
                    Push(Null);
                    break;
            }

            if (curFrame.InstructionPointer != CurrentFrame.InstructionPointer) {
                continue;
            }
            if (CurrentFrame.InstructionPointer >= CurrentFrame.Instructions.Count - 1) {
                break;
            }
            CurrentFrame.InstructionPointer++;
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
        if (StackPointer-1 < 0) {
            return Null;
        }
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
        if (left is String leftString && right is String rightString) {
            try {
                var result = opCode switch {
                    OpCode.OP_ADD => leftString.Value + rightString.Value,
                    _ => throw new ArgumentOutOfRangeException(nameof(opCode), opCode, null)
                };
                Push(new String(result));
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
        if (operand is Null @null) {
            return Push(True);
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

    private Object BuildArray(int startIndex, int endIndex) {
        var elements = new Object[endIndex - startIndex];
        for (var i = startIndex; i < endIndex; i++) {
            elements[i - startIndex] = Stack[i];
        }
        return new Array(elements.ToList());
    }
    
    private Object? BuildHash(int startIndex, int endIndex) {
        var hashedPairs = new Dictionary<HashKey, HashPair>();
        for (var i = startIndex; i < endIndex; i+=2) {
            var key = Stack[i];
            var value = Stack[i+1];
            var pair = new HashPair(key, value);
            if (key is not Hashable hashableKey) {
                return null;
            }
            hashedPairs.TryAdd(hashableKey.HashKey, pair);
        }

        return new Hash(hashedPairs);
    }

    private bool ExecuteIndexExpression(Object left, Object index) {
        switch (left) {
            case Array array:
                if (index is Integer integer) {
                    return ExecuteArrayIndex(array, integer);
                }
                break;
            case Hash hash:
                return ExecuteHashIndex(hash, index);
        }

        return false;
    }

    private bool ExecuteArrayIndex(Array array, Integer integer) {
        var index = integer.Value;
        var max = (long)array.Elements.Count - 1;
        if (index < 0 || index > max) {
            Push(Null);
            return true;
        }
        Push(array.Elements[(int)index]);
        return true;
    }
    
    private bool ExecuteHashIndex(Hash hash, Object index) {
        if (index is not Hashable hashableKey) {
            return false;
        }

        try {
            var pair = hash.Pairs[hashableKey.HashKey];
            Push(pair.Value);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            Push(Null);
            return true;
        }
        
        return true;
    }

    private bool IsTrue(Object obj) {
        return obj switch {
            Boolean boolean => boolean.Value,
            Null @null => false, 
            _ => true
        };
    }

    private void PushFrame(Frame frame) {
        Frames[FrameIndex] = frame;
        FrameIndex++;
    }

    private Frame PopFrame() {
        var frame = CurrentFrame;
        FrameIndex--;
        return frame;
    }
}