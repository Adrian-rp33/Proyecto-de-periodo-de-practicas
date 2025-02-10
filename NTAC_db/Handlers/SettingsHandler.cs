using NTAC_db.DTO;

namespace NTAC_db.Handlers
{

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public class SettingsHandler
    {

        //Atributos
        private SettingsObj Settings;        
        private FileHandler FileH;

        public SettingsObj settings
        {
            get { return Settings; }
            set { Settings = value; }
        }

        public FileHandler fileH
        {
            get { return FileH; }
            set { FileH = value; }
        }

        //Constructor con FileHandler por parametro
        public SettingsHandler(FileHandler FileH) {

            this.FileH = FileH; 
            /*
             * En el instanciamiento intenta recoger los datos del fichero
             * Si no devuelve nada porque el fichero no exista, creara nuevos ajustes por defecto
             */
            if (FileH.Deserialice() == null)
            {
                Settings = new();
                FileH.Serialice(Settings);
            }
            
            Settings = FileH.Deserialice();

        }

        
    }
}
