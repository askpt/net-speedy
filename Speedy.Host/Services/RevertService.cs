namespace Speedy.Host.Services;

public class RevertService : IRevertService
{
    public string Revert(string text)
    {
        var chars = text.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
}

public interface IRevertService
{
    string Revert(string text);
}
