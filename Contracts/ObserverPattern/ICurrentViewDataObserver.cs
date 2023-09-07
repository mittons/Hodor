namespace Contracts.ObserverPattern
{
    public interface ICurrentViewDataObserver
    {
        void OnViewTaskFocusChanged(object sender);
        void OnViewDataSetChanged(object sender);
    }
}
