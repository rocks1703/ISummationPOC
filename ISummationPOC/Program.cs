using Azure.Storage.Blobs;
using FluentValidation;
using FluentValidation.AspNetCore;
using ISummationPOC.DBContext;
using ISummationPOC.Handler;
using ISummationPOC.Repository;
using ISummationPOC.Service;
using ISummationPOC.Validation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;
using static ISummationPOC.Service.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMediator, Mediator>();
builder.Services.AddScoped<UserHandler>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<FileUploadHandler>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Add Validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserValidation>();

// Configure Azure Blob Storage settings
builder.Services.Configure<AzureBlobStorageSettings>(builder.Configuration.GetSection("AzureBlobStorage"));

// Register BlobServiceClient as a singleton
builder.Services.AddSingleton(x =>
{
    var connectionString = builder.Configuration.GetConnectionString("StorageConnectionString");
    return new BlobServiceClient(connectionString);
});

// Configure the database context
builder.Services.AddDbContext<ISummationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnectionString"));
});

// Add Application Insights Telemetry
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/User/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
