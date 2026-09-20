using System.Collections.Generic;
using UnityEngine;

namespace Strangeman.Utils.State
{
    public sealed class StateMachineRegistry
    {
        private static StateMachineRegistry _instance;
        public static StateMachineRegistry Instance => _instance ??= new StateMachineRegistry();

        private readonly List<Strangeman.Utils.State.StateMachine> _stateMachineTrackers = new();

        public IReadOnlyList<Strangeman.Utils.State.StateMachine> StateMachines => _stateMachineTrackers;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Initialize()
        {
            _instance ??= new StateMachineRegistry();
            _instance._stateMachineTrackers.Clear();
        }

        public void RegisterMachine(Strangeman.Utils.State.StateMachine toRegister)
        {
            if (toRegister == null || _stateMachineTrackers.Contains(toRegister))
                return;

            _stateMachineTrackers.Add(toRegister);
        }

        public void RemoveMachine(Strangeman.Utils.State.StateMachine toRemove)
        {
            if (toRemove == null)
                return;

            _stateMachineTrackers.Remove(toRemove);
        }
    }
}