// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.9.2

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

/// <summary>
/// エラーたぶんapi投げている部分かな？
/// </summary>
namespace EchoBot1.Bots
{
    public class BotHandler : ActivityHandler
    {

        public BotHandler()
        {
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            Debug.WriteLine(WordBooks.wordbooks.Count());
            var message = turnContext.Activity.RemoveRecipientMention();//message that mention deleted
            Debug.WriteLine(turnContext.Activity.From);
            if (message == null)
            {
                foreach (WordBook wb in WordBooks.wordbooks)
                {
                    if (wb.user.ToString() == turnContext.Activity.From.ToString())
                    {
                        wb.addWordFromPic(turnContext).Wait();
                        Debug.WriteLine("ok");
                        break;
                    }
                }
            }
            else if (message.Contains("word"))
            {
                foreach (WordBook wb in WordBooks.wordbooks)
                {
                    if (wb.user.Equals(turnContext.Activity.From))
                    {
                        wb.UpdateContext(turnContext).Wait();
                    }
                }
            }

            else if (message.Contains("echo"))//echobot
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(message, message), cancellationToken);
            }

        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    var m = "ようこそ" + member + "さん！";
                    WordBooks.wordbooks.Add(new WordBook(member));
                    Debug.WriteLine(WordBooks.wordbooks.Count());
                    await turnContext.SendActivityAsync(MessageFactory.Text(m,m), cancellationToken);
                }
            }
        }

        
    }
}
