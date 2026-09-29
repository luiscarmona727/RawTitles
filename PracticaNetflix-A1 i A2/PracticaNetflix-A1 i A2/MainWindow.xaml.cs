using PracticaNetflix_A1_i_A2.DAO;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace PracticaNetflix_A1_i_A2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // La UI només parla amb la interfície IDAO, mai amb IDAOImpl directament
        private readonly IDAO dao = new IDAOFactory().CreateDAO(); // canvia el nom si has renamed el mètode

        private string ultimFitxer; // ruta completa de l'últim fitxer generat

        public MainWindow()
        {
            InitializeComponent();
        }

    }
}