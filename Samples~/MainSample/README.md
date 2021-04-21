This sample shows how to use the DualSense gamepad.

## Summary

All components are on the Scripts folder and implements the ```AbstractDualSenseBehaviour``` class. 
This class is used as a listener by the ```DualSenseMonitor``` component to set an instance of the ```DualSenseGamepadHID``` every time a DualSense gamepad is connected to the system.

The Scripts are:

1. DualSenseMonitor: notifies all the following components about a DualSense connection or disconnection and resets a DualSense instance when disabled.
2. DualSenseButtonInput: displays all DualSense button inputs using SpriteRenderer instances.
3. DualSenseRumble: sets the DualSense rumble, aka motor speeds.
4. DualSenseTouchpadColor: sets the DualSense Touchpad light bar color.
5. DualSenseTrigger: sets the DualSense Triggers.

Those components are used on the ```DualSense``` prefab. You can open the ```MainSample``` scene and check those features.