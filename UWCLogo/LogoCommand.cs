namespace UWCLogo;

public abstract record LogoCommand
{
    public abstract void Execute(ILogoEngine logo);
}

public record GroupCommand(params LogoCommand[] Commands) : LogoCommand
{
    public override void Execute(ILogoEngine logo)
    {
        foreach (var command in Commands)
        {
            command.Execute(logo);
        }
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
}

public record ForwardCommand(double Distance) : LogoCommand
{
    public override void Execute(ILogoEngine logo) => logo.Forward(Distance);
}

public record BackwardCommand(double Distance) : LogoCommand
{
    public override void Execute(ILogoEngine logo) => logo.Backward(Distance);
}

public record RightCommand(double Angle) : LogoCommand
{
    public override void Execute(ILogoEngine logo) => logo.Right(Angle);
}

public record LeftCommand(double Angle) : LogoCommand
{
    public override void Execute(ILogoEngine logo) => logo.Left(Angle);
}

public record PenUpCommand : LogoCommand
{
    public override void Execute(ILogoEngine logo) => logo.PenUp();
}

public record PenDownCommand : LogoCommand
{
    public override void Execute(ILogoEngine logo) => logo.PenDown();
}
