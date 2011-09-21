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
using Common.Logging;
using Spring.Context;
using Spring.Context.Support;
using Spring.Messaging.Amqp.Core;

namespace Spring.Amqp.HelloWorld.Producer.Async
{
    /// <summary>
    /// A sample asynchronous producer.
    /// </summary>
    /// <remarks></remarks>
    class Program
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// Starts the program.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <remarks></remarks>
        public static void Main(string[] args)
        {
            using (var ctx = ContextRegistry.GetContext())
            {
                var amqpTemplate = (IAmqpTemplate)ctx.GetObject("IAmqpTemplate");
                int i = 0;
                while (true)
                {
                    amqpTemplate.ConvertAndSend("Hello World " + i++);
                    Logger.Info("Hello world message sent.");
                    Thread.Sleep(3000);
                }
            }            
        }
    }
}