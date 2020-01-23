using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PaletteGenerator
{

    public class Row : INotifyPropertyChanged
    {

        public Row() { }

        public Row(int columns, Color left, Color right) =>
            Recalculate(columns, left, right).ConfigureAwait(false);

        public ObservableCollection<Color> Left { get; } = new ObservableCollection<Color>();
        public ObservableCollection<Color> Right { get; } = new ObservableCollection<Color>();

        public Color Center { get; set; } = Colors.LightSkyBlue;

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void OnAllPropertiesChanged() =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));

        #endregion

        public async Task Recalculate(int columns, Color left, Color right)
        {

            var c = columns / 2 - 1;

            var leftSide = (await Calculate(c, Center, left)).Reverse().ToArray();
            var rightSide = await Calculate(c, Center, right);

            Left.Clear();
            Right.Clear();

            Left.AddRange(leftSide);
            Right.AddRange(rightSide);

            OnAllPropertiesChanged();

        }

        Task<Color[]> Calculate(int count, Color center, Color color) =>
            Task.Run(() => count.Select(i => center.Blend(color, 1 - ((float)i / count))));

    }

}
