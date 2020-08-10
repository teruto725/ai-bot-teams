using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EchoBot1.Bots
{
    public abstract class BotApp
    {
        public System.Threading.CancellationToken cancellationToken;
        public ITurnContext<IMessageActivity> turnContext;
        public async Task reply(string m)//reply
        {
            await turnContext.SendActivityAsync(MessageFactory.Text(m, m),cancellationToken);
        }
        //replyようにトークンを更新する
        public void UpdateContextToken(ITurnContext<IMessageActivity> turnContext, System.Threading.CancellationToken cancellationToken)
        {
            this.turnContext = turnContext;
            this.cancellationToken = cancellationToken;
        }

        public async Task<List<string>> getPictureUrls()//pictureのstreamを取得する
        {
            List<string> urls = new List<string>();
            IMessageActivity activity = turnContext.Activity;
            reply(activity.Attachments.Count().ToString()).Wait();
            if (activity.Attachments != null && activity.Attachments.Any()) //メッセージに画像が含まれていたら
            {
                foreach (var file in activity.Attachments)
                {
                    reply(file.ToString()).Wait();
                    reply(file.ContentType.ToString()).Wait();

                    var remoteFileUrl = file.ContentUrl;
                    var localFileName = Path.Combine(Path.GetTempPath(), file.Name);
                    reply("remotefile" + remoteFileUrl).Wait();
                    // Download the actual attachment
                    using (var webClient = new WebClient())
                    {
                        webClient.DownloadFile(remoteFileUrl, localFileName);
                    }
                    reply("localfile" + localFileName).Wait();
                    urls.Add(localFileName);
                }
                return urls;
                
            }
            else {
                reply(activity.Attachments[0].ToString()).Wait();
                reply(activity.Attachments[0].Name).Wait();
                reply("No attachment").Wait();
                return null;
            }
            
        }
    }

}