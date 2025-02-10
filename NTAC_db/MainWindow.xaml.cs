using NTAC_db.GUI;
using NTAC_db.Interfaces;
using System.ComponentModel;
using System.Windows;
using NTAC_db.AppBehavior;

namespace NTAC_db
{
    /// <summary>
    /// Logica de la ventana principal de la aplicacion, desde aqui se puede
    /// acceder a la visualizacion de datos, ya sea escogiendo ficheros o accediendo
    /// desde una base de datos a la que se este conectado. Tambien permite conectarse 
    /// al cliente de Talk2M a traves del API proporcionada, ademas de abrir un proceso 
    /// para las apliaciones de visualizacion para las pantallas de PLC escogida
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class MainWindow : Window
    {
        private AppController controller;

        public MainWindow()
        {
            InitializeComponent();
            controller = new();
            ThemeInterface.ChangeTheme(controller.settingsHandler.settings.access.darkTheme);
        }

        /// <summary>
        /// Se ejecuta al cerrar la ventana, cerrara definitivamente la aplicacion
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            System.Windows.Application.Current.Shutdown(); //Cerrado definitivo de la aplicacion
        }

        /// <summary>
        /// Boton para acceder mediante seleccion de ficheros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            new FilesSelection(this, controller).ShowDialog();
        }

        /// <summary>
        /// Boton para acceder mediante base de datos, si no se tiene en los ajustes una guardada, 
        /// abre una ventana para pedir configuracion de la misma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accessDB_Click(object sender, RoutedEventArgs e)
        {
            //Try para evitar que la aplicacion deje de funcionar si no se realiza la conexion
            try
            {
                if (!controller.settingsHandler.settings.conValue.hasDB)
                {
                    System.Windows.MessageBox.Show("No se ha configurado la base de datos, introduzca los datos de conexion a continuación.",
                        "No DB Detected", MessageBoxButton.OK, MessageBoxImage.Warning);
                    new DbDataDialog(this, controller).ShowDialog();
                }
                else
                    //Si se tiene base de datos se entrara directamente recogiendo los datos desde ella
                    new DataShowWindow(this, controller).Show();
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("No es posible realizar la conexion a la base de datos, " +
                    "comprueba la conexion del servidor que la tiene.", "Not Possible to connect", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        /// <summary>
        /// Abre la ventana para la seleccion de aplicaciones que comenzar para la visualizacion de pantallas
        /// segun los ajustes, aunque tambien puede simplemente abrir eCatcher si el usuario a configurado
        /// que ninguna aplicacion debe mostrarse para visualizar la pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScreenVisualization_Click(object sender, RoutedEventArgs e)
        {
            new ProgramsStartupWindow(controller.settingsHandler.settings.ecatcher.program).Show();
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

        /// <summary>
        /// Boton para minimizar la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minimice_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Funcion que permite mover la ventana manteniendo el click izquierdo 
        /// sobre la barra de menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized) 
            {
                this.WindowState = WindowState.Normal;
            }
            this.DragMove();
        }

    }

}