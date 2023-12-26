using RedMage115.RedLang.Core.RedObject;
using Xunit.Abstractions;
using Boolean = RedMage115.RedLang.Core.RedObject.Boolean;
using Object = RedMage115.RedLang.Core.RedObject.Object;
using String = RedMage115.RedLang.Core.RedObject.String;

namespace RedMage115.RedLang.CoreTests;

public class ObjectTests {
    private readonly ITestOutputHelper _testOutputHelper;
    public ObjectTests(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    private void TestHashKeyString() {
        var inputs = new List<(String input, String same, String diff)>() {
            (new String("Hello"), new String("Hello"), new String("Hello1")),
            (new String("Hello1"), new String("Hello1"), new String("Hello")),
            (new String("Hello"), new String("Hello"), new String("Hello "))
        };
        foreach (var inputTuple in inputs) {
            var target = inputTuple.input.HashKey;
            var same = inputTuple.same.HashKey;
            var diff = inputTuple.diff.HashKey;
            _testOutputHelper.WriteLine($"Target: {target.Value}, same: {same.Value}, diff: {diff.Value}");
            Assert.Equal(target, same);
            Assert.NotEqual(target, diff);
        }
    }
    
    [Fact]
    private void TestHashKeyInt() {
        var inputs = new List<(Integer input, Integer same, Integer diff)>() {
            (new Integer(1), new Integer(1), new Integer(2)),
            (new Integer(23123), new Integer(23123), new Integer(1)),
            (new Integer(-100), new Integer(-100), new Integer(100))
        };
        foreach (var inputTuple in inputs) {
            var target = inputTuple.input.HashKey;
            var same = inputTuple.same.HashKey;
            var diff = inputTuple.diff.HashKey;
            _testOutputHelper.WriteLine($"Target: {target.Value}, same: {same.Value}, diff: {diff.Value}");
            Assert.Equal(target, same);
            Assert.NotEqual(target, diff);
        }
    }
    
    [Fact]
    private void TestHashKeyBool() {
        var inputs = new List<(Boolean input, Boolean same, Boolean diff)>() {
            (new Boolean(true), new Boolean(true), new Boolean(false)),
            (new Boolean(false), new Boolean(false), new Boolean(true)),
        };
        foreach (var inputTuple in inputs) {
            var target = inputTuple.input.HashKey;
            var same = inputTuple.same.HashKey;
            var diff = inputTuple.diff.HashKey;
            _testOutputHelper.WriteLine($"Target: {target.Value}, same: {same.Value}, diff: {diff.Value}");
            Assert.Equal(target, same);
            Assert.NotEqual(target, diff);
        }
    }
    
    [Fact]
    private void TestHashKeyChanged() {
        var inputs = new List<String>() {
            new String("Hello"), new String("Hello "), new String("Hello1"),
        };
        foreach (var input in inputs) {
            var s = input;
            var target = s.HashKey;
            s.Value += "aaa";
            var changed = s.HashKey;
            _testOutputHelper.WriteLine($"Target: {target.Value}, changed: {changed.Value}");
            Assert.NotEqual(target, changed);
        }
    }
    
    [Fact]
    private void TestHashKeyTypes() {
        var str = new String("\0").HashKey;
        var integer = new Integer(0).HashKey;
        var boolean = new Boolean(false).HashKey;
        _testOutputHelper.WriteLine($"String: {str.Value}, Int: {integer.Value}, Bool: {boolean.Value}");
        Assert.NotEqual(str, integer);
        Assert.NotEqual(str, boolean);
        Assert.NotEqual(integer, boolean);
    }
}