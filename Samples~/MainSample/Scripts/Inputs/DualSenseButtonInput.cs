using UnityEngine;

namespace DualSenseSample.Inputs
{
    /// <summary>
    /// Component to display all DualSense button inputs using the local SpriteRenderer instances.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class DualSenseButtonInput : AbstractDualSenseBehaviour
    {
        [Header("Buttons")]
        public SpriteRenderer cross;
        public SpriteRenderer square;
        public SpriteRenderer circle;
        public SpriteRenderer triangle;
        public SpriteRenderer r1;
        public SpriteRenderer r2;
        public SpriteRenderer r3;
        public SpriteRenderer l1;
        public SpriteRenderer l2;
        public SpriteRenderer l3;
        public SpriteRenderer create;
        public SpriteRenderer options;
        public SpriteRenderer touchpad;
        public SpriteRenderer dpadUp;
        public SpriteRenderer dpadLeft;
        public SpriteRenderer dpadDown;
        public SpriteRenderer dpadRight;
        public SpriteRenderer rightStick;
        public SpriteRenderer leftStick;
        public SpriteRenderer microphone;

        private void Update()
        {
            if (DualSense == null) return;

            cross.enabled = DualSense.crossButton.isPressed;
            square.enabled = DualSense.squareButton.isPressed;
            circle.enabled = DualSense.circleButton.isPressed;
            triangle.enabled = DualSense.triangleButton.isPressed;

            r1.enabled = DualSense.R1.isPressed;
            r2.enabled = DualSense.R2.isPressed;
            r3.enabled = DualSense.R3.isPressed;

            l1.enabled = DualSense.L1.isPressed;
            l2.enabled = DualSense.L2.isPressed;
            l3.enabled = DualSense.L3.isPressed;

            create.enabled = DualSense.shareButton.isPressed;
            options.enabled = DualSense.optionsButton.isPressed;
            touchpad.enabled = DualSense.touchpadButton.isPressed;

            dpadUp.enabled = DualSense.dpad.up.isPressed;
            dpadLeft.enabled = DualSense.dpad.left.isPressed;
            dpadRight.enabled = DualSense.dpad.right.isPressed;
            dpadDown.enabled = DualSense.dpad.down.isPressed;

            var hasLeftStickValue = Mathf.Abs(DualSense.leftStick.ReadValue().sqrMagnitude) > 0F;
            var hasRightStickValue = Mathf.Abs(DualSense.rightStick.ReadValue().sqrMagnitude) > 0F;

            leftStick.enabled = hasLeftStickValue;
            rightStick.enabled = hasRightStickValue;
            microphone.enabled = DualSense.micMuteButton.isPressed;
        }
    }
}