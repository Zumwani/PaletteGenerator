using System.ComponentModel;

namespace PaletteGenerator.UI
{

    /// <summary>Contains utility functions for working with the cursor.</summary>
    static class DesignModeUtility
    {

        /// <summary>Gets if we're currently in design mode.</summary>
        public static bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject());

    }

}
