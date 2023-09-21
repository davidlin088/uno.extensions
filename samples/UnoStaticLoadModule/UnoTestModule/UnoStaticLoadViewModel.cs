using CommunityToolkit.Mvvm.ComponentModel;

namespace UnoStaticLoadModule
{
    public class UnoStaticLoadViewModel : ObservableObject
    {
        IUnoStaticLoadModel _model;

        private string _showText = string.Empty;
        public string ShowText
        {
            set 
            {
                SetProperty(ref _showText, value); 
            }

            get { return _showText; }
        }

        public UnoStaticLoadViewModel()
        {
            this.ShowText = "STATIC LOAD";
        }
    }
}
