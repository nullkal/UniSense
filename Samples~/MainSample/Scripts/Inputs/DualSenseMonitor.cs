using UniSense;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DualSenseSample.Inputs
{
    /// <summary>
    /// Device monitor for DualSense gamepad.
    /// <para>
    /// Notifies all listeners about a DualSense connection or disconnection and 
    /// resets a DualSense instance when disabled.
    /// </para>
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class DualSenseMonitor : MonoBehaviour
    {
        private AbstractDualSenseBehaviour[] listeners;

        private void Start()
        {
            listeners = GetComponentsInChildren<AbstractDualSenseBehaviour>();
            var dualSense = DualSenseGamepadHID.FindCurrent();
            var isDualSenseConected = dualSense != null;
            if (isDualSenseConected) NotifyConnection(dualSense);
            else NotifyDisconnection();
        }

        private void OnEnable() => InputSystem.onDeviceChange += OnDeviceChange;

        private void OnDisable()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
            var dualSense = DualSenseGamepadHID.FindCurrent();
            dualSense?.Reset();
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            var isNotDualSense = !(device is DualSenseGamepadHID);
            if (isNotDualSense) return;

            switch (change)
            {
                case InputDeviceChange.Added:
                    NotifyConnection(device as DualSenseGamepadHID);
                    break;
                case InputDeviceChange.Reconnected:
                    NotifyConnection(device as DualSenseGamepadHID);
                    break;
                case InputDeviceChange.Disconnected:
                    NotifyDisconnection();
                    break;
            }
        }

        private void NotifyConnection(DualSenseGamepadHID dualSense)
        {
            foreach (var listener in listeners)
            {
                listener.OnConnect(dualSense);
            }
        }

        private void NotifyDisconnection()
        {
            foreach (var listener in listeners)
            {
                listener.OnDisconnect();
            }
        }
    }
}
