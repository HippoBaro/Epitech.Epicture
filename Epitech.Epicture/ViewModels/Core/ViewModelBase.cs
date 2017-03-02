using System.ComponentModel;
using System.Runtime.CompilerServices;
using Epitech.Epicture.Properties;

namespace Epitech.Epicture.ViewModels.Core
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        private bool _isFetching;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsFetching
        {
            get { return _isFetching; }
            set
            {
                _isFetching = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnDisplay()
        {
            
        }
    }
}
