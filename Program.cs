using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;

using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Core;
using QuizManager.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient<GoogleScholarService>();
builder.Services.AddHttpClient<ICordisService, CordisService>();
// Register IDbContextFactory<AppDbContext> // ayto den xreiazetai pleon den to kanw xrisi pou8ena. To afinw an xreiastei sto mellon gia IDbContextFactory
builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString"));
});


// Register InternshipEmailService with proper configuration
builder.Services.AddScoped<InternshipEmailService>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var emailSettings = config.GetSection("EmailSettings");
    return new InternshipEmailService(
        emailSettings["SmtpUsername"],
        emailSettings["SmtpPassword"],
        emailSettings["SupportEmail"],
        emailSettings["NoReplyEmail"]);
});
builder.Services.AddScoped<QuizManager.Data.IEmailService, QuizManager.Data.EmailService>();
builder.Services.AddScoped<FileUploadService>();
builder.Services.AddScoped<HttpClient>(sp => new HttpClient { BaseAddress = new Uri("https://dev-75kcw8hj0pzojdod.us.auth0.com/api/v2") });
// Add HttpClient for Auth0 Management API
builder.Services.AddHttpClient();

// Register Auth0 service
builder.Services.AddScoped<IAuth0Service, Auth0Service>();
builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.AdditionalProviderParameters.Add("screen_hint", "signup");
});

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | 
                        ForwardedHeaders.XForwardedProto |
                        ForwardedHeaders.XForwardedHost

    // Trust all proxies (since Nginx is on your local machine)
    
});

// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Error");
//     app.UseHsts();
// }


app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
