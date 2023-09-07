using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BingoRandomizer.Windows
{
    /// <summary>
    /// Interaktionslogik für SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        bool _reset;
        public SettingsDialog(int rate = 15, int factor = 20, bool reset = false)
        {
            InitializeComponent();
            rateText.Text = rate.ToString();
            factorText.Text = factor.ToString();
            _reset = reset;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _reset = true;
            DialogResult = true;
        }
        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public string Rate
        {
            get { return rateText.Text; }
        }
        public string Factor
        {
            get { return factorText.Text; }
        }

        public bool Reset
        {
            get { return _reset; }
        }

        
    }
}
