namespace AuthService.Services
{
    public class IdService : IIdService
    {
        public string GenerateId()
        {
            Guid id = Guid.NewGuid();
            return id.ToString();
        }
    }
}
