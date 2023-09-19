using CommunityToolkit.Mvvm.ComponentModel;

namespace UnoTestModule
{
    public class UnoTestViewModel : ObservableObject
    {
        IUnoTestModel _model;

        private string _showText = string.Empty;
        public string ShowText
        {
            set 
            {
                SetProperty(ref _showText, value); 
            }

            get { return _showText; }
        }

        public UnoTestViewModel()
        {
            //_model = model;
            //this.ShowText = _model.GetShowString();
            this.ShowText = "HEEEEEEEEEEEEEEEEE";
        }
    }
}
