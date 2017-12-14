using System.Collections.Generic;
using System.Linq;

namespace yazlab1p3.Models
{
    public class WebSite : SubUrl
    {
        public WebSite(string url)
        {
            Url = url;
        }
    }

    public class SubUrl
    {
        public string Url { get; set; }
        public string ParentUrl { get; set; }
        public List<SubUrl> SubUrls { get; set; }

        public SubUrl()
        {
            SubUrls = new List<SubUrl>();
        }
        public void SetSubUrlList(List<SubUrl> subUrlList)
        {
            SubUrls = subUrlList;
        }

        public SubUrl AddSubUrl(SubUrl subUrl)
        {
            SubUrls.Add(subUrl);
            var url = subUrl.Url;

            return SubUrls.FirstOrDefault(p => p.Url == url);
        }

        public SubUrl GetSubUrl(int index)
        {
            return SubUrls[index];
        }
        public SubUrl GetSubUrl(string url)
        {
            return SubUrls.FirstOrDefault(p => p.Url == url);
        }

        public List<SubUrl> GetSubUrlList()
        {
            return SubUrls;
        }
    }
}
