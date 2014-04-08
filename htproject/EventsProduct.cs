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
    private tuote selectedProduct;
    private kategoria selectedProductCategory;

    private void LoadProductListFromDB()
    {
      lbProducts.ItemsSource = null;

      var result = from p in db.tuotteet
                   orderby p.tuotenimi
                   select p;

      lbProducts.ItemsSource = result.ToList();
      lbProducts.DisplayMemberPath = "tuotenimi";
      lbProducts.SelectedValuePath = "idtuote";
      lbProducts.Items.Refresh();

      // sidotaan tässä myös kategoriat comboboxiin
      cbProductCategory.ItemsSource = null;
      var result2 = from c in db.kategoriat
                    orderby c.nimi
                    select new { c.idkategoria, c.nimi };
      
      cbProductCategory.ItemsSource = result2.ToList();
      cbProductCategory.DisplayMemberPath = "nimi";
      cbProductCategory.SelectedValuePath = "idkategoria";
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

        // TODO: valitse kategoria cb:stä
        selectedProductCategoryId = selectedProduct.kategoria.First().idkategoria;
        cbProductCategory.SelectedValue = selectedProductCategoryId;
        
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
      
    }



    private void cbProductCategory_DropDownClosed(object sender, EventArgs e)
    {
      if (selectedProductId < 0) return;

      selectedProductCategoryId = (int)cbProductCategory.SelectedValue;

      if (selectedProductCategoryId < 0) return;

      var product = (from p in db.tuotteet
                     where p.idtuote == selectedProductId
                     select p).First();

      if (product.kategoria.First().idkategoria != selectedProductCategoryId)
      {
        var category = from c in db.kategoriat
                       where c.idkategoria == selectedProductCategoryId
                       select c;
        // käytetään vain yhtä kategoriaa vaikka db sallisikin monta
        product.kategoria.Clear();
        product.kategoria.Add(category.First());
      }
    }
  }
}