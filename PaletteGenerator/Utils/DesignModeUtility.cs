using System.ComponentModel;

namespace PaletteGenerator.UI
{

    public static class DesignModeUtility
    {

        public static bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject());

    }

}
