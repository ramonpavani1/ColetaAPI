using Cidade_Inteligente.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSingleton(connectionString);

builder.Services.AddScoped<IFrotaRepositorio, FrotaRepositorio>();
builder.Services.AddScoped<INotificacaoRepositorio, NotificacaoRepositorio>();
builder.Services.AddScoped<IRecipienteRepositorio, RecipienteRepositorio>();
builder.Services.AddScoped<IResiduoRepositorio, ResiduoRepositorio>();

builder.Services.AddAuthentication(options =>
{
options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
options.TokenValidationParameters = new TokenValidationParameters
{
ValidateIssuer = true,
ValidateAudience = true,
ValidateLifetime = true,
ValidateIssuerSigningKey = true,
ValidIssuer = "ColetaAPI",
ValidAudience = "Sustentabilidade",
IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
};
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
Name = "Authorization",
Type = SecuritySchemeType.ApiKey,
Scheme = "Bearer",
BearerFormat = "JWT",
In = ParameterLocation.Header,
Description = "Insira o token JWT com o prefixo 'Bearer '"
});

c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
app.UseSwagger();
app.UseSwaggerUI();
}

app.MapPost("/login", (LoginModel login) =>
{
if (login.Username == "Coleta" && login.Password == "coleta@coleta123")
{
var token = GenerateJwtToken("Coleta", "Sustentabilidade");
return Results.Ok(new { Token = token });
}
return Results.Unauthorized();
});

app.MapGet("/secure", () => "Você está autenticado!").RequireAuthorization();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

string GenerateJwtToken(string username, string audience)
{
var claims = new[] {
        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username)
    };

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]));
var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

var token = new JwtSecurityToken(
    issuer: "ColetaAPI",
    audience: audience,
    claims: claims,
    expires: DateTime.Now.AddHours(1),
    signingCredentials: credentials
);

return new JwtSecurityTokenHandler().WriteToken(token);
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}