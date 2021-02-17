using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BlazorNews.Data;
using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Material;
using Blazorise.Icons.Material;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorNews
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddBlazorise(options => { options.ChangeTextOnKeyPress = true; })
                .AddMaterialProviders()
                .AddMaterialIcons();

            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton(sp => new RestService(new HttpClient()));
            builder.Services.AddBlazoredLocalStorage();
            
            
            var host = builder.Build();

            host.Services.UseMaterialProviders().UseMaterialIcons();

            await host.RunAsync();
        }
    }
}
