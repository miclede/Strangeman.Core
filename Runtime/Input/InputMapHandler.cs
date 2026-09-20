using UnityEngine;
using UnityEngine.InputSystem;

namespace Strangeman.Utils.Input
{
    public abstract class InputMapHandler : ScriptableObject
    {
        protected InputDirector inputDirector;
        protected InputActionMap inputActionMap;
        internal InputActionMap Get() => inputActionMap;

        public void Initialize(InputDirector director)
        {
            inputDirector = director;
            SetupCallbacks();
        }

        public bool Active() => inputDirector is not null && inputDirector.ActiveHandler == this;

        protected abstract void SetupCallbacks();
    }

    /* Exmaple: A shooter input handler that is trying to access the system actions in the class generated via the input asset: InputSystemActions
         public class ShooterInputHandler : InputMapHandler, IShooterActions
        {
        protected override void SetupCallbacks()
        {
            var actionClass = InputDirector.Get<InputSystemActions>();
            actionClass.Shooter.SetCallbacks(this); //setting the callback is necessary to function
            inputActionMap = actionClass.Shooter.Get();
        }
        */
}