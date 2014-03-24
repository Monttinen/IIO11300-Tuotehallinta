using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JAMK.ICT;


namespace JAMK.ICT
{
  class Product
  {
    int ProductId;
    string ProductName;
    string ProductDescrption;
    double ProductPrice;

    public Product(int id, string name, string description, double price){
      // TODO lisää tarkistuksia
      this.ProductId = id;
      this.ProductName = name;
      this.ProductDescrption = description;
      this.ProductPrice = price;
    }
  }

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

    public void AddProduct(Product p){
      this.Products.Add(p);
    }
  }

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

    public void AddProduct(Product p){
      this.Products.Add(p);
    }
  }
}
