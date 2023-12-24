using RedMage115.RedLang.Core.RedParser;
using Xunit.Abstractions;

namespace RedMage115.RedLang.CoreTests;

public static class ParserErrorTests {
    public static void TestParserErrors(ITestOutputHelper testOutputHelper, Parser parser, int expectedErrors) {
        foreach (var error in parser.Errors) {
            testOutputHelper.WriteLine(error);
        }
        Assert.Equal(expectedErrors, parser.Errors.Count);
    }
}