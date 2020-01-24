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

        public Row(int columns, Color left, Color right, float hueOffset) =>
            Recalculate(columns, left, right, hueOffset).ConfigureAwait(false);

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

        public async Task Recalculate(int columns, Color left, Color right, float hueOffset)
        {

            Left.Clear();
            Right.Clear();

            var c = columns / 2 + 1;
            Left.AddRange(await Calculate(left, Center, c, hueOffset));
            Right.AddRange(await Calculate(Center, right, c, hueOffset));

        }

        public static Task<IEnumerable<Color>> Calculate(Color start, Color end, int steps, float hueOffset) =>
        Task.Run(() => 
            start.Blend(end, steps).Skip(1).SkipLast(1).Select(c => c.OffsetHue(hueOffset * 360))
        );

    }

}
