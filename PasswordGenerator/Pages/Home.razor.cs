using Microsoft.JSInterop;
using System.Security.Cryptography;
using System.Text;

namespace PasswordGenerator.Pages
{
    public partial class Home
    {
        private string GeneratedPassword { get; set; } = "";
        private int PasswordLength { get; set; } = 12;
        private bool IncludeLowercase { get; set; } = true;
        private bool IncludeUppercase { get; set; } = true;
        private bool IncludeNumbers { get; set; } = true;
        private bool IncludeSymbols { get; set; } = true;

        private bool IsGenerateButtonDisabled => !IncludeLowercase && !IncludeUppercase && !IncludeNumbers && !IncludeSymbols;

        protected override void OnInitialized()
        {
            GeneratePassword();
        }

        private void GeneratePassword()
        {
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            const string symbols = "@#$%!&*";

            var characterSet = "";
            if (IncludeLowercase) characterSet += lowercase;
            if (IncludeUppercase) characterSet += uppercase;
            if (IncludeNumbers) characterSet += numbers;
            if (IncludeSymbols) characterSet += symbols;

            if (characterSet.Length == 0)
            {
                GeneratedPassword = "Selecione pelo menos uma opção";
                return;
            }

            StringBuilder password = EnsurePasswordContainsAllCharacterTypes(lowercase, uppercase, numbers, symbols);
            password = GenerateRemainingRandomCharacters(characterSet, password);

            GeneratedPassword = ShufflePassword(password.ToString());
        }

        private StringBuilder GenerateRemainingRandomCharacters(string characterSet, StringBuilder password)
        {
            for (int i = password.Length; i < PasswordLength; i++)
            {
                password.Append(GetRandomCharacter(characterSet));
            }

            return password;
        }

        private StringBuilder EnsurePasswordContainsAllCharacterTypes(string lowercase, string uppercase, string numbers, string symbols)
        {
            var password = new StringBuilder();
            if (IncludeLowercase) password.Append(GetRandomCharacter(lowercase));
            if (IncludeUppercase) password.Append(GetRandomCharacter(uppercase));
            if (IncludeNumbers) password.Append(GetRandomCharacter(numbers));
            if (IncludeSymbols) password.Append(GetRandomCharacter(symbols));
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

        private async Task CopyPassword()
        {
            await JS.InvokeVoidAsync("copyToClipboard", "passwordOutput");
        }
    }
}
