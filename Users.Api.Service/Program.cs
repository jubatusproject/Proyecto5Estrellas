using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;
using Asp.Versioning;
using Jubatus.Common.MongoDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Users.Api.Service;
using Users.Api.Service.Models;
using Users.Api.Service.Settings;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.AddMongo().AddMongoRepository<UsersEntity>(UserMessages.SERVICECOLLECTIONNAME);

var dbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
ArgumentNullException.ThrowIfNull(dbSettings);

// Adicionamos el HealthCheck del Servicio
builder.Services.AddHealthChecks();

// Configuramos el Swagger para que nos permita ejecutar las API's desde el Navegador de forma segura con un BearerToken
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(UserMessages.SECURITYDEFINITIONNAME, new OpenApiSecurityScheme
    {
        Description = @"JMT Authorization header using the Bearer scheme. \r\n\r\n
            Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n
            Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = UserMessages.SECURITYDEFINITIONNAME
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = UserMessages.SECURITYDEFINITIONNAME
                },
                Scheme = "oauth2",
                Name = UserMessages.SECURITYDEFINITIONNAME,
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

/* Registramos el Servicio para el manejo del versionamiento de las API's */
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

/// Requests Rate Limiter by Stefan Djokic → Email URL: https://mail.google.com/mail/u/0/?ogbl#label/Newsletter%2FStefan+Djokic/FMfcgzGslkkkQrPwRgfrxvcJprjmXrQg
builder.Services.AddRateLimiter(rateLimiterOptions =>
    rateLimiterOptions.AddFixedWindowLimiter(policyName: UserMessages.RATELIMITIRPOLICYNAME, options =>
    {
        options.PermitLimit = 10;                                           // A maximum of 10 requests
        options.Window = TimeSpan.FromSeconds(5);                           // Per 5 seconds window.
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;    // Behaviour when not enough resources can be leased (Process oldest requests first).
        options.QueueLimit = 2;                                             // Maximum cumulative permit count of queued acquisition requests.
    })
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

/// Adicionamos el HealthCheck para validar el estado del Servicio
app.MapHealthChecks(UserEndPoints.HEALTHCHECKUSERSLIVE, new HealthCheckOptions
{
    Predicate = (_) => false
});

/* Adicionamos el HealthCheck para validar el estado de la conexión a MongoDB */
app.MapHealthChecks(UserEndPoints.HEALTHCHECKUSERSREADY, new HealthCheckOptions
{
    Predicate = (check) => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                duration = entry.Value.Duration.ToString(),
            })
        });

        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result).ConfigureAwait(false);
    }
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

/* Requests Rate Limiter by Stefan Djokic */
app.UseRateLimiter();
app.MapDefaultControllerRoute().RequireRateLimiting(UserMessages.RATELIMITIRPOLICYNAME);

await app.RunAsync().ConfigureAwait(false);
