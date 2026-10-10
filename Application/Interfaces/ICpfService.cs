namespace hackatonxp.Application.Interfaces
{
    public interface ICpfService
    {
        string Normalize(string? cpf);
        bool IsValid(string? cpf);
    }
}
