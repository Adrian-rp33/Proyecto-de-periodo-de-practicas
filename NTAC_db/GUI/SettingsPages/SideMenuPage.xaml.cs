using NTAC_db.AppBehavior;
using NTAC_db.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace NTAC_db.GUI.SettingsPages
{
    /// <summary>
    /// Lógica de interacción para SideMenuPage.xaml
    /// </summary>

    /*
     *
     * @Author Adrian Rivas Perez
     *
     */
    public partial class SideMenuPage : Page
    {
        /*
         * Tiene que tener la propiedad del frame que le vendra desde la ventana contenedora
         * para que se pueda interactuar con el frame que cambiara de pagina al hacer click en
         * cada opcion del menu
         */
        private readonly Frame frame;
        private AppController controller;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="frame">Frame contenedor de las paginas de ajustes</param>
        /// <param name="controller">AppController general</param>
        public SideMenuPage(Frame frame, AppController controller)
        {
            this.controller = controller;
            InitializeComponent();
            ThemeInterface.ChangeTheme(controller.settingsHandler.settings.access.darkTheme);
            this.frame = frame;
        }

        /// <summary>
        /// Boton de apertura de pagina de ajustes para valores maximos y minimos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Values_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new ValueSettingsPage(controller);
        }

        /// <summary>
        /// Boton de apertura de pagina de ajustes para base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DB_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new DBSettingsPage(controller);
        }

        /// <summary>
        /// Boton de apertura de pagina de ajustes para accesibilidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void theme_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new AccesibilityPage(controller);
        }
        
        /// <summary>
        /// Boton de apertura de pagina de ajustes para la seleccion de programa de pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eCatcher_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new ScreenVisualizationPage(controller);
        }
    }
}
