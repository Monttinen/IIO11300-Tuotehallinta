using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JAMK.ICT;


namespace ProductManagement
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private ArrayList Categories;
    private ArrayList Products;
    private ArrayList Packages;

    private bool Connected;
    public MainWindow()
    {
      InitializeComponent();
      tabCategory.Visibility = Visibility.Collapsed;
      tabProduct.Visibility = Visibility.Collapsed;
      tabPackage.Visibility = Visibility.Collapsed;

      Categories = new ArrayList();
      Products = new ArrayList();
      Packages = new ArrayList();

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      if (!Connected)
        Connect();
      else
        Disconnect();
    }

    private void Connect()
    {
      if (!Connected)
      {
        sbiStatus.Content = "Connected";
        btnConnect.Content = "Disconnect";
        tabCategory.Visibility = Visibility.Visible;
        tabProduct.Visibility = Visibility.Visible;
        tabPackage.Visibility = Visibility.Visible;

        Connected = true;
      }
      
    }

    private void Disconnect()
    {
        sbiStatus.Content = "";
        btnConnect.Content = "Login";
        tabCategory.Visibility = Visibility.Collapsed;
        tabProduct.Visibility = Visibility.Collapsed;
        tabPackage.Visibility = Visibility.Collapsed;

        Connected = false;
      
    }

    private void getDummyData()
    {
      // lisätään testaamista varten kamaa listoihin
      Product product1 = new Product(0, "Aamupala", "Aamupala yhdelle", 10.0);
      Products.Add(product1);
      Product product2 = new Product(1, "Lounas", "Lounas yhdelle", 15.0);
      Products.Add(product2);
      Product product3 = new Product(2, "Illallinen", "Illallinen yhdelle", 20.0);
      Products.Add(product3);
      Product product4 = new Product(3, "Yömyssy", "you know", 10.0);
      Products.Add(product4);

      Product product5 = new Product(4, "Liinavaate", "Liinavaatteet huoneeseen", 5.0);
      Products.Add(product5);

      Category tmp = new Category(0, "Ruokailu", "Sisältää ruokailut");
      tmp.AddProduct(product1);
      tmp.AddProduct(product2);
      tmp.AddProduct(product3);
      tmp.AddProduct(product4);
      Categories.Add(tmp);

      tmp = new Category(1, "Huoneen varattavat", "Sisältää huoneeseen varattavia tuotteita");
      tmp.AddProduct(product5);
      Categories.Add(tmp);

      Categories.Add(new Category(2, "Joku kategoria", "Sisältää tietoa"));
      Categories.Add(new Category(3, "Vielä yksi kategoria", "Sisältää myös tietoa"));

      Package package1 = new Package(0, "Yöpyminen", 120.0, 123);
      package1.AddProduct(product1);
      package1.AddProduct(product5);

      Packages.Add(package1);

    }
  }
}
