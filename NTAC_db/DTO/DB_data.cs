namespace NTAC_db.DTO
{

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public class DB_data
    {
        //Atributos
        private bool HasDB;
        private string Address;
        private string Port;
        private string DBName;
        private string TableName;
        private string User;
        private string Password;

        public bool hasDB
        {
            get { return HasDB; }
            set { HasDB = value; }
        }

        public string address
        {
            get { return Address; }
            set { Address = value; }
        }

        public string port
        {
            get { return Port; }

            set { Port = value; }
        }

        public string DbName
        {
            get { return DBName; }
            set { DBName = value; }
        }

        public string tableName
        {
            get { return TableName; }
            set { TableName = value; }
        }

        public string user
        {
            get { return User; }
            set { User = value; }
        }

        public string password
        {
            get { return Password; }
            set { Password = value; }
        }

        //Contructor vacio con valores por default
        public DB_data() 
        {
            HasDB = false;
            Address = "null";
            Port = "null";
            DBName = "null";
            TableName = "null";
            User = "null";
            Password = "null";
        }
    }
}
