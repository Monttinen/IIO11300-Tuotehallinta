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
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private ArrayList Categories;
    private ArrayList Products;
    private ArrayList Packages;
    private LekaEntities db;
    
    private bool Connected;
    public MainWindow()
    {
      InitializeComponent();
      tabCategory.Visibility = Visibility.Collapsed;
      tabProduct.Visibility = Visibility.Collapsed;
      tabPackage.Visibility = Visibility.Collapsed;

      Categories = new ArrayList();
      Products = new ArrayList();
      Packages = new ArrayList();
      db = new LekaEntities();

    }

    private void btnConnect_Click(object sender, RoutedEventArgs e)
    {
      if (!Connected)
        Connect();
      else
        Disconnect();
    }

    private void Connect()
    {
      if (!Connected)
      {
        if (checkLogin(tbUser.Text, tbPassword.Password))
        {
          sbiStatus.Content = "Connected";
          btnConnect.Content = "Disconnect";
          tabCategory.Visibility = Visibility.Visible;
          tabProduct.Visibility = Visibility.Visible;
          tabPackage.Visibility = Visibility.Visible;

          Connected = true;
          
        }
      }
      
    }

    private void Disconnect()
    {
        sbiStatus.Content = "";
        btnConnect.Content = "Login";
        tabCategory.Visibility = Visibility.Collapsed;
        tabProduct.Visibility = Visibility.Collapsed;
        tabPackage.Visibility = Visibility.Collapsed;

        Connected = false;
      
    }

    private bool checkLogin(string user, string password)
    {
      try
      {
        var result = from u in db.users
                     where u.password == password && u.username == user
                     select u;
        if (result.Count() > 0)
        {
          return true;
        }
        else
        {
          MessageBox.Show("Väärät tunnukset");
          return false;
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
        return false;
      }
        
    }


  }
}
