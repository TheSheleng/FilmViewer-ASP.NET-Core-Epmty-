using FilmViewer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Reflection.PortableExecutable;
using static System.Net.WebRequestMethods;

namespace FilmViewer.Html
{
    public static class HtmlBuilder
    {
        private static string BuildHtml(string Body, string StylePath, string ScriptPath)
        {
            return $"""
                <!DOCTYPE html>
                <html lang="en">
                <head>
                    <meta charset="UTF-8">
                    <meta name="viewport" content="width=device-width, initial-scale=1.0">
                    <title>FilmViewer</title>
                    <link rel="stylesheet" href="{StylePath}">
                </head>
                <body>
                    {Body}
    
                    <script src="{ScriptPath}"></script>
                </body>
                </html>
                """;
        }

        private static string BuildBody(string header, string main, string footer)
        {
            return $"""
                <header>{header}</header>
                <main>{main}</main>
                <footer>{footer}</footer>
                """;
        }

        private static string BuildLogo()
        {
            return $"""
                <a class='logo' href='/'>
                    <span class='logoWord1'>Film</span>
                    <span class='logoWord2'>Viewer</span>
                </a>
                """;
        }

        private static string BuildSearch()
        {
            return $"""
                <div class='searchBlock'>
                    <input id='searchInput'>
                    <button id='searchBtn'>Search</button>
                </div>
                """;
        }

        private static async Task<string> BuildGenres()
        {
            string HtmlGenre = "<div class='genresList'\"'>";

            foreach (var genre in (await MovieApi.GetGenreList()).genres)
            {
                HtmlGenre += $"<a class='genreBtn' href='/Genre/{genre.id}/1'>{genre.name.ToUpperInvariant()}</a>";
            }

            HtmlGenre += "</div>";

            return HtmlGenre;
        }

        private static string BuildFilmsList(Movies Films)
        {
            string HtmlFilms = "<div class='filmsList'\"'>";

            foreach (var film in Films.Results)
            {
                HtmlFilms += $"""
                    <a class='filmBtn' href='/Film/{film.id}'>
                        <img class='filmImg' src='https://image.tmdb.org/t/p/w185/{film.poster_path}' alt='Film poster'></img>
                        <span class='filmTitle'>{film.title}</span>  
                        <div class='filmVotes'>{Math.Round(film.vote_average, 1)}</div>
                    </a>
                    """;
            }

            HtmlFilms += "</div>";

            return HtmlFilms;
        }

        private static string BuildPagesList(int page, string path)
        {
            const int PreviousPages = 5;
            const int NextPages = 5;
            const int PagesToShow = 501;

            string HtmlPages = "<div class='pagesList'>";

            for (int i = Math.Max(page - PreviousPages, 1);
                i < Math.Min(page + NextPages + 1, PagesToShow);
                i++)
            {
                HtmlPages += $"<a class='pageBtn' href='{path}/{i}'>{i}</a>";
            }

            HtmlPages += "</div>";

            return HtmlPages;
        }

        private static async Task<string> BuildHeader()
        {
            return $"<div class='topHeader'>{BuildLogo()} {BuildSearch()}</div>" + await BuildGenres();
        }

        public static async Task<string> BuildPopularPage(Movies movies, string PagesPath)
        {
            string HtmlFilms = BuildFilmsList(movies);
            string HtmlPages = BuildPagesList(movies.Page, PagesPath);

            return BuildHtml(
                BuildBody(
                    await BuildHeader(),
                    HtmlFilms + HtmlPages,
                    "@ Maid by Shelengovskiy Andrey"),
                "/styles/style.css", 
                "/scripts/script.js");
        }

        public static async Task<string> BuildFilmPage(Movie movie)
        {
            return BuildHtml(
                BuildBody(
                    await BuildHeader(),
                    $"""
                        <img class='movieBackground' src='https://image.tmdb.org/t/p/original/{movie.backdrop_path}' alt='Film poster'></img>
                        <div class='moviePresent'>
                            <span class='movieTitle'>{movie.title}</span>
                            <div class='filmVotes'>{Math.Round(movie.vote_average, 1)}</div>
                        </div>
                        <div class='movieOverview'>{movie.overview}</div>
                    """,
                    "@ Maid by Shelengovskiy Andrey"),
                "/styles/style.css",
                "/scripts/script.js");
        }
    }
}
