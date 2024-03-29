using workoutapp.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using workoutapp.Tools;
using System.Text.Json.Serialization;
using workoutapp;
//using workoutapp.Seed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//adding configuration variable
ConfigurationManager configuration = builder.Configuration;


//adding authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(opt =>
        opt.UseNpgsql(builder.Configuration.GetConnectionString("SampleDbConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEnd",
        builder =>
        {
            builder.WithOrigins("https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowCredentials()
            .SetPreflightMaxAge(TimeSpan.FromSeconds(10000))
            .WithExposedHeaders("Access-Control-Allow-Origin");
        }
        );
});

builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
//builder.Services.AddScoped<ExerciseSeeder>();


var app = builder.Build();

//var scope = app.Services.CreateScope();
//var seeder = scope.ServiceProvider.GetRequiredService<ExerciseSeeder>();

// Configure the HTTP request pipeline.

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("FrontEnd");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "workoutapp");
});


app.UseAuthorization();

app.MapControllers().RequireCors("FrontEnd");

app.Run();

