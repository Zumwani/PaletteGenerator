using System.ComponentModel;

namespace PaletteGenerator.UI
{

    static class DesignModeUtility
    {

        public static bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject());

    }

}
