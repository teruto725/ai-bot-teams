// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.9.2

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Newtonsoft.Json.Linq;
/// <summary>
/// teams��ŉ摜���������ɃG���[�o��Ƃ��납��
/// </summary>
/// 

namespace EchoBot1.Bots
{

    public class BotHandler: ActivityHandler//���̃N���X�C�x���g���N���邽�тɌĂяo����Ă�̂Œ���
    {
        public BotHandler()
        {
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            
            Debug.WriteLine(WordBooks.wordbooks.Count());
            var message = turnContext.Activity.RemoveRecipientMention();//message that mention deleted
            ChannelAccount user = turnContext.Activity.From;//���b�Z�[�W�𑗂��Ă���user
            Debug.WriteLine(turnContext.Activity.From);

            if (message == null || message.Contains("word"))
            {
                WordBook yourWb = WordBooks.getYourBook(user);//user��wordbook
                yourWb.addWordFromPic(turnContext, cancellationToken).Wait();             
            }

            else if (message.Contains("echo"))//echobot
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(message, message),cancellationToken);
            }
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    var m = "�悤����" + member + "����I";
                    await turnContext.SendActivityAsync(MessageFactory.Text(m,m), cancellationToken);
                }
            }
        }

    }
}
