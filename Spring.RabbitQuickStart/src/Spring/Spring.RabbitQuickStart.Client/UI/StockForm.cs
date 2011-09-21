#region License

/*
 * Copyright 2002-2009 the original author or authors.
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
using System.Windows.Forms;
using Common.Logging;
using Spring.Context.Support;
using Spring.RabbitQuickStart.Common.Data;
using Spring.Messaging.Amqp.Core;
using Spring.Messaging.Amqp.Rabbit.Connection;
using Spring.Messaging.Amqp.Rabbit.Core;

namespace Spring.RabbitQuickStart.Client.UI
{
    /// <summary>
    /// 
    /// </summary>
    /// <author>Mark Pollack</author>
    /// <author>Don McRae</author>
    public partial class StockForm : Form
    {
        #region Logging Definition

        private static readonly ILog log = LogManager.GetLogger(typeof (StockForm));

        #endregion

        private StockController stockController;
        private string _currentBinding;

        public StockForm()
        {
            InitializeComponent();
            stockController = ContextRegistry.GetContext()["StockController"] as StockController;
            stockController.StockForm = this;
        }

        public StockController Controller
        {
            set { stockController = value; }
        }

        private void OnSendTradeRequest(object sender, EventArgs e)
        {
            //In this simple example no data is collected from the view.
            //Instead a hardcoded trade request is created in the controller.
            tradeRequestStatusTextBox.Text = "Request Pending...";
            stockController.SendTradeRequest();            
            log.Info("Sent trade request.");
        }

        public void UpdateTrade(TradeResponse trade)
        {
            Invoke(new MethodInvoker(
                       delegate
                           {
                               tradeRequestStatusTextBox.Text = "Confirmed. " + trade.Ticker + " " + trade.Price;
                           }));
        }

        public void UpdateMarketData(Quote quote)
        {
            Invoke(new MethodInvoker(
                       delegate
                           {
                               marketDataListBox.Items.Add(quote.Stock.StockExchange.ToString() + "." + quote.Stock.Ticker + " " + quote.Price);
                           }));
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            RebindQueue(txtRoutingKey.Text);
            marketDataListBox.Items.Clear();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            RebindQueue(string.Empty);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            RebindQueue(txtRoutingKey.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            marketDataListBox.Items.Clear();
        }

        private void StockForm_Load(object sender, EventArgs e)
        {
            try
            {
                using (IConnectionFactory connectionFactory = new SingleConnectionFactory())
                {
                    IAmqpAdmin amqpAdmin = new RabbitAdmin(connectionFactory);

                    TopicExchange mktDataExchange = new TopicExchange("APP.STOCK.MARKETDATA", false, false);
                    amqpAdmin.DeclareExchange(mktDataExchange);
                    Spring.Messaging.Amqp.Core.Queue mktDataQueue = new Spring.Messaging.Amqp.Core.Queue("APP.STOCK.MARKETDATA");
                    amqpAdmin.DeclareQueue(mktDataQueue);

                    //Create the Exchange for MarketData Requests if it does not already exist.
                    //amqpAdmin.DeclareBinding(BindingBuilder.Bind(mktDataQueue).To(mktDataExchange).With(_currentBinding));
                    //Set up initial binding
                    RebindQueue("APP.STOCK.QUOTES.nasdaq.*");
                }
            }
            catch (Exception ex)
            {
                 log.ErrorFormat("Uncaught application exception.", ex);
            }
        }

        private void RebindQueue(string routingKey)
        {
            try
            {
                using (IConnectionFactory connectionFactory = new SingleConnectionFactory())
                {
                    IAmqpAdmin amqpAdmin = new RabbitAdmin(connectionFactory);

                    TopicExchange mktDataExchange = new TopicExchange("APP.STOCK.MARKETDATA", false, false);
                    Spring.Messaging.Amqp.Core.Queue mktDataQueue = new Spring.Messaging.Amqp.Core.Queue("APP.STOCK.MARKETDATA");

                    if (!string.IsNullOrEmpty(_currentBinding))
                        amqpAdmin.RemoveBinding(BindingBuilder.Bind(mktDataQueue).To(mktDataExchange).With(_currentBinding));

                    _currentBinding = routingKey;
                    if (!string.IsNullOrEmpty(_currentBinding))
                    {
                        amqpAdmin.DeclareBinding(BindingBuilder.Bind(mktDataQueue).To(mktDataExchange).With(_currentBinding));
                        txtRoutingKey.Text = _currentBinding;
                    }
                }

            }
            catch (Exception ex)
            {
                log.ErrorFormat("Uncaught application exception.", ex);
            }
        }
    }
}