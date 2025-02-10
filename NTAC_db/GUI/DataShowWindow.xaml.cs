using NTAC_db.Handlers;
using NTAC_db.DTO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using NTAC_db.AppBehavior;

namespace NTAC_db.GUI
{
    /// <summary>
    /// Logica de la ventana de visualizacion en tabla de los datos, desde
    /// la que tambien se permite acceder a los ajustes, preferencias, comparacion y 
    /// manejo de la base de datos
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class DataShowWindow : Window
    {
        public ObservableCollection<DataUnit>? DataList { get; set; }
        private Window parent;
        private AppController controller;
        private Task? task;

        /// <summary>
        /// Constructor accedido seleccionando ficheros
        /// </summary>
        /// <param name="DataList"></param>
        /// <param name="parent"></param>
        /// <param name="FileDialog"></param>
        /// <param name="controller"></param>
        public DataShowWindow(ObservableCollection<DataUnit> DataList, Window parent, Window FileDialog, AppController controller)
        {
            InitializeComponent();
            this.parent = parent;
            this.controller = controller;
            //Lectura de los datos del fichero
            this.DataList = new(DataList.OrderBy(d => d.timeStr).ToList());
            FileDialog.Close();
            table.DataContext = this;
            ApplyPreferences();
        }

        /// <summary>
        /// //Contructor desde el que se accede mediante la base de datos (configurandola desde el dialog)
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="previus"></param>
        /// <param name="controller"></param>
        public DataShowWindow(Window parent, Window previus, AppController controller) 
        {
            InitializeComponent();
            this.parent = parent;
            this.controller = controller;
            parent.Hide();
            previus.Close();

            RetrieveDataFromDB();
            ApplyPreferences();
        }

        /// <summary>
        /// Contructor desde el que se accede mediante la base de datos (teniendo una ya configurada)
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="controller"></param>
        public DataShowWindow(Window parent, AppController controller)
        {
            InitializeComponent();
            this.parent = parent;
            this.controller = controller;
            parent.Hide();

            RetrieveDataFromDB();
            ApplyPreferences();
            
        }

        /// <summary>
        /// Recoge de forma asincrona todos los datos de la base de datos
        /// </summary>
        private async void RetrieveDataFromDB() 
        {
            loading.Visibility = Visibility.Visible;
            DataList = await controller.DbController.GetAllDataUnits();
            
            //Si la lista es nula significa que no se ha realizado conexion correctamente (servidor apagado por ejemplo)
            if (DataList == null) 
            {
                parent.Show();
                this.Close();
            }
            table.DataContext = this;
            loading.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Se ejecuta al cerrar la ventana, vuelve a abrir la ventana principal
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            try
            {
                parent.Show();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Aplica las preferencias recogidas desde el fichero
        /// </summary>
        private void ApplyPreferences()
        {
            try
            {
                controller.attributesShown.GetPrefs();

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (!controller.attributesShown.prefs[i])
                        table.Columns[i].Visibility = Visibility.Collapsed;
                    else
                        table.Columns[i].Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex) 
            {
                System.Windows.MessageBox.Show("Error al aplicar preferencias:\n"+ex.StackTrace, "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        /// <summary>
        /// Boton para acceder a la ventana de preferencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void preferences_Click(object sender, RoutedEventArgs e)
        {
            new PreferencesDialog((AttributesShownHandler) controller.attributesShown.Clone()).ShowDialog();
            //Al cerrar el dialogo, actualiza la tabla aplicando las preferencias
            ApplyPreferences();
        }

        /// <summary>
        /// Boton para acceder a la ventana de comparacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void compare_Click(object sender, RoutedEventArgs e)
        {
            //Comprobacion de que existen datos en la lista
            if (DataList != null || DataList.Count() == 0)
                new ComparationWindow(DataList, controller).Show();
            else
                System.Windows.MessageBox.Show("No hay datos con los que realizar comparaciones", "No Data", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
        }

        /// <summary>
        /// Boton para agregar los datos visualizados a la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void addToDB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Comprobamos que tiene base de datos
                if (controller.settingsHandler.settings.conValue.hasDB)
                {
                    //Comprobacion de que no se esta ejecutando ya la tarea
                    if (task != null && (task.Status == TaskStatus.Running || task.Status == TaskStatus.WaitingForActivation
                        || task.Status == TaskStatus.WaitingToRun))
                    {
                        System.Windows.MessageBox.Show("Ya se esta ejecutando una operacion con la base de datos", "Already working",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    //Al tenerla, se pide si se desea confirmar la subida de datos
                    MessageBoxResult confirmation = System.Windows.MessageBox.Show("¿Se desean enviar los datos visualizados a la base de datos?",
                        "Subida de datos", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (confirmation == MessageBoxResult.Yes)
                    {
                        loading.Visibility = Visibility.Visible;
                        task = Task.Run(() => controller.DbController.InsertAllDataUnits(DataList));
                        await task;
                        loading.Visibility = Visibility.Hidden;
                    }

                }
                else //Si no la tiene, se pregunta si quiere configurar ahora la base de datos
                {
                    MessageBoxResult confirmation = System.Windows.MessageBox.Show("No se ha configurado una base de datos, ¿Desea configurarla ahora?",
                        "Configurar base de datos", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (confirmation == MessageBoxResult.Yes)
                    {
                        new DbDataDialog(controller).ShowDialog(); //Mostramos el dialog de pedida de datos
                        addToDB_Click(sender, e); //Se vuelve a llamar este mismo metodo para preguntar otra vez si se quiere subir los datos
                    }

                }
            }
            catch (Exception ex) 
            {
                loading.Visibility = Visibility.Hidden;
                System.Windows.MessageBox.Show("Se ha dado un error en la subida de datos (" + ex.Message + ").", "DB Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /// <summary>
        /// Boton para borrar todos los DataUnit de la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void deleteFromDB_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                //Comprobacion de que no se esta ejecutando ya la tarea
                if (task != null && (task.Status == TaskStatus.Running || task.Status == TaskStatus.WaitingForActivation
                        || task.Status == TaskStatus.WaitingToRun))
                {
                    System.Windows.MessageBox.Show("Ya se esta ejecutando una operacion con la base de datos", "Already working",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //Comprobamos que tiene base de datos
                if (controller.settingsHandler.settings.conValue.hasDB)
                {
                    loading.Visibility = Visibility.Visible;
                    task = Task.Run(() => controller.DbController.DeleteAllDataUnits());
                    await task;
                    loading.Visibility = Visibility.Hidden;
                }
                else //Si no tiene, se pide configuracion
                {
                    MessageBoxResult confirmation = System.Windows.MessageBox.Show("No se ha configurado una base de datos, ¿Desea configurarla ahora?",
                      "Configurar base de datos", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (confirmation == MessageBoxResult.Yes)
                    {
                        new DbDataDialog(controller).ShowDialog(); //Mostramos el dialog de pedida de datos
                        deleteFromDB_Click(sender, e); //Se vuelve a llamar este mismo metodo para pedir otra vez si se quiere eliminar
                    }

                }
            }
            catch(Exception ex)
            {
                loading.Visibility = Visibility.Hidden;
                System.Windows.MessageBox.Show("No se han podido eliminar registros (" + ex.Message + ").", "DB Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Boton para abrir la ventana de ajustes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new SettingsDialog(controller).ShowDialog();
                //Al cerrar se igualan los ajustes a los del fichero, que es lo que indica si se han guardado cambios
                controller.settingsHandler.settings = controller.fileH.Deserialice();
            }
            catch (InvalidOperationException er) 
            {
                Console.WriteLine("Error: " + er);
            }
        }

        /// <summary>
        /// Boton para cerrado de la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Boton para el minimizado de la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minimice_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

    }
}
