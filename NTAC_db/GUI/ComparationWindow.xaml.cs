using NTAC_db.AppBehavior;
using NTAC_db.DTO;
using NTAC_db.GUI.ComparationPages;
using System.Collections.ObjectModel;
using System.Windows;

namespace NTAC_db.GUI
{
    /// <summary>
    /// Lógica de interacción para ComparationDialog.xaml
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class ComparationWindow : Window
    {
        //Constructir con solo la lista, no se necesita como atributo aqui, ya que no la manejara
        public ComparationWindow(ObservableCollection<DataUnit> DataList, AppController controller)
        {
            InitializeComponent();
            //Iniciamos las paginas de contenido
            GraphPage graph = new(DataList, controller);
            dateInput.Content = new FilterInputPage(controller, graph);
            chart.Content = graph;
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
