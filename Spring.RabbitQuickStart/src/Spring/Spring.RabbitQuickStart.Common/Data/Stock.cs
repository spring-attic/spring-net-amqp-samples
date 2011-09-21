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
using System.Linq;
using System.Text;

namespace Spring.RabbitQuickStart.Common.Data
{
    /// <summary>
    /// Simple POCO class that represents a stock
    /// </summary>
    /// <author>Mark Pollack</author>
    /// <author>Don McRae</author>
    public class Stock
    {
        public String Ticker { get; set; }
        public StockExchange StockExchange { get; set; }

	    // For de-seialization:
	    public Stock() {
	    }
	
	    public Stock(StockExchange stockExchange, String ticker) {
		    this.StockExchange = stockExchange;
		    this.Ticker = ticker;
	    }
	
	    public override String ToString() {
		    return "Stock [ticker=" + Ticker + ", stockExchange=" + StockExchange + "]";
	    }
    }
}
