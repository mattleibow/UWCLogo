using System.Runtime.CompilerServices;
using System.Text;

namespace UWCLogo.Engine;

public abstract record LogoCommand
{
    public abstract CommandValue Execute(ILogoEngine logo);
}

public record RandomCommand(int MaxValue) : LogoCommand
{
    public override CommandValue Execute(ILogoEngine logo) =>
        logo.GetNewRandom(MaxValue);

    public override string ToString() => $"random {MaxValue}";
}

public record GroupCommand(params LogoCommand[] Commands) : LogoCommand
{
    public override CommandValue Execute(ILogoEngine logo)
    {
        if (Commands is null)
            return CommandValue.Void;

        ArgumentNullException.ThrowIfNull(logo);

        foreach (var command in Commands)
        {
            command.Execute(logo);
        }

            return CommandValue.Void;
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
    public override CommandValue Execute(ILogoEngine logo)
    {
        for (var i = 0; i < Count; i++)
        {
            Command.Execute(logo);
        }

        return CommandValue.Void;
    }

    public override string ToString() => $"repeat {Count} [ {Command} ]";
}

public record ForwardCommand(CommandValue Distance) : LogoCommand
{
    public override CommandValue Execute(ILogoEngine logo)
    {
        var value = Distance.ToDouble(logo);
        logo.Forward(value);

        return CommandValue.Void;
    }

    public override string ToString() => $"fd {Distance}";
}

public record BackwardCommand(double Distance) : LogoCommand
{
    public override CommandValue Execute(ILogoEngine logo)
    {
        logo.Backward(Distance);

        return CommandValue.Void;
    }

    public override string ToString() => $"bk {Distance}";
}

public record RightCommand(double Angle) : LogoCommand
{
    public override CommandValue Execute(ILogoEngine logo)
    {
        logo.Right(Angle);

        return CommandValue.Void;
    }

    public override string ToString() => $"rt {Angle}";
}

public record LeftCommand(double Angle) : LogoCommand
{
    public override CommandValue Execute(ILogoEngine logo)
    {
        logo.Left(Angle);

        return CommandValue.Void;
    }

    public override string ToString() => $"lt {Angle}";
}

public record PenUpCommand : LogoCommand
{
    public override CommandValue Execute(ILogoEngine logo)
    {
        logo.PenUp();

        return CommandValue.Void;
    }

    public override string ToString() => $"pu";
}

public record PenDownCommand : LogoCommand
{
    public override CommandValue Execute(ILogoEngine logo)
    {
        logo.PenDown();

        return CommandValue.Void;
    }

    public override string ToString() => $"pd";
}

public record ClearScreenCommand : LogoCommand
{
    public override CommandValue Execute(ILogoEngine logo)
    {
        logo.ClearScreen();

        return CommandValue.Void;
    }

    public override string ToString() => $"cs";
}

public abstract record CommandValue
{
    public abstract double ToDouble(ILogoEngine logo);

    public static implicit operator CommandValue(double value) =>
        new ConstantDoubleCommandValue(value);

    public static CommandValue Void => new VoidCommandValue();
}

public record VoidCommandValue : CommandValue
{
    public override double ToDouble(ILogoEngine logo) =>
        throw new InvalidOperationException("Void command value cannot be converted to a double");
}

public record ConstantDoubleCommandValue(double Value) : CommandValue
{
    public override double ToDouble(ILogoEngine logo) => Value;

    public override string ToString() =>
        Value.ToString("0.#");
}

public record LogoCommandCommandValue(LogoCommand Command) : CommandValue
{
    public override double ToDouble(ILogoEngine logo) =>
        Command.Execute(logo).ToDouble(logo);

    public override string ToString() => Command.ToString();
}
