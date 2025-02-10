using NTAC_db.AppBehavior;
using OpenTK.Graphics.OpenGL;
using System.Windows;

namespace NTAC_db.GUI
{
    /// <summary>
    /// Lógica de interacción para DbDataDialog.xaml
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class DbDataDialog : Window
    {
        //Atributos
        private AppController controller;
        private Window window;
       
        //Constructor para la llamada desde la ventana principal
        public DbDataDialog(Window window ,AppController controller)
        {
            InitializeComponent();
            this.controller = controller;
            this.window = window;
        }

        //Constructor para la llamada desde el dialog de datos
        public DbDataDialog(AppController controller)
        {
            InitializeComponent();
            this.controller = controller;
        }

        /// <summary>
        /// Boton para la confirmacion de los datos de la base de datos. Comprueba que se han rellenado 
        /// los campos y que es posible realizar la conexion a la base de datos descrita
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            //Si algun campo esta vacio, dara error
            if (string.IsNullOrEmpty(address.Text) || string.IsNullOrEmpty(port.Text) || string.IsNullOrEmpty(dbName.Text) || string.IsNullOrEmpty(tableName.Text))
                System.Windows.MessageBox.Show("Hay campos sin rellenar, por favor rellena todos", "Empty Inputs", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            else 
            {
                //Si no es posible conectarse a esa base de datos, avisara
                if (controller.DbController.isPossibleToConnect(address.Text, port.Text, dbName.Text)) 
                {
                    //Se recogen los nuevos datos de conexion
                    controller.settingsHandler.settings.conValue.hasDB = true;
                    controller.settingsHandler.settings.conValue.address = address.Text;
                    controller.settingsHandler.settings.conValue.port = port.Text;
                    controller.settingsHandler.settings.conValue.DbName = dbName.Text;
                    controller.settingsHandler.settings.conValue.tableName = tableName.Text;

                    //Se guarda en ajustes tambien
                    controller.fileH.Serialice(controller.settingsHandler.settings);

                    //Se instancia la nueva conexion con los datos
                    controller.DbController = new(address.Text, port.Text, dbName.Text, tableName.Text, "root", "root");
                    if (window != null)
                        new DataShowWindow(window, this, controller).Show();
                    else 
                    {
                        System.Windows.MessageBox.Show("Se ha configurado la base de datos correctamente.", "Configuration OK",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }

            }
        }

        /// <summary>
        /// Boton para cancelar y cerrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
