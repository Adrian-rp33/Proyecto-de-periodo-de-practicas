using MySql.Data.MySqlClient;
using NTAC_db.DTO;
using System.Collections.ObjectModel;
using System.Windows;

namespace NTAC_db.daos
{
    public class DaoDataUnit
    {

        //Atributos
        private static DaoDataUnit? Instance;
        private MySqlConnection conn;
        private string DBName;
        private string tableName;

        //Constructor privado para que no se pueda modificar externamente la conexion (con direccion y puerto por parametro)
        private DaoDataUnit(string Address, string Port, string _DBName, string _tableName, string user, string password)
        {

            try
            {
                DBName = _DBName;
                tableName = _tableName;

                //Estructura de la conexion --> servidor (direccion), Puerto, nombre de la base de datos, usuario, contraseña
                string ConnectionString = $"server={Address}"
                                        + $";port={Port}"
                                        + $";database={DBName}"
                                        + $";user={user}"
                                        + $";password={password}";

                conn = new(ConnectionString);   
            }
            catch (MySqlException ex)
            { 
                System.Windows.MessageBox.Show("Error al realizar la conexion a la base de datos:\n" + ex, "Connection Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        /// <summary>
        /// Crea una instancia con la conexion a la base de datos para mantener la conexion segura
        /// de cualquier tipo de modificacion
        /// </summary>
        /// <param name="Address"></param>
        /// <param name="Port"></param>
        /// <param name="DBName"></param>
        /// <returns>Instancia de la conexion</returns>
        public static DaoDataUnit GetInstance(string Address, string Port, string _DBName, string _tableName, string user, string password)
        {
            if(Instance == null)
                Instance = new(Address, Port, _DBName, _tableName, user, password);
            return Instance;
        }

        /*
         * Query
         */
        /// <summary>
        /// Recoge todos los registros de la tabla dataunits de la base de datos
        /// </summary>
        /// <returns>ObservableCollection para DataUnits</returns>
        public async Task<ObservableCollection<DataUnit>>? GetAll() 
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new($"select * from {tableName}", conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                ObservableCollection<DataUnit> dataUnits = new ObservableCollection<DataUnit>();

                while (reader != null && reader.Read())
                {
                    dataUnits.Add(new DataUnit(reader["TimeInt"].ToString(), reader["TimeStr"].ToString(), reader["b_masa1"].ToString(),
                                    reader["b_masa2"].ToString(), reader["b_masa3"].ToString(), reader["b_masa4"].ToString(),
                                    reader["caudal1"].ToString(), reader["caudal2"].ToString(), reader["caudal_acum1"].ToString(),
                                    reader["caudal_acum2"].ToString(), reader["decanter"].ToString(), reader["dec_horas_parcial"].ToString(),
                                    reader["dec_horas_total"].ToString(), reader["int_md"].ToString(), reader["par_bd"].ToString(),
                                    reader["rpm_bd"].ToString(), reader["rpm_diff"].ToString(), reader["rpm_md"].ToString(),
                                    reader["t_rod_alim"].ToString(), reader["t_rod_sal"].ToString(), reader["vibrac"].ToString()));
                }

                conn.Close();
                //Si no ha recogido nada por que no hay datos retorna null y muestra un mensaje de error
                if (dataUnits.Count() == 0)
                {
                    System.Windows.MessageBox.Show("No hay datos que recoger de la base de datos.", "No Data", MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                    return dataUnits;
                }
                System.Windows.MessageBox.Show("Se han recogido los datos correctamente.", "Data Retrieved", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return dataUnits;
            }
            catch (MySqlException ex)
            {
                return null;
            }

        }

        /// <summary>
        /// Inserta una serie de registros en la tabla dataunits, en caso de no poder insertar alguno,
        /// va sumando a una variable NotInserted para luego avisar al usuario de la cantidad de 
        /// registros que no se han podido o subir y en caso de no poder insertar ninguno tambien lo hara saber.
        /// </summary>
        /// <param name="data"></param>
        public async Task InsertData(ObservableCollection<DataUnit> data)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = new($"insert into {tableName}() values(@timeInt, @date, @bm1, @bm2, @bm3, @bm4, @c1, @c2, @ca1, @ca2," +
                    "@dec, @horas_parcial, @horas_total, @int_md, @par_bd, @rpm_bd, @rpm_diff, @rpm_md, @rod_alim, @rod_sal, @vibrac)", conn);

                //Creacion de parametros para la subida de datos
                cmd.Parameters.Add("?timeInt", MySqlDbType.Int32);
                cmd.Parameters.Add("?date", MySqlDbType.DateTime);
                cmd.Parameters.Add("?bm1", MySqlDbType.Float);
                cmd.Parameters.Add("?bm2", MySqlDbType.Float);
                cmd.Parameters.Add("?bm3", MySqlDbType.Float);
                cmd.Parameters.Add("?bm4", MySqlDbType.Float);
                cmd.Parameters.Add("?c1", MySqlDbType.Float);
                cmd.Parameters.Add("?c2", MySqlDbType.Float);
                cmd.Parameters.Add("?ca1", MySqlDbType.Int64);
                cmd.Parameters.Add("?ca2", MySqlDbType.Int64);
                cmd.Parameters.Add("?dec", MySqlDbType.Float);
                cmd.Parameters.Add("?horas_parcial", MySqlDbType.Float);
                cmd.Parameters.Add("?horas_total", MySqlDbType.Float);
                cmd.Parameters.Add("?int_md", MySqlDbType.Float);
                cmd.Parameters.Add("?par_bd", MySqlDbType.Float);
                cmd.Parameters.Add("?rpm_bd", MySqlDbType.Float);
                cmd.Parameters.Add("?rpm_diff", MySqlDbType.Float);
                cmd.Parameters.Add("?rpm_md", MySqlDbType.Float);
                cmd.Parameters.Add("?rod_alim", MySqlDbType.Double);
                cmd.Parameters.Add("?rod_sal", MySqlDbType.Double);
                cmd.Parameters.Add("?vibrac", MySqlDbType.Double);

                int notInserted = 0;

                if (data != null)
                    foreach (DataUnit d in data)
                    {
                        try //Se van pasando los valores de 1 en 1
                        {
                            cmd.Parameters["?timeInt"].Value = d.timeInt;
                            cmd.Parameters["?date"].Value = d.timeStr;
                            cmd.Parameters["?bm1"].Value = d.b_masa1;
                            cmd.Parameters["?bm2"].Value = d.b_masa2;
                            cmd.Parameters["?bm3"].Value = d.b_masa3;
                            cmd.Parameters["?bm4"].Value = d.b_masa4;
                            cmd.Parameters["?c1"].Value = d.caudal1;
                            cmd.Parameters["?c2"].Value = d.caudal2;
                            cmd.Parameters["?ca1"].Value = d.caudal_acum1;
                            cmd.Parameters["?ca2"].Value = d.caudal_acum2;
                            cmd.Parameters["?dec"].Value = d.decanter;
                            cmd.Parameters["?horas_parcial"].Value = d.dec_horas_parcial;
                            cmd.Parameters["?horas_total"].Value = d.dec_horas_total;
                            cmd.Parameters["?int_md"].Value = d.int_md;
                            cmd.Parameters["?par_bd"].Value = d.par_bd;
                            cmd.Parameters["?rpm_bd"].Value = d.rpm_bd;
                            cmd.Parameters["?rpm_diff"].Value = d.rpm_diff;
                            cmd.Parameters["?rpm_md"].Value = d.rpm_md;
                            cmd.Parameters["?rod_alim"].Value = d.t_rod_alim;
                            cmd.Parameters["?rod_sal"].Value = d.t_rod_sal;
                            cmd.Parameters["?vibrac"].Value = d.vibrac;

                            cmd.ExecuteNonQuery();
                        }
                        catch (MySqlException) //En caso de error iremos sumando registros no insertados
                        {
                            notInserted++;
                        }

                    }
                else //Si no hay ningun dato
                    System.Windows.MessageBox.Show("No se han subido datos, ya que no hay datos que subir.", "Not Uploaded",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                //Comprobacion de cantidad de datos insertados
                if (notInserted > 0 && notInserted < data.Count()) //Algunos de los datos no se han introducido
                    System.Windows.MessageBox.Show($"No se han logrado subir {notInserted} registros de los {data.Count} registros, puede que " +
                        "se deba a que ya estan en la base de datos.", "Not Uploaded", MessageBoxButton.OK, MessageBoxImage.Warning);
                else if (notInserted == data.Count()) //Ninguno de los datos se ha introducido
                    System.Windows.MessageBox.Show("No se han logrado subir ninguno de los registros, puede que se deba a que " +
                        "estos ya estan en la base de datos.", "Not Uploaded", MessageBoxButton.OK, MessageBoxImage.Error);
                else //Todos los datos se han introducido
                    System.Windows.MessageBox.Show("Se han subido los datos correctamente.", "Upload OK", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                conn.Close();
            }
            catch (MySqlException ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        /*
         * Borrado de datos
         */
        /// <summary>
        /// Borrar todos los datos de la tabla dataunits
        /// </summary>
        public async Task DeleteData()
        {
            //Mensaje de confirmacion
            MessageBoxResult confirmation = System.Windows.MessageBox.Show($"Estas tratando de borrar todos los datos de la tabla {tableName} de la " +
                $"base de datos {DBName} ¿Estas seguro?" , "Confirm delete all data", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (confirmation == MessageBoxResult.Yes) 
            {
                conn.Open();
                MySqlCommand cmd = new ($"delete from {tableName}", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                System.Windows.MessageBox.Show("Se han eliminado todos los datos", "Delete OK", System.Windows.MessageBoxButton.OK, 
                    System.Windows.MessageBoxImage.Information);
            }
                
        }

        /// <summary>
        /// Borrado individual de un registro con la fecha
        /// </summary>
        /// <param name="d"></param>
        public async Task DeleteData(DataUnit d) 
        {
            //Mensaje de confirmacion
            MessageBoxResult confirmation = System.Windows.MessageBox.Show("Estas tratando de borrar un registro completamente de la base de datos "
                + $"{DBName} ¿Estas seguro?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (confirmation == MessageBoxResult.Yes)
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new($"delete from {tableName} where TimeStr={d.timeStr}", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (MySqlException ex) 
                {
                    throw new Exception(ex.Message);
                }
            }

        }

    }
}
