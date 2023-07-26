namespace Trash.Gameplay
{
    public interface IOnMouseButtonHold
    {
        void OnLeftMouseHoldStart();
        void OnLeftMouseHoldEnd();
        void OnRightMouseHoldStart();
        void OnRightMouseHoldEnd();
    }
}