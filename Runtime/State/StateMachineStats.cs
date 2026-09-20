using UnityEngine;

namespace Strangeman.Utils.State
{
    public struct StateMachineStats
    {
        public string StateMachineDebuggerName;
        public string CurrentStateName;
        public int TotalTransitionsCount;
        public int CurrentTransitionCount;
        public int AnyTransitionCount;
        public Object RunnerObject;
    }

    public partial class StateMachine
    {
        private UnityEngine.Object RunnerObject { get; set; }

        public StateMachine(UnityEngine.Object runner)
        {
            RunnerObject = runner;
            StateMachineRegistry.Instance.RegisterMachine(this);
        }
        
        public StateMachineStats GetStats()
        {
            var totalTransitions = 0;
            foreach (var transitions in _transitions.Values)
                totalTransitions += transitions.Count;

            return new StateMachineStats
            {
                StateMachineDebuggerName = RunnerObject != null ? RunnerObject.name : nameof(Strangeman.Utils.State.StateMachine),
                CurrentStateName = CurrentState?.GetType().Name ?? "<null>",
                TotalTransitionsCount = totalTransitions,
                CurrentTransitionCount = _currentTransitions.Count,
                AnyTransitionCount = _anyTransitions.Count,
                RunnerObject = RunnerObject
            };
        }
    }
}