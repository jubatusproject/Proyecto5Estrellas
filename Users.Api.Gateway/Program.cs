using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Users.Api.Gateway;
using Users.Api.Gateway.Settings;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configuramos el Swagger para que nos permita ejecutar las API's desde el Navegador de forma segura con un BearerToken
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JMT Authorization header using the Bearer scheme. \r\n\r\n
            Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n
            Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

/* Con la línea "options.SuppressAsyncSuffixInActionNames = false;" le estamos diciendo al compilador que No ...
   ... elimine el sufijo "Async" de los nombres de los métodos, ejemplo cuando usamos: nameof(MethodNameAsync) */
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
ArgumentNullException.ThrowIfNull(jwtSettings);

// Adding JWT
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var Key = Encoding.UTF8.GetBytes(jwtSettings.JwtKey);

    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.JwtIssuer,
        ValidAudience = jwtSettings.JwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Key)
    };
});

/// Requests Rate Limiter by Stefan Djokic → Email URL: https://mail.google.com/mail/u/0/?ogbl#label/Newsletter%2FStefan+Djokic/FMfcgzGslkkkQrPwRgfrxvcJprjmXrQg
builder.Services.AddRateLimiter(rateLimiterOptions =>
    rateLimiterOptions.AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 10;                                           // A maximum of 10 requests
        options.Window = TimeSpan.FromSeconds(5);                           // Per 5 seconds window.
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;    // Behaviour when not enough resources can be leased (Process oldest requests first).
        options.QueueLimit = 2;                                             // Maximum cumulative permit count of queued acquisition requests.
    })
);

/// Adicionamos el Servicio de Reverse Proxy Server (API Gateway)
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

/// Adicionamos el HealthCheck del Servicio
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

/// Adicionamos el HealthCheck para validar el estado del Servicio
app.MapHealthChecks(ApiEndPoints.HEALTHCHECKGATEWAYLIVE, new HealthCheckOptions
{
    Predicate = (_) => false
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

/* Requests Rate Limiter by Stefan Djokic */
app.UseRateLimiter();
app.MapDefaultControllerRoute().RequireRateLimiting("fixed");

await app.RunAsync().ConfigureAwait(false);
