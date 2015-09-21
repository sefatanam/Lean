﻿/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
*/

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using QuantConnect.Data;
using QuantConnect.Data.Market;

namespace QuantConnect.Tests.Common
{
    [TestFixture]
    public class SymbolTests
    {
        private JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        [Test]
        public void SurvivesRoundtripSerialization()
        {
            var expected = new Symbol("sid", "value");
            var json = JsonConvert.SerializeObject(expected, Settings);
            var actual = JsonConvert.DeserializeObject<Symbol>(json, Settings);
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void SurvivesRoundtripSerializationWithTypeNameHandling()
        {
            var expected = new Symbol("sid", "value");
            var json = JsonConvert.SerializeObject(expected, Settings);
            var actual = JsonConvert.DeserializeObject<Symbol>(json);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HandlesListTicks()
        {
            const string json = @"{'$type':'System.Collections.Generic.List`1[[QuantConnect.Data.BaseData, QuantConnect.Common]], mscorlib',
'$values':[{'$type':'QuantConnect.Data.Market.Tick, QuantConnect.Common',
'TickType':0,'Quantity':1,'Exchange':'',
'SaleCondition':'',
'Suspicious':false,'BidPrice':0.72722,'AskPrice':0.7278,'LastPrice':0.72722,'DataType':2,'IsFillForward':false,'Time':'2015-09-18T16:52:37.379',
'EndTime':'2015-09-18T16:52:37.379',
'Symbol':{'$type':'QuantConnect.Symbol, QuantConnect.Common',
'Value':'EURGBP',
'SID':'EURGBP'},'Value':0.72722,'Price':0.72722}]}";

            var expected = new Symbol("EURGBP");
            var settings = Settings;
            var actual = JsonConvert.DeserializeObject<List<BaseData>>(json, settings);
            Assert.AreEqual(expected, actual[0].Symbol);
        }

        [Test]
        public void SymbolTypeNameHandling()
        {
            const string json = @"{'$type':'QuantConnect.Symbol, QuantConnect.Common',
'Value':'EURGBP',
'Permtick':'EURGBP'}";
            var expected = new Symbol("EURGBP");
            var actual = JsonConvert.DeserializeObject<Symbol>(json);//, settings);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TickRoundTrip()
        {
            var tick = new Tick
            {
                Symbol = "EURGBP",
                AskPrice = 1,
                Time = DateTime.Now,
                Exchange = "",
                Value = 2,
                EndTime = DateTime.Now,
                Quantity = 1,
                BidPrice = 2,
                SaleCondition = ""
            };
            var json = JsonConvert.SerializeObject(tick, Settings);
            var actual = JsonConvert.DeserializeObject<Tick>(json, Settings);
            Assert.AreEqual(tick.Symbol, actual.Symbol);

            json = JsonConvert.SerializeObject(tick, Settings);
            actual = JsonConvert.DeserializeObject<Tick>(json);
            Assert.AreEqual(tick.Symbol, actual.Symbol);
        }
    }
}
