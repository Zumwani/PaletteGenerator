using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Palettetitizer
{

    public class Row : INotifyPropertyChanged
    {

        public Row() { }

        public Row(int columns, Color left, Color right) =>
            Recalculate(columns, left, right);

        public ObservableCollection<Color> Left { get; } = new ObservableCollection<Color>();
        public ObservableCollection<Color> Right { get; } = new ObservableCollection<Color>();

        public Color Center { get; set; } = Colors.Red;

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void OnAllPropertiesChanged() =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));

        #endregion

        public void Recalculate(int columns, Color left, Color right)
        {

            var c = columns / 2 + 1;
            CalculateSide(Left, c, left, Center);
            CalculateSide(Right, c, Center, right);

            OnAllPropertiesChanged();

        }

        void CalculateSide(IList<Color> list, int count, Color left, Color right)
        {

            list.Clear();

            if (count == 0)
                return;

            for (int i = 1; i < count - 1; i++)
                list.Add(left.Blend(right, 1 - ((float)i / count)));

        }

    }

}
