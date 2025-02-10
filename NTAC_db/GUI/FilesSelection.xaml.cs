using NTAC_db.AppBehavior;
using NTAC_db.DTO;
using System.Collections.ObjectModel;
using System.Windows;

namespace NTAC_db.GUI
{
    /// <summary>
    /// Lógica de interacción para FilesSelection.xaml
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class FilesSelection : Window
    {
        private Window parent;
        private AppController controller;
        public ObservableCollection<String> FilesPaths { get; set; } //Lista con rutas de ficheros
        public FilesSelection(Window parent, AppController controller)
        {
            InitializeComponent();
            this.controller = controller;
            this.parent = parent;
            parent.Hide(); //Dejamos de mostrar la ventana principal
            FilesPaths = new();
            list.DataContext = this;
        }

        /// <summary>
        /// Boton para aceptar, una vez activado enviara la lista de ficheros seleccionados a la ventana
        /// de visualizacion de datos en tabla y cerrara esta ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            //Comprobacion de que se han seleccionado ficheros
            if (FilesPaths.Count() > 0)
            {
                loading.Visibility = Visibility.Visible;
                
                //Desactivar botones
                accept.IsEnabled = false;
                cancel.IsEnabled = false;
                selectFile.IsEnabled = false;
                removeFile.IsEnabled = false;

                ObservableCollection<DataUnit> DataList = await Task.Run(() => controller.fileH.ReadFileDataUnit(FilesPaths));
                new DataShowWindow(DataList, parent, this, controller).Show();
            }
            else
                System.Windows.MessageBox.Show("No se han seleccionado ficheros.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        /// <summary>
        /// Boton para cancelar y cerrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            parent.Show();
            this.Close();
        }

        /// <summary>
        /// Boton que abre un dialogo de seleccion de ficheros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            //Control de errores por si no se selecciona ningun fichero o por si se selecciona algo que no sea un fichero
            try
            {
                OpenFileDialog ofd = new();
                ofd.ShowDialog();
                if(!String.IsNullOrEmpty(ofd.FileName))
                    FilesPaths.Add(ofd.FileName);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Boton que eliminara el fichero seleccionado de la lista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (list.Items.Count.ToString() != "0") //Si hay algun fichero ya introducido
            {
                if (list.SelectedIndex != -1) //Si se ha seleccionado algun fichero
                {
                    FilesPaths.RemoveAt(list.SelectedIndex);
                }
                else
                    System.Windows.MessageBox.Show("No se ha seleccionado ningun fichero de la lista.", "Not Selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
                System.Windows.MessageBox.Show("No hay ficheros para quitar de la lista.", "No Files" , MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
