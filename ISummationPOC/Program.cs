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

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserValidation>());
builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpdateUserValidation>());

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMediator, Mediator>();
builder.Services.AddScoped<UserHandler>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<FileUploadHandler>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserValidation>();




builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetValue<string>("AzureBlobStorage:ConnectionString")));
builder.Services.AddSingleton(x => x.GetRequiredService<BlobServiceClient>().GetBlobContainerClient(builder.Configuration.GetValue<string>("AzureBlobStorage:ContainerName")));


builder.Services.AddDbContext<ISummationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DataBaseConnection"));
});
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["localstorage:blob"]!, preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["localstorage:queue"]!, preferMsi: true);
});
builder.Services.AddApplicationInsightsTelemetry();



var app = builder.Build();

// Configure the HTTP request pipeline.
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

       // pattern: "{controller=User}/{action=GetUsers}/{id?}");
        pattern: "{controller=Home}/{action=Index}/{id?}");

});

app.Run();
