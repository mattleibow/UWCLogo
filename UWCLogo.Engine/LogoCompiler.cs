namespace UWCLogo.Engine;

public class LogoCompiler
{
    public static LogoCommand Compile(string source)
    {
        var cursor = 0;
        return Compile(source, ref cursor);
    }

    private static LogoCommand Compile(ReadOnlySpan<char> source, ref int cursor)
    {
        var commands = new List<LogoCommand>();

        while (cursor < source.Length)
        {
            var (wordStart, wordEnd) = ParseWord(source, ref cursor);

            // no more words found so we're done
            if (wordStart == wordEnd)
                break;

            var word = source[wordStart..wordEnd];

            // check for end of command and we're done
            if (IsCommand(word, "]"))
                break;

            if (IsCommand(word, "clearscreen") || IsCommand(word, "cs"))
            {
                commands.Add(new ClearScreenCommand());
            }
            else if (IsCommand(word, "penup") || IsCommand(word, "pu"))
            {
                commands.Add(new PenUpCommand());
            }
            else if (IsCommand(word, "pendown") || IsCommand(word, "pd"))
            {
                commands.Add(new PenDownCommand());
            }
            else if (IsCommand(word, "forward") || IsCommand(word, "fd"))
            {
                var distance = ParseDouble(source, ref cursor);
                commands.Add(new ForwardCommand(distance));
            }
            else if (IsCommand(word, "backward") || IsCommand(word, "bk"))
            {
                var distance = ParseDouble(source, ref cursor);
                commands.Add(new BackwardCommand(distance));
            }
            else if (IsCommand(word, "left") || IsCommand(word, "lt"))
            {
                var angle = ParseDouble(source, ref cursor);
                commands.Add(new LeftCommand(angle));
            }
            else if (IsCommand(word, "right") || IsCommand(word, "rt"))
            {
                var angle = ParseDouble(source, ref cursor);
                commands.Add(new RightCommand(angle));
            }
            else if (IsCommand(word, "repeat"))
            {
                // read count
                var count = ParseInteger(source, ref cursor);

                // read [
                var (startBracket, endBracket) = ParseWord(source, ref cursor);
                var bracket = source[startBracket..endBracket];
                if (!IsCommand(bracket, "["))
                    throw new InvalidOperationException($"[{startBracket}, {endBracket}] Expected a '[' at position {startBracket} but found '{bracket}'.");

                // read nested commands
                var nestedCommand = Compile(source, ref cursor);

                commands.Add(new RepeatCommand(count, nestedCommand));
            }
            else
            {
                throw new InvalidOperationException($"[{wordStart}, {wordEnd}] I don't know how to {word}.");
            }
        }

        if (commands.Count == 0)
        {
            throw new InvalidOperationException("No commands were found.");
        }

        if (commands.Count == 1)
        {
            return commands[0];
        }

        return new GroupCommand(commands.ToArray());
    }

    private static double ParseDouble(ReadOnlySpan<char> source, ref int cursor)
    {
        var (start, end) = ParseWord(source, ref cursor);
        var word = source[start..end];

        if (word.Length == 0)
            throw new InvalidOperationException($"[{start}, {end}] Expected a number at position {start}.");

        if (!double.TryParse(word, out var number))
            throw new InvalidOperationException($"[{start}, {end}] Expected a number at position {start} but found '{word}'.");

        return number;
    }

    private static int ParseInteger(ReadOnlySpan<char> source, ref int cursor)
    {
        var (start, end) = ParseWord(source, ref cursor);
        var word = source[start..end];

        if (word.Length == 0)
            throw new InvalidOperationException($"[{start}, {end}] Expected an integer at position {start}.");

        if (!int.TryParse(word, out var number))
            throw new InvalidOperationException($"[{start}, {end}] Expected an integer at position {start} but found '{word}'.");

        return number;
    }

    private static bool IsCommand(ReadOnlySpan<char> word, string command) =>
        word.Equals(command, StringComparison.InvariantCultureIgnoreCase);

    private static (int WordStart, int WordEnd) ParseWord(ReadOnlySpan<char> source, ref int cursor)
    {
        // skip whitespace
        while (cursor < source.Length)
        {
            var currentChar = source[cursor];
            if (!char.IsWhiteSpace(currentChar))
                break;

            cursor++;
        }

        // start actual processing

        var wordStart = cursor;

        // find end of word
        while (cursor < source.Length)
        {
            var currentChar = source[cursor];

            if (char.IsWhiteSpace(currentChar))
                break;

            cursor++;
        }

        var wordEnd = cursor;

        return (wordStart, wordEnd);
    }
}
