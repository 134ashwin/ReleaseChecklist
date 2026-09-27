using Microsoft.EntityFrameworkCore;
using ReleaseChecklist.Api.Data;
using ReleaseChecklist.Api.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// 1. Connection string setup
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 3. Add CORS policy for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 4. Add Hot Chocolate GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

app.UseCors("AllowAngular");

// 5. Map GraphQL endpoint
app.MapGraphQL("/graphql");

app.Run();