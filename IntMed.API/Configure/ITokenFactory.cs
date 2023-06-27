using IntmedAPI.Models;

namespace IntMed.API.Configure
{
    public interface ITokenFactory
    {

        Token GenerateToken(string user);
    }
}
