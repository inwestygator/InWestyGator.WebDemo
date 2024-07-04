using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication;
using InWestyGator.WebDemo.Handlers;
using InWestyGator.WebDemo.DataAccess.Extensions;
using InWestyGator.WebDemo.Business.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using InWestyGator.WebDemo.DataAccess;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddWebServices();

// TODO: create some actual configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddWebPostgreDataAccess(connectionString);

builder.Services.AddAuthentication("BasicAuthentication").
            AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler<AuthenticationSchemeOptions>>
            ("BasicAuthentication", null);
builder.Services.AddExceptionHandler<BasicExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline. AKA Middlewares in order.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<WebDbContext>();
        dbContext.Database.EnsureCreated();
    }
    // example
    //app.UseExceptionHandler("/error");
    app.UseSwagger();
    app.UseSwaggerUI();
}

// for authentication
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();