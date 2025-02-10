using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using NTAC_db.AppBehavior;

namespace NTAC_db.GUI.SettingsPages
{
    /// <summary>
    /// Lógica de interacción para ValueSettingsPage.xaml
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class ValueSettingsPage : Page
    {
        private AppController controller;

        public ValueSettingsPage(AppController controller)
        {
            InitializeComponent();

            this.controller = controller;
            //Su data context debe ser el de controller y sus ajustes en especifico
            this.DataContext = controller.settingsHandler.settings.values; 
        }

        /// <summary>
        /// Recoge los datos introducidos en los campos
        /// </summary>
        public void GetData() 
        {
            CultureInfo ci = System.Globalization.CultureInfo.InvariantCulture;

            try
            {
                controller.settingsHandler.settings.values.b_masa_min = float.Parse(bm_min.Text, ci);
                controller.settingsHandler.settings.values.b_masa_max = float.Parse(bm_max.Text, ci);
                controller.settingsHandler.settings.values.caudal_min = float.Parse(cau_min.Text, ci);
                controller.settingsHandler.settings.values.caudal_max = float.Parse(cau_max.Text, ci);
                controller.settingsHandler.settings.values.decanter_min = float.Parse(dec_min.Text, ci);
                controller.settingsHandler.settings.values.decanter_max = float.Parse(dec_max.Text, ci);
                controller.settingsHandler.settings.values.rod_alim_min = float.Parse(rod_alim_min.Text, ci);
                controller.settingsHandler.settings.values.rod_alim_max = float.Parse(rod_alim_max.Text, ci);
                controller.settingsHandler.settings.values.rod_sal_min = float.Parse(rod_sal_min.Text, ci);
                controller.settingsHandler.settings.values.rod_sal_max = float.Parse(rod_sal_max.Text, ci);
            }
            catch (FormatException)
            {
                System.Windows.MessageBox.Show("Se han detectado campos con formato incorrecto, introduce solo numeros reales.", "Format Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
