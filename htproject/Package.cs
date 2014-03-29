using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JAMK.ICT
{
    class Package
    {
        int PackageId;
        string PackageName;
        double PackagePrice;
        ArrayList Products;
        // huonetieto tulee hotellinx järjestelmästä, ei ole varmaa tietoa rakenteesta
        int RoomId;

        public Package(int id, string name, double price, int roomid)
        {
            this.PackageId = id;
            this.PackageName = name;
            this.PackagePrice = price;
            this.RoomId = roomid;

            Products = new ArrayList();
        }

        public void AddProduct(Product p)
        {
            this.Products.Add(p);
        }
    }
}
