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
    /// Simple POCO class that represents a quote
    /// </summary>
    /// <author>Mark Pollack</author>
    /// <author>Don McRae</author>
    public class Quote
    {
        public Stock Stock { get; set; }
        public String Price { get; set; }
        public long Timestamp { get; set; }

        public Quote()
            : this(null, null)
        {
		    
	    }

        public Quote(Stock stock, String price)
            : this(stock, price, DateTime.Now.Ticks)
        {
	    }

	    public Quote(Stock stock, String price, long timestamp) {
		    this.Stock = stock;
		    this.Price = price;
		    this.Timestamp = timestamp;
	    }

	    public String getTimeString() {
            DateTime date = new DateTime(Timestamp);
            return date.ToShortTimeString();
	    }

	
	    public override String ToString() {
		    return "Quote [time=" + getTimeString() + ", stock=" + Stock + ", price=" + Price + "]";
	    }
    }
}
