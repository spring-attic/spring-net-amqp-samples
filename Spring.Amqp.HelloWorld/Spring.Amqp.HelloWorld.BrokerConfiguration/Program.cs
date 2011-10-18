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
#region

using System;
using Spring.Messaging.Amqp.Core;
using Spring.Messaging.Amqp.Rabbit.Connection;
using Spring.Messaging.Amqp.Rabbit.Core;

#endregion

namespace Spring.Amqp.HelloWorld.BrokerConfiguration
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (IConnectionFactory connectionFactory = new CachingConnectionFactory())
            {
                IAmqpAdmin amqpAdmin = new RabbitAdmin(connectionFactory);

                var helloWorldQueue = new Queue("hello.world.queue");

                amqpAdmin.DeclareQueue(helloWorldQueue);

                //Each queue is automatically bound to the default direct exchange.      
   
                Console.WriteLine("Queue [hello.world.queue] has been declared.");
                Console.WriteLine("Press 'enter' to exit");
                Console.ReadLine();
            }
        }
    }
}