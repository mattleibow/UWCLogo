using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NSubstitute;

namespace UWCLogo.Engine.Tests;

public class LogoEngineTests
{
    [Fact]
    public void ForwardCommandCallsForward()
    {
        var engine = Substitute.For<ILogoEngine>();

        var command = new ForwardCommand(100);

        command.Execute(engine);

        engine.Received().Forward(100);
    }

    [Fact]
    public void AllCommandsFire()
    {
        var engine = Substitute.For<ILogoEngine>();

        var command =
            new GroupCommand(
                new ForwardCommand(100),
                new ClearScreenCommand(),
                new ForwardCommand(100));

        command.Execute(engine);

        Received.InOrder(() =>
        {
            engine.Forward(100);
            engine.ClearScreen();
            engine.Forward(100);
        });
    }

    [Fact]
    public void RandomDoesCallGetNewRandom()
    {
        var command = new RandomCommand(200);

        var engine = Substitute.For<ILogoEngine>();

        command.Execute(engine);

        engine.Received().GetNewRandom(200);
    }

    [Fact]
    public void RandomDoesEvaluateToARandomNumber()
    {
        var command = new RandomCommand(200);

        var engine = Substitute.For<ILogoEngine>();

        engine.GetNewRandom(200)
            .Returns(x => new ConstantDoubleCommandValue(100));

        var value = command.Execute(engine);

        engine.Received().GetNewRandom(200);

        var constant = Assert.IsAssignableFrom<ConstantDoubleCommandValue>(value);

        Assert.Equal(100, constant.Value);
        Assert.Equal(100, value.ToDouble(engine));
    }
}
