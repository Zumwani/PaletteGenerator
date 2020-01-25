using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaletteGenerator.UI
{

    public partial class PresetsManager : UserControl
    {

        public PresetsManager() =>
            InitializeComponent();


        public BindingList<Preset> Presets { get; } = new BindingList<Preset>();

    }

}
