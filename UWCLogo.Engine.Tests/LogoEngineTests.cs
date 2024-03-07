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
}
