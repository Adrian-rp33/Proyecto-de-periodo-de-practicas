using NTAC_db.AppBehavior;
using OpenTK.Compute.OpenCL;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace NTAC_db.GUI.SettingsPages
{
    /// <summary>
    /// Lógica de la pagina de ajustes ScreenVisualizationPage. Se configura
    /// el programa utilizado para la visualizacion del ewon, eligiendo 1 todos los programas
    /// o ninguno
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class ScreenVisualizationPage : Page
    {

        private AppController controller;
        public ScreenVisualizationPage(AppController controller)
        {
            InitializeComponent();
            this.controller = controller;
            programBox.ItemsSource = new ObservableCollection<string>(["Todos", "WinGP", "Pro - face", "Ninguno"]);

            //Comprobar el programa seleccionado y mostrarlo en el ComboBox
            switch (controller.settingsHandler.settings.ecatcher.program)
            {
                case "Ninguno":
                    programBox.SelectedIndex = 3;
                    break;

                case "Pro - face":
                    programBox.SelectedIndex = 2;
                    break;

                case "WinGP":
                    programBox.SelectedIndex = 1;
                    break;

                default:
                    programBox.SelectedIndex = 0;
                    break;
            }

        }

        /// <summary>
        /// Recoge todos los datos de los campos
        /// </summary>
        public void GetData()
        {
            controller.settingsHandler.settings.ecatcher.program = programBox.SelectedItem.ToString();
        }


        /// <summary>
        /// Evento para cambio de seleccion del ComboBox de programas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void programBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetData();
        }
    }

}
