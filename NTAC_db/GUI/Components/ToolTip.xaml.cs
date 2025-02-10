using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NTAC_db.GUI.Components
{
    /// <summary>
    /// Interaction logic for ToolTip.xaml
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class ToolTip : System.Windows.Controls.UserControl
    {

        /// <summary>
        /// Constructor desde el que se recogen los datos pasados y se muestran
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="values"></param>
        public ToolTip(string[] properties, dynamic[] values)
        {
            InitializeComponent();
            UpdateComponent(properties, values);
        }

        /// <summary>
        /// Actualiza el componente para que tenga los datos que necesita
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="values"></param>
        private void UpdateComponent(string[] properties, dynamic[] values)
        {
            this.Height = 40 * properties.Count();
            for (int i=0; i < properties.Count(); i++)
            {
                Lines.Content += properties[i] + " " + values[i] + "\n";
            }
        }
    }
}
