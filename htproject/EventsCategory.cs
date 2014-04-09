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
  /// Event handlerit kategorioille
  /// </summary>
  public partial class MainWindow : Window
  {
    private int selectedCategoryId = -1;     // Muokataan VAIN updateSelectedCategories() funktiossa!
    private int selectedCategoryIndex = -1;  // Muokataan VAIN updateSelectedCategories() funktiossa!


    /// <summary>
    /// Lataa kategorialistan tietokannasta
    /// </summary>
    private void LoadCategoryListFromDB()
    {
      lbCategories.ItemsSource = null;

      var result = from c in db.kategoriat
                   orderby c.nimi
                   select c;

      lbCategories.ItemsSource = result.ToList();
      lbCategories.Items.Refresh();
      lbCategories.DisplayMemberPath = "nimi";
      lbCategories.SelectedValuePath = "idkategoria";

    }


    // lisää tänne kategoria handlereita

    /// <summary>
    /// Kun käyttäjä valitsee listasta jonkin toisen kategorian.
    /// -> asetetaan uusi kategoria valituksi, piirretään sen tiedot näyttöön
    /// </summary>
    private void lbCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      updateSelectedCategories();
      showCategoryDetails();
    }


    /// <summary>
    /// Näyttää valitun kategorian tiedot näytöllä
    /// </summary>
    private void showCategoryDetails()
    {
      if (selectedCategoryIndex == -1)
      { // jos mikään kategoria ei aktiivisena, poistutaan
        tbCategoryName.Text = "";
        tbCategoryDescription.Text = "";
        tbCategoryName.IsEnabled = false;
        tbCategoryDescription.IsEnabled = false;

        return;
      }

      var result = from c in db.kategoriat
                   where c.idkategoria == selectedCategoryId
                   select c;
      var selectedCategory = result.FirstOrDefault();
      if (selectedCategory != null)
      {
        sbiStatus.Content = string.Format("Valittu kategoria {0}", selectedCategory.nimi);
        tbCategoryName.Text = selectedCategory.nimi;
        tbCategoryDescription.Text = selectedCategory.kuvaus;
        tbCategoryName.IsEnabled = true;
        tbCategoryDescription.IsEnabled = true;
      }
    }


    /// <summary>
    /// Lisää uuden kategorian, asettaa sen aktiiviseksi muokkausta varten.
    /// Lisätään kategoria tietokantaan samantien.
    /// </summary>
    private void btnAddCategory_Click(object sender, RoutedEventArgs e)
    {
      kategoria k = new kategoria { nimi = "<uusi kategoria>", kuvaus = "kategorian kuvaus" };
      db.kategoriat.Add(k);
      sbiStatus.Content = "Lisätty uusi kategoria";
      db.SaveChanges();
      LoadCategoryListFromDB();
      // päivitetään myös tuotteiden kategoriacombobox
      UpdateProductCategoryList();

      selectCategoryID(k.idkategoria);
      showCategoryDetails();
    }


    /// <summary>
    /// Poistetaan kategoria. Poistetaan sekä entiteettikokoelmasta että tietokannasta.
    /// Asetetaan listassa edellinen (tai listan ensimmäinen) kategoria valituksi.
    /// </summary>
    private void btnRemoveCategory_Click(object sender, RoutedEventArgs e)
    {

      int tmp = selectedCategoryIndex;
      var result = from c in db.kategoriat
                   where c.idkategoria == selectedCategoryId
                   select c;
      if (result.Count() > 0)
      {
        var k = result.First();
        db.kategoriat.Remove(k);

        sbiStatus.Content = string.Format("Poistettiin kategoria {0}", k.nimi);
        db.SaveChanges();
        LoadCategoryListFromDB();
        // päivitetään myös tuotteiden kategoriacombobox
        UpdateProductCategoryList();
        selectCategoryIndex(tmp - 1);
        showCategoryDetails();
      }

    }


    /// <summary>
    /// Kategorian nimestä siirrytty jonnekkin muualle
    /// -> tallennetaan tehdyt muutokset entiteettikokoelmaan, ei vielä tietokantaan
    /// </summary>
    private void tbCategoryName_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbCategoryName.Text == "Name" || tbCategoryName.Text == "") return;
      var result = (from c in db.kategoriat
                    where c.idkategoria == selectedCategoryId
                    select c).First().nimi = tbCategoryName.Text;
      
      
    }


    /// <summary>
    /// Kategorian kuvauksesta siirrytty muualle
    /// -> tallennetaan tehdyt muutokset entiteettikokoelmaan, ei vielä tietokantaan
    /// </summary>
    private void tbCategoryDescription_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbCategoryDescription.Text == "Description" || tbCategoryDescription.Text == "") return;
      var result = (from c in db.kategoriat
                    where c.idkategoria == selectedCategoryId
                    select c).First().kuvaus = tbCategoryDescription.Text;   
    }


    /// <summary>
    /// Asettaa näkyviin kategorian kategoriaID:n perusteella.
    /// </summary>
    /// <param name="id">Kategorian ID joka asetetaa näkyville.</param>
    private void selectCategoryID(int id)
    {
      try { lbCategories.SelectedValue = id; }
      catch (Exception ex) {
        selectCategoryIndex(0);
      }
      updateSelectedCategories();
    }


    /// <summary>
    /// Päivittää julkiset muuttujat kategorian id:stä ja indeksistä
    /// </summary>
    private void updateSelectedCategories()
    {
      try {
        selectedCategoryId = int.Parse(lbCategories.SelectedValue.ToString());
      }
      catch (Exception ex) { selectedCategoryId = -1; }
      selectedCategoryIndex = lbCategories.SelectedIndex;
    }


    /// <summary>
    /// Asettaa näkyviin kategorian listboxin indexin perusteella
    /// </summary>
    /// <param name="index">Listan indexi, joka halutaan näkyville</param>
    private void selectCategoryIndex(int index)
    {
      if (index < -1) return;
      if ((lbCategories.Items.Count - 1) >= index) lbCategories.SelectedIndex = index;
      else lbCategories.SelectedIndex = -1;
      updateSelectedCategories();
    }
  }
}