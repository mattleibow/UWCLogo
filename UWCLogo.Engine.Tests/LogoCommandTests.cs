namespace UWCLogo.Engine.Tests;

public class LogoCommandTests
{
    [Fact]
    public void NullCommandsDoesNotThrow()
    {
        var cmd = new GroupCommand(null);
        cmd.Execute(null);
    }

    [Fact]
    public void NullExecuteCommandsDoesNotThrow()
    {
        var cmd = new GroupCommand(new ForwardCommand(100));
        Assert.Throws<ArgumentNullException>(() => cmd.Execute(null));
    }

    [Fact]
    public void RepeatCommandToStringIsCorrect()
    {
        // repeat 3 [ fd 100 ]
        var cmd = new RepeatCommand(3, new ForwardCommand(100));

        var str = cmd.ToString();

        Assert.Equal("repeat 3 [ fd 100 ]", str);
    }

    [Fact]
    public void RepeatCommandWithMultipleCommandsToStringIsCorrect()
    {
        // repeat 3 [ fd 100 rt 90 ]
        var cmd =
            new RepeatCommand(3,
                new GroupCommand(
                    new ForwardCommand(100),
                    new RightCommand(90)));

        var str = cmd.ToString();

        Assert.Equal("repeat 3 [ fd 100 rt 90 ]", str);
    }
}