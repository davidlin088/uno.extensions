
using Uno.Extensions.Navigation;

namespace TreehouseInterface
{
    public interface ITreehouseModule
    {
        void OnInitialized(IViewRegistry views, IRouteRegistry routes);
    }
}
