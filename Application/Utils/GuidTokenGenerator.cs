public class GuidTokenGenerator : ITokenGenerator
{
    public string GenerateToken()
    {
        return Guid.NewGuid().ToString();
    }
}
