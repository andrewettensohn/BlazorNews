﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorNews.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;

namespace BlazorNews.Data
{
    public class RestService
    {
        private readonly HttpClient Client;
        public List<int> ArticleIds { get; set; }

        public RestService(HttpClient http)
        {
            Client = http;
        }

        public async Task<List<Article>> GetArticlesFromIds(int startIndex, int numArticles)
        {
            try
            {
                List<int> ids = ArticleIds.GetRange(startIndex, numArticles);

                Uri itemBaseUri = new Uri(Constants.itemBaseUrl);
                List<Article> articles = new List<Article>();

                foreach (int articleId in ids)
                {
                    HttpResponseMessage articleResponse = await Client.GetAsync($"{itemBaseUri}/{articleId}.json");

                    if (articleResponse.StatusCode == HttpStatusCode.OK)
                    {
                        string jsonContent = await articleResponse.Content.ReadAsStringAsync();
                        Article article = JsonConvert.DeserializeObject<Article>(jsonContent);
                        article.Id = articleId;
                        articles.Add(article);
                    }
                }
                articles.RemoveAll(x => string.IsNullOrEmpty(x.Url)); //Remove non-articles
                Console.WriteLine($"returned {articles.Count} articles");

                return articles;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task GetArticleIds()
        {
            Uri topStoryIdsUri = new Uri(Constants.topStoriesUri);

            try
            {
                Console.WriteLine($"Making request for article Ids");
                HttpResponseMessage idResponse = await Client.GetAsync(topStoryIdsUri);

                if (idResponse.StatusCode == HttpStatusCode.OK)
                {
                    string idsJsonContent = await idResponse.Content.ReadAsStringAsync();
                    ArticleIds = JsonConvert.DeserializeObject<List<int>>(idsJsonContent);
                    Console.WriteLine($"Returned {ArticleIds.Count} articles");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public async Task<List<Comment>> GetComments(List<int> commentIds)
        {
            try
            {

                Uri itemBaseUri = new Uri(Constants.itemBaseUrl);
                List<Comment> comments = new List<Comment>();

                foreach (int id in commentIds)
                {
                    HttpResponseMessage commentResponse = await Client.GetAsync($"{itemBaseUri}/{id}.json");

                    if (commentResponse.StatusCode == HttpStatusCode.OK)
                    {
                        string jsonContent = await commentResponse.Content.ReadAsStringAsync();
                        Comment comment = JsonConvert.DeserializeObject<Comment>(jsonContent);
                        //comment.Id = id;
                        comments.Add(comment);
                    }
                }
                //comment.RemoveAll(x => string.IsNullOrEmpty(x.Url)); //Remove non-articles
                Console.WriteLine($"returned {comments.Count} comments");

                return comments;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
