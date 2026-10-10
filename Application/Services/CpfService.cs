using System.Linq;
using Advisor360.Application.Interfaces;

namespace Advisor360.Application.Services
{
    public class CpfService : ICpfService
    {
        public string Normalize(string? cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return string.Empty;

            return new string(cpf.Where(char.IsDigit).ToArray());
        }

        public bool IsValid(string? cpf)
        {
            var n = Normalize(cpf);
            if (n.Length != 11)
                return false;

            // Reject sequences
            var invalids = new[] { "00000000000", "11111111111", "22222222222", "33333333333", "44444444444", "55555555555", "66666666666", "77777777777", "88888888888", "99999999999" };
            if (invalids.Contains(n))
                return false;

            int[] numbers = n.Select(c => c - '0').ToArray();

            // Validate first digit
            int sum = 0;
            for (int i = 0; i < 9; i++) sum += numbers[i] * (10 - i);
            int remainder = sum % 11;
            int firstDigit = remainder < 2 ? 0 : 11 - remainder;
            if (numbers[9] != firstDigit) return false;

            // Validate second digit
            sum = 0;
            for (int i = 0; i < 10; i++) sum += numbers[i] * (11 - i);
            remainder = sum % 11;
            int secondDigit = remainder < 2 ? 0 : 11 - remainder;
            if (numbers[10] != secondDigit) return false;

            return true;
        }
    }
}
