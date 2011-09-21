#region License

/*
 * Copyright 2002-2010 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;
using Spring.Messaging.Amqp.Core;
using Spring.Messaging.Amqp.Rabbit.Core.Support;
using Spring.RabbitQuickStart.Client.Gateways;
using Spring.RabbitQuickStart.Common.Data;

namespace Spring.RabbitQuickStart.Client.Gateways
{
    /// <summary>
    /// 
    /// </summary>
    /// <author>Mark Pollack</author>
    /// <author>Don McRae</author>
    public class RabbitStockServiceGateway : RabbitGatewaySupport, IStockServiceGateway
    {
        private string defaultReplyToQueue;
        
        public string DefaultReplyToQueue
        {
            set { defaultReplyToQueue = value; }
        }

        public void Send(TradeRequest tradeRequest)
        {
            RabbitTemplate.ConvertAndSend(tradeRequest, delegate(Message message)
            {
                message.MessageProperties.ReplyTo = new Address(defaultReplyToQueue);
                message.MessageProperties.CorrelationId = new Guid().ToByteArray();
                return message;
            });

        }
    }
}