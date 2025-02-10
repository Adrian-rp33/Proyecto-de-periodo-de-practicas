namespace NTAC_db.DTO
{

    /*
    *
    * @author Adrian Rivas Perez
    *
    */
    public class eCatcher
    {

        private string Program;

        public string program
        {
            get { return Program; }
            set { Program = value; }
        }

        public eCatcher()
        {
            Program = "Todos";
        }
    }
}
