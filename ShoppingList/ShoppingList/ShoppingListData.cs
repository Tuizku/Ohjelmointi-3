using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList
{
    [Serializable]
    public class ShoppingListData
    {
        public string Name;
        public List<Item> Items;
    }
}
