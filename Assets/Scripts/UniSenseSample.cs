using UniSense;
using UnityEngine;
using UnityEngine.InputSystem;

public class UniSenseSample : MonoBehaviour
{
    private DualSenseGamepadHID m_dualSense = null;
    
    private void Start()
    {
        foreach (var gamepad in Gamepad.all)
        {
            if (!(gamepad is DualSenseGamepadHID)) continue;
            m_dualSense = gamepad as DualSenseGamepadHID;
            break;
        }
    }

    private void Update()
    {
        if (m_dualSense != null)
        {
            // Samples of input handling is omitted; it's almost the same as DUALSHOCK 4's way!
            // `DualSenseGamePadHID` inherits `DualShockGamepad`, so you can use DualSense gamepad
            // in almost all code which is handling DUALSHOCK 4 gamepad.

            var playerLed = (byte) (0b00001 << (Mathf.FloorToInt(Time.time) % 5));
            var lightBarColor = Color.HSVToRGB(Mathf.Repeat(Time.time / 7.0f, 1.0f), 1.0f, 1.0f);

            var state = new DualSenseGamepadState
            {
                LeftTrigger =
                    new DualSenseTriggerState()
                    { // Left trigger is clicky
                        EffectType = DualSenseTriggerEffectType.SectionResistance,
                        Section = {StartPosition = 0x00, EndPosition = 0x60, Force = 0xFF,}
                    },
                RightTrigger =
                    new DualSenseTriggerState()
                    { // Right trigger is heavy
                        EffectType = DualSenseTriggerEffectType.ContinuousResistance,
                        Continuous = {StartPosition = 0x00, Force = 0xFF,}
                    },
                LightBarColor = lightBarColor,
                PlayerLed = new PlayerLedState(playerLed, true)
            };
            m_dualSense.SetGamepadState(state);
        }
    }
}
