using DGSearchBot;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DGSearchBot
{
    public class Commands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Returns Pong")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong\n").ConfigureAwait(false);
        }

        [Command("mimic")]
        [Description("Mimics the next words uttered by a user.")]
        public async Task Mimic(CommandContext ctx)
        {
            var interact = ctx.Client.GetInteractivity();

            var message = await interact.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Content);
        }

        [Command("random")]
        [Description("Generates a random number within the given range.")]
        public async Task Random(CommandContext ctx, int min, int max)
        {
            var rnd = new Random();
            await ctx.RespondAsync($"🎲 Your random number is: {rnd.Next(min, max)}");
        }

        [Command("weather")]
        [Description("Returns the weather for a specified area/zip code within the USA.")]
        public async Task Weather(CommandContext ctx, string zip)
        {
            var json = string.Empty;
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJSON>(json);
            var weatherApiToken = configJson.Weatherapi;

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"data/2.5/weather?zip={zip}&units=imperial&appid={weatherApiToken}");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<OpenWeatherResponse>(stringResult);
                    await ctx.Channel.SendMessageAsync($"Current weather in {rawWeather.Name}");
                    await ctx.Channel.SendMessageAsync($"Temp: {rawWeather.Main.Temp}°F");
                    await ctx.Channel.SendMessageAsync($"Feels like: {rawWeather.Main.Feels_Like}°F");
                    await ctx.Channel.SendMessageAsync($"Humidity: {rawWeather.Main.Humidity}%");
                    await ctx.Channel.SendMessageAsync($"Wind: {rawWeather.Wind.Speed} mph, {rawWeather.Wind.Deg}°");
                }
                catch (HttpRequestException httpRequestException)
                {
                    Console.WriteLine(httpRequestException);
                    return;
                }
            }
        }
    }
}
