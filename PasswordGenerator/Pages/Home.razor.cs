using Microsoft.JSInterop;

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
            GeneratedPassword = PasswordGeneratorService.Generate(IncludeLowercase, IncludeUppercase, IncludeNumbers, IncludeSymbols, PasswordLength);
        }

        private async Task CopyPassword()
        {
            await JS.InvokeVoidAsync("copyToClipboard", "passwordOutput");
        }
    }
}
