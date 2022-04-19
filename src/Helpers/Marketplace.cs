using SpaceSimulation.components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace SpaceSimulation.Helpers
{
    class Marketplace
    {
        public GoodMarket[] offers { get; }
        public Goods[] goods;
        // Creates the marketplace with all goods known, but no existing offers.
        public Marketplace()
        {
            List<Goods> items;
            using (StreamReader r = new StreamReader("Content/goodsList.json"))
            {
                string json = r.ReadToEnd();
                items = JsonSerializer.Deserialize<List<Goods>>(json);
            }
            offers = new GoodMarket[items.Count];
            goods = new Goods[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                goods[i] = items[i];
            }
            items = null;
        }

    }
}
