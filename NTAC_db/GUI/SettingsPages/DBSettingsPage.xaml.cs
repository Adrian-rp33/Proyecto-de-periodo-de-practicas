using NTAC_db.AppBehavior;
using OpenTK.Graphics.OpenGL;
using System.Windows;

namespace NTAC_db.GUI.SettingsPages
{
    /// <summary>
    /// Lógica de interacción para DBSettings.xaml
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class DBSettingsPage : System.Windows.Controls.Page
    {

        private AppController controller;
        public DBSettingsPage(AppController controller)
        {
            InitializeComponent();
            this.controller = controller;
            //Cambio al DataContext del controller y su apartado especifico
            this.DataContext = controller.settingsHandler.settings.conValue;
            FirstCheck(); //Llamada inicial al instanciar
        }

        /// <summary>
        /// Comprobacion inicial de los ajustes que estaban en el fichero
        /// </summary>
        private void FirstCheck() 
        {
            if (controller.settingsHandler.settings.conValue.hasDB)
            {
                addressBox.IsEnabled = true;
                portBox.IsEnabled = true;
                DBBox.IsEnabled = true;
                tableBox.IsEnabled = true;
            }
            else
            {
                addressBox.IsEnabled = false;
                portBox.IsEnabled = false;
                DBBox.IsEnabled = false;
                tableBox.IsEnabled = false;
            }
        }

        /// <summary>
        /// Metodo que activa o desactiva componentes de la ventana si se marca
        /// o desmarca las casilla de base de datos
        /// </summary>
        private void ActivateControls()
        {
            if (dbSwitch.IsChecked.Value)
            {
                addressBox.IsEnabled = true;
                portBox.IsEnabled = true;
                DBBox.IsEnabled = true;
                tableBox.IsEnabled = true;
                userBox.IsEnabled = true;
                passwordBox.IsEnabled = true;
            }
            else 
            {
                addressBox.IsEnabled = false;
                portBox.IsEnabled = false;
                DBBox.IsEnabled = false;
                tableBox.IsEnabled = false;
                userBox.IsEnabled = false;
                passwordBox.IsEnabled = false;
            }
        }

        /// <summary>
        /// Click de la casilla de base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dbSwitch_Click(object sender, RoutedEventArgs e)
        {
            ActivateControls();
        }

        /// <summary>
        /// Recoge los datos introducidos en los campos
        /// </summary>
        public void GetData()
        {
            //Se recogen datos de puerto y direccion solo si esta activo, si no les da el valor de "null"
            if (dbSwitch.IsChecked.Value)
            {
                //Si algun campo esta vacio, dara error
                if (string.IsNullOrEmpty(addressBox.Text) || string.IsNullOrEmpty(portBox.Text) || string.IsNullOrEmpty(DBBox.Text) 
                    || string.IsNullOrEmpty(tableBox.Text) || string.IsNullOrEmpty(userBox.Text) || string.IsNullOrEmpty(passwordBox.Password))
                    System.Windows.MessageBox.Show("Hay campos sin rellenar, por favor rellena todos", "Empty Inputs", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    //Si no es posible conectarse a esa base de datos, avisara
                    if (controller.DbController.isPossibleToConnect(addressBox.Text, portBox.Text, DBBox.Text))
                    {
                        //Se recogen los nuevos datos de conexion
                        controller.settingsHandler.settings.conValue.hasDB = true;
                        controller.settingsHandler.settings.conValue.address = addressBox.Text;
                        controller.settingsHandler.settings.conValue.port = portBox.Text;
                        controller.settingsHandler.settings.conValue.DbName = DBBox.Text;
                        controller.settingsHandler.settings.conValue.tableName = tableBox.Text;
                        controller.settingsHandler.settings.conValue.user = userBox.Text;
                        controller.settingsHandler.settings.conValue.password = passwordBox.Password;


                        //Se instancia la nueva conexion con los datos
                        controller.DbController = new(addressBox.Text, portBox.Text, DBBox.Text, tableBox.Text, 
                            userBox.Text, passwordBox.Password);
                    }

                }
            }
            else 
            {
                controller.settingsHandler.settings.conValue.hasDB = false;
                controller.settingsHandler.settings.conValue.address = "null";
                controller.settingsHandler.settings.conValue.port = "null";
                controller.settingsHandler.settings.conValue.DbName = "null";
                controller.settingsHandler.settings.conValue.tableName = "null";
            }
                
        }
    }
}
