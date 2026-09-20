using System.Collections.Generic;
using UnityEngine;

namespace Strangeman.Utils.Visitor
{
    [CreateAssetMenu(menuName = "Strangeman/Core/Action")]
    public class ScriptableAction : ScriptableObject
    {
        [field: SerializeField] public string Label { get; private set; }
        [field: SerializeReference] public List<IVisitor> Effects { get; private set; }
        
        //helper method - better to use this than target the effect list itself.
        public void ProcessEffects(IVisitable target)
        {
            foreach (var effect in Effects) effect.Visit(target);
        }

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(Label)) Label = name;
            Effects ??= new List<IVisitor>();
        }
    }
}