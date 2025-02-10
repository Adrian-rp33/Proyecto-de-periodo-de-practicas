using System.Diagnostics;
using System.Windows;

namespace NTAC_db.GUI
{
    /// <summary>
    /// Logica de interaccion con ProgramsStartupWindow. Permite la apertura de los programas para la visualizacion
    /// de pantallas y de eCatcher
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class ProgramsStartupWindow : Window
    {
        /// <summary>
        /// Constructor, al inciar, comprueba desde los ajustes que aplicacion/es
        /// se han seleccionado para visualizar pantallas y ocultara aquellas que no deban estar
        /// </summary>
        public ProgramsStartupWindow(string program)
        {
            InitializeComponent();

            //Comprobar segun los programas seleccionados desde los ajustes
            switch (program) 
            {
                case "Ninguno":
                    proFace.Visibility = Visibility.Hidden;
                    winGp.Visibility = Visibility.Hidden;
                    break;

                case "Pro - face":
                    winGp.Visibility = Visibility.Hidden;
                    break;

                case "WinGP":
                    proFace.Visibility = Visibility.Hidden;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Crea un proceso para abrir eCatcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eCatcher_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\Program Files (x86)\\eCatcher-Talk2M\\eCatcher.exe");
        }

        /// <summary>
        /// Crea un proceso para abrir Pro - Face
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void proFace_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\Pro-face\\Pro-face Remote HMI Client for Win\\PRHClient.exe");
        }

        /// <summary>
        /// Crea un proceso para abrir WinGP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void winGp_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\Program Files (x86)\\Pro-face\\WinGP\\PCRuntime.exe");
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
            this.DragMove();
        }
    }
}
