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
    public partial class FrmSetFamilyView : Window
    {
        public FrmSetFamilyView(FrmSetFamilyViewModel model)
        {
            InitializeComponent();
            this.DataContext = model;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var cmb = cmbDirection;
            (this.DataContext as FrmSetFamilyViewModel).BtnClick();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
