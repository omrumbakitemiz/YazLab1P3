
using System.Collections.Generic;
using yazlab1p3.Util;

namespace yazlab1p3.Models
{
    public class Tree
    {
        public void AddData()
        {
            WebSite webSite = new WebSite("bilgisayar.kocaeli.edu.tr");

            webSite.AddSubUrl(new SubUrl
            {
                ParentUrl = "http://bilgisayar.kocaeli.edu.tr",
                Url = "http://bilgisayar.kocaeli.edu.tr/makaleler.php"
            });

            webSite.AddSubUrl(new SubUrl
            {
                ParentUrl = "http://bilgisayar.kocaeli.edu.tr",
                Url = "http://bilgisayar.kocaeli.edu.tr/gezinti.php"
            });

            webSite.AddSubUrl(new SubUrl
            {
                ParentUrl = "http://bilgisayar.kocaeli.edu.tr",
                Url = "http://bilgisayar.kocaeli.edu.tr/haberler.php"
            });

            SubUrl subUrl1 = new SubUrl
            {
                ParentUrl = "http://bilgisayar.kocaeli.edu.tr/makaleler.php",
                Url = "http://bilgisayar.kocaeli.edu.tr/makaleler.php/etherium"
            };
            SubUrl subUrl2 = new SubUrl
            {
                ParentUrl = "http://bilgisayar.kocaeli.edu.tr/makaleler.php",
                Url = "http://bilgisayar.kocaeli.edu.tr/makaleler.php/bitcoin"
            };
            webSite.GetSubUrl(0).AddSubUrl(subUrl1);
            webSite.GetSubUrl(0).AddSubUrl(subUrl2);

            SubUrl subUrl3 = new SubUrl
            {
                ParentUrl = "http://bilgisayar.kocaeli.edu.tr/haberler.php",
                Url = "http://bilgisayar.kocaeli.edu.tr/makaleler.php/yarisma"
            };
            SubUrl subUrl4 = new SubUrl
            {
                ParentUrl = "http://bilgisayar.kocaeli.edu.tr/haberler.php",
                Url = "http://bilgisayar.kocaeli.edu.tr/makaleler.php/sinav"
            };
            webSite.GetSubUrl(1).AddSubUrl(subUrl3);
            webSite.GetSubUrl(1).AddSubUrl(subUrl4);
        }

        public void Test()
        {
            WebSite webSite = new WebSite("http://bilgisayar.kocaeli.edu.tr");

            var subUrlStringList = Utilities.GetSubUrls(webSite.Url);
            List<SubUrl> subUrlList = new List<SubUrl>();

            foreach (var item in subUrlStringList)
            {
                SubUrl subUrl = new SubUrl
                {
                    ParentUrl = webSite.Url,
                    Url = item
                };

                subUrlList.Add(subUrl);
            }

            foreach (var subUrl in subUrlList)
            {
                webSite.AddSubUrl(subUrl);
            }

            //*************//

            foreach (var item in webSite.SubUrls)
            {
                var subSubUrlStringList = Utilities.GetSubUrls(item.Url);
                List<SubUrl> subSubUrlList = new List<SubUrl>();

                foreach (var subSubUrl in subSubUrlStringList)
                {
                    SubUrl subUrl = new SubUrl
                    {
                        ParentUrl = item.Url,
                        Url = subSubUrl
                    };

                    subSubUrlList.Add(subUrl);
                }
            }


            //foreach (var item in webSite.SubUrls)
            //{
            //    var data = Utilities.GetSubUrls(item.Url);
            //    item.SubUrls = data;
            //}
        }
    }
}

