using workoutapp.DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
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

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();


app.UseRouting();

app.UseCors("FrontEnd");

app.UseAuthorization();

app.MapControllers().RequireCors("FrontEnd");

app.Run();

