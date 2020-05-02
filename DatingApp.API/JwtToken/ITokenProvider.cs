using System.IdentityModel.Tokens.Jwt;
using DatingApp.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.JwtToken
{
    public interface ITokenProvider
    {
        void Tokenizer(User logged, out JwtSecurityTokenHandler tokenHandler, out SecurityToken token);
    }
}