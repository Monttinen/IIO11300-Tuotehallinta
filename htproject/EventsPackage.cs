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
  /// Event handlerit pakettinäkymälle
  /// </summary>
  public partial class MainWindow : Window
  {
    private int selectedPackageId = -1;
    private int selectedPackageIndex = -1;

    /// <summary>
    /// Hakee pakettilistan tietokannasta ja asettaa myös paketteihin liitettävien tuotteiden listan
    /// </summary>
    private void LoadPackageListFromDB()
    {
      lbPackages.ItemsSource = null;

      var result = from p in db.paketit
                   orderby p.nimi
                   select p;

      lbPackages.ItemsSource = result.ToList();
      lbPackages.DisplayMemberPath = "nimi";
      lbPackages.SelectedValuePath = "pakettiID";
      lbPackages.Items.Refresh();

      lbPackageAvailableProducts.ItemsSource = lbProducts.ItemsSource; // hähä :P
      lbPackageAvailableProducts.DisplayMemberPath = "tuotenimi";
      lbPackageAvailableProducts.SelectedValuePath = "idtuote";
      lbPackageAvailableProducts.Items.Refresh();
    }


    /// <summary>
    /// Lisää paketin ja tallentaa sen tietokantaan. Lisäksi valitsee listasta.
    /// </summary>
    private void btnPackageAdd_Click(object sender, RoutedEventArgs e)
    {
      paketti p = new paketti { nimi = "<uusi paketti>", paketinHinta = 0.0, huoneID = 0, pakettiKuvaus = "paketin kuvaus" };
      db.paketit.Add(p);
      sbiStatus.Content = "Lisätty uusi paketti";
      db.SaveChanges();
      LoadPackageListFromDB();

      selectPackageID(p.pakettiID);
      showPackageDetails();

    }

    /// <summary>
    /// Listasta valittu paketti
    /// </summary>
    private void lbPackages_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      updateSelectedPackage();
      showPackageDetails();
    }

    /// <summary>
    /// Poistaa paketin tietokannasta.
    /// </summary>
    private void btnPackageRemove_Click(object sender, RoutedEventArgs e)
    {
      int tmp = selectedPackageIndex;
      var result = from p in db.paketit
                   where p.pakettiID == selectedPackageId
                   select p;
      if (result.Count() > 0)
      {
        var p = result.First();
        db.paketit.Remove(p);

        sbiStatus.Content = string.Format("Poistettiin paketti {0}", p.nimi);
        db.SaveChanges();
        LoadPackageListFromDB();

        selectPackageIndex(tmp - 1);
        showPackageDetails();
      }
    }

    /// <summary>
    /// Lisää valitun tuotteen pakettiin.
    /// </summary>
    private void btnPackageAddProduct_Click(object sender, RoutedEventArgs e)
    {
      var tuote = from p in db.tuotteet
                  where p.idtuote == (int)lbPackageAvailableProducts.SelectedValue
                  select p;

      (from p in db.paketit
                  where p.pakettiID == selectedPackageId
                  select p).First().tuote.Add(tuote.First());

      db.SaveChanges();
      showPackageDetails();

    }

    /// <summary>
    /// Poistaa valitun tuotteen paketista.
    /// </summary>
    private void btnPackageRemoveProduct_Click(object sender, RoutedEventArgs e)
    {
      var tuote = from p in db.tuotteet
                  where p.idtuote == (int)lbPackageProducts.SelectedValue
                  select p;

      (from p in db.paketit
       where p.pakettiID == selectedPackageId
       select p).First().tuote.Remove(tuote.First());

      db.SaveChanges();
      showPackageDetails();
    }

    /// <summary>
    /// Tallennetaan nimen kun focus poistuu.
    /// Tallentaa entiteettikokoelmaan, ei tietokantaan
    /// </summary>
    private void tbPackageName_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbPackageName.Text == "") return;
      var result = (from p in db.paketit
                    where p.pakettiID == selectedPackageId
                    select p).First().nimi = tbPackageName.Text;

    }

    /// <summary>
    /// Tallennetaan huoneen tiedot heti kun focus poistuu.
    /// Tallentaa entiteettikokoelmaan, ei tietokantaan
    /// </summary>
    private void tbPackageRoomID_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbPackageRoomID.Text == "") return;
      tbPackageRoomID.Text = tbPackageRoomID.Text.Replace(" ", string.Empty);
      var result = (from p in db.paketit
                    where p.pakettiID == selectedPackageId
                    select p).First().huoneID = int.Parse(tbPackageRoomID.Text);
    }

    /// <summary>
    /// Tallennetaan paketin hinnan heti kun focus poistuu.
    /// Tallentaa entiteettikokoelmaan, ei tietokantaan
    /// </summary>
    private void tbPackagePrice_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbPackagePrice.Text == "") return;
      tbPackagePrice.Text = tbPackagePrice.Text.Replace(" ", string.Empty);
      var result = (from p in db.paketit
                    where p.pakettiID == selectedPackageId
                    select p).First().paketinHinta = double.Parse(tbPackagePrice.Text);
    }

    /// <summary>
    /// Asettaa näkyviin paketin pakettiID:n perusteella.
    /// </summary>
    /// <param name="id">Paketin ID joka asetetaa näkyville.</param>
    private void selectPackageID(int id)
    {
      try { lbPackages.SelectedValue = id; }
      catch (Exception ex)
      {
        selectPackageIndex(0);
      }
      updateSelectedPackage();
    }


    /// <summary>
    /// Hallinnoi julkisia muuttujia, ja että ne on ajan tasalla.
    /// </summary>
    private void updateSelectedPackage()
    {
      try
      {
        selectedPackageId = int.Parse(lbPackages.SelectedValue.ToString());
      }
      catch (Exception ex) { selectedPackageId = -1; }
      selectedPackageIndex = lbPackages.SelectedIndex;
    }


    /// <summary>
    /// Asettaa näkyviin paketin listboxin indexin perusteella
    /// </summary>
    /// <param name="index">Listan indexi, joka halutaan näkyville</param>
    private void selectPackageIndex(int index)
    {
      if (index < -1) return;
      if ((lbPackages.Items.Count - 1) >= index) lbPackages.SelectedIndex = index;
      else lbPackages.SelectedIndex = -1;
      updateSelectedPackage();
    }

    /// <summary>
    /// Näyttää valitun paketin tiedot
    /// </summary>
    private void showPackageDetails()
    {
      if (selectedPackageIndex == -1)
      {
        tbPackageName.Text = "";
        tbPackageRoomID.Text = "";
        tbPackagePrice.Text = "";
        tbPackageName.IsEnabled = false;
        tbPackageRoomID.IsEnabled = false;
        tbPackagePrice.IsEnabled = false;
        lbPackageProducts.IsEnabled = false;
        lbPackageAvailableProducts.IsEnabled = false;
        btnPackageAddProduct.IsEnabled = false;
        btnPackageRemoveProduct.IsEnabled = false;

        return;
      }

      var result = from p in db.paketit
                   where p.pakettiID == selectedPackageId
                   select p;
      var selectedPackage = result.FirstOrDefault();
      if (selectedPackage != null)
      {
        sbiStatus.Content = string.Format("Valittu paketti {0}", selectedPackage.nimi);
        tbPackageName.Text = selectedPackage.nimi;
        tbPackageRoomID.Text = string.Format("{0}", selectedPackage.huoneID);
        tbPackagePrice.Text = string.Format("{0}", selectedPackage.paketinHinta);
        tbPackageName.IsEnabled = true;
        tbPackageRoomID.IsEnabled = true;
        tbPackagePrice.IsEnabled = true;
        lbPackageProducts.IsEnabled = true;
        lbPackageAvailableProducts.IsEnabled = true;
        btnPackageAddProduct.IsEnabled = true;
        btnPackageRemoveProduct.IsEnabled = true;
      }

      showPackageProducts();
    }

    /// <summary>
    /// Näyttää valitun paketin tuotteet listassa
    /// </summary>
    private void showPackageProducts()
    {
      var result = from p in db.paketit
                   where p.pakettiID == selectedPackageId
                   select p;
      var selectedPackage = result.FirstOrDefault();

      var result2 = from p in db.paketit
                    where p.pakettiID == selectedPackageId
                    select p.tuote;
      if (result2.Count() < 1) return;
      lbPackageProducts.ItemsSource = result2.First().ToList();
      lbPackageProducts.DisplayMemberPath = "tuotenimi";
      lbPackageProducts.SelectedValuePath = "idtuote";
      lbPackageProducts.Items.Refresh();
    }

  }
}
