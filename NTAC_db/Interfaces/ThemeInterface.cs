using System.Windows;

namespace NTAC_db.Interfaces
{
    public interface ThemeInterface
    {
        //Constantes con la ubicacion de los temas
        private const String lightThemeUri = "Resources/Themes/Light.xaml";
        private const String darkThemeUri = "Resources/Themes/Dark.xaml";

        //@return ResourceDictionary creado a partir de la ruta
        public static void ChangeTheme(Boolean active)
        {
            Uri uri;
            //active --> DarkTheme
            //!active --> LightTheme
            if (active)
                uri = new(darkThemeUri, UriKind.Relative);
            else 
                uri = new(lightThemeUri, UriKind.Relative);

            ResourceDictionary Theme = new() { Source = uri };
            App.Current.Resources.Clear(); //Se quita el tema actual para que solo se tenga que cambiar en la ventana
            App.Current.Resources.MergedDictionaries.Add(Theme); //Agregamos el seleccionado
        }

    }
}
