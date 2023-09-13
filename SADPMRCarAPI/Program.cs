global using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SADPMRCarAPI.Data;
using System.Text;
using SADPMRCarAPI.Automapper;
using SADPMRCarAPI.Services.DepartmentService;
using SADPMRCarAPI.Services.TripService;
using SADPMRCarAPI.Services.CarService;
using SADPMRCarAPI.Services.CarAcessoriesService;
using SADPMRCarAPI.Services.CarModelService;
using Newtonsoft.Json;
using SADPMRCarAPI.Model;
using Microsoft.Exchange.WebServices.Data;
using System.Security.Claims;
using SADPMRCarAPI.Services.User;
using SADPMRCarAPI.Services.TripApprovalService;
using SADPMRCarAPI.Services.Admin;
using SADPMRCarAPI.Services.AssignService;
using SADPMRCarAPI.Services.AssignCarToServiceService;
using SADPMRCarAPI.Services.UploadFile;
using SADPMRCarAPI.Services.AssignAccessoriesToCar;

//Created By Sinethemba Vitsha 

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;


//for the Dependency Injection AND autoMapper
var mapperConfiguration = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<RentalProfile>();
});

IMapper mapper = mapperConfiguration.CreateMapper();
builder.Services.AddAutoMapper(typeof(Program).Assembly);


//Db Context
builder.Services.AddDbContext<ApplicationDbContext>();

//HttpContext
builder.Services.AddHttpContextAccessor();

// Connection String 
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("connMSSQL")));

// For Identity
builder.Services.AddIdentity<ApplicationUserIdentity, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
    options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    options.ClaimsIdentity.UserIdClaimType = "UserID";
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<AuthenticatorTokenProvider<ApplicationUserIdentity>>(
        TokenOptions.DefaultAuthenticatorProvider);

//AddScoped method registers the service with a scoped lifetime, the lifetime of a single request
builder.Services.AddScoped<SignInManager<ApplicationUserIdentity>>();
builder.Services.AddScoped<IUploadfileService, UploadfileService>();


// Adding Authentication
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false ,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["BearerToken:Issuer"],
        ValidAudience = builder.Configuration["BearerToken:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//Adding the service
builder.Services.AddTransient<IDepartmentService, DepartmentService>();

builder.Services.AddTransient<ITripService,TripService>();

builder.Services.AddTransient<ICarServiceService, CarServiceRepository>();

builder.Services.AddTransient<ICarAccessoriesService, CarAccessoriesRepository>();

builder.Services.AddTransient<ICarModelService, CarRepository>();

builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddTransient<ITripApprovalService, TripApprovalService>();

builder.Services.AddTransient<IAdminService, AdminService>();

builder.Services.AddTransient<IAssignCarService , AssignCarService>();

builder.Services.AddTransient<IAssignCarToService, AssignCarToService>();

builder.Services.AddTransient<IUploadfileService, UploadfileService>(); 

builder.Services.AddTransient<IAssignAccessoriesToCarService ,  AssignAccessoriesToCarService>();   




builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SADPMR_Trip_Authorization_App",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:8080")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});





//Claims
builder.Services.Configure<IdentityOptions>(options =>options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier);
builder.Services.AddHttpContextAccessor();

//json 
builder.Services.AddControllers().AddNewtonsoftJson(options =>options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.Use(async (context, next) =>
{
    await next.Invoke();
    var data = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
app.UseCors();

