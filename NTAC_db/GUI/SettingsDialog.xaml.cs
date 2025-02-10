using NTAC_db.GUI.SettingsPages;
using System.Windows;
using NTAC_db.AppBehavior;

namespace NTAC_db.GUI
{
    /// <summary>
    /// Lógica de interacción para SettingsDialog.xaml
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class SettingsDialog : Window
    {
        //Atributos
        private AppController controller;

        public SettingsDialog(AppController controller) 
        { 
            InitializeComponent();
            this.controller = controller;
            //Carga de paginas
            sideMenu.Content = new SideMenuPage(setting, controller);
            setting.Content = new ValueSettingsPage(controller);
        }

        /// <summary>
        /// Boton para aplicar los cambios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void apply_Click(object sender, RoutedEventArgs e)
        {
            controller.fileH.Serialice(controller.settingsHandler.settings);
        }

        /// <summary>
        /// Boton para cerrar la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }

}
