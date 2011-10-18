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


using System.Collections;
using Common.Logging;
using Spring.RabbitQuickStart.Common.Data;
using Spring.RabbitQuickStart.Server.Services;
using Spring.Util;

namespace Spring.RabbitQuickStart.Server.Handlers
{
    /// <summary>
    /// Simple POCO class that represents a simple trade request
    /// </summary>
    /// <author>Mark Pollack</author>
    public class StockAppHandler
    {
        #region Logging

        private readonly ILog logger = LogManager.GetLogger(typeof(StockAppHandler));

        #endregion
        private readonly IExecutionVenueService _executionVenueService;

        private readonly ICreditCheckService _creditCheckService;

        private readonly ITradingService _tradingService;

        public StockAppHandler(IExecutionVenueService executionVenueService, ICreditCheckService creditCheckService, ITradingService tradingService)
        {
            _executionVenueService = executionVenueService;
            _creditCheckService = creditCheckService;
            _tradingService = tradingService;
        }

        public TradeResponse Handle(TradeRequest tradeRequest)
        {
            logger.Info("Recieved trade request");
            TradeResponse tradeResponse;
            ArrayList errors = new ArrayList();
            if (_creditCheckService.CanExecute(tradeRequest, errors))
            {
                tradeResponse = _executionVenueService.ExecuteTradeRequest(tradeRequest);
            }
            else
            {
                tradeResponse = new TradeResponse();
                tradeResponse.Error = true;
                tradeResponse.ErrorMessage = StringUtils.ArrayToCommaDelimitedString(errors.ToArray());

            }
            _tradingService.ProcessTrade(tradeRequest, tradeResponse);
            return tradeResponse;
        }
    }
}