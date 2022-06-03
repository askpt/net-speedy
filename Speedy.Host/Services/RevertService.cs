using System.Runtime.InteropServices;

namespace Speedy.Host.Services;

public class RevertService : IRevertService
{
    public string Revert(string text)
    {
        var chars = text.ToCharArray();
        Array.Reverse(chars);
        return $"{new string(chars)} + OS: {RuntimeInformation.OSArchitecture}";
    }
}

public interface IRevertService
{
    string Revert(string text);
}
