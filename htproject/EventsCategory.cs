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
    private int selectedLBIndex = -1;

    private void LoadCategoryListFromDB()
    {
      lbCategories.ItemsSource = null;

      var result = from c in db.kategoriat
                   orderby c.nimi
                   select c;

      lbCategories.ItemsSource = result.ToList();

      lbCategories.DisplayMemberPath = "nimi";
      lbCategories.SelectedValuePath = "idkategoria";

    }


    // lisää tänne kategoria handlereita

    private void lbCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      selectedLBIndex = lbCategories.SelectedIndex;
      if (selectedLBIndex == -1) return;

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
      }
    }

    private void btnAddCategory_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnRemoveCategory_Click(object sender, RoutedEventArgs e)
    {

    }

    private void tbCategoryName_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbCategoryName.Text == "Name" || tbCategoryName.Text == "") return;
      var result = (from c in db.kategoriat
                    where c.idkategoria == selectedCategoryId
                    select c).First().nimi = tbCategoryName.Text;
      // päivitä listbox
      LoadCategoryListFromDB();
    }

    private void tbCategoryDescription_LostFocus(object sender, RoutedEventArgs e)
    {
      if (tbCategoryDescription.Text == "Description" || tbCategoryDescription.Text == "") return;
      var result = (from c in db.kategoriat
                    where c.idkategoria == selectedCategoryId
                    select c).First().kuvaus = tbCategoryDescription.Text;
      // päivitä listbox 
      LoadCategoryListFromDB();
    }
  }
}