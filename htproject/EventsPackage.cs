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


    }

    // lisää tänne paketti handlereita
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

    private void lbPackages_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      updateSelectedPackage();
      showPackageDetails();
    }


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

    private void btnPackageAddProduct_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnPackageRemoveProduct_Click(object sender, RoutedEventArgs e)
    {

    }

    private void tbPackageName_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbPackageName.Text == "") return;
      var result = (from p in db.paketit
                    where p.pakettiID == selectedPackageId
                    select p).First().nimi = tbPackageName.Text;
      
    }

    private void tbPackageRoomID_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbPackageRoomID.Text == "") return;
      var result = (from p in db.paketit
                    where p.pakettiID == selectedPackageId
                    select p).First().huoneID = int.Parse(tbPackageRoomID.Text);
    }

    private void tbPackagePrice_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbPackagePrice.Text == "") return;
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
