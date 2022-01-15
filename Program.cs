using Microsoft.EntityFrameworkCore;
using MinimalJwt.DbAccess;
using MinimalJwt.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MinimalJwtContext>( opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/movie", async (MinimalJwtContext ctx) => {
    return await ctx.Movies.ToListAsync();
});

app.MapPost("/api/movie", async(MinimalJwtContext ctx, MovieDto movieDto) =>{

    var movie = new Movie();
    movie.Title = movieDto.Title;
    movie.Description = movieDto.Description;
    movie.Rating = movieDto.Rating;

    ctx.Movies.Add(movie);
    await  ctx.SaveChangesAsync();
    return movie;
});

app.MapPut("/api/movie/{id}", async (int id, MinimalJwtContext ctx, MovieDto movieDto)=>{

    var movie = await ctx.Movies.FindAsync(id);

    if (movie is null) return Results.NotFound();

    movie.Title = movieDto.Title;
    movie.Description=movieDto.Description;
    movie.Rating=movieDto.Rating;

    await ctx.SaveChangesAsync();
    return Results.NoContent();

});

app.MapDelete("/api/movie/{id}", async (int id, MinimalJwtContext ctx) => {

    if (await ctx.Movies.FindAsync(id) is Movie movie)
    {
        ctx.Movies.Remove(movie);
        await ctx.SaveChangesAsync();
        return Results.Ok();
    }
    return Results.NotFound();

});


app.Run();

internal record MovieDto(string Title, string Description, double Rating);