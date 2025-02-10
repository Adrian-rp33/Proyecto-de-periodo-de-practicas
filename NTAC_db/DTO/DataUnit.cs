using System.Globalization;

namespace NTAC_db.DTO
{
    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public class DataUnit : IComparable<DataUnit>
    {
        //Atributos de clase
        private int TimeInt;
        private DateTime TimeStr;
        private float B_masa1;
        private float B_masa2;
        private float B_masa3;
        private float B_masa4;
        private float Caudal1;
        private float Caudal2;
        private long Caudal_acum1;
        private long Caudal_acum2;
        private float Decanter;
        private float Dec_horas_parcial;
        private float Dec_horas_total;
        private float Int_md;
        private float Par_bd;
        private float Rpm_bd;
        private float Rpm_diff;
        private float Rpm_md;
        private double T_rod_alim;
        private double T_rod_sal;
        private double Vibrac;

        public int timeInt
        {
            get { return TimeInt; }
            set { TimeInt = value; }
        }

        public DateTime timeStr
        {
            get { return TimeStr; }
            set { TimeStr = value; }
        }

        public float b_masa1
        {
            get { return B_masa1; }
            set { B_masa1 = value; }
        }

        public float b_masa2
        {
            get { return B_masa2; }
            set { B_masa2 = value; }
        }

        public float b_masa3
        {
            get { return B_masa3; }
            set { B_masa3 = value; }
        }

        public float b_masa4
        {
            get { return B_masa4; }
            set { B_masa4 = value; }
        }

        public float caudal1
        {
            get { return Caudal1; }
            set { Caudal1 = value; }
        }

        public float caudal2
        {
            get { return Caudal2; }
            set { Caudal2 = value; }
        }

        public long caudal_acum1
        {
            get { return Caudal_acum1; }
            set { Caudal_acum1 = value; }
        }

        public long caudal_acum2
        {
            get { return Caudal_acum2; }
            set { Caudal_acum2 = value; }
        }

        public float decanter
        {
            get { return Decanter; }
            set { Decanter = value; }
        }

        public float dec_horas_parcial
        {
            get { return Dec_horas_parcial; }
            set { Dec_horas_parcial = value; }
        }

        public float dec_horas_total
        {
            get { return Dec_horas_total; }
            set { Dec_horas_total = value; }
        }

        public float int_md
        {
            get { return Int_md; }
            set { Int_md = value; }
        }

        public float par_bd
        {
            get { return Par_bd; }
            set { Par_bd = value; }
        }

        public float rpm_bd
        {
            get { return Rpm_bd; }
            set { Rpm_bd = value; }
        }

        public float rpm_diff
        {
            get { return Rpm_diff; }
            set { Rpm_diff = value; }

        }

        public float rpm_md
        {
            get { return Rpm_md; }
            set { Rpm_bd = value; }
        }

        public double t_rod_alim
        {
            get { return T_rod_alim; }
            set { T_rod_alim = value; }
        }

        public double t_rod_sal
        {
            get { return T_rod_sal; }
            set { T_rod_sal = value; }
        }

        public double vibrac
        {
            get { return Vibrac; }
            set { Vibrac = value; }
        }

        //Constructor vacio
        public DataUnit() { }

        //Constructor con todo por parametro (se pasa de String al tipo de dato necesario)
        public DataUnit(string timeInt, string timeStr, string b_masa1, string b_masa2, 
            string b_masa3, string b_masa4, string caudal1, string caudal2, string caudal_acum1, 
            string caudal_acum2, string decanter, string dec_horas_parcial, string dec_horas_total, 
            string int_md, string par_bd, string rpm_bd, string rpm_diff, string rpm_md, string rod_alim, string rod_sal, 
            string vibrac)
        {

            CultureInfo ci = System.Globalization.CultureInfo.InvariantCulture;
            DateTime date = DateTime.ParseExact(timeStr.Replace('"', ' ').Trim(), "dd/MM/yyyy HH:mm:ss", ci);

            //Volvemos a llevarlo a string para tener el mismo formato que la base de datos
            string dateStr = date.ToString("yyyy/MM/dd HH:mm:ss", ci);
            date = DateTime.Parse(dateStr);

            TimeInt = int.Parse(timeInt);
            TimeStr = date;
            this.B_masa1 = float.Parse(b_masa1, ci);
            this.B_masa2 = float.Parse(b_masa2, ci);
            this.B_masa3 = float.Parse(b_masa3, ci);
            this.B_masa4 = float.Parse(b_masa4, ci);
            this.Caudal1 = float.Parse(caudal1, ci);
            this.Caudal2 = float.Parse(caudal2, ci);
            this.Caudal_acum1 = long.Parse(caudal_acum1);
            this.Caudal_acum2 = long.Parse(caudal_acum2);
            this.Decanter = float.Parse(decanter, ci);
            this.Dec_horas_parcial = float.Parse(dec_horas_parcial, ci);
            this.Dec_horas_total = float.Parse(dec_horas_total, ci);
            this.Int_md = int.Parse(int_md);
            this.Par_bd = float.Parse(par_bd, ci);
            this.Rpm_bd = float.Parse(rpm_bd, ci);
            this.Rpm_diff = float.Parse(rpm_diff, ci);
            this.Rpm_md = float.Parse(rpm_md, ci);
            T_rod_alim = double.Parse(rod_alim, ci);
            T_rod_sal = double.Parse(rod_sal, ci);
            this.Vibrac = double.Parse(vibrac, ci);
        }

        /// <summary>
        /// Compara el DataUnit actual con otro y devuelve un entero en funcion de si son iguales o no
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Int</returns>
        public int CompareTo(DataUnit? other)
        {
            if (other == null) return 1;
            else
                return 0;
        }


        /// <summary>
        /// Devuelve el valor de un atributo por el nombre del mismo, al ser dynamic
        /// puede devolver distintos tipos de dato
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns>Double || Float || Long</returns>
        public dynamic? GetAttributeByName(string attribute) 
        {

            switch (attribute)
            {
                case "Bomba masa 1":
                    return B_masa1;

                case "Bomba masa 2":
                    return B_masa2;

                case "Bomba masa 3":
                    return B_masa3;

                case "Bomba masa 4":
                    return B_masa4;

                case "Caudal 1":
                    return Caudal1;

                case "Caudal 2":
                    return Caudal2;

                case "Caudal acumulado 1":
                    return Caudal_acum1;

                case "Caudal acumulado 2":
                    return Caudal_acum2;

                case "Decanter":
                    return Decanter;

                case "Dec horas parcial":
                    return Dec_horas_parcial;

                case "Dec horas total":
                    return Dec_horas_total;

                case "Entero md":
                    return Int_md;

                case "Par bd":
                    return Par_bd;

                case "Rpm bd":
                    return Rpm_bd;
                
                case "Rpm diff":
                    return Rpm_diff;

                case "Rpm md":
                    return Rpm_md;

                case "T rod alim":
                    return T_rod_alim;

                case "T rod salida":
                    return T_rod_sal;

                case "Vibracion":
                    return Vibrac;

                default:
                    return null;

            }
        }
    }
    
}
