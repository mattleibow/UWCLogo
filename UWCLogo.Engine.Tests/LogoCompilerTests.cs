namespace UWCLogo.Engine.Tests;

public class LogoCompilerTests
{
    [Theory]
    [InlineData("forward 100")]
    [InlineData("fd 100")]
    [InlineData("fd     100")]
    [InlineData("fd     100     ")]
    [InlineData("     fd     100")]
    [InlineData("    fd     100     ")]
    public void ForwardCommandCompiles(string source)
    {
        var command = LogoCompiler.Compile(source);

        var forward = Assert.IsType<ForwardCommand>(command);
        Assert.Equal(100, forward.Distance);

        Assert.Equal("fd 100", forward.ToString());
    }

    [Theory]
    [InlineData("backward 100")]
    [InlineData("bk 100")]
    [InlineData("bk     100")]
    [InlineData("bk     100     ")]
    [InlineData("     bk     100")]
    [InlineData("    bk     100     ")]
    public void BackwardCommandCompiles(string source)
    {
        var command = LogoCompiler.Compile(source);

        var backward = Assert.IsType<BackwardCommand>(command);
        Assert.Equal(100, backward.Distance);

        Assert.Equal("bk 100", backward.ToString());
    }

    [Theory]
    [InlineData("forward 100", "fd 100")]
    [InlineData("fd 100", "fd 100")]
    [InlineData("backward 100", "bk 100")]
    [InlineData("bk 100", "bk 100")]
    [InlineData("rt 90", "rt 90")]
    [InlineData("right 90", "rt 90")]
    [InlineData("lt 90", "lt 90")]
    [InlineData("left 90", "lt 90")]
    public void SingleCommandCompiles(string source, string expected)
    {
        var command = LogoCompiler.Compile(source);

        Assert.Equal(expected, command.ToString());
    }

    [Theory]
    [InlineData("repeat 4 [ fd 100 ]", "repeat 4 [ fd 100 ]")]
    public void RepeatCommandCompiles(string source, string expected)
    {
        var command = LogoCompiler.Compile(source);

        Assert.Equal(expected, command.ToString());
    }

    [Theory]
    [InlineData("forward 100 backward 60", "fd 100 bk 60")]
    [InlineData("fd 100 bk 60", "fd 100 bk 60")]
    [InlineData("fd 100 bk 60 fd 10 fd 45", "fd 100 bk 60 fd 10 fd 45")]
    [InlineData("fd 100       bk 60", "fd 100 bk 60")]
    [InlineData("fd 100 bk 60   ", "fd 100 bk 60")]
    [InlineData("   fd 100 bk 60", "fd 100 bk 60")]
    [InlineData("   fd 100 bk 60   ", "fd 100 bk 60")]
    [InlineData("   fd 100   bk 60   ", "fd 100 bk 60")]
    public void MultipleCommandsCompiles(string source, string expected)
    {
        var command = LogoCompiler.Compile(source);

        Assert.Equal(expected, command.ToString());
    }

    [Fact]
    public void InvalidCommandThrows()
    {
        var ex = Assert.Throws<InvalidOperationException>(() => LogoCompiler.Compile("invalid"));

        Assert.StartsWith("[0, 7]", ex.Message);
    }

    [Theory]
    [InlineData("forward", "[7, 7]")]
    [InlineData("forward cats", "[8, 12]")]
    public void InvalidFowardCommandThrows(string source, string errorPosition)
    {
        var error = Assert.Throws<InvalidOperationException>(() => LogoCompiler.Compile(source));

        Assert.StartsWith(errorPosition, error.Message);
    }
}
