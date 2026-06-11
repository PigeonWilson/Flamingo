using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CommonLibrary
{
    public class BaseObject : INotifyPropertyChanged
    {
        // Event required by INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        // Helper method to raise the event
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DateTime? _stamp;
        public DateTime? Stamp
        {
            get { return _stamp; }
            set
            {
                if (_stamp != value)
                {
                    _stamp = value;
                    OnPropertyChanged(nameof(Stamp));
                }
            }
        }

        private string? _guid;
        public string? Guid
        {
            get { return _guid; }
            set 
            {
                if (_guid != value)
                {
                    _guid = value;
                    OnPropertyChanged(nameof(Guid));   
                }
                
            }
        }

        private void Initialize()
        {
            this.Stamp = DateTime.Now;
            this.Guid = System.Guid.NewGuid().ToString();
        }

        public BaseObject()
        {
            this.Initialize();
        }
    }
}
