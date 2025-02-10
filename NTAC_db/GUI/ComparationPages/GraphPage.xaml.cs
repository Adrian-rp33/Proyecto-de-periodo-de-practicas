using NTAC_db.AppBehavior;
using NTAC_db.DTO;
using System.Windows.Controls;
using Color = ScottPlot.Color;

namespace NTAC_db.GUI.ComparationPages
{
    /// <summary>
    /// Lógica de interacción para GraphPage.xaml, esta ventana maneja los datos y los introduce
    /// en graficas desde la que se podran visualizar los cambios de forma mas visual
    /// </summary>

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public partial class GraphPage : Page
    {
        private AppController controller;
        private IEnumerable<DataUnit> DataList;
        private List<string> AttributesApplyed;
        private ScottPlot.Plot myPlot;
        private ScottPlot.Coordinates coords;

        /// <summary>
        /// Constructor donde se inicializa la grafica con formatos en ambos ejes y se le dan los datos
        /// que necesita para tener valores que aplicar a la grafica
        /// </summary>
        /// <param name="DataList">Lista de registros</param>
        /// <see cref="StyleGraph"/>
        public GraphPage(IEnumerable<DataUnit> DataList, AppController controller)
        {
            InitializeComponent();
            this.controller = controller;
            this.DataList = DataList;
            AttributesApplyed = new();
            DataContext = this;
            myPlot = chart.Plot;

            StyleGraph();
        }

        /// <summary>
        /// Dar estilos a la grafica
        /// </summary>
        private void StyleGraph()
        {
            myPlot.Axes.Title.Label.Text = "Gráfica de datos";
            myPlot.Axes.Bottom.Label.Text = "Fecha";
            myPlot.Axes.Left.Label.Text = "Rendimiento (%)";

            myPlot.Legend.IsVisible = true;
            myPlot.Axes.DateTimeTicksBottom(); //Se cambia el formato a fecha del eje X (se autoajusta al ir ampliando)

            myPlot.ShowLegend(); //Mostrar leyenda

            //Colores
            if (controller.settingsHandler.settings.access.darkTheme) 
            {
                myPlot.Grid.MajorLineColor = Color.FromHex("#0e3d54");
                myPlot.FigureBackground.Color = Color.FromHex("#07263b");
                myPlot.DataBackground.Color = Color.FromHex("#0b3049");
                myPlot.Axes.Color(Color.FromHex("#a0acb5")); //Incluye el color de la fuente de letra
            }

        }

        /// <summary>
        /// Evento para recoger la posicion del raton y mostrar los datos reales
        /// en el punto X donde se encuentre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void graph_MouseOver(object sender, System.Windows.Input.MouseEventArgs e) 
        {
            System.Windows.Point p = e.GetPosition(chart); //Punto en el que se encuentra el mouse (es relativo a la grafica)
            ScottPlot.Pixel mousePixel = new(p.X * chart.DisplayScale, p.Y * chart.DisplayScale);
            coords = chart.Plot.GetCoordinates(mousePixel); //Coordenadas dentro de la grafica

            //CUANDO SE AGREGUE UN TOOLTIP, HABRA QUE AGREGAR QUE SE HAGA UN chart.Refresh(), para que la imagen de la grafica se actualice
        }

        /// <summary>
        /// Recoge los datos de los atributos pasados en listas
        /// </summary>
        /// <param name="DataList">Lista de registros</param>
        /// <param name="attribute">Atributo al que hacer rendimiento</param>

        private List<int> RetrieveElementData(IEnumerable<DataUnit> DataList, String attribute) 
        {
            List<int> values = new();
            int aux;
            //Se recoge el porcentaje de rendimiento de ese atributo (comparado con el maximo pasado en los ajustes)
            foreach (DataUnit d in DataList)
            {
                if (controller.settingsHandler.settings.values.GetValueByName(attribute) > 0)
                {
                    aux = (int)Math.Round((d.GetAttributeByName(attribute) / controller.settingsHandler.settings.values.GetValueByName(attribute)) * 100, 0);
                    values.Add(aux);
                }
                else 
                {
                    throw new Exception();
                }
            }
            
            return values;
        }

        /// <summary>
        /// Recoge las fechas de todos los registros y devuelve una lista con todas ellas
        /// </summary>
        /// <param name="DataList"></param>
        /// <returns>List DateTime</returns>
        private List<DateTime> RetrieveDates(IEnumerable<DataUnit> DataList) 
        {
            List<DateTime> dates = new List<DateTime>();

            foreach (DataUnit d in DataList) 
            {
                dates.Add(d.timeStr);
            }

            return dates;
        }

        /// <summary>
        /// Compara las fechas de toda la lista de datos para ver si sus fechas son posteriores o anteriores
        /// a la fecha minima, si la fecha es posterior. Se guardan todos los registros con fechas posteriores a la
        /// pasada y luego se retorna
        /// </summary>
        /// <param name="minDate">DateTime min</param>
        /// <returns>List'DataUnit'</returns>
        private List<DataUnit> RetrieveFromDate(DateTime minDate)
        {
            List<DataUnit> _DataList = new();
            foreach (DataUnit d in DataList)
            {
                if (DateTime.Compare(d.timeStr, minDate) >= 0) //Compare() 0 --> igual ; 1 --> posterior
                {
                    _DataList.Add(d);                
                }
            }

            return _DataList;
        }

        /// <summary>
        /// Aplica filtros desde el inicio (fecha). En la creacion primero se limpia toda la grafica, 
        /// y luego se vuelven a crear todas las lineas, consume mas recursos pero se evitan mas errores 
        /// de esta forma
        /// </summary>
        public void Filter(IEnumerable<String> AttributesToApply)
        {

            try
            {
                myPlot.Clear();
                DateTime[] dates = RetrieveDates(DataList).ToArray();
                myPlot.Axes.SetLimits(dates.Min().ToOADate(), dates.Max().ToOADate(), 0, 100);

                //Bucle donde se crean todas las lineas de la grafica
                foreach (string att in AttributesToApply)
                {
                    var aux = myPlot.Add.Scatter(dates.Select(x => x.ToOADate()).ToArray(),
                                RetrieveElementData(DataList, att).ToArray());

                    //Estilos de la linea
                    aux.Smooth = true;
                    aux.LineWidth = 3;
                    aux.LegendText = att;
                    aux.MarkerSize = 0;

                }

                AttributesApplyed = AttributesToApply.ToList();
                chart.Refresh();
            }
            catch (Exception) 
            {
                throw new Exception("Error, no se ha establecido el limite/s de alguno de los atributos seleccionados.");
            }
            
        }

        /// <summary>
        /// Aplica filtros teniendo una fecha minima que indique desde cuando se van a ver los datos. En la creacion 
        /// primero se limpia toda la grafica, y luego se vuelven a crear todas las lineas, 
        /// consume mas recursos pero se evitan mas errores de esta forma
        /// </summary>
        /// <param name="MinDate">DateTime Fecha minima</param>
        public void Filter(IEnumerable<string> AttributesToApply, DateTime MinDate)
        {
            try
            {
                myPlot.Clear();
                List<DataUnit> _DataList = RetrieveFromDate(MinDate); //Lista de datos desde la fecha minima
                DateTime[] dates = RetrieveDates(_DataList).ToArray();
                myPlot.Axes.SetLimits(dates.Min().ToOADate(), dates.Max().ToOADate(), 0, 100);

                //Bucle donde se crean todas las lineas de la grafica
                foreach (string att in AttributesToApply)
                {
                    var aux = myPlot.Add.Scatter(dates.Select(x => x.ToOADate()).ToArray(),
                                RetrieveElementData(_DataList, att).ToArray());

                    //Estilos de la linea
                    aux.Smooth = true;
                    aux.LineWidth = 3;
                    aux.LegendText = att;
                    aux.MarkerSize = 0;

                }

                AttributesApplyed = AttributesToApply.ToList();
                chart.Refresh();
            }
            catch (Exception) 
            {
                throw new Exception("Error, no se ha establecido el limite/s de alguno de los atributos seleccionados.");
            }
        }

    }

}
