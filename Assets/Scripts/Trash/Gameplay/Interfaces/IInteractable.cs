namespace Trash.Gameplay
{
    public interface IInteractable
    {
        void OnHoverEnter();
        void OnHoverExit();
        void OnMouseDown();
        void OnMouseUp();
    }
}