#region License

/*
 * Copyright 2002-2010 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;
using System.Configuration;
using Spring.Erlang.Connection;
using Spring.Messaging.Amqp.Core;
using Spring.Messaging.Amqp.Rabbit.Connection;
using Spring.Messaging.Amqp.Rabbit.Core;
using IConnectionFactory = Spring.Messaging.Amqp.Rabbit.Connection.IConnectionFactory;

namespace Spring.RabbitQuickStart.BrokerConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    /// <author>Mark Pollack</author>
    /// <author>Don McRae</author>
    class Program
    {
        static void Main(string[] args)
        {
            using (IConnectionFactory connectionFactory = new CachingConnectionFactory())
            {
                IAmqpAdmin amqpAdmin = new RabbitAdmin(connectionFactory);

                //Each queue is automatically bound to the default direct exchange.      
                amqpAdmin.DeclareQueue(new Queue(ConfigurationManager.AppSettings["STOCK_REQUEST_QUEUE_NAME"]));
                amqpAdmin.DeclareQueue(new Queue(ConfigurationManager.AppSettings["STOCK_RESPONSE_QUEUE_NAME"]));

                TopicExchange mktDataExchange = new TopicExchange(ConfigurationManager.AppSettings["MARKET_DATA_EXCHANGE_NAME"], false, false);
                amqpAdmin.DeclareExchange(mktDataExchange);
                Queue mktDataQueue = new Queue(ConfigurationManager.AppSettings["MARKET_DATA_QUEUE_NAME"]);
                amqpAdmin.DeclareQueue(mktDataQueue);

                Console.WriteLine("Queues and exchanges have been declared.");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
            }
        }
    }
}
