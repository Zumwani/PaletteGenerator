using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Drawing2D;
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

        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        void OnAllPropertiesChanged() =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));

        #endregion

        public void Recalculate(int columns, Color left, Color right)
        {

            var c = columns / 2;
            CalculateSide(Left, c, left, Center);
            CalculateSide(Right, c, Center, right);

            OnAllPropertiesChanged();

        }

        void CalculateSide(IList<Color> list, int count, Color left, Color right)
        {

            list.Clear();

            if (count == 0)
                return;

            ColorBlend blend = new ColorBlend(count);
            blend.Colors[0] = left.ToDrawing();
            blend.Colors[count - 1] = right.ToDrawing();

            foreach (var color in blend.Colors)
                list.Add(color.ToWPF());

        }

    }

}
