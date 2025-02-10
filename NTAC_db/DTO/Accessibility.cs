using NTAC_db.Interfaces;

namespace NTAC_db.DTO
{

    /*
     *
     * @author Adrian Rivas Perez
     *
     */
    public class Accessibility
    {
        private Boolean DarkTheme;

        public Boolean darkTheme
        {
            get { return DarkTheme; }
            set { 
                DarkTheme = value;
                ThemeInterface.ChangeTheme(DarkTheme);
            }
        }

        public Accessibility() 
        { 
            darkTheme = false;
        }
    }
}
