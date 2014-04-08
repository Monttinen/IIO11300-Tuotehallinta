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
        tbProductPrice.Text = "";

        tbProductName.IsEnabled = false;
        tbProductDescription.IsEnabled = false;
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
        tbProductPrice.Text = string.Format("{0}", selectedProduct.hinta);

        tbProductName.IsEnabled = true;
        tbProductDescription.IsEnabled = true;
        tbProductPrice.IsEnabled = true;
        cbProductCategory.IsEnabled = true;

        // TODO: valitse kategoria cb:stä
        if (selectedProduct.kategoria.Count > 0)
        {
          selectedProductCategoryId = selectedProduct.kategoria.First().idkategoria;
          cbProductCategory.SelectedValue = selectedProductCategoryId;
        }
        else
        {
          selectedProductCategoryId = -1;
          cbProductCategory.SelectedValue = null;
        }
        
      }

    }

    private void btnAddProduct_Click(object sender, RoutedEventArgs e)
    {
      tuote t = new tuote { tuotenimi = "<uusi tuote>", hinta = 0.0, kuvaus = "tuotteen kuvaus" };
      db.tuotteet.Add(t);
      sbiStatus.Content = "Lisätty uusi tuote";
      db.SaveChanges();
      LoadProductListFromDB();

      // TODO: tässä kohtaa voitaisiin valita lisätty valmiiksi

    }

    private void btnRemoveProduct_Click(object sender, RoutedEventArgs e)
    {
      var result = from p in db.tuotteet
                   where p.idtuote == selectedProductId
                   select p;

      if (result.Count() > 0)
      {
        var p = result.First();
        db.tuotteet.Remove(p);

        sbiStatus.Content = string.Format("Poistettiin tuote {0}", p.tuotenimi);
        db.SaveChanges();
        LoadProductListFromDB();
      }
    }

    private void cbProductCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // ei käytetä tätä vaan DropDownClosed
    }

    private void cbProductCategory_DropDownClosed(object sender, EventArgs e)
    {
      if (cbProductCategory.SelectedValue == null || selectedProductId < 0) return;

      selectedProductCategoryId = (int)cbProductCategory.SelectedValue;

      if (selectedProductCategoryId < 0) return;

      var product = (from p in db.tuotteet
                     where p.idtuote == selectedProductId
                     select p).First();

      if (product.kategoria.Count < 1 || product.kategoria.First().idkategoria != selectedProductCategoryId)
      {
        var category = from c in db.kategoriat
                       where c.idkategoria == selectedProductCategoryId
                       select c;
        // käytetään vain yhtä kategoriaa vaikka db sallisikin monta
        product.kategoria.Clear();
        product.kategoria.Add(category.First());
      }
    }

    private void tbProductName_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbProductName.Text == "") return;
      var result = (from p in db.tuotteet
                    where p.idtuote == selectedProductId
                    select p).First().tuotenimi = tbProductName.Text;
    }

    private void tbProductPrice_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbProductPrice.Text == "") return;
      var result = (from p in db.tuotteet
                    where p.idtuote == selectedProductId
                    select p).First().hinta = double.Parse(tbProductPrice.Text);
    }

    private void tbProductDescription_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbProductDescription.Text == "") return;
      var result = (from p in db.tuotteet
                    where p.idtuote == selectedProductId
                    select p).First().kuvaus = tbProductDescription.Text;
    }
  }
}