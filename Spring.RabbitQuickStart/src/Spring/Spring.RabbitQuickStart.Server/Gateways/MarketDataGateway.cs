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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Common.Logging;
using Spring.Messaging.Amqp.Rabbit.Core.Support;
using Spring.RabbitQuickStart.Common.Data;

namespace Spring.RabbitQuickStart.Server.Gateways
{
    /// <summary>
    /// 
    /// </summary>
    /// <author>Mark Pollack</author>
    /// <author>Don McRae</author>
    public class MarketDataGateway : RabbitGatewaySupport, IMarketDataGateway
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (MarketDataGateway));

        private bool _done = false;

        protected readonly Random random = new Random();
        private TimeSpan sleepTimeInSeconds = new TimeSpan(0,0,0,1,0);
        private List<MockStock> stocks = new List<MockStock>();

        public MarketDataGateway()
        {
            this.stocks.Add(new MockStock("AAPL", StockExchange.nasdaq, 255));
            this.stocks.Add(new MockStock("CSCO", StockExchange.nasdaq, 22));
            this.stocks.Add(new MockStock("DELL", StockExchange.nasdaq, 15));
            this.stocks.Add(new MockStock("GOOG", StockExchange.nasdaq, 500));
            this.stocks.Add(new MockStock("INTC", StockExchange.nasdaq, 22));
            this.stocks.Add(new MockStock("MSFT", StockExchange.nasdaq, 29));
            this.stocks.Add(new MockStock("ORCL", StockExchange.nasdaq, 24));
            this.stocks.Add(new MockStock("CAJ", StockExchange.nyse, 43));
            this.stocks.Add(new MockStock("F", StockExchange.nyse, 12));
            this.stocks.Add(new MockStock("GE", StockExchange.nyse, 18));
            this.stocks.Add(new MockStock("HMC", StockExchange.nyse, 32));
            this.stocks.Add(new MockStock("HPQ", StockExchange.nyse, 48));
            this.stocks.Add(new MockStock("IBM", StockExchange.nyse, 130));
            this.stocks.Add(new MockStock("TM", StockExchange.nyse, 76));
        }

        public TimeSpan SleepTimeInSeconds
        {
            set { sleepTimeInSeconds = value; }
        }

        public void SendMarketData()
        {
            while (!_done)
            {
                Quote quote = GenerateFakeQuote();
                Stock stock = quote.Stock;
                log.InfoFormat("Sending market data for {0}.", stock.Ticker);
                String routingKey = "APP.STOCK.QUOTES." + stock.StockExchange + "." + stock.Ticker;
                String  exchange = "APP.STOCK.MARKETDATA";
                RabbitTemplate.ConvertAndSend(exchange, routingKey, quote);
                log.Info("Sleeping " + sleepTimeInSeconds + " seconds before sending more market data.");
                Thread.Sleep(sleepTimeInSeconds);
            }
        }

        public void Stop()
        {
            _done = true;
        }

        private Quote GenerateFakeQuote()
        {
            MockStock stock = stocks[random.Next(0, stocks.Count - 1)];
            string price = stock.randomPrice();
             return new Quote(stock, price); 
        }

 

        public class MockStock : Stock
        {
            private int _basePrice;
            protected readonly Random random = new Random();

            public MockStock(string ticker, StockExchange stockexchange, int basePrice)
                : base(stockexchange, ticker)
            {
                _basePrice = basePrice; 
            }

            public String randomPrice()
            {
                return string.Format("{0:##.##}", this._basePrice + Math.Abs(Gaussian()));
            }

            private double Gaussian()
            {
                double x1, x2, w;

                do
                {
                    x1 = 2.0 * random.NextDouble() - 1.0;
                    x2 = 2.0 * random.NextDouble() - 1.0;
                    w = x1 * x1 + x2 * x2;
                } while (w >= 1.0);

                w = Math.Sqrt(-2.0 * Math.Log(w) / w);

                return x1 * w;
            }
        }
    }
}