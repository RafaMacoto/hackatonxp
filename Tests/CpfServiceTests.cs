using hackatonxp.Application.Services;
using Xunit;

namespace hackatonxp.UnitTests
{
    public class CpfServiceTests
    {
        private readonly CpfService _service = new();

        [Theory]
        [InlineData("529.982.247-25")]
        [InlineData("52998224725")]
        public void Normalize_RemovesPunctuation(string input)
        {
            var normalized = _service.Normalize(input);
            Assert.Equal("52998224725", normalized);
        }

        [Theory]
        [InlineData("529.982.247-25", true)]
        [InlineData("52998224725", true)]
        [InlineData("111.111.111-11", false)]
        [InlineData("12345678900", false)]
        public void IsValid_ValidatesCorrectly(string input, bool expected)
        {
            var valid = _service.IsValid(input);
            Assert.Equal(expected, valid);
        }
    }
}
