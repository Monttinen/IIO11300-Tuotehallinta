﻿using System;
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
    private int selectedProductId = -1;          // Muokataan VAIN updateSelectedProducts() funktiossa!
    private int selectedProductIndex = -1;       // Muokataan VAIN updateSelectedProducts() funktiossa!
    private int selectedProductCategoryId = -1; 

   
    /// <summary>
    /// Valitsee tuotteen listassa valituksi id:n perusteella.
    /// Lisäksi Laittaa tuotteen tiedot näyttöön
    /// </summary>
    /// <param name="id">Tuotteen id</param>
    private void selectProductId(int id)
    {
      try
      {
        lbProducts.SelectedValue = id;
      }
      catch (Exception) { selectProductIndex(0); }
      updateSelectedProducts();
      showProductDetails();
    }


    /// <summary>
    /// Valitsee tuotteen listassa valitussa listboxin indeksin perusteella.
    /// Lisäksi laittaa tuotteen tiedot näyttöön
    /// </summary>
    /// <param name="index">Listboxin indeksi</param>
    private void selectProductIndex(int index)
    {
      if (index < -1) return;
      if (lbProducts.Items.Count - 1 >= index) lbProducts.SelectedIndex = index;
      else lbProducts.SelectedIndex = -1;
      updateSelectedProducts();
      showProductDetails();
    }


    /// <summary>
    /// Päivittää julkiset muuttujat valitun tuotteen id:stä ja indeksistä ajan tasalle
    /// </summary>
    private void updateSelectedProducts()
    {
      try {
        selectedProductId = int.Parse(lbProducts.SelectedValue.ToString());
      }
      catch (Exception) { selectedProductId = -1; }
      selectedProductIndex = lbProducts.SelectedIndex;
    }


    /// <summary>
    /// Hakee tuotelistaan tietokannassa olevan version
    /// </summary>
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

      UpdateProductCategoryList();

    }

    /// <summary>
    /// Päivittää comboboxin kategorialistan
    /// </summary>
    private void UpdateProductCategoryList()
    {
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


    /// <summary>
    /// Piirtää tekstikenttiin valitun tuotteen tiedot
    /// </summary>
    private void showProductDetails()
    {
      if (selectedProductIndex == -1)
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

        // Valitsee kategorian comboboxiin
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


    /// <summary>
    /// Listasta valittu eri tuote: asetetaan tuote valituksi ja piirretään näyttöön
    /// </summary>
    private void lbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      updateSelectedProducts();
      showProductDetails();
    }


    /// <summary>
    /// Lisää tuotteen. Tallentaa samantien tietokantaan. Asettaa lisätyn tuotteen valituksi.
    /// </summary>
    private void btnAddProduct_Click(object sender, RoutedEventArgs e)
    { /* tuottelle id kannasta (auto increment) */
      tuote t = new tuote { tuotenimi = "<uusi tuote>", hinta = 0.0, kuvaus = "tuotteen kuvaus" };
      db.tuotteet.Add(t);
      sbiStatus.Content = "Lisätty uusi tuote";
      db.SaveChanges(); 
      LoadProductListFromDB();

      selectProductId(t.idtuote);
      showProductDetails();
    }

    /// <summary>
    /// Poistaa tuotteen tietokannasta. Asetetaan listassa edellinen valituksi.
    /// </summary>
    private void btnRemoveProduct_Click(object sender, RoutedEventArgs e)
    {
      int tmp = selectedProductIndex;
      var result = from p in db.tuotteet
                   where p.idtuote == selectedProductId
                   select p;

      if (result.Count() > 0)
      {
        var p = result.First();
        if (p.paketti.Count > 0)
        { /* Tietokanta ei salli poistaa jos tuote kuuluu pakettiin, mutta entiteettikokoelma 
           * (jostain syystä?) sallisi, siksi tarkistus tässä.*/
          sbiStatus.Content = "You can't delete this product as it's part of a package";
          return;
        }
        db.tuotteet.Remove(p);

        sbiStatus.Content = string.Format("Poistettiin tuote {0}", p.tuotenimi);
        try { db.SaveChanges(); }
        catch (System.Data.Entity.Infrastructure.DbUpdateException) { 
          sbiStatus.Content = "You can't delete this product as it's part of a package";
        } // tämä estetään jo ylempänä, tässä kuitenkin koska näin sen tulisi kait toimia
        LoadProductListFromDB();
        selectProductIndex(tmp - 1);
        showProductDetails();
      }

    }

    private void cbProductCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // ei käytetä tätä vaan DropDownClosed
    }

    /// <summary>
    /// Kun comboboxissa valittu kategoria tuotteelle, lisätään tuotteelle se kategoriaksi.
    /// </summary>
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


    /// <summary>
    /// Tallennetaan nimen tiedot heti kun focus poistuu.
    /// Tallentaa entiteettikokoelmaan, ei tietokantaan
    /// </summary>
    private void tbProductName_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbProductName.Text == "") return;
      var result = (from p in db.tuotteet
                    where p.idtuote == selectedProductId
                    select p).First().tuotenimi = tbProductName.Text;
    }


    /// <summary>
    /// Tallennetaan tuotteen hinta kun focus poistuu.
    /// Tallentaa entiteettikokoelmaan, ei tietokantaan.
    /// Jos hinta ei ole kelvollinen double, tallennetaan hinnaksi 0.
    /// </summary>
    private void tbProductPrice_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbProductPrice.Text == "") return;
      tbProductPrice.Text = tbProductPrice.Text.Replace(" ", string.Empty);
      double price;
      if (!double.TryParse(tbProductPrice.Text, out price))
      {
        price = 0;
        sbiStatus.Content = "Incalid price detected, inserted 0 instead.";
      }
      var result = (from p in db.tuotteet
                    where p.idtuote == selectedProductId
                    select p).First().hinta = price;
    }


    /// <summary>
    /// Tuotteen kuvaus tallennetaan entiteettikokoelmaan, ei tietokantaan,
    /// kun siirretään focus jonnekkin muualle
    /// </summary>
    private void tbProductDescription_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbProductDescription.Text == "") return;
      var result = (from p in db.tuotteet
                    where p.idtuote == selectedProductId
                    select p).First().kuvaus = tbProductDescription.Text;
    }
  }
}