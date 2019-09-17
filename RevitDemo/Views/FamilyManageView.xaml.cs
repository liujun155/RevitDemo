using RevitDemo.Entity;
using RevitDemo.ViewModel;
using System;
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

namespace RevitDemo.Views
{
    /// <summary>
    /// FamilyManageView.xaml 的交互逻辑
    /// </summary>
    public partial class FamilyManageView : Window
    {
        public FamilyManageView(FamilyManageViewModel model)
        {
            InitializeComponent();
            this.DataContext = model;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as FamilyManageViewModel).LoadFamily();
        }

        private void Check_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            (ck.DataContext as FamilyEnt).IsChecked = (bool)ck.IsChecked;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg.SelectedItem != null)
                (this.DataContext as FamilyManageViewModel).SelFamily = dg.SelectedItem as FamilyEnt;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            (this.DataContext as FamilyManageViewModel).LoadMulFamily();
        }
    }
}
