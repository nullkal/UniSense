using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace UniSense.LowLevel
{
    [StructLayout(LayoutKind.Explicit, Size = kSize)]
    internal unsafe struct DualSenseHIDOutputReport : IInputDeviceCommandInfo
    {
        public static FourCC Type => new FourCC('H', 'I', 'D', 'O');

        internal const int kSize = InputDeviceCommand.BaseCommandSize + 48;
        internal const int kTriggerParamSize = 9;
        internal const int kReportId = 2;

        [Flags]
        internal enum Flags1 : byte
        {
            MainMotors1 = 0x01,
            MainMotors2 = 0x02,
            RightTrigger = 0x04,
            LeftTrigger = 0x08,
        }

        [Flags]
        internal enum Flags2 : byte
        {
            MicLed = 0x01,
            SetLightBarColor = 0x04,
            PlayerLed = 0x10,
        }

        internal enum InternalMicLedState : byte
        {
            Off = 0x00,
            On = 0x01,
            Pulsating = 0x02,
        }

        [Flags]
        internal enum LedFlags : byte
        {
            PlayerLedBrightness = 0x01,
            LightBarFade        = 0x02,
        }

        internal enum InternalPlayerLedBrightness : byte
        {
            High = 0x0,
            Medium = 0x1,
            Low = 0x2,
        }

        [FieldOffset(0)] public InputDeviceCommand baseCommand;

        [FieldOffset(InputDeviceCommand.BaseCommandSize + 0)] public byte reportId;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 1)] public Flags1 flags1;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 2)] public Flags2 flags2;

        [FieldOffset(InputDeviceCommand.BaseCommandSize + 3)] public byte lowFrequencyMotorSpeed;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 4)] public byte highFrequencyMotorSpeed;

        [FieldOffset(InputDeviceCommand.BaseCommandSize + 9)] public InternalMicLedState micLedState;
        
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 11)] public byte rightTriggerMode;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 12)] public fixed byte rightTriggerParams[kTriggerParamSize];
        
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 22)] public byte leftTriggerMode;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 23)] public fixed byte leftTriggerParams[kTriggerParamSize];
        
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 37)] public byte powerReduction;
        
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 39)] public LedFlags ledFlags;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 42)] public byte ledPulseOption;

        [FieldOffset(InputDeviceCommand.BaseCommandSize + 43)] public InternalPlayerLedBrightness playerLedBrightness;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 44)] public byte playerLedState;

        [FieldOffset(InputDeviceCommand.BaseCommandSize + 45)] public byte lightBarRed;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 46)] public byte lightBarGreen;
        [FieldOffset(InputDeviceCommand.BaseCommandSize + 47)] public byte lightBarBlue;

        public FourCC typeStatic => Type;

        public void SetMotorSpeeds(float lowFreq, float highFreq)
        {
            flags1 |= Flags1.MainMotors1 | Flags1.MainMotors2;
            lowFrequencyMotorSpeed = (byte)Mathf.Clamp(lowFreq * 255, 0, 255);
            highFrequencyMotorSpeed = (byte)Mathf.Clamp(highFreq * 255, 0, 255);
        }

        public void ResetMotorSpeeds(bool resetImmediately = false)
        {
            if (resetImmediately)
            {
                flags1 &= ~(Flags1.MainMotors1 | Flags1.MainMotors2);
            }
            else
            {
                flags1 &= ~Flags1.MainMotors2;
            }
        }

        public void SetMicLedState(DualSenseMicLedState state)
        {
            flags2 |= Flags2.MicLed;
            switch (state)
            {
                case DualSenseMicLedState.Off:
                    micLedState = InternalMicLedState.Off;
                    break;
                case DualSenseMicLedState.On:
                    micLedState = InternalMicLedState.On;
                    break;
                case DualSenseMicLedState.Pulsating:
                    micLedState = InternalMicLedState.Pulsating;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void SetRightTriggerState(DualSenseTriggerState state)
        {
            flags1 |= Flags1.RightTrigger;
            fixed (byte* p = rightTriggerParams)
            {
                SetTriggerState(state, ref rightTriggerMode, p);
            }
        }

        public void SetLeftTriggerState(DualSenseTriggerState state)
        {
            flags1 |= Flags1.LeftTrigger;
            fixed (byte* p = leftTriggerParams)
            {
                SetTriggerState(state, ref leftTriggerMode, p);
            }
        }

        private void SetTriggerState(DualSenseTriggerState state, ref byte triggerMode, byte* triggerParams)
        {
            switch (state.EffectType)
            {
                case DualSenseTriggerEffectType.NoResistance:
                    triggerMode = 0x00;
                    ClearTriggerParams(triggerParams);
                    break;
                case DualSenseTriggerEffectType.ContinuousResistance:
                    triggerMode = 0x01;
                    ClearTriggerParams(triggerParams);
                    triggerParams[0] = state.Continuous.StartPosition;
                    triggerParams[1] = state.Continuous.Force;
                    break;
                case DualSenseTriggerEffectType.SectionResistance:
                    triggerMode = 0x02;
                    ClearTriggerParams(triggerParams);
                    triggerParams[0] = state.Section.StartPosition;
                    triggerParams[1] = state.Section.EndPosition;
                    triggerParams[2] = state.Section.Force;
                    break;
                case DualSenseTriggerEffectType.EffectEx:
                    triggerMode = 0x26;
                    ClearTriggerParams(triggerParams);
                    triggerParams[0] = (byte) (0xff - state.EffectEx.StartPosition);
                    triggerParams[1] = (byte) (state.EffectEx.KeepEffect ? 0x02 : 0x00);
                    triggerParams[3] = state.EffectEx.BeginForce;
                    triggerParams[4] = state.EffectEx.MiddleForce;
                    triggerParams[5] = state.EffectEx.EndForce;
                    triggerParams[8] = state.EffectEx.Frequency;
                    break;
                case DualSenseTriggerEffectType.Calibrate:
                    triggerMode = 0xFC;
                    ClearTriggerParams(triggerParams);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ClearTriggerParams(byte* triggerParams)
        {
            for (int i = 0; i < kTriggerParamSize; i++)
            {
                triggerParams[i] = 0;
            }
        }

        public void SetLightBarColor(Color color)
        {
            flags2 |= Flags2.SetLightBarColor;
            ledFlags |= LedFlags.LightBarFade;
            ledPulseOption = 0x02;
            lightBarRed = (byte) Mathf.Clamp(color.r * 255, 0, 255);
            lightBarGreen = (byte) Mathf.Clamp(color.g * 255, 0, 255);
            lightBarBlue = (byte) Mathf.Clamp(color.b * 255, 0, 255);
        }

        public void SetPlayerLedBrightness(PlayerLedBrightness brightness)
        {
            ledFlags |= LedFlags.PlayerLedBrightness;
            ledPulseOption = 0x02;
            switch (brightness)
            {
                case PlayerLedBrightness.High:
                    playerLedBrightness = InternalPlayerLedBrightness.High;
                    break;
                case PlayerLedBrightness.Medium:
                    playerLedBrightness = InternalPlayerLedBrightness.Medium;
                    break;
                case PlayerLedBrightness.Low:
                    playerLedBrightness = InternalPlayerLedBrightness.Low;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(brightness), brightness, null);
            }
        }

        public void SetPlayerLedState(PlayerLedState state)
        {
            flags2 |= Flags2.PlayerLed;
            ledPulseOption = 0x02;
            playerLedState = state.Value;
        }

        public void DisableLightBarAndPlayerLed()
        {
            ledFlags |= LedFlags.LightBarFade;
            ledPulseOption = 0x01;
        }

        public static DualSenseHIDOutputReport Create()
        {
            return new DualSenseHIDOutputReport
            {
                baseCommand = new InputDeviceCommand(Type, kSize),
                reportId = kReportId,
            };
        }
    }
}