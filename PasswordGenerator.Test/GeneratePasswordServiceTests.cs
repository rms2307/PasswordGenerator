using PasswordGenerator.Services.Interfaces;
using PasswordGenerator.Services;

namespace PasswordGenerator.Test
{
    public class GeneratePasswordServiceTests
    {
        private readonly IGeneratePasswordService _service;

        public GeneratePasswordServiceTests()
        {
            _service = new GeneratePasswordService();
        }

        [Fact]
        public void Generate_SemNenhumaOpcaoSelecionada_DeveRetornarMensagemDeErro()
        {
            // Act
            var resultado = _service.Generate(false, false, false, false, 10);

            // Assert
            Assert.Equal("Selecione pelo menos uma opção", resultado);
        }

        [Theory]
        [InlineData(true, false, false, false)]
        [InlineData(false, true, false, false)]
        [InlineData(false, false, true, false)]
        [InlineData(false, false, false, true)]
        public void Generate_ComUmaOpcaoSelecionada_DeveRetornarSenhaComTamanhoCorreto(bool lower, bool upper, bool numbers, bool symbols)
        {
            // Act
            var resultado = _service.Generate(lower, upper, numbers, symbols, 10);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(10, resultado.Length);
        }

        [Fact]
        public void Generate_TodasOpcoesSelecionadas_DeveRetornarSenhaComTodosTiposDeCaracteres()
        {
            // Act
            var resultado = _service.Generate(true, true, true, true, 12);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(12, resultado.Length);
            Assert.Contains(resultado, c => char.IsLower(c));
            Assert.Contains(resultado, c => char.IsUpper(c));
            Assert.Contains(resultado, c => char.IsDigit(c));
            Assert.Contains(resultado, c => "@#$%!&*".Contains(c));
        }

        [Fact]
        public void Generate_TamanhoMenorQueTiposSelecionados_DeveRetornarSenhaComMesmoTamanho()
        {
            // Act
            var resultado = _service.Generate(true, true, true, true, 3);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Length);
        }
    }
}
