using NTAC_db.Handlers;
using System.Windows;

namespace NTAC_db.GUI
{
    /// <summary>
    /// Lógica de interacción para PreferencesDialog.xaml
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class PreferencesDialog : Window
    {
        //Todos los atributos listados
        String[] list = {"Fecha en entero", "Fecha", "Bomba masa 1", "Bomba masa 2", "Bomba masa 3", "Bomba masa 4", 
            "Caudal 1", "Caudal 2", "Caudal acumulado 1", "Caudal acumulado 2", "Decanter", "Dec horas parcial", 
            "Dec horas total", "Entero md", "Par bd", "Rpm bd", "Rpm diff", "Rpm md", "T rod alim", "T rod salida", 
            "Vibracion"};

        private AttributesShownHandler Handler;

        public PreferencesDialog(AttributesShownHandler Handler)
        {
            InitializeComponent();
            this.Handler = Handler;
            Handler.GetPrefs();
            UpdateLists();
        }

        /// <summary>
        /// Actualiza las listas con las preferencias activadas o desactivadas
        /// </summary>
        private void UpdateLists()
        {
            //Limpiamos las listas de datos
            prefsOn.Items.Clear();
            prefsOff.Items.Clear();

            //Agregamos los datos que tocan a cada lista
            for (int i=0; i<21; i++)
            {
                //prefs[i] = true -> preferencias activas agrega esta preferencia
                //prefs[i] = false -> preferencias desactivadas agrega esta preferencia
                if (Handler.prefs[i])
                    prefsOn.Items.Add(list.ElementAt(i));
                else
                    prefsOff.Items.Add(list.ElementAt(i));
            }
        }

        /// <summary>
        /// Boton de cancelado y cerrado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Boton para pedir la confirmacion de los cambios realizados en las preferencias. 
        /// Una vez se confirmen, se cerrara la ventana y se reescribira el fichero de preferencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("¿Se quieren confirmar los cambios?" , 
                "Aplicar cambios", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes) 
            {
                Handler.setNewPrefs();
                System.Windows.MessageBox.Show("Se han realizado los cambios correctamente", "Changes Applied", 
                   MessageBoxButton.OK, MessageBoxImage.Information);
            }
            this.Close();  
        }

        /// <summary>
        /// Boton para agregar a preferencias activas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (prefsOff.Items.Count.ToString() != "0")
            {
                if (prefsOff.SelectedIndex != -1) {
                    Handler.prefs[GetIndex(prefsOff.SelectedItem.ToString())] = true;
                    UpdateLists(); //Finalmente se actualizan las listas
                }
                else
                    System.Windows.MessageBox.Show("No se ha seleccionado ningun elemento de la lista.", "Not Selected", 
                       MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else //Si no hay mas elementos en la lista
                System.Windows.MessageBox.Show("No hay preferencias desactivadas para activar.", "No Preferences Dissabled", 
                   MessageBoxButton.OK, MessageBoxImage.Error);
                
        }

        /// <summary>
        /// Boton para agregar a preferecias desactivadas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (prefsOn.Items.Count.ToString() != "0")
            {
                if (prefsOn.SelectedIndex != -1) 
                {
                    Handler.prefs[GetIndex(prefsOn.SelectedItem.ToString())] = false;
                    UpdateLists();
                }
                    
                else
                    System.Windows.MessageBox.Show("No se ha seleccionado ningun elemento de la lista.", "Not Selected", 
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
                System.Windows.MessageBox.Show("No hay preferencias activadas para desactivar.", "No Preferences Enabled", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Recoge la posicion de la preferencia y la pasa a entero
        /// </summary>
        /// <param name="preference"></param>
        /// <returns>int con la posicion en la lista de la preferencia</returns>
        private int GetIndex(String preference)
        {
            for (int i=0; i<21; i++) 
            {
                if (list[i].Equals(preference))
                    return i;
            }
            return -1; //Caso de que sea error
        }
    }
}
