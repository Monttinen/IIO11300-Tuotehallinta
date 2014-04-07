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
  /// Event handlerit tuotteille
  /// </summary>
  public partial class MainWindow : Window
  {
    private int selectedProductId = -1;
    private int selectedProductCategoryId = -1;

    private void LoadProductListFromDB()
    {
      lbProducts.ItemsSource = null;

      var result = from p in db.tuotteet
                   orderby p.tuotenimi
                   select p;

      lbProducts.ItemsSource = result.ToList();
      lbProducts.Items.Refresh();
      lbProducts.DisplayMemberPath = "tuotenimi";
      lbProducts.SelectedValuePath = "idtuote";

      // sidotaan tässä myös kategoriat comboboxiin
      cbProductCategory.ItemsSource = null;
      var result2 = from c in db.kategoriat
                    orderby c.nimi
                    select c.nimi;
      
      cbProductCategory.ItemsSource = result2.ToList();
      cbProductCategory.Items.Refresh();
    }

    // lisää tuotteiden handlerit tänne


    private void lbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (lbProducts.SelectedIndex == -1)
      {
        tbProductName.Text = "";
        tbProductDescription.Text = "";
        tbProductOther.Text = "";
        tbProductPrice.Text = "";

        tbProductName.IsEnabled = false;
        tbProductDescription.IsEnabled = false;
        tbProductOther.IsEnabled = false;
        tbProductPrice.IsEnabled = false;
        cbProductCategory.IsEnabled = false;

        return;
      }

      selectedProductId = int.Parse(lbProducts.SelectedValue.ToString());


      var result = from p in db.tuotteet
                   where p.idtuote == selectedProductId
                   select p;
      var selectedProduct = result.FirstOrDefault();
      if (selectedProduct != null)
      {
        sbiStatus.Content = string.Format("Valittu tuote {0}", selectedProduct.tuotenimi);
        tbProductName.Text = selectedProduct.tuotenimi;
        tbProductDescription.Text = selectedProduct.kuvaus;
        tbProductOther.Text = "";
        tbProductPrice.Text = string.Format("{0}", selectedProduct.hinta);

        tbProductName.IsEnabled = true;
        tbProductDescription.IsEnabled = true;
        tbProductOther.IsEnabled = false;
        tbProductPrice.IsEnabled = true;
        cbProductCategory.IsEnabled = true;
      }
    }

    private void btnAddProduct_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnRemoveProduct_Click(object sender, RoutedEventArgs e)
    {

    }

    private void cbProductCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // bugaa
      /*selectedProductCategoryId = int.Parse(cbProductCategory.SelectedValue.ToString());
      var category = from c in db.kategoriat
                     where c.idkategoria == selectedProductCategoryId
                     select c;

      var product = (from p in db.tuotteet
                    where p.idtuote == selectedProductId
                    select p).First();

      if(product.kategoria.Count < 1) // käytetään vain yhtä kategoriaa vaikka db sallisikin monta
        product.kategoria.Add((kategoria)category);*/
    }
  }
}