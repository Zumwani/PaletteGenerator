using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PaletteGenerator.Models
{

    class Property<T> : INotifyPropertyChanged
    {

        T value;
        public T Value
        {
            get => value;
            set { this.value = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public Property() { }
        public Property(T defaultValue) { value = defaultValue; }

        public static implicit operator T(Property<T> property) =>
            property.Value;

    }

}
