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
  /// Event handlerit pakettinäkymälle
  /// </summary>
  public partial class MainWindow : Window
  {
    // lisää tänne paketti handlereita
    private void btnPackageAdd_Click(object sender, RoutedEventArgs e)
    {
      MessageBox.Show("toimii");
      
    }

    private void lbPackages_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private void btnPackageRemove_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnPackageAddProduct_Click(object sender, RoutedEventArgs e)
    {

    }

    private void btnPackageRemoveProduct_Click(object sender, RoutedEventArgs e)
    {

    }
  }
}
