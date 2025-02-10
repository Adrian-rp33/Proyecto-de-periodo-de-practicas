namespace NTAC_db.DTO
{

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public class Max_MinValues
    {
        
        private float B_masa_min;
        private float B_masa_max;
        private float Caudal_min;
        private float Caudal_max;
        private float Decanter_max;
        private float Decanter_min;
        private float Rpm_min;
        private float Rpm_max;
        private float Rod_alim_min;
        private float Rod_alim_max;
        private float Rod_sal_min;
        private float Rod_sal_max;

        public float b_masa_min
        {
            get { return B_masa_min; }
            set { B_masa_min = value; }
        }

        public float b_masa_max
        {
            get { return B_masa_max; }
            set { B_masa_max = value; }
        }

        public float caudal_min
        {
            get { return Caudal_min; }
            set { Caudal_min = value; }
        }

        public float caudal_max
        {
            get { return Caudal_max; }
            set { Caudal_max = value; }
        }

        public float decanter_min
        {
            get { return Decanter_min; }
            set { Decanter_min = value; }
        }

        public float decanter_max 
        {
            get { return Decanter_max; }
            set { Decanter_max = value; }
        }

        public float rpm_min 
        {
            get { return Rpm_min; }
            set { Rpm_min = value; }
        }

        public float rpm_max 
        {
            get { return Rpm_max; }
            set { Rpm_max = value; }
        }

        public float rod_alim_min
        {
            get { return Rod_alim_min; }
            set { Rod_alim_min = value; }
        }

        public float rod_alim_max
        {
            get{ return Rod_alim_max; }
            set { Rod_alim_max = value; }
        }

        public float rod_sal_min
        {
            get { return Rod_sal_min; }
            set { Rod_sal_min = value; }
        }

        public float rod_sal_max
        {
            get { return Rod_sal_max; }
            set { Rod_sal_max = value; }
        }

        /// <summary>
        /// Constructor con datos por default
        /// </summary>
        public Max_MinValues() 
        {
            B_masa_min = 0f;
            B_masa_max = 0f;
            Caudal_min = 0f;
            Caudal_max = 0f;
            Decanter_min = 0f;
            Decanter_max = 0f;
            Rpm_min = 0f;
            Rpm_max = 0f;
            Rod_alim_min = 0f;
            Rod_alim_max = 0f;
            Rod_sal_min = 0f;
            Rod_sal_max = 0f;
        }

        public float GetValueByName(String valueName) 
        {
            switch (valueName)
            {
                case "Bomba masa 1":
                    return B_masa_max;

                case "Bomba masa 2":
                    return B_masa_max;

                case "Bomba masa 3":
                    return B_masa_max;

                case "Bomba masa 4":
                    return B_masa_max;

                case "Caudal 1":
                    return Caudal_max;

                case "Caudal 2":
                    return Caudal_max;

                case "Decanter":
                    return Decanter_max;

                case "Rpm bd":
                    return Rpm_max;

                case "Rpm diff":
                    return Rpm_max;

                case "Rpm md":
                    return Rpm_max;

                case "T rod alim":
                    return Rod_alim_max;

                case "T rod salida":
                    return Rod_sal_max;

                default:
                    return 0f;

            }
        }
    }
}
