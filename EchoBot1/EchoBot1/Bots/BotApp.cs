using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EchoBot1.Bots
{
    public abstract class BotApp
    {

        public async Task reply(ITurnContext<IMessageActivity> turnContext, string m)//reply
        {
            await turnContext.SendActivityAsync(MessageFactory.Text(m, m));
        }


        public async Task<List<string>> getPictureUrls(ITurnContext<IMessageActivity> turnContext)//pictureのstreamを取得する
        {
            List<string> urls = new List<string>();
            var activity = turnContext.Activity;
            if (activity.Attachments != null && activity.Attachments.Any()) //メッセージに画像が含まれていたら
            {
                var replyText = string.Empty;
                foreach (var file in activity.Attachments)
                {
                    var remoteFileUrl = file.ContentUrl;
                    var localFileName = Path.Combine(Path.GetTempPath(), file.Name);

                    // Download the actual attachment
                    using (var webClient = new WebClient())
                    {
                        webClient.DownloadFile(remoteFileUrl, localFileName);
                    }
                    urls.Add(localFileName);
                }
            }
            return urls;
        }
    }

}