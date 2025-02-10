using NTAC_db.AppBehabior;
using NTAC_db.Handlers;

namespace NTAC_db.AppBehavior
{
    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public class AppController
    {
        //Handlers aplicacion para el manejo completo de la aplicacion
        private FileHandler FileH;
        private AttributesShownHandler AttributesShown;
        private SettingsHandler SettingsHandler;
        private DataBaseController DBController;

        //Solo getters, los setters no son necesarios
        public FileHandler fileH { get { return FileH; } }
        public AttributesShownHandler attributesShown { get{ return AttributesShown; } }
        public SettingsHandler settingsHandler { get{ return SettingsHandler; } }
        public DataBaseController DbController 
        {
            get{ return DBController; }
            set { DBController = value; }
        }

        /// <summary>
        /// Constructor default para AppController
        /// </summary>
        public AppController() 
        {
            FileH = new();
            AttributesShown = new(FileH);
            SettingsHandler = new(FileH);

            //Se pasan por parametro los datos de la conexion solo si desde el principio se sabe que tiene conexion
            if (SettingsHandler.settings.conValue.hasDB)
                DBController = new(SettingsHandler.settings.conValue.address,
                                    SettingsHandler.settings.conValue.port,
                                    SettingsHandler.settings.conValue.DbName,
                                    SettingsHandler.settings.conValue.tableName,
                                    SettingsHandler.settings.conValue.user,
                                    SettingsHandler.settings.conValue.password);
            else //En caso contrario se crea la conexion vacia
                DBController = new();

        }
        
    }
}
