using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PaletteGenerator
{

    public class Row : INotifyPropertyChanged
    {

        public Row() { }

        public BindingList<Color> Left { get; } = new BindingList<Color>();
        public BindingList<Color> Right { get; } = new BindingList<Color>();

        Color center = Colors.LightSkyBlue;
        public Color Center 
        {
            get => center;
            set { center = value; OnPropertyChanged(); }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        void OnAllPropertiesChanged() =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));

        #endregion

        public Task<(IEnumerable<Color> left, IEnumerable<Color> right)> Calculate(Color left, Color right, int steps, float hueOffset) =>
            Task.Run(() =>
            (left.Blend(Center, steps).Skip(1).SkipLast(1).Select(c => c.OffsetHue(hueOffset * 360)),
             Center.Blend(right, steps).Skip(1).SkipLast(1).Select(c => c.OffsetHue(hueOffset * 360)))
            );

        public void SetColors((IEnumerable<Color> left, IEnumerable<Color> right) colors)
        {
            Left.Set(colors.left);
            Right.Set(colors.right);
        }

    }

}
