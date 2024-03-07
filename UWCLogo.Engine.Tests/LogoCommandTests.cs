using NSubstitute;

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

    [Fact]
    public void Test()
    {
        var engine = Substitute.For<ILogoEngine>();

        engine.GetNewRandom(200)
            .Returns(x => new ConstantDoubleCommandValue(100));
        
        var command =
            new ForwardCommand(
                new LogoCommandCommandValue(
                    new RandomCommand(200)));

        var value = command.Execute(engine);

        Assert.IsType<VoidCommandValue>(value);

        engine.Received().Forward(100);
    }

    [Fact]
    public void RandomDoesEvaluateToARandomNumber()
    {
        var pendingValue = 
            new LogoCommandCommandValue(
                new RandomCommand(200));

        var engine = Substitute.For<ILogoEngine>();

        engine.GetNewRandom(200)
            .Returns(x => new ConstantDoubleCommandValue(100));

        var value = pendingValue.ToDouble(engine);

        engine.Received().GetNewRandom(200);

        Assert.Equal(100, value);
    }
}