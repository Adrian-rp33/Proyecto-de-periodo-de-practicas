using NTAC_db.DTO;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows;
using System.Xml.Serialization;

namespace NTAC_db.Handlers
{
    /*
     *
     * @author Adrian Rivas Perez
     *
     */

    public class FileHandler
    {
        //Atributos
        private DataUnit? aux;
        private readonly String prefsPath = @".\prefs.config";
        private static readonly string[] value = {
            "Fecha en entero",
            "Fecha",
            "Bomba masa 1",
            "Bomba masa 2",
            "Bomba masa 3",
            "Bomba masa 4",
            "Caudal 1",
            "Caudal 2",
            "Caudal acumulado 1",
            "Caudal acumulado 2",
            "Decanter",
            "Dec horas parcial",
            "Dec horas total",
            "Entero md",
            "Par bd",
            "Rpm bd",
            "Rpm diff",
            "Rpm md",
            "T rod alim",
            "T rod salida",
            "Vibracion"
        };
        private readonly String[] prefsBase = value;
        private readonly String settingsPath = @".\Settings.xml";

        //Constructor vacio
        public FileHandler() { }

        //Metodo que concede permisos sobre un ficheros
        private void GrantPermissions(String fileToGrant)
        {
            try 
            {
                //Creacion de las reglas de acceso
                FileSystemAccessRule rules = new (WindowsIdentity.GetCurrent().Name, FileSystemRights.FullControl, AccessControlType.Allow);
                new FileInfo(fileToGrant).GetAccessControl().AddAccessRule(rules); //Aplicacion de reglas sobre el fichero que se ha creado
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show("Error intentando dar permisos sobre el fichero:\n" + ex.StackTrace, "Couldn't Access", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        /*
         * Metodos para la extraccion de datos para DataUnits
         */

        /// <summary>
        /// Lee los ficheros pasados por parametro
        /// </summary>
        /// <param name="FilesPaths"></param>
        /// <returns>Lista con todos los DataUnits extraidos de los ficheros</returns>
        public async Task<ObservableCollection<DataUnit>> ReadFileDataUnit(ObservableCollection<String> FilesPaths)
        {
            ObservableCollection<DataUnit> DataList = new();
            StreamReader reader;
            foreach (String f in FilesPaths) 
            {
                if (!String.IsNullOrEmpty(f)) 
                {
                    reader = new StreamReader(f);
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (CreateObjectFrom(line.Split(";")))
                            DataList.Add(aux);
                    }
                }

            }

            return DataList;

        }

        /// <summary>
        /// Crea un DataUnit con la linea pasada del fichero estando dividida
        /// </summary>
        /// <param name="singleData"></param>
        /// <returns>Boolean indicando si se ha podido crear un DataUnit con los datos de la linea</returns>
        private Boolean CreateObjectFrom(string[] singleData)
        {
            try
            {
                aux = new(singleData[0], singleData[1], singleData[2], singleData[3],
                    singleData[4], singleData[5], singleData[6], singleData[7],
                    singleData[8], singleData[9], singleData[10], singleData[11],
                    singleData[12], singleData[13], singleData[14], singleData[15],
                    singleData[16], singleData[17], singleData[18],
                    singleData[19], singleData[20]);

                return true;
            }
            catch (System.FormatException) //Si tenemos un error de formato retorna false
            {
                return false;
            }

        }

        /// <summary>
        /// Recoge las preferencias del fichero dedicada a las mismas.
        /// En caso de que se haya modificado el fichero manualmente y no cumpla
        /// con el formato de lectura, se borrara y creara uno nuevo con los preferencias default
        /// </summary>
        /// <returns>Array de Boolean que indican que atributos seran visibles en la tabla de visualizacion de datos</returns>
        public Boolean[]? SeePrefs()
        {
            Boolean[] prefs = new Boolean[21];
            //Comprobacion de que existe
            if (!File.Exists(prefsPath))
            {
                FileStream fs = File.Create(prefsPath);
                fs.Close();
                GrantPermissions(prefsPath);
                ReDoPrefs(SetDefaultPrefs());
            }

            /* 
             * Se hará la lectura linea por linea indicando el numero de linea (de esta forma podemos tener
             * los datos ordenados en el Array de preferencias) 
            */
            for (int i=0; i<21; i++)
            {
                try
                {
                    String line = File.ReadLines(prefsPath).ElementAt(i);
                    String[] auxStr = line.Split(':');

                    if (auxStr[1].Trim() == "true")
                        prefs[i] = true;
                    else if (auxStr[1].Trim() == "false")
                        prefs[i] = false;
                    else
                    {
                        System.Windows.MessageBox.Show("Error, parece que se ha modificado el fichero de preferencias manualmente" +
                            " y no cumple con el formato utilizado para la aplicación, se procederá a la eliminación" +
                            " y creación de un nuevo fichero de preferencias.", "Modified File", System.Windows.MessageBoxButton.OK,
                            System.Windows.MessageBoxImage.Exclamation);
                        File.Delete(prefsPath);
                        return null;
                    }
                }
                catch (Exception ex) 
                { }
            }

            return prefs;
        }

        /// <summary>
        /// Se hace un Array con todas las preferencias con los valores por default
        /// (todo a "true")
        /// </summary>
        /// <returns>Array Boolean con preferencias por default</returns>
        private Boolean[] SetDefaultPrefs()
        {
            Boolean [] prefs = new Boolean[21];
            //Ponemos datos por default inicialmente
            for (int i = 0; i < 21; i++)
            {
                prefs[i] = true;
            }
            return prefs;
        }

        /// <summary>
        /// Reescribe las preferencias teniendo en cuenta el array Boolean pasado.
        /// Cuenta con control de error por si no es posible reescribir el fichero 
        /// (por ejemplo por falta de permisos)
        /// </summary>
        /// <param name="newPrefs"></param>
        public void ReDoPrefs(Boolean[] newPrefs)
        {

            //Usando la base del documento, agregamos lo que necesitamos para formarlo
            try
            {
                StreamWriter sw = new(prefsPath, false);
                //Escribimos linea por linea comprobando si esta activo o no
                for (int i=0; i<21; i++) 
                {
                    if (newPrefs[i] == true)
                        sw.WriteLine(prefsBase[i] + ": true");
                    else
                        sw.WriteLine(prefsBase[i] + ": false");
                }
                sw.Close();
            }
            catch (Exception e) 
            {
                System.Windows.MessageBox.Show("Ha habido un error a la hora de reescribir las preferencias:\n" + e, "Couldn't Rewrite",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

        }

        /*
         * Metodos para el manejo de fichero con los ajustes de la aplicación
         * Se maneja un fichero XML
         */

        /// <summary>
        /// Serializacion de objeto tipo SettingsObj.
        /// Control de error para la falta de derechos.
        /// </summary>
        /// <param name="newSettings"></param>
        public void Serialice(SettingsObj newSettings)
        {
            //Si no existe crea el fichero de ajustes
            if (!File.Exists(settingsPath)) 
            {
                FileStream fs = File.Create(settingsPath);
                GrantPermissions(settingsPath);
                fs.Close();
            }
            try 
            {
                TextWriter tw = new StreamWriter(settingsPath);
                XmlSerializer serializer = new(typeof(SettingsObj)); //Serializador

                serializer.Serialize(tw, newSettings); //Serializacion
                tw.Close();
                System.Windows.MessageBox.Show("Se han guardado y aplicado los ajustes correctamente.", "Settings OK",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("No se pudieron reescribir los ajustes:\n" + ex.StackTrace, "Couldn't Rewrite", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Deserializacion del objeto tipo SettingsObj.
        /// Control de error por si faltan derechos de lectura
        /// </summary>
        /// <returns></returns>
        public SettingsObj? Deserialice()
        {
            if (!File.Exists(settingsPath))
                return null; //Si no existe el fichero el objeto tampoco

            try
            {
                Stream s = File.OpenRead(settingsPath);
                XmlSerializer deserializer = new(typeof(SettingsObj));

                //Casteamos al tipo de objeto que guarda los ajustes y lo devolvemos
                return (SettingsObj)deserializer.Deserialize(s);
            }
            catch (Exception ex) 
            {
                System.Windows.MessageBox.Show("No se pudieron recoger los ajustes de la aplicacion." + ex.StackTrace, "Couldn't Retrieve", 
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return null;
            }
        }

    }

}
