using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAMK.ICT
{
    class Product
    {
        int ProductId;
        string ProductName;
        string ProductDescrption;
        double ProductPrice;

        public Product(int id, string name, string description, double price)
        {
            // TODO lisää tarkistuksia
            this.ProductId = id;
            this.ProductName = name;
            this.ProductDescrption = description;
            this.ProductPrice = price;
        }
    }
}
