
using TreehouseInterface;
using Uno.Extensions.Navigation;

namespace UnoTestModule
{
    public class UnoTestModule : ITreehouseModule
    {
        public void OnInitialized(IViewRegistry views, IRouteRegistry routes) 
        {
            views.Register(new ViewMap<UnoTestPage, UnoTestViewModel>());
            var viewMap = views.FindByViewModel<UnoTestViewModel>();
            routes.Register(new RouteMap("UnoTest", viewMap));
        }
    }
}
