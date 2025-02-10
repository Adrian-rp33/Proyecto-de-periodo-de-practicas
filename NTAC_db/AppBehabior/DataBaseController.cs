using MySql.Data.MySqlClient;
using NTAC_db.daos;
using NTAC_db.DTO;
using System.Collections.ObjectModel;
using System.Data;

namespace NTAC_db.AppBehabior
{

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public class DataBaseController
    {
        private DaoDataUnit DaoDataUnits;

        //Constructor en el que no se crea una instancia de conexion
        public DataBaseController() { }

        //Constructor recogiendo la instancia de conexion con los datos de la base de datos
        public DataBaseController(string Address, string Port, string DBName, string tableName, string user, string password)
        {
            DaoDataUnits = DaoDataUnit.GetInstance(Address, Port, DBName, tableName, user, password);
        }

        /// <summary>
        /// Comprueba que la conexion a la base de datos descrita es posible
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Port"></param>
        /// <param name="DBName"></param>
        /// <returns>bool indicando si es posible o no</returns>
        public bool isPossibleToConnect(string Address, string Port, string DBName)
        {
            string ConnectionString = "server=" + Address
                        + ";port=" + Port
                        + ";database=" + DBName
                        + ";user=user"
                        + ";password=user";
            MySqlConnection aux = new();
            bool Possible = false;
            try
            {
                aux = new(ConnectionString);
                aux.Open();
                Possible = true;
            }
            catch (ArgumentException) //Formato de datos invalido
            {
                MessageBox.Show("Se han introducido datos con formato invalidos");
            }
            catch (MySqlException ex)
            {
                //Comprobacion del error mediante numero
                switch (ex.Number)
                {
                    case 1042: //Direccionamiento invalio
                        MessageBox.Show("Error de direccionamiento , comprueba direccion y puerto.");
                        break;
                    case 0: //Acceso
                        MessageBox.Show("Error de acceso, comprueba el nombre de la base de datos, usuario y contraseña");
                        break;
                    default:
                        break;
                }

            }
            finally //Despues de comprobar cerramos la conexion
            {
                if (aux.State == ConnectionState.Open)
                    aux.Close();
            }

            return Possible;
        }

        /*
         * Manejo de la tabla DataUnit
         */

        /// <summary>
        /// Recoge todos los registros de la tabla dataunits
        /// </summary>
        /// <returns>ObservableCollection para DataUnit</returns>
        public async Task<ObservableCollection<DataUnit>>? GetAllDataUnits()
        {
            return await DaoDataUnits.GetAll();
        }

        /// <summary>
        /// Metodo que sirve para guardar varios regitros, estos registros tienen que ser pasados
        /// como una lista para objetos del tipo DataUnit
        /// </summary>
        /// <param name="DataList"></param>
        public async Task InsertAllDataUnits(ObservableCollection<DataUnit> DataList) 
        {
            await DaoDataUnits.InsertData(DataList);
        }

        /// <summary>
        /// Borra todos los registros de la tabla dataunits
        /// </summary>
        public async Task DeleteAllDataUnits() 
        { 
            await DaoDataUnits.DeleteData();
        }
    
    }

}
