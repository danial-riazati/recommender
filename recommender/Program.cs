using Microsoft.EntityFrameworkCore;
using recommender.DataProvide;
using recommender.Repos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RecommenderDBContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("mysqlconnection")));

builder.Services.AddScoped<ICarRepo, CarRepo>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
}
  );

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();



app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());


app.MapControllers();

app.Run();
