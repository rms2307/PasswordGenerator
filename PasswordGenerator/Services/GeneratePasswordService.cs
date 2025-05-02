using PasswordGenerator.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PasswordGenerator.Services
{
    public class GeneratePasswordService : IGeneratePasswordService
    {
        public string Generate(bool includeLowercase, bool includeUppercase, bool includeNumbers, bool includeSymbols, int passwordLength)
        {
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            const string symbols = "@#$%!&*";

            var characterSet = "";
            if (includeLowercase) characterSet += lowercase;
            if (includeUppercase) characterSet += uppercase;
            if (includeNumbers) characterSet += numbers;
            if (includeSymbols) characterSet += symbols;

            if (characterSet.Length == 0)
            {
                return "Selecione pelo menos uma opção";
            }

            var password = new StringBuilder();
            if (includeLowercase) password.Append(GetRandomCharacter(lowercase));
            if (includeUppercase) password.Append(GetRandomCharacter(uppercase));
            if (includeNumbers) password.Append(GetRandomCharacter(numbers));
            if (includeSymbols) password.Append(GetRandomCharacter(symbols));

            password = GenerateRemainingRandomCharacters(characterSet, password, passwordLength);

            return ShufflePassword(password.ToString());
        }

        private StringBuilder GenerateRemainingRandomCharacters(string characterSet, StringBuilder password, int passwordLength)
        {
            for (int i = password.Length; i < passwordLength; i++)
            {
                password.Append(GetRandomCharacter(characterSet));
            }

            return password;
        }

        private string ShufflePassword(string password)
        {
            var array = password.ToCharArray();
            using (var rng = RandomNumberGenerator.Create())
            {
                int n = array.Length;
                while (n > 1)
                {
                    byte[] box = new byte[1];
                    rng.GetBytes(box);
                    int k = box[0] % n;
                    n--;
                    (array[n], array[k]) = (array[k], array[n]);
                }
            }
            return new string(array);
        }

        private char GetRandomCharacter(string chars)
        {
            byte[] randomByte = new byte[1];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomByte);
            }

            int index = randomByte[0] % chars.Length;
            return chars[index];
        }
    }
}
