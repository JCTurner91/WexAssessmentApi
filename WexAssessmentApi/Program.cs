using Microsoft.IdentityModel.Tokens;
using WexAssessmentApi.Models;
using WexAssessmentApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Add a singleton of ProductRepository so the ProductController can receive it via dependency injection.
// This will allow the in-memory data to persist between API calls.
builder.Services.AddSingleton<ProductRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Snippet of code pulled from the official Duende IdentityServer documentation.
// QuickGuide found here: https://docs.duendesoftware.com/identityserver/v6/quickstarts/1_client_credentials/
builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5001";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
