namespace NTAC_db.Handlers
{

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public class AttributesShownHandler
    {
        //Array Boolean para guardar preferencias
        public Boolean[]? prefs { get; set; }
        //FileHandler que permita el manejo del fichero de preferencias
        private FileHandler fileHandler;

        /*
         * Constructor default
         * 
         * @see SetDefaultPrefs()
        */
        public AttributesShownHandler(FileHandler fileHandler) 
        {
            this.fileHandler = fileHandler;
            fileHandler.SeePrefs();
                
        }

        //Constructor vacio
        public AttributesShownHandler() {}

        /// <summary>
        /// Iguala preferencias del handler con las del fichero
        /// </summary>
        public void GetPrefs()
        {
            prefs = fileHandler.SeePrefs();
        }

        /// <summary>
        /// Reescribe las preferencias teniendo en cuenta las del handlers
        /// </summary>
        public void setNewPrefs() 
        {
            fileHandler.ReDoPrefs(prefs);
        }

        /// <summary>
        /// Clone obj type AttributesShownHandler
        /// </summary>
        /// <returns>this Copy</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

}
