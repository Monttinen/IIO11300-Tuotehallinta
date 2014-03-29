using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAMK.ICT
{
    class Category
    {
        int CategoryId;
        string CategoryName;
        string CategoryDescription;
        ArrayList Products;

        public Category(int id, string name, string category)
        {
            // TODO Lisää tarkistukset
            this.CategoryId = id;
            this.CategoryName = name;
            this.CategoryDescription = category;

            Products = new ArrayList();
        }

        public void AddProduct(Product p)
        {
            this.Products.Add(p);
        }
    }
}
