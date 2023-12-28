using System.Runtime.Intrinsics.X86;
using RedMage115.RedLang.Core.RedCode;
using RedMage115.RedLang.Core.RedObject;
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
                case OpCode.OP_ADD:
                    var right = Pop();
                    var left = Pop();
                    if (right is Integer rightInteger && left is Integer leftInteger) {
                        var result = rightInteger.Value + leftInteger.Value;
                        Push(new Integer(result));
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
}