using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZS1Planv2.Model.Network
{
    public class HtmlDownloader
    {
        public async Task<string> GetHtmlSource(string url)
        {
            HttpResponseMessage stream = await new HttpClient().GetAsync(url);
            byte[] data = await stream.Content.ReadAsByteArrayAsync();

            return @Encoding.UTF8.GetString(data);
        }
    }
}
