namespace NTAC_db.DTO
{

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public class SettingsObj
    {
        //Atributos
        private Max_MinValues Values;
        private DB_data ConValue;
        private Accessibility Access;
        private eCatcher eCatcher;

        public Max_MinValues values 
        {
            get { return Values; }
            set { Values = value; }
        }

        public DB_data conValue 
        {
            get { return ConValue; }

            set { ConValue = value; }
        }

        public Accessibility access 
        {
            get { return Access; }
            set { Access = value; }
        }

        public eCatcher ecatcher
        {
            get { return eCatcher; }
            set { eCatcher = value; }
        }

        /// <summary>
        /// Contructor ocn datos por default
        /// </summary>
        public SettingsObj() 
        { 
            Values = new();
            ConValue = new();
            Access = new();
            eCatcher = new();
        }
    }
}
