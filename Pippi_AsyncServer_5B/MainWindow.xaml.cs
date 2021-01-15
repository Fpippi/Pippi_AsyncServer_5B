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
using Pippi_AsyncSocketLib;

namespace Pippi_AsyncServer_5B
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AsyncSocketServer mServer;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_Ascolto_Click(object sender, RoutedEventArgs e)
        {
            mServer.InizioAscolto();
        }

        private void btn_Disconetti_Click(object sender, RoutedEventArgs e)
        {
            mServer.disconetti();
        }

        private void Btn_invia_Click(object sender, RoutedEventArgs e)
        {
            mServer.inviaATutti(Txt_messagio.Text);
        }
    }
}
