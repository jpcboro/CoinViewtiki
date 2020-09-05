using System.Collections.Generic;

namespace CoinViewTiki.Models
{
    public class GroupedCoins : List<Coin>
    {
        public string Name { get; private set; }

        public GroupedCoins(string name, List<Coin> coins) : base(coins)
        {
            Name = name;
        }
    }
}