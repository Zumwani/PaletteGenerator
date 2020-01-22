using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Palettetitizer
{
    public class Row : INotifyPropertyChanged
    {

        public Row() { }

        public Row(int columns) =>
            Recalculate(columns);

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

        public void Recalculate(int columns)
        {

            var c = columns / 2;
            Left.Clear();
            for (int i = 0; i < c; i++)
                Left.Add(Colors.Black);

            Right.Clear();
            for (int i = 0; i < c; i++)
                Right.Add(Colors.Black);

            OnAllPropertiesChanged();

        }

    }

}
