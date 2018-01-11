// Interfaces for making AbstractStates update

namespace GameTemplate
{
    public interface IUpdate
    {
        void Update();
    }

    public interface IFixedUpdate
    {
        void FixedUpdate();
    }

    public interface ILateUpdate
    {
        void LateUpdate();
    }
}