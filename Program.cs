global using FilmViewer.Api;
global using FilmViewer.Models;
using FilmViewer.Html;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mime;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.UseDefaultFiles();
app.UseStaticFiles();

/*app.Map("/GetGenreList", async () =>
{
    var genres = await MovieApi.GetGenreList();
    return JsonSerializer.Serialize(genres);
});

app.Map("/GetMoviesByGenre/{GenreId:int}/{Page:int}", async (int GenreId, int Page) =>
{
    var movies = await MovieApi.GetMoviesByGenre(GenreId, Page);
    return JsonSerializer.Serialize(movies);
});*/


app.Map("/{page:int?}", async (HttpContext context, int page = 1) =>
{
    context.Response.ContentType = "text/html";

    Movies movies = await MovieApi.GetPopularMovies(page);

    return await HtmlBuilder.BuildPopularPage(movies, "");
});

app.Map("/Genre/{id:int}/{page:int?}", async (HttpContext context, int id, int page = 1) =>
{
    context.Response.ContentType = "text/html";

    Movies movies = await MovieApi.GetMoviesByGenre(id, page);

    return await HtmlBuilder.BuildPopularPage(movies, $"/Genre/{id}");
});

app.Map("/Search/{search}/{page:int?}", async (HttpContext context, string search, int page = 1) =>
{
    context.Response.ContentType = "text/html";

    Movies movies = await MovieApi.GetMoviesByName(search, page);

    return await HtmlBuilder.BuildPopularPage(movies, $"/Search/{search}");
});

app.Map("/Film/{id:int?}", async (HttpContext context, int id = 1) =>
{
    context.Response.ContentType = "text/html";

    Movie movie = await MovieApi.GetMovieById(id);

    return await HtmlBuilder.BuildFilmPage(movie);
});

app.Run();
