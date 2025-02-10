using NTAC_db.AppBehavior;
using System.Windows.Controls;

namespace NTAC_db.GUI.SettingsPages
{
    /// <summary>
    /// Lógica de interacción con la pagina de ajustes de accesibilidad. Contiene los ajustes relacionados
    /// con al accesibilidad, como lo puede ser por ejemplo el color de la aplicacion general
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class AccesibilityPage : Page
    {

        private AppController controller;
        public AccesibilityPage(AppController controller)
        {
            InitializeComponent();
            this.controller = controller;
            DataContext = controller.settingsHandler.settings.access;
        }

        /// <summary>
        /// Recoge los datos introducidos en los campos
        /// </summary>
        public void GetData()
        {
            controller.settingsHandler.settings.access.darkTheme = themeSwitch.IsChecked.Value;
        }
    }
}
