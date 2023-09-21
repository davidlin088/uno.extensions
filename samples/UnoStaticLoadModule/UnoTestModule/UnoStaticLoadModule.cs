
using TreehouseInterface;
using Uno.Extensions.Navigation;

namespace UnoStaticLoadModule
{
    public class UnoStaticLoadModule : ITreehouseModule
    {
        public void OnInitialized(IViewRegistry views, IRouteRegistry routes) 
        {
            views.Register(new ViewMap<UnoStaticLoadPage, UnoStaticLoadViewModel>());
            var viewMap = views.FindByViewModel<UnoStaticLoadViewModel>();
            routes.Register(new RouteMap("UnoStaticLoad", viewMap));
        }
    }
}
