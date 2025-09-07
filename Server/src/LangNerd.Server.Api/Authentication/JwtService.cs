using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using System.Text.Unicode;
using System.IdentityModel.Tokens.Jwt;
using Azure.Identity;
namespace LangNerd.Server.Api.Authentication;

public static class JwtService
{
    /*
    public static IServiceCollection JwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<string>("JWT_SECRET_KEY") == null)
            throw new Exception("JWT_SECRET_KEY is not set, unable to run app");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT_SECRET_KEY")))

                    };
                });
        return services;
    }
    */
    public static string CreateSignature(string data, string secretkey)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secretkey);
        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Base64UrlEncode(hash);
    }
    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
                      .TrimEnd('=')
                      .Replace('+', '-')
                      .Replace('/', '_');
    }
    private static string Base64UrlDecode(string input)
    {
        string s = input.Replace('-', '+').Replace('_', '/');
        switch (s.Length % 4)
        {
            case 2: s += "=="; break;
            case 3: s += "="; break;
        }
        var bytes = Convert.FromBase64String(s);
        return Encoding.UTF8.GetString(bytes);
    }
    public static string GenerateJwt(string username, IConfiguration configuration)
    {
        //var secretkey = configuration.GetValue<string>("SECRET_JWT_KEY");
        //var token = new JwtSecurityTokenHandler().CreateToken();

        var jwtHeader = new { alg = "base64", type = "JWT", };
        var payload = new { username = username, exp = DateTimeOffset.UtcNow.AddHours(1) };

        string jwtHeaderBase64 = Base64UrlEncode(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(jwtHeader)));
        string payloadBase64 = Base64UrlEncode(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload)));
        var data = $"{jwtHeaderBase64}.{payloadBase64}";
        var signature = CreateSignature(data, configuration.GetValue<string>("SECRET_JWT_KEY"));
        return $"{jwtHeaderBase64}.{payloadBase64}.{signature}";
    }
    public static string? ValidateJwt(string token, IConfiguration configuration)
    {

        var secretkey = configuration.GetValue<string>("SECRET_JWT_KEY");
        var parts = token.Split('.');
        if (parts.Length != 3)
            throw new Exception("Wrong Token Format");

        var data = $"{parts[0]}.{parts[1]}";
        var signature = parts[2];

        var computedSignature = CreateSignature(data, secretkey);
        if (computedSignature != signature)
            return null;

        var payloadJson = Base64UrlDecode(parts[1]);
        var payloadData = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);
        var exp = payloadData["exp"].ToString();
        if (DateTimeOffset.UtcNow > DateTimeOffset.Parse(exp))
            return null;
        return payloadData["username"].ToString();
    }

}

