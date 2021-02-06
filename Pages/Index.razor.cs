using Blazored.LocalStorage;
using BlazorNews.Data;
using BlazorNews.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorNews.Pages
{
    public partial class Index
    {
        [Inject]
        public RestService RestService { get; set; }

        [Inject]
        ILocalStorageService LocalStorage { get; set; }

        private List<Article> SavedArticles { get; set; } = new List<Article>();
        private string loadingCss = string.Empty;
        private string selectedTab = "feed";
        private BlazorNewsTheme Theme { get; set; } = new BlazorNewsTheme();

        protected async override Task OnInitializedAsync()
        {
            try
            {
                await RestService.GetArticleIds();
                await LoadMoreArticles(new ItemsProviderRequest(0, 15, CancellationToken.None));
                await base.OnInitializedAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            //await GetTheme();
        }

        private async Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            if (selectedTab == "saved")
            {
                SavedArticles = await GetSavedArticles();
            }
        }

        private async ValueTask<ItemsProviderResult<Article>> LoadMoreArticles(ItemsProviderRequest request)
        {
            loadingCss = string.Empty;
            try
            {
                int numArticles = Math.Min(request.Count, 500 - request.StartIndex);
                Console.WriteLine($"requesting {numArticles}");
                List<Article> newArticles = await RestService.GetArticlesFromIds(request.StartIndex, numArticles);

                if (SavedArticles != null && SavedArticles.Count > 0)
                {
                    Console.WriteLine($"Checking for saved articles for {newArticles.Count} articles");
                    SetArticleSaveToTrueIfSaved(newArticles);
                    loadingCss = "d-none";
                    return new ItemsProviderResult<Article>(newArticles, 500);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            loadingCss = "d-none";
            return new ItemsProviderResult<Article>(new List<Article>(), 500);
        }

        private List<Article> SetArticleSaveToTrueIfSaved(List<Article> articles)
        {
            articles.ForEach(x =>
            {
                if (SavedArticles.Any(y => y.Id == x.Id))
                {
                    x.Saved = true;
                }
            });

            return articles;
        }

        private async Task SaveArticle(Article article)
        {
            article.Saved = true;
            List<Article> savedArticles = await LocalStorage.GetItemAsync<List<Article>>("savedArticles");

            if (savedArticles != null)
            {
                if (!savedArticles.Any(x => x.Id == article.Id))
                {
                    savedArticles.Add(article);
                    await LocalStorage.SetItemAsync("savedArticles", savedArticles);
                }
            }
            else
            {
                savedArticles = new List<Article> { article };
                await LocalStorage.SetItemAsync("savedArticles", savedArticles);
            }

            savedArticles = SetArticleSaveToTrueIfSaved(savedArticles);
        }

        private async Task<List<Article>> GetSavedArticles()
        {
            List<Article> localSavedArticles = await LocalStorage.GetItemAsync<List<Article>>("savedArticles");

            if (localSavedArticles != null && localSavedArticles.Count > 0)
            {
                SavedArticles = localSavedArticles;
                SavedArticles.ForEach(x => x.Saved = true);

                return SavedArticles;
            }
            else
            {
                return new List<Article>();
            }
        }

        private async Task DeleteAllSavedArticles()
        {
            await LocalStorage.ClearAsync();
            //await GetFeed();
        }

        private async Task DeleteArticle(Article article)
        {
            List<Article> savedArticles = await LocalStorage.GetItemAsync<List<Article>>("savedArticles");
            savedArticles.RemoveAll(x => x.Id == article.Id);
            await LocalStorage.SetItemAsync("savedArticles", savedArticles);
            //await GetFeed();
        }

        private async Task GetTheme()
        {
            string savedTheme = await LocalStorage.GetItemAsStringAsync("themeSetting");

            if (string.IsNullOrWhiteSpace(savedTheme) || savedTheme == "light")
            {
                Theme = new BlazorNewsTheme
                {
                    MainBackground = "bg-light",
                    CardBackground = "bg-light",
                    TextColor = "text-dark"
                };
            }
            else if (savedTheme == "dark")
            {
                Theme = new BlazorNewsTheme
                {
                    MainBackground = "bg-dark",
                    CardBackground = "darkCard",
                    TextColor = "text-light"
                };
            }
        }

        private async Task ToggleThemeSetting()
        {

            string currentTheme = await LocalStorage.GetItemAsStringAsync("themeSetting");

            if (string.IsNullOrWhiteSpace(currentTheme) || currentTheme == "light")
            {
                await LocalStorage.SetItemAsync("themeSetting", "dark");
            }
            else if (currentTheme == "dark")
            {
                await LocalStorage.SetItemAsync("themeSetting", "light");
            }

            await GetTheme();
        }
    }
}
