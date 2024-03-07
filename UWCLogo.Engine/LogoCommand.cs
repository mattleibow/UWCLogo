using System.Text;

namespace UWCLogo.Engine;

public abstract record LogoCommand
{
    public abstract void Execute(ILogoEngine logo);
}

public record GroupCommand(params LogoCommand[] Commands) : LogoCommand
{
    public override void Execute(ILogoEngine logo)
    {
        if (Commands is null)
            return;

        ArgumentNullException.ThrowIfNull(logo);

        foreach (var command in Commands)
        {
            command.Execute(logo);
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var command in Commands)
        {
            if (sb.Length > 0)
                sb.Append(" ");

            sb.Append(command);
        }
        return sb.ToString();
    }
}

public record RepeatCommand(int Count, LogoCommand Command) : LogoCommand
{
    public override void Execute(ILogoEngine logo)
    {
        for (var i = 0; i < Count; i++)
        {
            Command.Execute(logo);
        }
    }

    public override string ToString() => $"repeat {Count} [ {Command} ]";
}

public record ForwardCommand(double Distance) : LogoCommand
{
    public override void Execute(ILogoEngine logo) => logo.Forward(Distance);

    public override string ToString() => $"fd {Distance}";
}

public record BackwardCommand(double Distance) : LogoCommand
{
    public override void Execute(ILogoEngine logo) => logo.Backward(Distance);

    public override string ToString() => $"bk {Distance}";
}

public record RightCommand(double Angle) : LogoCommand
{
    public override void Execute(ILogoEngine logo) => logo.Right(Angle);

    public override string ToString() => $"rt {Angle}";
}

public record LeftCommand(double Angle) : LogoCommand
{
    public override void Execute(ILogoEngine logo) => logo.Left(Angle);

    public override string ToString() => $"lt {Angle}";
}

public record PenUpCommand : LogoCommand
{
    public override void Execute(ILogoEngine logo) => logo.PenUp();

    public override string ToString() => $"pu";
}

public record PenDownCommand : LogoCommand
{
    public override void Execute(ILogoEngine logo) => logo.PenDown();

    public override string ToString() => $"pd";
}

public record ClearScreenCommand : LogoCommand
{
    public override void Execute(ILogoEngine logo) => logo.ClearScreen();

    public override string ToString() => $"cs";
}
