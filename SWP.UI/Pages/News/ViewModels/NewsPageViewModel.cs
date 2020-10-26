using System;
using System.Collections.Generic;

namespace SWP.UI.Pages.News.Manager.ViewModels
{
    public class NewsPageViewModel
    {
        public IEnumerable<NewsViewModel> News { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public bool NextPage { get; set; }
        public string Category { get; set; }

        public List<string> Categories { get; set; }

        public NewsPageViewModel()
        {
            Categories = new List<string>();

            foreach (NewsCategory category in (NewsCategory[])Enum.GetValues(typeof(NewsCategory)))
            {
                Categories.Add(category.ToString());
            }
        }

        public IEnumerable<int> PageNumbers(int pageNumber, int pageCount)
        {
            if (pageCount <= 5)
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    yield return i;
                }
            }
            else
            {
                int midPoint = pageNumber < 3 ? 3 : pageNumber > pageCount - 2 ? pageCount - 2 : pageNumber;

                int lowerBound = midPoint - 2;
                int upperBount = midPoint + 2;

                if (lowerBound != 1)
                {
                    yield return 1;
                    if (lowerBound - 1 > 1)
                    {
                        yield return -1;
                    }
                }

                for (int i = lowerBound; i <= upperBount; i++)
                {
                    yield return i;
                }

                if (upperBount != pageCount)
                {
                    if (pageCount - upperBount > 1)
                    {
                        yield return -1;
                    }
                    yield return pageCount;
                }
            }
        }
    }
}
