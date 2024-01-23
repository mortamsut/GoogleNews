using GoogleNews.DAL;
using Microsoft.AspNetCore.OutputCaching;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(policy => policy
        .Expire(TimeSpan.FromMinutes(10)));
    options.AddPolicy("NewsPolicy", policy => policy
     .Expire(TimeSpan.FromMinutes(60)));
});

builder.Services.AddSingleton<RssService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});
builder.Services.AddRazorPages();
var app = builder.Build();

app.UseCors();

app.UseOutputCache();


app.MapRazorPages();

//string rssFeedUrl = "http://news.google.com/news?pz=1&amp;cf=all&amp;ned=en_il&amp;hl=en&amp;output=rss";
string rssFeedUrl = "https://www.93fm.co.il/feed/";
//RssService rssService = new RssService();
app.MapGet("/", (RssService rssService) =>
{
    return rssService.ReadRssFeed(rssFeedUrl);
});
app.MapGet("/{id}", (RssService rssService, string id) =>
{
    return rssService.ReadRssFeedById(rssFeedUrl, id);
});
var port = 5001;
app.Run($"http://localhost:{port}/");

