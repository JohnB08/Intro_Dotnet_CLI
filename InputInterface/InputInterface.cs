
namespace Intro_Dotnet_CLI.InputInterface;
public interface IUserInterface
{
    public void Send(string message);
    public T Request<T>(string message);
}
public class UserInterface : IUserInterface
{
    private readonly Dictionary<Type, Func<string, object>> _parsers;
    /// <summary>
    /// Object enabling sending requests and receiving responses to users of the program.
    /// <example>
    /// For Example:
    /// <code>
    /// //Initializing the user interface.
    /// var ui = new UserInterface();
    /// //Sending a request to the user requesting their age:
    /// var userAge = ui.Request{T}();
    /// //Sending a message telling the user program received their response.
    /// ui.Send($"Your age is {userAge}.");
    /// </code>
    /// </example>
    /// </summary>
    public UserInterface()
    {
        _parsers = new Dictionary<Type, Func<string, object>>
        {
            {typeof(int), input=>ParseInt(input)},
            {typeof(string),input => input},
            {typeof(float), input=>ParseFloat(input)},
            {typeof(double), input=>ParseDouble(input)},
            {typeof(decimal), input=>ParseDecimal(input)},
            {typeof(bool), input=>ParseBool(input)}
        };
    }
    /// <summary>
    /// A function requesting a strict typed input from the users.
    /// The following types are supported by the interface:
    /// <list type="bullet">
    /// <item>
    /// <term>string</term>
    /// <description>a datatype representing a string of some characters, usually represents a word, or words.</description>
    /// </item>
    /// <item>
    /// <term>bool</term>
    /// <description>a datatype representing some affirmation, can have the value true or false.</description>
    /// </item>
    /// <item>
    /// <term>decimal</term>
    /// <description>a C# spesific datatype representing an accurate decimal number. more taxing on the system than a float and double, but considered accurate for financial work.</description>
    /// </item>
    /// <item>
    /// <term>float</term>
    /// <description>a datatype representing some decimal number, generally considered inaccurate, but lightweight compared to other decimal datatypes. Ideal for simple decimal operations.</description>
    /// </item>
    /// <item>
    /// <term>double</term>
    /// <description>a datatype representing some decimal number. Has twice the available data accessible for storing values than floats, and are considered somewhat accurate.</description>
    /// </item>
    /// <item>
    /// <term>int</term>
    /// <description>a datatype representing a whole number. max size is (+ -) 2^31</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="message">The message representing the request</param>
    /// <typeparam name="T">The type of data requested</typeparam>
    /// <returns>some value of type T requested from the user</returns>
    public T Request<T>(string message)
    {
        try
        {
            var input = RequestInput(message);
            var success = _parsers.TryGetValue(typeof(T), out var parseFunc);
            if (!success || parseFunc is null) throw new NotImplementedException($"Input of type {typeof(T)} is not yet supported.");
            return (T)parseFunc(input);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Please try again.");
            return Request<T>(message);
        }
    }

    private static bool ParseBool(string input)
    {
        var success = bool.TryParse(input, out var output);
        if (!success) throw new FormatException($"Input {input} is not a valid boolean value.");
        return output;
    }

    private static int ParseInt(string input)
    {
        var success = int.TryParse(input, out var output);
        if (!success) throw new FormatException($"Input {input} could not be parsed, expected type of {typeof(int)}.");
        return output;
    }
    private static float ParseFloat(string input)
    {
        var success = float.TryParse(input, out var output);
        if (!success) throw new FormatException($"Input {input} could not be parsed, expected type of {typeof(float)}.");
        return output;
    }
    private static double ParseDouble(string input)
    {
        var success = double.TryParse(input, out var output);
        if (!success) throw new FormatException($"Input {input} could not be parsed, expected type of {typeof(double)}.");
        return output;
    }
    private static decimal ParseDecimal(string input)
    {
        var success = decimal.TryParse(input, out var output);
        if (!success) throw new FormatException($"Input {input} could not be parsed, expected type of {typeof(decimal)}.");
        return output;
    }
    /// <summary>
    /// A method to add a new parser to the parser dictionary. 
    /// </summary>
    /// <param name="parser">The parser function parsing a string to the parsed type T</param>
    /// <typeparam name="T">The type T representing the desired parsed datatype</typeparam>
    /// <exception cref="ArgumentException">If the parser allready exists in the parsing dictionary, throws an ArgumentException error.</exception>
    public void AddParser<T>(Func<string, object> parser)
    {
        if (_parsers.ContainsKey(typeof(T))) throw new ArgumentException($"Parser for type {typeof(T)} already exists.");
        _parsers[typeof(T)] = parser;
    }
    /// <summary>
    /// Method to Send a response to users of the program. 
    /// </summary>
    /// <param name="message">A string representing the message being sent to the user.</param>
    public void Send(string message)
    {
        Console.WriteLine(message);
    }

    private static string RequestInput(string message)
    {
        string? input = null;
        while (input is null)
        {
            Console.WriteLine(message);
            input = Console.ReadLine();
        }
        return input;
    }
}