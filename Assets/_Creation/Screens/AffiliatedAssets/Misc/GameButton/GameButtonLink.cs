using Genesis.Wisdom;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Genesis.Creation {
    internal sealed class GameButtonLink: MonoBehaviour { //POD
        [field: SerializeField]
        internal EventTrigger MyEventTrigger {
            get;
            private set;
        }

        [field: SerializeField]
        internal AbstractAnim PtrDownAnim {
            get;
            private set;
        }

        [field: SerializeField]
        internal AbstractAnim PtrUpAnim {
            get;
            private set;
        }
    }
}