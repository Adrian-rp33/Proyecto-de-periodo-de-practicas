using NTAC_db.AppBehavior;
using NTAC_db.DTO;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace NTAC_db.GUI.ComparationPages
{
    /// <summary>
    /// Lógica de interacción para DateInputPage.xaml
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class FilterInputPage : Page
    {

        private AppController controller;

        public ObservableCollection<String> AttributesDisabled { get; set; }
        public ObservableCollection<String> AttributesEnabled { get; set; }
        private GraphPage graph;

        //Constructor con la otra pagina y la lista de datos
        public FilterInputPage(AppController controller, GraphPage graph)
        {
            InitializeComponent();
            this.controller = controller;
            this.graph = graph;

            //Inicializacion de listas
            AttributesDisabled = new() {"Bomba masa 1", "Bomba masa 2", "Bomba masa 3", "Bomba masa 4", "Caudal 1", 
                                       "Caudal 2", "Decanter", "Rpm bd", "Rpm diff", "Rpm md", "T rod alim", "T rod salida"};
            AttributesEnabled = new();

            DataContext = this;
        }

        /// <summary>
        /// Recoge los atributos seleccionados, hace las comprobaciones necesarias para el filtrado
        /// y por ultimo llama a los metodos necesarios de la otra pagina para mostrar la grafica con 
        /// los datos seleccionados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
    
            //Comprobar que la lista de atributos seleccionados tiene atributos
            if (AttributesEnabled.Count > 0)
            {
                //Comprobar si se usa una fecha por la que filtrar
                if (!String.IsNullOrEmpty(dateInput.Text))
                {
                    try
                    {
                        DateTime Date = DateTime.ParseExact(dateInput.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        graph.Filter(AttributesEnabled, Date);
                    }
                    catch (FormatException) //Si se pasa fecha con un formato invalido
                    {
                        System.Windows.MessageBox.Show("Se ha introducido una fecha con un formato invalido", "Invalid Format",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex) 
                    {
                        System.Windows.MessageBox.Show(ex.Message, "Filter error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    try
                    {
                        graph.Filter(AttributesEnabled);
                    }
                    catch (Exception ex) 
                    {
                        System.Windows.MessageBox.Show(ex.Message, "Filter error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
                System.Windows.MessageBox.Show("No se han seleccionado atributos por los que filtrar.", "Not Selected",
                    MessageBoxButton.OK, MessageBoxImage.Warning);

        }

        /// <summary>
        /// Agrega elementos a la lista de atributos habilitados y los quita de la lista de desabilitados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_Click(object sender, RoutedEventArgs e)
        {
            //Comprobar si hay elementos en la lista
            if (ListDisabled.Items.Count != 0)
            {
                //Comprobar si se ha seleccionado algun elemento
                if (ListDisabled.SelectedIndex != -1)
                {
                    AttributesEnabled.Add(ListDisabled.SelectedItem.ToString());
                    AttributesDisabled.RemoveAt(ListDisabled.SelectedIndex);
                }
                else
                    System.Windows.MessageBox.Show("No se ha seleccionado ningun elemento de la lista", "Not Selected",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                System.Windows.MessageBox.Show("No hay propiedades para habilitar", "Not found", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Agrega elementos a la lista de desabilitados y los quita de la lista de habilitados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remove_Click(object sender, RoutedEventArgs e)
        {
            //Comprobar si hay elementos en la lista
            if (ListEnabled.Items.Count != 0)
            {
                //Comprobar si se ha seleccionado algun elemento
                if (ListEnabled.SelectedIndex != -1)
                {
                    AttributesDisabled.Add(ListEnabled.SelectedItem.ToString());
                    AttributesEnabled.RemoveAt(ListEnabled.SelectedIndex);
                }
                else
                    System.Windows.MessageBox.Show("No se ha seleccionado ningun elemento de la lista", "Not Selected",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                System.Windows.MessageBox.Show("No hay propiedades para habilitar", "Not found", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

        }
    }
}
