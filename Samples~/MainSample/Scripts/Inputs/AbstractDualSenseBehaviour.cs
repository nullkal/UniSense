using UniSense;
using UnityEngine;

namespace DualSenseSample.Inputs
{
    /// <summary>
    /// MonoBehaviour component for a DualSense.
    /// <para>
    /// <see cref="DualSenseMonitor"/> adds this component as a listener and calls the 
    /// <see cref="OnConnect(DualSenseGamepadHID)"/> and <see cref="OnDisconnect"/> functions.</para>
    /// </summary>
    public abstract class AbstractDualSenseBehaviour : MonoBehaviour
    {
        /// <summary>
        /// The DualSense instance. 
        /// It can be null if no one is connected so always 
        /// use the null-conditional operator <c>?.</c> on it.
        /// </summary>
        public DualSenseGamepadHID DualSense { get; protected set; }

        internal virtual void OnConnect(DualSenseGamepadHID dualSense)
            => DualSense = dualSense;

        internal virtual void OnDisconnect() => DualSense = null;
    }
}