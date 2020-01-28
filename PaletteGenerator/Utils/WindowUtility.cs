namespace PaletteGenerator
{

    public abstract class WindowUtility
    {

        public static MainWindow Current => 
            (MainWindow)System.Windows.Application.Current.MainWindow;

    }

}
