using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.PortalApp.Stores.News.NewsAdmin.Actions
{
    public class PostNewsAction : IAction
    {
        public const string PostNews = "POST_NEWS_ACTION";
        public string Name => PostNews;

        public int MyProperty { get; set; }
    }
}
