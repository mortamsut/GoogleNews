using GoogleNews.DAL;

namespace GoogleNews_server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Add production error handling here
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                endpoints.MapGet("/", (RssService rssService) =>
                {
                    //string rssFeedUrl = "http://news.google.com/news?pz=1&amp;cf=all&amp;ned=en_il&amp;hl=en&amp;output=rss";
                    string rssFeedUrl = "https://www.93fm.co.il/feed/";
                    return rssService.ReadRssFeed(rssFeedUrl);
                });
                endpoints.MapGet("/{id}", (RssService rssService,string id) =>
                {
                    //string rssFeedUrl = "http://news.google.com/news?pz=1&amp;cf=all&amp;ned=en_il&amp;hl=en&amp;output=rss";
                    string rssFeedUrl = "https://www.93fm.co.il/feed/";
                    return rssService.ReadRssFeedById(rssFeedUrl,id);
                });
            });
        }

    }
}
