using Microsoft.AspNetCore.SignalR;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBot1.Bots
{
    public static class WordBooks
    {
        public static List<WordBook> wordbooks = new List<WordBook>();

        public static WordBook getYourBook(ChannelAccount user)
        {
            foreach(WordBook wb in wordbooks)
            {
                if (user.ToString().Equals(wb.user.ToString()))
                {
                    return wb;
                }
            }
            //存在しないなら新しく作成
            WordBook newWb = new WordBook(user);
            wordbooks.Add(newWb);
            Debug.WriteLine("new book");
            return newWb;
        }
    }


    public class WordBook : BotApp
    {
        public ChannelAccount user;
        public ComputerVison cvClient = new ComputerVison();
        public Translator tlClient = new Translator();
        public Words words = new Words();
        public WordBook(ChannelAccount user)
        {
            this.user = user;
        }


        public async Task addWordFromPic(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            UpdateContextToken(turnContext, cancellationToken);//とりあえずcontextとtokenをアップデートする
            List<String> urls = await getPictureUrls();
            reply("urls").Wait();
            if (urls.Count() == 0)
            {
                reply("画像が読み込めないよ！ごめんなさいm(__)m").Wait();
                return;
            }
            else
            {
                List<String> engs = new List<string>();
                foreach(String url in urls)
                {
                    using(Stream urlstream = File.OpenRead(url))
                    {
                        List<String> engsn = await cvClient.getTextFromPicture(urlstream);
                        engs.AddRange(engsn);
                    }
                }
                if(engs.Count == 0)
                {
                    reply("画像から文字を読み取れなかった！勉強不足(´；ω；`)ｳｩｩ").Wait();
                    return;
                }
                else
                {
                    String replyStr = "";
                    foreach (String eng in engs)
                    {
                        Debug.WriteLine(eng);
                        String jap = tlClient.translateWord(eng, "en", "ja");
                        if(jap != "No confidence")
                        {
                            bool result = words.addTempWord(eng, jap);
                            Debug.WriteLine("addTempWord" + result);
                            if (result)
                            {
                                replyStr += eng + ":" + jap + ", ";
                            }
                        }
                    }
                    if (replyStr == "")
                    {
                        reply("画像から文字を読み取れなかった！ごめんなさい！").Wait();
                    }
                    else
                    {
                        reply("検出した単語" + replyStr).Wait();
                    }
                }
            }
        }

    }
}
