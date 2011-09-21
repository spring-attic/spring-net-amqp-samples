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
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using Spring.Context.Support;
using Spring.Messaging.Amqp.Rabbit.Core;
using Spring.RabbitQuickStart.Server.Gateways;

namespace Spring.RabbitQuickStart.Server
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
            try
            {
                // Using Spring's IoC container
                ContextRegistry.GetContext();

                Console.Out.WriteLine("Server listening...");
                IMarketDataGateway marketDataService =
                    ContextRegistry.GetContext().GetObject("MarketDataGateway") as MarketDataGateway;
                ThreadStart job = new ThreadStart(marketDataService.SendMarketData);
                Thread thread = new Thread(job);
                thread.Start();
                Console.Out.WriteLine("--- Press <return> to quit ---");
                Console.ReadLine();
                marketDataService.Stop();
                thread.Join(2000);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e);
                Console.Out.WriteLine("--- Press <return> to quit ---");
                Console.ReadLine();
            }
        }
    }
}
