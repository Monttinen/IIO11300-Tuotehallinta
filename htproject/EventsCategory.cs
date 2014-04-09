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
    private int selectedCategoryId = -1;
    private int selectedLBIndexCategory = -1;

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

    private void lbCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      selectedLBIndexCategory = lbCategories.SelectedIndex;
      if (selectedLBIndexCategory == -1)
      {
        tbCategoryName.Text = "";
        tbCategoryDescription.Text = "";
        tbCategoryName.IsEnabled = false;
        tbCategoryDescription.IsEnabled = false;

        return;
      }

      selectedCategoryId = int.Parse(lbCategories.SelectedValue.ToString());
      

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

    private void btnAddCategory_Click(object sender, RoutedEventArgs e)
    {
      kategoria k = new kategoria { nimi = "<uusi kategoria>", kuvaus = "kategorian kuvaus" };
      db.kategoriat.Add(k);
      sbiStatus.Content = "Lisätty uusi kategoria";
      db.SaveChanges();
      LoadCategoryListFromDB();

      // TODO: tässä kohtaa voitaisiin valita lisätty valmiiksi
      lbCategories.SelectedIndex = 0;
      
    }

    private void btnRemoveCategory_Click(object sender, RoutedEventArgs e)
    {
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
      }
    }

    private void tbCategoryName_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbCategoryName.Text == "Name" || tbCategoryName.Text == "") return;
      var result = (from c in db.kategoriat
                    where c.idkategoria == selectedCategoryId
                    select c).First().nimi = tbCategoryName.Text;
      
      
    }

    private void tbCategoryDescription_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbCategoryDescription.Text == "Description" || tbCategoryDescription.Text == "") return;
      var result = (from c in db.kategoriat
                    where c.idkategoria == selectedCategoryId
                    select c).First().kuvaus = tbCategoryDescription.Text;
      
      
    }
  }
}