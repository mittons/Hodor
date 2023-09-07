namespace Contracts.MediatorPattern
{
    public interface ITextEditorDisplayMediator
    {
        void AddTextEditorDisplay(ITextEditorDisplay display);
        void RemoveTextEditorDisplay(ITextEditorDisplay display);
        void DisplayTaskDetailsInTextEditor(int taskId);
        void DisplayProjectForestInTextEditor(int projectForestId);
    }
}
