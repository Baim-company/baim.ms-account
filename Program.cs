using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Services.Abstractions;
using PersonalAccount.API.Services.Mapping;
using PersonalAccount.API.Services;
using PersonalAccount.API.Services.Interfaces;
using PersonalAccount.API.Data.DbContexts;
using Global.Infrastructure.Middlewares;
using PersonalAccount.API.Services.Implementations;
using Identity.API.Data.Initialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
builder.Services.AddDbContext<AgileDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("BaimAgile")));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(configuration["AllowedOrigins"]!.Split(";"))
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        RoleClaimType = "Role"
    };
});


builder.Services.AddSwaggerGen(options =>
{
    const string scheme = "Bearer";
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My Account Service",
        Version = "v1"
    });

    options.AddSecurityDefinition(
        name: scheme,
        new OpenApiSecurityScheme()
        {
            Description = "Enter here jwt token without \'Bearer\'. Exapmle: eyJhbGciOiJIUzI1NiIsInR...",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = scheme
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement() {
            {
                new OpenApiSecurityScheme() {
                    Reference = new OpenApiReference() {
                        Id = scheme,
                        Type = ReferenceType.SecurityScheme
                    }
                } ,
                new string[] {}
            }
        }
    );
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("StaffOnly", policy => policy.RequireRole("Staff"));
    options.AddPolicy("UserAdminOnly", policy => policy.RequireRole("UserAdmin"));
    options.AddPolicy("StaffAndAdminOnly", policy => policy.RequireRole("Staff", "Admin"));
    options.AddPolicy("HighPriorityOnly", policy => policy.RequireRole("Admin", "Staff","UserAdmin"));
});


builder.Services.AddScoped<IProjectTaskCheckListService, ProjectTaskCheckListService>();
builder.Services.AddScoped<IProjectSubTicketService, ProjectSubTicketService>();
builder.Services.AddScoped<ITypeOfActivityService, TypeOfActivityService>();
builder.Services.AddScoped<IProjectTicketService, ProjectTicketService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IStaffImagesService, StaffImagesService>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();
builder.Services.AddScoped<IUserPhotoService, UserPhotoService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IVoenService, VoenService>();
builder.Services.AddScoped<IAdminService, AdminService>();
 
builder.Services.AddScoped<GlobalExceptionsMiddleware>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddHttpClient<IVoenService, VoenService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await DbInitializer.InitializeAsync(serviceProvider);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionsMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();