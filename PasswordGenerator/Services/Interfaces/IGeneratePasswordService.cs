namespace PasswordGenerator.Services.Interfaces
{
    public interface IGeneratePasswordService
    {
        string Generate(bool includeLowercase, bool includeUppercase, bool includeNumbers, bool includeSymbols, int passwordLength);
    }
}
