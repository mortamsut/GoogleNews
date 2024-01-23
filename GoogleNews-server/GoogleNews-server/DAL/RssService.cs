using System.Xml;
using System.ServiceModel.Syndication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Threading;
using System.Xml.Linq;
using GoogleNews_server;

namespace GoogleNews.DAL
{
    [Route("/")]
    [ApiController]
    public class RssService
    {

        private readonly IOutputCacheStore _cache;
        public RssService(IOutputCacheStore cache =null)
        {
            _cache = cache;
        }
        //get all Rss feeds
        [HttpGet]
        [OutputCache(PolicyName = "NewsPolicy")]
        public List<FeedItem> ReadRssFeed(string feedUrl)
        {
            List<FeedItem > feedItems = new List<FeedItem>();
            
            try
            {
                XmlReader reader = new XmlTextReader(feedUrl);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                if(feed != null )
                {
                    foreach( var item in feed.Items ) {
                        string id = item.Id;
                        string title = item.Title.Text;
                        string description = item.Summary?.Text ?? string.Empty;
                        string link = item.Links.FirstOrDefault()?.Uri.AbsoluteUri ?? string.Empty;
                        feedItems.Add(new FeedItem() { Title = title, Description = description, Link = link, Id = id }) ;
                    }
                }
            }
            catch(Exception ex ) 
            {
                Console.WriteLine("Error read RSS " + ex.Message);
            }
            return feedItems;
        }
        // get Rss feed by id
        [HttpGet("{id}")]
        [OutputCache(PolicyName = "NewsPolicy")]
        public FeedItem ReadRssFeedById(string feedUrl, string id)
        {
            FeedItem feedItem = new FeedItem();
            try
            {
                XmlReader reader = new XmlTextReader(feedUrl);
                SyndicationFeed feed = SyndicationFeed.Load(reader);

                if (feed != null)
                {
                    foreach (var item in feed.Items)
                    {
                        var uri = new Uri(item.Id);
                        var queryString = uri.Query;
                        var queryParams = System.Web.HttpUtility.ParseQueryString(queryString);
                        string pValue = queryParams["p"];

                        if (pValue == id)
                        {
                            string title = item.Title.Text;
                            string description = item.Summary?.Text ?? string.Empty;
                            string link = item.Links.FirstOrDefault()?.Uri.AbsoluteUri ?? string.Empty;
                            feedItem=new FeedItem() { Title = title,Description=description, Link = link, Id = id };
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error read RSS " + ex.Message);
            }
         return feedItem;
        }
    }
}
