using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace PaletteGenerator.Collections
{

    public class Presets : MarkupExtension
    {

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            
            var list = new BindingList<Preset>();
            list.AddRange(Preset.LoadAll().ConfigureAwait(false).GetAwaiter().GetResult());
            return list;

        }

    }

}
