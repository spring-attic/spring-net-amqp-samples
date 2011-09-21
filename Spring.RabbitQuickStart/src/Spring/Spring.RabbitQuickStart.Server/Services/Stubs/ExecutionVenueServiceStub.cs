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
using System.Threading;
using Common.Logging;
using Spring.RabbitQuickStart.Common.Data;

namespace Spring.RabbitQuickStart.Server.Services.Stubs
{
    public class ExecutionVenueServiceStub : IExecutionVenueService
    {
        #region Logging

        private readonly ILog logger = LogManager.GetLogger(typeof(ExecutionVenueServiceStub));

        #endregion

        public TradeResponse ExecuteTradeRequest(TradeRequest request)
        {
            TradeResponse response = new TradeResponse();
            response.OrderType = request.OrderType;
            response.Price = CalculatePrice(request.Ticker, request.Quantity, request.OrderType, request.Price, request.UserName);
            response.Quantity = request.Quantity;
            response.Ticker = request.Ticker;
            response.ConfirmationNumber = new Guid().ToString();

            logger.Info("Sleeping 2 seconds to simulate processing...");
            Thread.Sleep(2000);
            return response;
        }

        private decimal CalculatePrice(string ticker, long quantity, string ordertype, decimal limitPrice, string userName)
        {
            // provide as sophisticated an impl as testing requires...for now all the same price.
            if (ordertype.CompareTo("LIMIT") == 0)
            {
                return limitPrice;
            }
            else
            {
                return 27.6m;
            }
            
        }
    }
}