
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using System;

namespace SaccFlightAndVehicles
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]

    public class AircraftInspector : UdonSharpBehaviour
    {
        public SaccAirVehicle Vehicle;

        [Header("")]
        [Tooltip("普通は自動的に設定されます。階層がSacc標準と異なる場合は手動で設定してください。")]
        public SaccEntity VehicleEntity;
        [Tooltip("普通は自動的に設定されます。階層がSacc標準と異なる場合は手動で設定してください。")]
        public SAV_HUDController HUDController;
        [Tooltip("普通は自動的に設定されます。階層がSacc標準と異なる場合は手動で設定してください。")]
        public SAV_EffectsController EffectsController;
        private Rigidbody VehicleRigidbody;

        [Header("")]
        public Button ApplyAndRespawnButton;
        public Text TooltipText;

        [Header("")]

        public InputField MassInputField;
        public InputField GroundDetectorRayDistanceInputField;
        public InputField HealthInputField;
        public InputField ExplodeHealthInputField;
        public Toggle RepeatingWorldToggle;
        public InputField RepeatingWorldDistanceInputField;
        public Toggle HasAfterburnerToggle;
        public InputField ThrottleAfterburnerPointInputField;
        public Toggle VTOLOnlyToggle;
        public InputField ThrottleStrengthInputField;
        public Toggle VerticalThrottleToggle;
        public Toggle JoystickPushPullPitchToggle;
        public InputField JoystickPushPullDistanceInputField;
        public InputField AfterburnerThrustMultiInputField;
        public InputField AccelerationResponseInputField;
        public InputField EngineSpoolDownSpeedMultiInputField;
        public InputField AirFrictionInputField;
        public InputField PitchStrengthInputField;
        public InputField PitchThrustVecMultiInputField;
        public InputField PitchFrictionInputField;
        public InputField PitchConstantFrictionInputField;
        public InputField PitchResponseInputField;
        public InputField ReversingPitchStrengthMultiInputField;
        public InputField YawStrengthInputField;
        public InputField YawThrustVecMultiInputField;
        public InputField YawFrictionInputField;
        public InputField YawConstantFrictionInputField;
        public InputField YawResponseInputField;
        public InputField ReversingYawStrengthMultiInputField;
        public InputField RollStrengthInputField;
        public InputField RollThrustVecMultiInputField;
        public InputField RollFrictionInputField;
        public InputField RollConstantFrictionInputField;
        public InputField RollResponseInputField;
        public InputField ReversingRollStrengthMultiInputField;
        public InputField PitchDownStrMultiInputField;
        public InputField PitchDownLiftMultiInputField;
        public InputField InertiaTensorRotationMultiInputField;
        public Toggle InvertITRYawToggle;
        public InputField AdverseYawInputField;
        public InputField AdverseRollInputField;
        public InputField RotMultiMaxSpeedInputField;
        public InputField VelStraightenStrPitchInputField;
        public InputField VelStraightenStrYawInputField;
        public InputField MaxAngleOfAttackPitchInputField;
        public InputField MaxAngleOfAttackYawInputField;
        public InputField AoaCurveStrengthInputField;
        public InputField HighPitchAoaMinControlInputField;
        public InputField HighYawAoaMinControlInputField;
        public Toggle DoStallForcesToggle;
        public InputField PitchAoaPitchForceMultiInputField;
        public InputField YawAoaRollForceMultiInputField;
        public InputField HighPitchAoaMinLiftInputField;
        public InputField HighYawAoaMinLiftInputField;
        public InputField ZeroLiftAoAInputField;
        public InputField TaxiRotationSpeedInputField;
        public InputField TaxiRotationResponseInputField;
        public Toggle DisallowTaxiRotationWhileStillToggle;
        public InputField TaxiFullTurningSpeedInputField;
        public Toggle WheelSuspensionToggle;
        public InputField LiftInputField;
        public InputField SidewaysLiftInputField;
        public InputField MaxLiftInputField;
        public InputField VelLiftInputField;
        public InputField VelLiftMaxInputField;
        public InputField MaxGsInputField;
        public InputField GDamageInputField;
        public InputField Crash_Damage_SpeedInputField;
        public InputField Crash_Death_SpeedInputField;
        public InputField GroundEffectMaxDistanceInputField;
        public InputField GroundEffectStrengthInputField;
        public InputField GroundEffectLiftMaxInputField;
        public InputField VTOLAngleTurnRateInputField;
        public InputField VTOLDefaultValueInputField;
        public Toggle VTOLAllowAfterburnerToggle;
        public InputField VTOLThrottleStrengthMultiInputField;
        public InputField VTOLMinAngleInputField;
        public InputField VTOLMaxAngleInputField;
        public InputField VTOLPitchThrustVecMultiInputField;
        public InputField VTOLYawThrustVecMultiInputField;
        public InputField VTOLRollThrustVecMultiInputField;
        public InputField VTOLLoseControlSpeedInputField;
        public InputField VTOLGroundEffectStrengthInputField;
        public InputField EnterVTOLEvent_AngleInputField;
        public Toggle ReverseThrustAllowAfterburnerToggle;
        public InputField SoundBarrierStrengthInputField;
        public InputField SoundBarrierWidthInputField;
        public InputField FuelInputField;
        public InputField LowFuelInputField;
        public InputField FuelConsumptionInputField;
        public InputField MinFuelConsumptionInputField;
        public InputField FuelConsumptionABMultiInputField;
        public InputField RefuelTimeInputField;
        public InputField RepairTimeInputField;
        public InputField RespawnDelayInputField;
        public InputField InvincibleAfterSpawnInputField;
        public InputField BulletDamageTakenInputField;
        public Toggle PredictDamageToggle;
        public InputField SmallCrashSpeedInputField;
        public InputField MediumCrashSpeedInputField;
        public InputField BigCrashSpeedInputField;
        public InputField MissileDamageTakenMultiplierInputField;
        public InputField MissilePushForceInputField;
        public InputField SeaLevelInputField;
        public InputField AtmosphereThinningStartInputField;
        public InputField AtmosphereThinningEndInputField;
        public Toggle SquareJoyInputToggle;
        public Toggle DroneModeToggle;

        public Text MassValueText;
        public Text GroundDetectorRayDistanceValueText;
        public Text HealthValueText;
        public Text ExplodeHealthValueText;
        public Text RepeatingWorldValueText;
        public Text RepeatingWorldDistanceValueText;
        public Text HasAfterburnerValueText;
        public Text ThrottleAfterburnerPointValueText;
        public Text VTOLOnlyValueText;
        public Text ThrottleStrengthValueText;
        public Text VerticalThrottleValueText;
        public Text JoystickPushPullPitchValueText;
        public Text JoystickPushPullDistanceValueText;
        public Text AfterburnerThrustMultiValueText;
        public Text AccelerationResponseValueText;
        public Text EngineSpoolDownSpeedMultiValueText;
        public Text AirFrictionValueText;
        public Text PitchStrengthValueText;
        public Text PitchThrustVecMultiValueText;
        public Text PitchFrictionValueText;
        public Text PitchConstantFrictionValueText;
        public Text PitchResponseValueText;
        public Text ReversingPitchStrengthMultiValueText;
        public Text YawStrengthValueText;
        public Text YawThrustVecMultiValueText;
        public Text YawFrictionValueText;
        public Text YawConstantFrictionValueText;
        public Text YawResponseValueText;
        public Text ReversingYawStrengthMultiValueText;
        public Text RollStrengthValueText;
        public Text RollThrustVecMultiValueText;
        public Text RollFrictionValueText;
        public Text RollConstantFrictionValueText;
        public Text RollResponseValueText;
        public Text ReversingRollStrengthMultiValueText;
        public Text PitchDownStrMultiValueText;
        public Text PitchDownLiftMultiValueText;
        public Text InertiaTensorRotationMultiValueText;
        public Text InvertITRYawValueText;
        public Text AdverseYawValueText;
        public Text AdverseRollValueText;
        public Text RotMultiMaxSpeedValueText;
        public Text VelStraightenStrPitchValueText;
        public Text VelStraightenStrYawValueText;
        public Text MaxAngleOfAttackPitchValueText;
        public Text MaxAngleOfAttackYawValueText;
        public Text AoaCurveStrengthValueText;
        public Text HighPitchAoaMinControlValueText;
        public Text HighYawAoaMinControlValueText;
        public Text DoStallForcesValueText;
        public Text PitchAoaPitchForceMultiValueText;
        public Text YawAoaRollForceMultiValueText;
        public Text HighPitchAoaMinLiftValueText;
        public Text HighYawAoaMinLiftValueText;
        public Text ZeroLiftAoAValueText;
        public Text TaxiRotationSpeedValueText;
        public Text TaxiRotationResponseValueText;
        public Text DisallowTaxiRotationWhileStillValueText;
        public Text TaxiFullTurningSpeedValueText;
        public Text WheelSuspensionValueText;
        public Text LiftValueText;
        public Text SidewaysLiftValueText;
        public Text MaxLiftValueText;
        public Text VelLiftValueText;
        public Text VelLiftMaxValueText;
        public Text MaxGsValueText;
        public Text GDamageValueText;
        public Text Crash_Damage_SpeedValueText;
        public Text Crash_Death_SpeedValueText;
        public Text GroundEffectMaxDistanceValueText;
        public Text GroundEffectStrengthValueText;
        public Text GroundEffectLiftMaxValueText;
        public Text VTOLAngleTurnRateValueText;
        public Text VTOLDefaultValueValueText;
        public Text VTOLAllowAfterburnerValueText;
        public Text VTOLThrottleStrengthMultiValueText;
        public Text VTOLMinAngleValueText;
        public Text VTOLMaxAngleValueText;
        public Text VTOLPitchThrustVecMultiValueText;
        public Text VTOLYawThrustVecMultiValueText;
        public Text VTOLRollThrustVecMultiValueText;
        public Text VTOLLoseControlSpeedValueText;
        public Text VTOLGroundEffectStrengthValueText;
        public Text EnterVTOLEvent_AngleValueText;
        public Text ReverseThrustAllowAfterburnerValueText;
        public Text SoundBarrierStrengthValueText;
        public Text SoundBarrierWidthValueText;
        public Text FuelValueText;
        public Text LowFuelValueText;
        public Text FuelConsumptionValueText;
        public Text MinFuelConsumptionValueText;
        public Text FuelConsumptionABMultiValueText;
        public Text RefuelTimeValueText;
        public Text RepairTimeValueText;
        public Text RespawnDelayValueText;
        public Text InvincibleAfterSpawnValueText;
        public Text BulletDamageTakenValueText;
        public Text PredictDamageValueText;
        public Text SmallCrashSpeedValueText;
        public Text MediumCrashSpeedValueText;
        public Text BigCrashSpeedValueText;
        public Text MissileDamageTakenMultiplierValueText;
        public Text MissilePushForceValueText;
        public Text SeaLevelValueText;
        public Text AtmosphereThinningStartValueText;
        public Text AtmosphereThinningEndValueText;
        public Text SquareJoyInputValueText;
        public Text DroneModeValueText;

        [UdonSynced] private float[] _syncedFloatValues = new float[97];
        [UdonSynced] private bool[] _syncedBoolValues = new bool[14];

        private float[] _defaultFloatValues = new float[97];
        private bool[] _defaultBoolValues = new bool[14];

        private bool Initialized = false;

        private ColorBlock NormalButtonColor;
        private ColorBlock HighlightedButtonColor;


        private void ArraysToInputs(float[] floatValues, bool[] boolValues)
        {
            MassInputField.text = floatValues[0].ToString();
            GroundDetectorRayDistanceInputField.text = floatValues[1].ToString();
            HealthInputField.text = floatValues[2].ToString();
            ExplodeHealthInputField.text = floatValues[3].ToString();
            RepeatingWorldToggle.isOn = boolValues[0];
            RepeatingWorldDistanceInputField.text = floatValues[4].ToString();
            HasAfterburnerToggle.isOn = boolValues[1];
            ThrottleAfterburnerPointInputField.text = floatValues[5].ToString();
            VTOLOnlyToggle.isOn = boolValues[2];
            ThrottleStrengthInputField.text = floatValues[6].ToString();
            VerticalThrottleToggle.isOn = boolValues[3];
            JoystickPushPullPitchToggle.isOn = boolValues[4];
            JoystickPushPullDistanceInputField.text = floatValues[7].ToString();
            AfterburnerThrustMultiInputField.text = floatValues[8].ToString();
            AccelerationResponseInputField.text = floatValues[9].ToString();
            EngineSpoolDownSpeedMultiInputField.text = floatValues[10].ToString();
            AirFrictionInputField.text = floatValues[11].ToString();
            PitchStrengthInputField.text = floatValues[12].ToString();
            PitchThrustVecMultiInputField.text = floatValues[13].ToString();
            PitchFrictionInputField.text = floatValues[14].ToString();
            PitchConstantFrictionInputField.text = floatValues[15].ToString();
            PitchResponseInputField.text = floatValues[16].ToString();
            ReversingPitchStrengthMultiInputField.text = floatValues[17].ToString();
            YawStrengthInputField.text = floatValues[18].ToString();
            YawThrustVecMultiInputField.text = floatValues[19].ToString();
            YawFrictionInputField.text = floatValues[20].ToString();
            YawConstantFrictionInputField.text = floatValues[21].ToString();
            YawResponseInputField.text = floatValues[22].ToString();
            ReversingYawStrengthMultiInputField.text = floatValues[23].ToString();
            RollStrengthInputField.text = floatValues[24].ToString();
            RollThrustVecMultiInputField.text = floatValues[25].ToString();
            RollFrictionInputField.text = floatValues[26].ToString();
            RollConstantFrictionInputField.text = floatValues[27].ToString();
            RollResponseInputField.text = floatValues[28].ToString();
            ReversingRollStrengthMultiInputField.text = floatValues[29].ToString();
            PitchDownStrMultiInputField.text = floatValues[30].ToString();
            PitchDownLiftMultiInputField.text = floatValues[31].ToString();
            InertiaTensorRotationMultiInputField.text = floatValues[32].ToString();
            InvertITRYawToggle.isOn = boolValues[5];
            AdverseYawInputField.text = floatValues[33].ToString();
            AdverseRollInputField.text = floatValues[34].ToString();
            RotMultiMaxSpeedInputField.text = floatValues[35].ToString();
            VelStraightenStrPitchInputField.text = floatValues[36].ToString();
            VelStraightenStrYawInputField.text = floatValues[37].ToString();
            MaxAngleOfAttackPitchInputField.text = floatValues[38].ToString();
            MaxAngleOfAttackYawInputField.text = floatValues[39].ToString();
            AoaCurveStrengthInputField.text = floatValues[40].ToString();
            HighPitchAoaMinControlInputField.text = floatValues[41].ToString();
            HighYawAoaMinControlInputField.text = floatValues[42].ToString();
            DoStallForcesToggle.isOn = boolValues[6];
            PitchAoaPitchForceMultiInputField.text = floatValues[43].ToString();
            YawAoaRollForceMultiInputField.text = floatValues[44].ToString();
            HighPitchAoaMinLiftInputField.text = floatValues[45].ToString();
            HighYawAoaMinLiftInputField.text = floatValues[46].ToString();
            ZeroLiftAoAInputField.text = floatValues[47].ToString();
            TaxiRotationSpeedInputField.text = floatValues[48].ToString();
            TaxiRotationResponseInputField.text = floatValues[49].ToString();
            DisallowTaxiRotationWhileStillToggle.isOn = boolValues[7];
            TaxiFullTurningSpeedInputField.text = floatValues[50].ToString();
            WheelSuspensionToggle.isOn = boolValues[8];
            LiftInputField.text = floatValues[51].ToString();
            SidewaysLiftInputField.text = floatValues[52].ToString();
            MaxLiftInputField.text = floatValues[53].ToString();
            VelLiftInputField.text = floatValues[54].ToString();
            VelLiftMaxInputField.text = floatValues[55].ToString();
            MaxGsInputField.text = floatValues[56].ToString();
            GDamageInputField.text = floatValues[57].ToString();
            Crash_Damage_SpeedInputField.text = floatValues[58].ToString();
            Crash_Death_SpeedInputField.text = floatValues[59].ToString();
            GroundEffectMaxDistanceInputField.text = floatValues[60].ToString();
            GroundEffectStrengthInputField.text = floatValues[61].ToString();
            GroundEffectLiftMaxInputField.text = floatValues[62].ToString();
            VTOLAngleTurnRateInputField.text = floatValues[63].ToString();
            VTOLDefaultValueInputField.text = floatValues[64].ToString();
            VTOLAllowAfterburnerToggle.isOn = boolValues[9];
            VTOLThrottleStrengthMultiInputField.text = floatValues[65].ToString();
            VTOLMinAngleInputField.text = floatValues[66].ToString();
            VTOLMaxAngleInputField.text = floatValues[67].ToString();
            VTOLPitchThrustVecMultiInputField.text = floatValues[68].ToString();
            VTOLYawThrustVecMultiInputField.text = floatValues[69].ToString();
            VTOLRollThrustVecMultiInputField.text = floatValues[70].ToString();
            VTOLLoseControlSpeedInputField.text = floatValues[71].ToString();
            VTOLGroundEffectStrengthInputField.text = floatValues[72].ToString();
            EnterVTOLEvent_AngleInputField.text = floatValues[73].ToString();
            ReverseThrustAllowAfterburnerToggle.isOn = boolValues[10];
            SoundBarrierStrengthInputField.text = floatValues[77].ToString();
            SoundBarrierWidthInputField.text = floatValues[78].ToString();
            FuelInputField.text = floatValues[79].ToString();
            LowFuelInputField.text = floatValues[80].ToString();
            FuelConsumptionInputField.text = floatValues[81].ToString();
            MinFuelConsumptionInputField.text = floatValues[82].ToString();
            FuelConsumptionABMultiInputField.text = floatValues[83].ToString();
            RefuelTimeInputField.text = floatValues[84].ToString();
            RepairTimeInputField.text = floatValues[85].ToString();
            RespawnDelayInputField.text = floatValues[86].ToString();
            InvincibleAfterSpawnInputField.text = floatValues[87].ToString();
            BulletDamageTakenInputField.text = floatValues[88].ToString();
            PredictDamageToggle.isOn = boolValues[11];
            SmallCrashSpeedInputField.text = floatValues[89].ToString();
            MediumCrashSpeedInputField.text = floatValues[90].ToString();
            BigCrashSpeedInputField.text = floatValues[91].ToString();
            MissileDamageTakenMultiplierInputField.text = floatValues[92].ToString();
            MissilePushForceInputField.text = floatValues[93].ToString();
            SeaLevelInputField.text = floatValues[94].ToString();
            AtmosphereThinningStartInputField.text = floatValues[95].ToString();
            AtmosphereThinningEndInputField.text = floatValues[96].ToString();
            SquareJoyInputToggle.isOn = boolValues[12];
            DroneModeToggle.isOn = boolValues[13];
        }

        private void InputsToSyncedArrays()
        {
            _syncedFloatValues[0] = ToFloat(MassInputField);
            _syncedFloatValues[1] = ToFloat(GroundDetectorRayDistanceInputField);
            _syncedFloatValues[2] = ToFloat(HealthInputField);
            _syncedFloatValues[3] = ToFloat(ExplodeHealthInputField);
            _syncedBoolValues[0] = ToBool(RepeatingWorldToggle);
            _syncedFloatValues[4] = ToFloat(RepeatingWorldDistanceInputField);
            _syncedBoolValues[1] = ToBool(HasAfterburnerToggle);
            _syncedFloatValues[5] = ToFloat(ThrottleAfterburnerPointInputField);
            _syncedBoolValues[2] = ToBool(VTOLOnlyToggle);
            _syncedFloatValues[6] = ToFloat(ThrottleStrengthInputField);
            _syncedBoolValues[3] = ToBool(VerticalThrottleToggle);
            _syncedBoolValues[4] = ToBool(JoystickPushPullPitchToggle);
            _syncedFloatValues[7] = ToFloat(JoystickPushPullDistanceInputField);
            _syncedFloatValues[8] = ToFloat(AfterburnerThrustMultiInputField);
            _syncedFloatValues[9] = ToFloat(AccelerationResponseInputField);
            _syncedFloatValues[10] = ToFloat(EngineSpoolDownSpeedMultiInputField);
            _syncedFloatValues[11] = ToFloat(AirFrictionInputField);
            _syncedFloatValues[12] = ToFloat(PitchStrengthInputField);
            _syncedFloatValues[13] = ToFloat(PitchThrustVecMultiInputField);
            _syncedFloatValues[14] = ToFloat(PitchFrictionInputField);
            _syncedFloatValues[15] = ToFloat(PitchConstantFrictionInputField);
            _syncedFloatValues[16] = ToFloat(PitchResponseInputField);
            _syncedFloatValues[17] = ToFloat(ReversingPitchStrengthMultiInputField);
            _syncedFloatValues[18] = ToFloat(YawStrengthInputField);
            _syncedFloatValues[19] = ToFloat(YawThrustVecMultiInputField);
            _syncedFloatValues[20] = ToFloat(YawFrictionInputField);
            _syncedFloatValues[21] = ToFloat(YawConstantFrictionInputField);
            _syncedFloatValues[22] = ToFloat(YawResponseInputField);
            _syncedFloatValues[23] = ToFloat(ReversingYawStrengthMultiInputField);
            _syncedFloatValues[24] = ToFloat(RollStrengthInputField);
            _syncedFloatValues[25] = ToFloat(RollThrustVecMultiInputField);
            _syncedFloatValues[26] = ToFloat(RollFrictionInputField);
            _syncedFloatValues[27] = ToFloat(RollConstantFrictionInputField);
            _syncedFloatValues[28] = ToFloat(RollResponseInputField);
            _syncedFloatValues[29] = ToFloat(ReversingRollStrengthMultiInputField);
            _syncedFloatValues[30] = ToFloat(PitchDownStrMultiInputField);
            _syncedFloatValues[31] = ToFloat(PitchDownLiftMultiInputField);
            _syncedFloatValues[32] = ToFloat(InertiaTensorRotationMultiInputField);
            _syncedBoolValues[5] = ToBool(InvertITRYawToggle);
            _syncedFloatValues[33] = ToFloat(AdverseYawInputField);
            _syncedFloatValues[34] = ToFloat(AdverseRollInputField);
            _syncedFloatValues[35] = ToFloat(RotMultiMaxSpeedInputField);
            _syncedFloatValues[36] = ToFloat(VelStraightenStrPitchInputField);
            _syncedFloatValues[37] = ToFloat(VelStraightenStrYawInputField);
            _syncedFloatValues[38] = ToFloat(MaxAngleOfAttackPitchInputField);
            _syncedFloatValues[39] = ToFloat(MaxAngleOfAttackYawInputField);
            _syncedFloatValues[40] = ToFloat(AoaCurveStrengthInputField);
            _syncedFloatValues[41] = ToFloat(HighPitchAoaMinControlInputField);
            _syncedFloatValues[42] = ToFloat(HighYawAoaMinControlInputField);
            _syncedBoolValues[6] = ToBool(DoStallForcesToggle);
            _syncedFloatValues[43] = ToFloat(PitchAoaPitchForceMultiInputField);
            _syncedFloatValues[44] = ToFloat(YawAoaRollForceMultiInputField);
            _syncedFloatValues[45] = ToFloat(HighPitchAoaMinLiftInputField);
            _syncedFloatValues[46] = ToFloat(HighYawAoaMinLiftInputField);
            _syncedFloatValues[47] = ToFloat(ZeroLiftAoAInputField);
            _syncedFloatValues[48] = ToFloat(TaxiRotationSpeedInputField);
            _syncedFloatValues[49] = ToFloat(TaxiRotationResponseInputField);
            _syncedBoolValues[7] = ToBool(DisallowTaxiRotationWhileStillToggle);
            _syncedFloatValues[50] = ToFloat(TaxiFullTurningSpeedInputField);
            _syncedBoolValues[8] = ToBool(WheelSuspensionToggle);
            _syncedFloatValues[51] = ToFloat(LiftInputField);
            _syncedFloatValues[52] = ToFloat(SidewaysLiftInputField);
            _syncedFloatValues[53] = ToFloat(MaxLiftInputField);
            _syncedFloatValues[54] = ToFloat(VelLiftInputField);
            _syncedFloatValues[55] = ToFloat(VelLiftMaxInputField);
            _syncedFloatValues[56] = ToFloat(MaxGsInputField);
            _syncedFloatValues[57] = ToFloat(GDamageInputField);
            _syncedFloatValues[58] = ToFloat(Crash_Damage_SpeedInputField);
            _syncedFloatValues[59] = ToFloat(Crash_Death_SpeedInputField);
            _syncedFloatValues[60] = ToFloat(GroundEffectMaxDistanceInputField);
            _syncedFloatValues[61] = ToFloat(GroundEffectStrengthInputField);
            _syncedFloatValues[62] = ToFloat(GroundEffectLiftMaxInputField);
            _syncedFloatValues[63] = ToFloat(VTOLAngleTurnRateInputField);
            _syncedFloatValues[64] = ToFloat(VTOLDefaultValueInputField);
            _syncedBoolValues[9] = ToBool(VTOLAllowAfterburnerToggle);
            _syncedFloatValues[65] = ToFloat(VTOLThrottleStrengthMultiInputField);
            _syncedFloatValues[66] = ToFloat(VTOLMinAngleInputField);
            _syncedFloatValues[67] = ToFloat(VTOLMaxAngleInputField);
            _syncedFloatValues[68] = ToFloat(VTOLPitchThrustVecMultiInputField);
            _syncedFloatValues[69] = ToFloat(VTOLYawThrustVecMultiInputField);
            _syncedFloatValues[70] = ToFloat(VTOLRollThrustVecMultiInputField);
            _syncedFloatValues[71] = ToFloat(VTOLLoseControlSpeedInputField);
            _syncedFloatValues[72] = ToFloat(VTOLGroundEffectStrengthInputField);
            _syncedFloatValues[73] = ToFloat(EnterVTOLEvent_AngleInputField);
            _syncedBoolValues[10] = ToBool(ReverseThrustAllowAfterburnerToggle);
            _syncedFloatValues[77] = ToFloat(SoundBarrierStrengthInputField);
            _syncedFloatValues[78] = ToFloat(SoundBarrierWidthInputField);
            _syncedFloatValues[79] = ToFloat(FuelInputField);
            _syncedFloatValues[80] = ToFloat(LowFuelInputField);
            _syncedFloatValues[81] = ToFloat(FuelConsumptionInputField);
            _syncedFloatValues[82] = ToFloat(MinFuelConsumptionInputField);
            _syncedFloatValues[83] = ToFloat(FuelConsumptionABMultiInputField);
            _syncedFloatValues[84] = ToFloat(RefuelTimeInputField);
            _syncedFloatValues[85] = ToFloat(RepairTimeInputField);
            _syncedFloatValues[86] = ToFloat(RespawnDelayInputField);
            _syncedFloatValues[87] = ToFloat(InvincibleAfterSpawnInputField);
            _syncedFloatValues[88] = ToFloat(BulletDamageTakenInputField);
            _syncedBoolValues[11] = ToBool(PredictDamageToggle);
            _syncedFloatValues[89] = ToFloat(SmallCrashSpeedInputField);
            _syncedFloatValues[90] = ToFloat(MediumCrashSpeedInputField);
            _syncedFloatValues[91] = ToFloat(BigCrashSpeedInputField);
            _syncedFloatValues[92] = ToFloat(MissileDamageTakenMultiplierInputField);
            _syncedFloatValues[93] = ToFloat(MissilePushForceInputField);
            _syncedFloatValues[94] = ToFloat(SeaLevelInputField);
            _syncedFloatValues[95] = ToFloat(AtmosphereThinningStartInputField);
            _syncedFloatValues[96] = ToFloat(AtmosphereThinningEndInputField);
            _syncedBoolValues[12] = ToBool(SquareJoyInputToggle);
            _syncedBoolValues[13] = ToBool(DroneModeToggle);
        }

        private void ArraysToVehicleValues(float[] floatValues, bool[] boolValues)
        {
            VehicleRigidbody.mass = floatValues[0];

            Vehicle.SetProgramVariable("GroundDetectorRayDistance", floatValues[1]);
            Vehicle.SetProgramVariable("Health", floatValues[2]);
            Vehicle.SetProgramVariable("ExplodeHealth", floatValues[3]);
            Vehicle.SetProgramVariable("RepeatingWorld", boolValues[0]);
            Vehicle.SetProgramVariable("RepeatingWorldDistance", floatValues[4]);
            Vehicle.SetProgramVariable("HasAfterburner", boolValues[1]);
            Vehicle.SetProgramVariable("ThrottleAfterburnerPoint", floatValues[5]);
            Vehicle.SetProgramVariable("VTOLOnly", boolValues[2]);
            Vehicle.SetProgramVariable("ThrottleStrength", floatValues[6]);
            Vehicle.SetProgramVariable("VerticalThrottle", boolValues[3]);
            Vehicle.SetProgramVariable("JoystickPushPullPitch", boolValues[4]);
            Vehicle.SetProgramVariable("JoystickPushPullDistance", floatValues[7]);
            Vehicle.SetProgramVariable("AfterburnerThrustMulti", floatValues[8]);
            Vehicle.SetProgramVariable("AccelerationResponse", floatValues[9]);
            Vehicle.SetProgramVariable("EngineSpoolDownSpeedMulti", floatValues[10]);
            Vehicle.SetProgramVariable("AirFriction", floatValues[11]);
            Vehicle.SetProgramVariable("PitchStrength", floatValues[12]);
            Vehicle.SetProgramVariable("PitchThrustVecMulti", floatValues[13]);
            Vehicle.SetProgramVariable("PitchFriction", floatValues[14]);
            Vehicle.SetProgramVariable("PitchConstantFriction", floatValues[15]);
            Vehicle.SetProgramVariable("PitchResponse", floatValues[16]);
            Vehicle.SetProgramVariable("ReversingPitchStrengthMulti", floatValues[17]);
            Vehicle.SetProgramVariable("YawStrength", floatValues[18]);
            Vehicle.SetProgramVariable("YawThrustVecMulti", floatValues[19]);
            Vehicle.SetProgramVariable("YawFriction", floatValues[20]);
            Vehicle.SetProgramVariable("YawConstantFriction", floatValues[21]);
            Vehicle.SetProgramVariable("YawResponse", floatValues[22]);
            Vehicle.SetProgramVariable("ReversingYawStrengthMulti", floatValues[23]);
            Vehicle.SetProgramVariable("RollStrength", floatValues[24]);
            Vehicle.SetProgramVariable("RollThrustVecMulti", floatValues[25]);
            Vehicle.SetProgramVariable("RollFriction", floatValues[26]);
            Vehicle.SetProgramVariable("RollConstantFriction", floatValues[27]);
            Vehicle.SetProgramVariable("RollResponse", floatValues[28]);
            Vehicle.SetProgramVariable("ReversingRollStrengthMulti", floatValues[29]);
            Vehicle.SetProgramVariable("PitchDownStrMulti", floatValues[30]);
            Vehicle.SetProgramVariable("PitchDownLiftMulti", floatValues[31]);
            Vehicle.SetProgramVariable("InertiaTensorRotationMulti", floatValues[32]);
            Vehicle.SetProgramVariable("InvertITRYaw", boolValues[5]);
            Vehicle.SetProgramVariable("AdverseYaw", floatValues[33]);
            Vehicle.SetProgramVariable("AdverseRoll", floatValues[34]);
            Vehicle.SetProgramVariable("RotMultiMaxSpeed", floatValues[35]);
            Vehicle.SetProgramVariable("VelStraightenStrPitch", floatValues[36]);
            Vehicle.SetProgramVariable("VelStraightenStrYaw", floatValues[37]);
            Vehicle.SetProgramVariable("MaxAngleOfAttackPitch", floatValues[38]);
            Vehicle.SetProgramVariable("MaxAngleOfAttackYaw", floatValues[39]);
            Vehicle.SetProgramVariable("AoaCurveStrength", floatValues[40]);
            Vehicle.SetProgramVariable("HighPitchAoaMinControl", floatValues[41]);
            Vehicle.SetProgramVariable("HighYawAoaMinControl", floatValues[42]);
            Vehicle.SetProgramVariable("DoStallForces", boolValues[6]);
            Vehicle.SetProgramVariable("PitchAoaPitchForceMulti", floatValues[43]);
            Vehicle.SetProgramVariable("YawAoaRollForceMulti", floatValues[44]);
            Vehicle.SetProgramVariable("HighPitchAoaMinLift", floatValues[45]);
            Vehicle.SetProgramVariable("HighYawAoaMinLift", floatValues[46]);
            Vehicle.SetProgramVariable("ZeroLiftAoA", floatValues[47]);
            Vehicle.SetProgramVariable("TaxiRotationSpeed", floatValues[48]);
            Vehicle.SetProgramVariable("TaxiRotationResponse", floatValues[49]);
            Vehicle.SetProgramVariable("DisallowTaxiRotationWhileStill", boolValues[7]);
            Vehicle.SetProgramVariable("TaxiFullTurningSpeed", floatValues[50]);
            Vehicle.SetProgramVariable("WheelSuspension", boolValues[8]);
            Vehicle.SetProgramVariable("Lift", floatValues[51]);
            Vehicle.SetProgramVariable("SidewaysLift", floatValues[52]);
            Vehicle.SetProgramVariable("MaxLift", floatValues[53]);
            Vehicle.SetProgramVariable("VelLift", floatValues[54]);
            Vehicle.SetProgramVariable("VelLiftMax", floatValues[55]);
            Vehicle.SetProgramVariable("MaxGs", floatValues[56]);
            Vehicle.SetProgramVariable("GDamage", floatValues[57]);
            Vehicle.SetProgramVariable("Crash_Damage_Speed", floatValues[58]);
            Vehicle.SetProgramVariable("Crash_Death_Speed", floatValues[59]);
            Vehicle.SetProgramVariable("GroundEffectMaxDistance", floatValues[60]);
            Vehicle.SetProgramVariable("GroundEffectStrength", floatValues[61]);
            Vehicle.SetProgramVariable("GroundEffectLiftMax", floatValues[62]);
            Vehicle.SetProgramVariable("VTOLAngleTurnRate", floatValues[63]);
            Vehicle.SetProgramVariable("VTOLDefaultValue", floatValues[64]);
            Vehicle.SetProgramVariable("VTOLAllowAfterburner", boolValues[9]);
            Vehicle.SetProgramVariable("VTOLThrottleStrengthMulti", floatValues[65]);
            Vehicle.SetProgramVariable("VTOLMinAngle", floatValues[66]);
            Vehicle.SetProgramVariable("VTOLMaxAngle", floatValues[67]);
            Vehicle.SetProgramVariable("VTOLPitchThrustVecMulti", floatValues[68]);
            Vehicle.SetProgramVariable("VTOLYawThrustVecMulti", floatValues[69]);
            Vehicle.SetProgramVariable("VTOLRollThrustVecMulti", floatValues[70]);
            Vehicle.SetProgramVariable("VTOLLoseControlSpeed", floatValues[71]);
            Vehicle.SetProgramVariable("VTOLGroundEffectStrength", floatValues[72]);
            Vehicle.SetProgramVariable("EnterVTOLEvent_Angle", floatValues[73]);
            Vehicle.SetProgramVariable("ReverseThrustAllowAfterburner", boolValues[10]);
            Vehicle.SetProgramVariable("SoundBarrierStrength", floatValues[77]);
            Vehicle.SetProgramVariable("SoundBarrierWidth", floatValues[78]);
            Vehicle.SetProgramVariable("Fuel", floatValues[79]);
            Vehicle.SetProgramVariable("LowFuel", floatValues[80]);
            Vehicle.SetProgramVariable("FuelConsumption", floatValues[81]);
            Vehicle.SetProgramVariable("MinFuelConsumption", floatValues[82]);
            Vehicle.SetProgramVariable("FuelConsumptionABMulti", floatValues[83]);
            Vehicle.SetProgramVariable("RefuelTime", floatValues[84]);
            Vehicle.SetProgramVariable("RepairTime", floatValues[85]);
            Vehicle.SetProgramVariable("RespawnDelay", floatValues[86]);
            Vehicle.SetProgramVariable("InvincibleAfterSpawn", floatValues[87]);
            Vehicle.SetProgramVariable("BulletDamageTaken", floatValues[88]);
            Vehicle.SetProgramVariable("PredictDamage", boolValues[11]);
            Vehicle.SetProgramVariable("SmallCrashSpeed", floatValues[89]);
            Vehicle.SetProgramVariable("MediumCrashSpeed", floatValues[90]);
            Vehicle.SetProgramVariable("BigCrashSpeed", floatValues[91]);
            Vehicle.SetProgramVariable("MissileDamageTakenMultiplier", floatValues[92]);
            Vehicle.SetProgramVariable("MissilePushForce", floatValues[93]);
            Vehicle.SetProgramVariable("SeaLevel", floatValues[94]);
            Vehicle.SetProgramVariable("AtmosphereThinningStart", floatValues[95]);
            Vehicle.SetProgramVariable("AtmosphereThinningEnd", floatValues[96]);
            Vehicle.SetProgramVariable("SquareJoyInput", boolValues[12]);
            Vehicle.SetProgramVariable("DroneMode", boolValues[13]);

            ReAdjustVehicleValues();
            ShowActualValues();
        }

        private void VehicleValuesToDefaultArrays()
        {
            float rbMass = VehicleRigidbody.mass;
            float adjust = 1; //AutoAdjustValuesToMassの名残

            _defaultFloatValues[0] = rbMass;

            _defaultFloatValues[1] = (float)Vehicle.GetProgramVariable("GroundDetectorRayDistance");
            _defaultFloatValues[2] = (float)Vehicle.GetProgramVariable("Health");
            _defaultFloatValues[3] = (float)Vehicle.GetProgramVariable("ExplodeHealth");
            _defaultBoolValues[0] = (bool)Vehicle.GetProgramVariable("RepeatingWorld");
            _defaultFloatValues[4] = (float)Vehicle.GetProgramVariable("RepeatingWorldDistance");
            _defaultBoolValues[1] = (bool)Vehicle.GetProgramVariable("HasAfterburner");
            _defaultFloatValues[5] = (float)Vehicle.GetProgramVariable("ThrottleAfterburnerPoint");
            _defaultBoolValues[2] = (bool)Vehicle.GetProgramVariable("VTOLOnly");
            _defaultFloatValues[6] = (float)Vehicle.GetProgramVariable("ThrottleStrength") / adjust;
            _defaultBoolValues[3] = (bool)Vehicle.GetProgramVariable("VerticalThrottle");
            _defaultBoolValues[4] = (bool)Vehicle.GetProgramVariable("JoystickPushPullPitch");
            _defaultFloatValues[7] = (float)Vehicle.GetProgramVariable("JoystickPushPullDistance");
            _defaultFloatValues[8] = (float)Vehicle.GetProgramVariable("AfterburnerThrustMulti");
            _defaultFloatValues[9] = (float)Vehicle.GetProgramVariable("AccelerationResponse");
            _defaultFloatValues[10] = (float)Vehicle.GetProgramVariable("EngineSpoolDownSpeedMulti");
            _defaultFloatValues[11] = (float)Vehicle.GetProgramVariable("AirFriction");
            _defaultFloatValues[12] = (float)Vehicle.GetProgramVariable("PitchStrength") / adjust;
            _defaultFloatValues[13] = (float)Vehicle.GetProgramVariable("PitchThrustVecMulti");
            _defaultFloatValues[14] = (float)Vehicle.GetProgramVariable("PitchFriction") / adjust;
            _defaultFloatValues[15] = (float)Vehicle.GetProgramVariable("PitchConstantFriction") / adjust;
            _defaultFloatValues[16] = (float)Vehicle.GetProgramVariable("PitchResponse");
            _defaultFloatValues[17] = (float)Vehicle.GetProgramVariable("ReversingPitchStrengthMulti");
            _defaultFloatValues[18] = (float)Vehicle.GetProgramVariable("YawStrength") / adjust;
            _defaultFloatValues[19] = (float)Vehicle.GetProgramVariable("YawThrustVecMulti");
            _defaultFloatValues[20] = (float)Vehicle.GetProgramVariable("YawFriction") / adjust;
            _defaultFloatValues[21] = (float)Vehicle.GetProgramVariable("YawConstantFriction") / adjust;
            _defaultFloatValues[22] = (float)Vehicle.GetProgramVariable("YawResponse");
            _defaultFloatValues[23] = (float)Vehicle.GetProgramVariable("ReversingYawStrengthMulti");
            _defaultFloatValues[24] = (float)Vehicle.GetProgramVariable("RollStrength") / adjust;
            _defaultFloatValues[25] = (float)Vehicle.GetProgramVariable("RollThrustVecMulti");
            _defaultFloatValues[26] = (float)Vehicle.GetProgramVariable("RollFriction") / adjust;
            _defaultFloatValues[27] = (float)Vehicle.GetProgramVariable("RollConstantFriction") / adjust;
            _defaultFloatValues[28] = (float)Vehicle.GetProgramVariable("RollResponse");
            _defaultFloatValues[29] = (float)Vehicle.GetProgramVariable("ReversingRollStrengthMulti");
            _defaultFloatValues[30] = (float)Vehicle.GetProgramVariable("PitchDownStrMulti");
            _defaultFloatValues[31] = (float)Vehicle.GetProgramVariable("PitchDownLiftMulti");
            _defaultFloatValues[32] = (float)Vehicle.GetProgramVariable("InertiaTensorRotationMulti");
            _defaultBoolValues[5] = (bool)Vehicle.GetProgramVariable("InvertITRYaw");
            _defaultFloatValues[33] = (float)Vehicle.GetProgramVariable("AdverseYaw") / adjust;
            _defaultFloatValues[34] = (float)Vehicle.GetProgramVariable("AdverseRoll") / adjust;
            _defaultFloatValues[35] = (float)Vehicle.GetProgramVariable("RotMultiMaxSpeed");
            _defaultFloatValues[36] = (float)Vehicle.GetProgramVariable("VelStraightenStrPitch") / adjust;
            _defaultFloatValues[37] = (float)Vehicle.GetProgramVariable("VelStraightenStrYaw") / adjust;
            _defaultFloatValues[38] = (float)Vehicle.GetProgramVariable("MaxAngleOfAttackPitch");
            _defaultFloatValues[39] = (float)Vehicle.GetProgramVariable("MaxAngleOfAttackYaw");
            _defaultFloatValues[40] = (float)Vehicle.GetProgramVariable("AoaCurveStrength");
            _defaultFloatValues[41] = (float)Vehicle.GetProgramVariable("HighPitchAoaMinControl");
            _defaultFloatValues[42] = (float)Vehicle.GetProgramVariable("HighYawAoaMinControl");
            _defaultBoolValues[6] = (bool)Vehicle.GetProgramVariable("DoStallForces");
            _defaultFloatValues[43] = (float)Vehicle.GetProgramVariable("PitchAoaPitchForceMulti") / adjust;
            _defaultFloatValues[44] = (float)Vehicle.GetProgramVariable("YawAoaRollForceMulti") / adjust;
            _defaultFloatValues[45] = (float)Vehicle.GetProgramVariable("HighPitchAoaMinLift");
            _defaultFloatValues[46] = (float)Vehicle.GetProgramVariable("HighYawAoaMinLift");
            _defaultFloatValues[47] = (float)Vehicle.GetProgramVariable("ZeroLiftAoA");
            _defaultFloatValues[48] = (float)Vehicle.GetProgramVariable("TaxiRotationSpeed");
            _defaultFloatValues[49] = (float)Vehicle.GetProgramVariable("TaxiRotationResponse");
            _defaultBoolValues[7] = (bool)Vehicle.GetProgramVariable("DisallowTaxiRotationWhileStill");
            _defaultFloatValues[50] = (float)Vehicle.GetProgramVariable("TaxiFullTurningSpeed");
            _defaultBoolValues[8] = (bool)Vehicle.GetProgramVariable("WheelSuspension");
            _defaultFloatValues[51] = (float)Vehicle.GetProgramVariable("Lift") / adjust;
            _defaultFloatValues[52] = (float)Vehicle.GetProgramVariable("SidewaysLift");
            _defaultFloatValues[53] = (float)Vehicle.GetProgramVariable("MaxLift") / adjust;
            _defaultFloatValues[54] = (float)Vehicle.GetProgramVariable("VelLift");
            _defaultFloatValues[55] = (float)Vehicle.GetProgramVariable("VelLiftMax") / adjust;
            _defaultFloatValues[56] = (float)Vehicle.GetProgramVariable("MaxGs");
            _defaultFloatValues[57] = (float)Vehicle.GetProgramVariable("GDamage");
            _defaultFloatValues[58] = (float)Vehicle.GetProgramVariable("Crash_Damage_Speed");
            _defaultFloatValues[59] = (float)Vehicle.GetProgramVariable("Crash_Death_Speed");
            _defaultFloatValues[60] = (float)Vehicle.GetProgramVariable("GroundEffectMaxDistance");
            _defaultFloatValues[61] = (float)Vehicle.GetProgramVariable("GroundEffectStrength");
            _defaultFloatValues[62] = (float)Vehicle.GetProgramVariable("GroundEffectLiftMax") / adjust;
            _defaultFloatValues[63] = (float)Vehicle.GetProgramVariable("VTOLAngleTurnRate");
            _defaultFloatValues[64] = (float)Vehicle.GetProgramVariable("VTOLDefaultValue");
            _defaultBoolValues[9] = (bool)Vehicle.GetProgramVariable("VTOLAllowAfterburner");
            _defaultFloatValues[65] = (float)Vehicle.GetProgramVariable("VTOLThrottleStrengthMulti");
            _defaultFloatValues[66] = (float)Vehicle.GetProgramVariable("VTOLMinAngle");
            _defaultFloatValues[67] = (float)Vehicle.GetProgramVariable("VTOLMaxAngle");
            _defaultFloatValues[68] = (float)Vehicle.GetProgramVariable("VTOLPitchThrustVecMulti");
            _defaultFloatValues[69] = (float)Vehicle.GetProgramVariable("VTOLYawThrustVecMulti");
            _defaultFloatValues[70] = (float)Vehicle.GetProgramVariable("VTOLRollThrustVecMulti");
            _defaultFloatValues[71] = (float)Vehicle.GetProgramVariable("VTOLLoseControlSpeed");
            _defaultFloatValues[72] = (float)Vehicle.GetProgramVariable("VTOLGroundEffectStrength");
            _defaultFloatValues[73] = (float)Vehicle.GetProgramVariable("EnterVTOLEvent_Angle");
            _defaultBoolValues[10] = (bool)Vehicle.GetProgramVariable("ReverseThrustAllowAfterburner");
            _defaultFloatValues[77] = (float)Vehicle.GetProgramVariable("SoundBarrierStrength");
            _defaultFloatValues[78] = (float)Vehicle.GetProgramVariable("SoundBarrierWidth");
            _defaultFloatValues[79] = (float)Vehicle.GetProgramVariable("Fuel");
            _defaultFloatValues[80] = (float)Vehicle.GetProgramVariable("LowFuel");
            _defaultFloatValues[81] = (float)Vehicle.GetProgramVariable("FuelConsumption");
            _defaultFloatValues[82] = (float)Vehicle.GetProgramVariable("MinFuelConsumption");
            _defaultFloatValues[83] = (float)Vehicle.GetProgramVariable("FuelConsumptionABMulti");
            _defaultFloatValues[84] = (float)Vehicle.GetProgramVariable("RefuelTime");
            _defaultFloatValues[85] = (float)Vehicle.GetProgramVariable("RepairTime");
            _defaultFloatValues[86] = (float)Vehicle.GetProgramVariable("RespawnDelay");
            _defaultFloatValues[87] = (float)Vehicle.GetProgramVariable("InvincibleAfterSpawn");
            _defaultFloatValues[88] = (float)Vehicle.GetProgramVariable("BulletDamageTaken");
            _defaultBoolValues[11] = (bool)Vehicle.GetProgramVariable("PredictDamage");
            _defaultFloatValues[89] = (float)Vehicle.GetProgramVariable("SmallCrashSpeed");
            _defaultFloatValues[90] = (float)Vehicle.GetProgramVariable("MediumCrashSpeed");
            _defaultFloatValues[91] = (float)Vehicle.GetProgramVariable("BigCrashSpeed");
            _defaultFloatValues[92] = (float)Vehicle.GetProgramVariable("MissileDamageTakenMultiplier");
            _defaultFloatValues[93] = (float)Vehicle.GetProgramVariable("MissilePushForce");
            _defaultFloatValues[94] = (float)Vehicle.GetProgramVariable("SeaLevel");
            _defaultFloatValues[95] = (float)Vehicle.GetProgramVariable("AtmosphereThinningStart");
            _defaultFloatValues[96] = (float)Vehicle.GetProgramVariable("AtmosphereThinningEnd");
            _defaultBoolValues[12] = (bool)Vehicle.GetProgramVariable("SquareJoyInput");
            _defaultBoolValues[13] = (bool)Vehicle.GetProgramVariable("DroneMode");
        }

        private void ShowActualValues()
        {
            MassValueText.text = VehicleRigidbody.mass.ToString();

            GroundDetectorRayDistanceValueText.text = Vehicle.GetProgramVariable("GroundDetectorRayDistance").ToString();
            HealthValueText.text = Vehicle.GetProgramVariable("Health").ToString();
            ExplodeHealthValueText.text = Vehicle.GetProgramVariable("ExplodeHealth").ToString();
            RepeatingWorldValueText.text = Vehicle.GetProgramVariable("RepeatingWorld").ToString();
            RepeatingWorldDistanceValueText.text = Vehicle.GetProgramVariable("RepeatingWorldDistance").ToString();
            HasAfterburnerValueText.text = Vehicle.GetProgramVariable("HasAfterburner").ToString();
            ThrottleAfterburnerPointValueText.text = Vehicle.GetProgramVariable("ThrottleAfterburnerPoint").ToString();
            VTOLOnlyValueText.text = Vehicle.GetProgramVariable("VTOLOnly").ToString();
            ThrottleStrengthValueText.text = Vehicle.GetProgramVariable("ThrottleStrength").ToString();
            VerticalThrottleValueText.text = Vehicle.GetProgramVariable("VerticalThrottle").ToString();
            JoystickPushPullPitchValueText.text = Vehicle.GetProgramVariable("JoystickPushPullPitch").ToString();
            JoystickPushPullDistanceValueText.text = Vehicle.GetProgramVariable("JoystickPushPullDistance").ToString();
            AfterburnerThrustMultiValueText.text = Vehicle.GetProgramVariable("AfterburnerThrustMulti").ToString();
            AccelerationResponseValueText.text = Vehicle.GetProgramVariable("AccelerationResponse").ToString();
            EngineSpoolDownSpeedMultiValueText.text = Vehicle.GetProgramVariable("EngineSpoolDownSpeedMulti").ToString();
            AirFrictionValueText.text = Vehicle.GetProgramVariable("AirFriction").ToString();
            PitchStrengthValueText.text = Vehicle.GetProgramVariable("PitchStrength").ToString();
            PitchThrustVecMultiValueText.text = Vehicle.GetProgramVariable("PitchThrustVecMulti").ToString();
            PitchFrictionValueText.text = Vehicle.GetProgramVariable("PitchFriction").ToString();
            PitchConstantFrictionValueText.text = Vehicle.GetProgramVariable("PitchConstantFriction").ToString();
            PitchResponseValueText.text = Vehicle.GetProgramVariable("PitchResponse").ToString();
            ReversingPitchStrengthMultiValueText.text = Vehicle.GetProgramVariable("ReversingPitchStrengthMulti").ToString();
            YawStrengthValueText.text = Vehicle.GetProgramVariable("YawStrength").ToString();
            YawThrustVecMultiValueText.text = Vehicle.GetProgramVariable("YawThrustVecMulti").ToString();
            YawFrictionValueText.text = Vehicle.GetProgramVariable("YawFriction").ToString();
            YawConstantFrictionValueText.text = Vehicle.GetProgramVariable("YawConstantFriction").ToString();
            YawResponseValueText.text = Vehicle.GetProgramVariable("YawResponse").ToString();
            ReversingYawStrengthMultiValueText.text = Vehicle.GetProgramVariable("ReversingYawStrengthMulti").ToString();
            RollStrengthValueText.text = Vehicle.GetProgramVariable("RollStrength").ToString();
            RollThrustVecMultiValueText.text = Vehicle.GetProgramVariable("RollThrustVecMulti").ToString();
            RollFrictionValueText.text = Vehicle.GetProgramVariable("RollFriction").ToString();
            RollConstantFrictionValueText.text = Vehicle.GetProgramVariable("RollConstantFriction").ToString();
            RollResponseValueText.text = Vehicle.GetProgramVariable("RollResponse").ToString();
            ReversingRollStrengthMultiValueText.text = Vehicle.GetProgramVariable("ReversingRollStrengthMulti").ToString();
            PitchDownStrMultiValueText.text = Vehicle.GetProgramVariable("PitchDownStrMulti").ToString();
            PitchDownLiftMultiValueText.text = Vehicle.GetProgramVariable("PitchDownLiftMulti").ToString();
            InertiaTensorRotationMultiValueText.text = Vehicle.GetProgramVariable("InertiaTensorRotationMulti").ToString();
            InvertITRYawValueText.text = Vehicle.GetProgramVariable("InvertITRYaw").ToString();
            AdverseYawValueText.text = Vehicle.GetProgramVariable("AdverseYaw").ToString();
            AdverseRollValueText.text = Vehicle.GetProgramVariable("AdverseRoll").ToString();
            RotMultiMaxSpeedValueText.text = Vehicle.GetProgramVariable("RotMultiMaxSpeed").ToString();
            VelStraightenStrPitchValueText.text = Vehicle.GetProgramVariable("VelStraightenStrPitch").ToString();
            VelStraightenStrYawValueText.text = Vehicle.GetProgramVariable("VelStraightenStrYaw").ToString();
            MaxAngleOfAttackPitchValueText.text = Vehicle.GetProgramVariable("MaxAngleOfAttackPitch").ToString();
            MaxAngleOfAttackYawValueText.text = Vehicle.GetProgramVariable("MaxAngleOfAttackYaw").ToString();
            AoaCurveStrengthValueText.text = Vehicle.GetProgramVariable("AoaCurveStrength").ToString();
            HighPitchAoaMinControlValueText.text = Vehicle.GetProgramVariable("HighPitchAoaMinControl").ToString();
            HighYawAoaMinControlValueText.text = Vehicle.GetProgramVariable("HighYawAoaMinControl").ToString();
            DoStallForcesValueText.text = Vehicle.GetProgramVariable("DoStallForces").ToString();
            PitchAoaPitchForceMultiValueText.text = Vehicle.GetProgramVariable("PitchAoaPitchForceMulti").ToString();
            YawAoaRollForceMultiValueText.text = Vehicle.GetProgramVariable("YawAoaRollForceMulti").ToString();
            HighPitchAoaMinLiftValueText.text = Vehicle.GetProgramVariable("HighPitchAoaMinLift").ToString();
            HighYawAoaMinLiftValueText.text = Vehicle.GetProgramVariable("HighYawAoaMinLift").ToString();
            ZeroLiftAoAValueText.text = Vehicle.GetProgramVariable("ZeroLiftAoA").ToString();
            TaxiRotationSpeedValueText.text = Vehicle.GetProgramVariable("TaxiRotationSpeed").ToString();
            TaxiRotationResponseValueText.text = Vehicle.GetProgramVariable("TaxiRotationResponse").ToString();
            DisallowTaxiRotationWhileStillValueText.text = Vehicle.GetProgramVariable("DisallowTaxiRotationWhileStill").ToString();
            TaxiFullTurningSpeedValueText.text = Vehicle.GetProgramVariable("TaxiFullTurningSpeed").ToString();
            WheelSuspensionValueText.text = Vehicle.GetProgramVariable("WheelSuspension").ToString();
            LiftValueText.text = Vehicle.GetProgramVariable("Lift").ToString();
            SidewaysLiftValueText.text = Vehicle.GetProgramVariable("SidewaysLift").ToString();
            MaxLiftValueText.text = Vehicle.GetProgramVariable("MaxLift").ToString();
            VelLiftValueText.text = Vehicle.GetProgramVariable("VelLift").ToString();
            VelLiftMaxValueText.text = Vehicle.GetProgramVariable("VelLiftMax").ToString();
            MaxGsValueText.text = Vehicle.GetProgramVariable("MaxGs").ToString();
            GDamageValueText.text = Vehicle.GetProgramVariable("GDamage").ToString();
            Crash_Damage_SpeedValueText.text = Vehicle.GetProgramVariable("Crash_Damage_Speed").ToString();
            Crash_Death_SpeedValueText.text = Vehicle.GetProgramVariable("Crash_Death_Speed").ToString();
            GroundEffectMaxDistanceValueText.text = Vehicle.GetProgramVariable("GroundEffectMaxDistance").ToString();
            GroundEffectStrengthValueText.text = Vehicle.GetProgramVariable("GroundEffectStrength").ToString();
            GroundEffectLiftMaxValueText.text = Vehicle.GetProgramVariable("GroundEffectLiftMax").ToString();
            VTOLAngleTurnRateValueText.text = Vehicle.GetProgramVariable("VTOLAngleTurnRate").ToString();
            VTOLDefaultValueValueText.text = Vehicle.GetProgramVariable("VTOLDefaultValue").ToString();
            VTOLAllowAfterburnerValueText.text = Vehicle.GetProgramVariable("VTOLAllowAfterburner").ToString();
            VTOLThrottleStrengthMultiValueText.text = Vehicle.GetProgramVariable("VTOLThrottleStrengthMulti").ToString();
            VTOLMinAngleValueText.text = Vehicle.GetProgramVariable("VTOLMinAngle").ToString();
            VTOLMaxAngleValueText.text = Vehicle.GetProgramVariable("VTOLMaxAngle").ToString();
            VTOLPitchThrustVecMultiValueText.text = Vehicle.GetProgramVariable("VTOLPitchThrustVecMulti").ToString();
            VTOLYawThrustVecMultiValueText.text = Vehicle.GetProgramVariable("VTOLYawThrustVecMulti").ToString();
            VTOLRollThrustVecMultiValueText.text = Vehicle.GetProgramVariable("VTOLRollThrustVecMulti").ToString();
            VTOLLoseControlSpeedValueText.text = Vehicle.GetProgramVariable("VTOLLoseControlSpeed").ToString();
            VTOLGroundEffectStrengthValueText.text = Vehicle.GetProgramVariable("VTOLGroundEffectStrength").ToString();
            EnterVTOLEvent_AngleValueText.text = Vehicle.GetProgramVariable("EnterVTOLEvent_Angle").ToString();
            ReverseThrustAllowAfterburnerValueText.text = Vehicle.GetProgramVariable("ReverseThrustAllowAfterburner").ToString();
            SoundBarrierStrengthValueText.text = Vehicle.GetProgramVariable("SoundBarrierStrength").ToString();
            SoundBarrierWidthValueText.text = Vehicle.GetProgramVariable("SoundBarrierWidth").ToString();
            FuelValueText.text = Vehicle.GetProgramVariable("Fuel").ToString();
            LowFuelValueText.text = Vehicle.GetProgramVariable("LowFuel").ToString();
            FuelConsumptionValueText.text = Vehicle.GetProgramVariable("FuelConsumption").ToString();
            MinFuelConsumptionValueText.text = Vehicle.GetProgramVariable("MinFuelConsumption").ToString();
            FuelConsumptionABMultiValueText.text = Vehicle.GetProgramVariable("FuelConsumptionABMulti").ToString();
            RefuelTimeValueText.text = Vehicle.GetProgramVariable("RefuelTime").ToString();
            RepairTimeValueText.text = Vehicle.GetProgramVariable("RepairTime").ToString();
            RespawnDelayValueText.text = Vehicle.GetProgramVariable("RespawnDelay").ToString();
            InvincibleAfterSpawnValueText.text = Vehicle.GetProgramVariable("InvincibleAfterSpawn").ToString();
            BulletDamageTakenValueText.text = Vehicle.GetProgramVariable("BulletDamageTaken").ToString();
            PredictDamageValueText.text = Vehicle.GetProgramVariable("PredictDamage").ToString();
            SmallCrashSpeedValueText.text = Vehicle.GetProgramVariable("SmallCrashSpeed").ToString();
            MediumCrashSpeedValueText.text = Vehicle.GetProgramVariable("MediumCrashSpeed").ToString();
            BigCrashSpeedValueText.text = Vehicle.GetProgramVariable("BigCrashSpeed").ToString();
            MissileDamageTakenMultiplierValueText.text = Vehicle.GetProgramVariable("MissileDamageTakenMultiplier").ToString();
            MissilePushForceValueText.text = Vehicle.GetProgramVariable("MissilePushForce").ToString();
            SeaLevelValueText.text = Vehicle.GetProgramVariable("SeaLevel").ToString();
            AtmosphereThinningStartValueText.text = Vehicle.GetProgramVariable("AtmosphereThinningStart").ToString();
            AtmosphereThinningEndValueText.text = Vehicle.GetProgramVariable("AtmosphereThinningEnd").ToString();
            SquareJoyInputValueText.text = Vehicle.GetProgramVariable("SquareJoyInput").ToString();
            DroneModeValueText.text = Vehicle.GetProgramVariable("DroneMode").ToString();
        }

        public void ShowMassTooltip()
        {
            TooltipText.text = "Mass\nMass of rigidbody\n\nRigidbodyの質量";
        }
        public void ShowGroundDetectorRayDistanceTooltip()
        {
            TooltipText.text = "GroundDetectorRayDistance\nDistance traced down from the ground detector's position to see if the ground is there, in order to determine if the vehicle is grounded\n\n機体が接地しているかどうかを判断するために、地面がそこにあるかどうかを確認するために地面検出器の位置から追跡した距離";
        }
        public void ShowHealthTooltip()
        {
            TooltipText.text = "Health\nHP of the vehicle\n\n乗り物のHP";
        }
        public void ShowExplodeHealthTooltip()
        {
            TooltipText.text = "ExplodeHealth\nIf health is lower than this, vehicle instantly explodes\n\n体力がこれより低い場合、乗り物は即座に爆発します";
        }
        public void ShowRepeatingWorldTooltip()
        {
            TooltipText.text = "RepeatingWorld\nTeleport the vehicle to the oposite side of the map when flying too far in one direction?\n\n一方向に遠くまで飛行した場合、機体をマップの反対側にテレポートしますか?";
        }
        public void ShowRepeatingWorldDistanceTooltip()
        {
            TooltipText.text = "RepeatingWorldDistance\nDistance you can travel away from world origin before being teleported to the other side of the map. Not recommended to increase, floating point innacuracy and game freezing issues may occur if larger than default\n\nマップの反対側にテレポートされる前にワールド原点から離れて移動できる距離。増やすことはお勧めしません。デフォルトより大きい場合、浮動小数点の不正確性やゲームのフリーズの問題が発生する可能性があります。";
        }
        public void ShowHasAfterburnerTooltip()
        {
            TooltipText.text = "HasAfterburner\nHas afterburner?\n\nアフターバーナーがあるか";
        }
        public void ShowThrottleAfterburnerPointTooltip()
        {
            TooltipText.text = "ThrottleAfterburnerPoint\nPoint in the throttle at which afterburner enables, .8 = 80%\n[Range(0.0f, 1f)]\n\nアフターバーナーが有効になるスロットルのポイント、.8=80%";
        }
        public void ShowVTOLOnlyTooltip()
        {
            TooltipText.text = "VTOLOnly\nDisable Thrust/VTOL rotation values transition calculations and assume VTOL mode always (for helicopters)\n\n推力/VTOL回転値の遷移計算を無効にし、常にVTOLモードを想定します(ヘリコプターの場合)";
        }
        public void ShowThrottleStrengthTooltip()
        {
            TooltipText.text = "ThrottleStrength\nVehicle thrust at max throttle without afterburner\n\nアフターバーナーなしの最大スロットル時の機体推力";
        }
        public void ShowVerticalThrottleTooltip()
        {
            TooltipText.text = "VerticalThrottle\nMake VR Throttle motion controls use the Y axis instead of the Z axis for adjustment (Helicopter collective)\n\nVRスロットルモーションコントロールで調整にZ軸の代わりにY軸を使用するようにします(ヘリコプターコレクティブ)";
        }
        public void ShowJoystickPushPullPitchTooltip()
        {
            TooltipText.text = "JoystickPushPullPitch\nJoystick pitch input to be a slider-style yoke.\n\nジョイスティックのピッチ入力をスライダー形式のヨークにします。";
        }
        public void ShowJoystickPushPullDistanceTooltip()
        {
            TooltipText.text = "JoystickPushPullDistance\nJoystick sensitivity for above option.\n\n上記オプションのジョイスティックの感度。";
        }
        public void ShowAfterburnerThrustMultiTooltip()
        {
            TooltipText.text = "AfterburnerThrustMulti\nHow much more thrust the vehicle has when in full afterburner\n\nアフターバーナーが完全に点灯したときに機体の推力がどのくらい増加するか";
        }
        public void ShowAccelerationResponseTooltip()
        {
            TooltipText.text = "AccelerationResponse\nHow quickly the vehicle throttles up after throttle is increased (Lerp)\n\nスロットルを上げた後に機体がどれだけ早くスロットルアップするか(Lerp)";
        }
        public void ShowEngineSpoolDownSpeedMultiTooltip()
        {
            TooltipText.text = "EngineSpoolDownSpeedMulti\nHow quickly the vehicle throttles down relative to how fast it throttles up after throttle is decreased\n\nスロットルを下げた後のスロットルアップの速度に対する機体のスロットルダウンの速度";
        }
        public void ShowAirFrictionTooltip()
        {
            TooltipText.text = "AirFriction\nHow much the vehicle slows down (Speed lerped towards 0)\n\n機体の速度がどの程度低下するか(速度が0に向かって徐々に低下する)";
        }
        public void ShowPitchStrengthTooltip()
        {
            TooltipText.text = "PitchStrength\nPitch force multiplier, (gets stronger with airspeed)\n\nピッチ力乗数(対気速度とともに強くなります)";
        }
        public void ShowPitchThrustVecMultiTooltip()
        {
            TooltipText.text = "PitchThrustVecMulti\nPitch rotation force (as multiple of PitchStrength) (doesn't get stronger with airspeed, useful for helicopters and ridiculous jets). Setting this to a nonzero value disables inversion of joystick pitch controls when vehicle is travelling backwards\n[Range(0.0f, 1f)]\n\nピッチ回転力(PitchStrengthの倍数)(対気速度によっては強くなりません。ヘリコプターやばかげたジェット機に役立ちます)。これをゼロ以外の値に設定すると、機体が後進しているときにジョイスティックのピッチ制御の反転が無効になります。";
        }
        public void ShowPitchFrictionTooltip()
        {
            TooltipText.text = "PitchFriction\nForce that stops vehicle from pitching, (gets stronger with airspeed)\n\n機体のピッチングを止める力(対気速度が上がると強くなります)";
        }
        public void ShowPitchConstantFrictionTooltip()
        {
            TooltipText.text = "PitchConstantFriction\nForce that stops vehicle from pitching, (doesn't get stronger with airspeed)\n\n機体のピッチングを止める力(対気速度によっては強くなりません)";
        }
        public void ShowPitchResponseTooltip()
        {
            TooltipText.text = "PitchResponse\nHow quickly the vehicle responds to changes in joystick's pitch (Lerp)\n\nジョイスティックのピッチの変化に対する機体の反応速度(Lerp)";
        }
        public void ShowReversingPitchStrengthMultiTooltip()
        {
            TooltipText.text = "ReversingPitchStrengthMulti\nIf the vehicle is moving backwards, Pitch strength is multiplied by this. No effect if PitchThrustVecMulti is above 0\n\n機体が後退している場合、ピッチの強さはこれで乗算されます。PitchThrustVecMultiが0より大きい場合は効果なし";
        }
        public void ShowYawStrengthTooltip()
        {
            TooltipText.text = "YawStrength\nYaw force multiplier, (gets stronger with airspeed)\n\nヨー力乗数、(対気速度とともに強くなります)";
        }
        public void ShowYawThrustVecMultiTooltip()
        {
            TooltipText.text = "YawThrustVecMulti\nYaw rotation force (as multiple of YawStrength) (doesn't get stronger with airspeed, useful for helicopters and ridiculous jets). Setting this to a nonzero value disables inversion of joystick pitch controls when vehicle is travelling backwards\n[Range(0.0f, 1f)]\n\nヨー回転力(YawStrengthの倍数)(対気速度によっては強くなりません。ヘリコプターやとんでもないジェット機に役立ちます)。これをゼロ以外の値に設定すると、機体が後進しているときにジョイスティックのピッチ制御の反転が無効になります。";
        }
        public void ShowYawFrictionTooltip()
        {
            TooltipText.text = "YawFriction\nForce that stops vehicle from yawing, (gets stronger with airspeed)\n\n機体のヨーイングを止める力（対気速度が上がるにつれて強くなります）";
        }
        public void ShowYawConstantFrictionTooltip()
        {
            TooltipText.text = "YawConstantFriction\nForce that stops vehicle from yawing, (doesn't get stronger with airspeed)\n\n機体のヨーイングを止める力（対気速度によっては強くなりません）";
        }
        public void ShowYawResponseTooltip()
        {
            TooltipText.text = "YawResponse\nHow quickly the vehicle responds to changes in joystick's yaw (Lerp)\n\nジョイスティックのヨーの変化に対する機体の反応速度(Lerp)";
        }
        public void ShowReversingYawStrengthMultiTooltip()
        {
            TooltipText.text = "ReversingYawStrengthMulti\nIf the vehicle is moving backwards, Yaw strength is multiplied by this. No effect if YawThrustVecMulti is above 0\n\n機体が後退している場合、ヨー強度はこれによって乗算されます。YawThrustVecMultiが0より大きい場合は効果なし";
        }
        public void ShowRollStrengthTooltip()
        {
            TooltipText.text = "RollStrength\nRoll force multiplier, (gets stronger with airspeed)\n\nロール力乗数、(対気速度とともに強くなります)";
        }
        public void ShowRollThrustVecMultiTooltip()
        {
            TooltipText.text = "RollThrustVecMulti\nRoll rotation force (as multiple of RollStrength) (doesn't get stronger with airspeed, useful for helicopters and ridiculous jets). Setting this to a nonzero value disables inversion of joystick pitch controls when vehicle is travelling backwards\n[Range(0.0f, 1f)]\n\nロール回転力(RollStrengthの倍数)(対気速度によっては強くなりません。ヘリコプターやばかげたジェット機に役立ちます)。これをゼロ以外の値に設定すると、機体が後進しているときにジョイスティックのピッチ制御の反転が無効になります。";
        }
        public void ShowRollFrictionTooltip()
        {
            TooltipText.text = "RollFriction\nForce that stops vehicle from rolling, (gets stronger with airspeed)\n\n機体の回転を止める力（対気速度が上がると強くなります）";
        }
        public void ShowRollConstantFrictionTooltip()
        {
            TooltipText.text = "RollConstantFriction\nForce that stops vehicle from rolling, (doesn't get stronger with airspeed)\n\n機体の回転を止める力（対気速度によっては強くなりません）";
        }
        public void ShowRollResponseTooltip()
        {
            TooltipText.text = "RollResponse\nHow quickly the vehicle responds to changes in joystick's roll (Lerp)\n\nジョイスティックのロールの変化に対する機体の反応速度(Lerp)";
        }
        public void ShowReversingRollStrengthMultiTooltip()
        {
            TooltipText.text = "ReversingRollStrengthMulti\nIf the vehicle is moving backwards, Roll strength is multiplied by this. No effect if RollThrustVecMulti is above 0\n//reversing = AoA > 90\n\n機体が後退している場合、ロール強度にはこれが乗算されます。RollThrustVecMultiが0より大きい場合は効果なし";
        }
        public void ShowPitchDownStrMultiTooltip()
        {
            TooltipText.text = "PitchDownStrMulti\nMake pitching down a different strength than pitching up\n\nピッチダウンをピッチアップとは異なる強さにする";
        }
        public void ShowPitchDownLiftMultiTooltip()
        {
            TooltipText.text = "PitchDownLiftMulti\nWhen angle of attack is negative (air is hitting the top of the plane) multiply lift by this number (useful for making vehicles weak at flying upside down)\n\n迎え角が負の場合(空気が飛行機の上部に当たる場合)、揚力にこの数値を乗算します(機体が逆さまに飛行するのを弱くするのに役立ちます)";
        }
        public void ShowInertiaTensorRotationMultiTooltip()
        {
            TooltipText.text = "InertiaTensorRotationMulti\nAdjust the rotation of Unity's inbuilt Inertia Tensor Rotation, which is a function of rigidbodies. If set to 0, the plane will be very stable and feel boring to fly.\n\nUnityに組み込まれたInertiaTensorRotationの回転を調整します。これはリジッドボディの関数です。0に設定すると、飛行機は非常に安定し、飛行するのが退屈に感じられます。";
        }
        public void ShowInvertITRYawTooltip()
        {
            TooltipText.text = "InvertITRYaw\nInverts Z axis of the Inertia Tensor Rotation, causing the direction of the yawing experienced after rolling to invert\n\n慣性テンソル回転のZ軸を反転し、回転後に発生するヨーイングの方向を反転させます。";
        }
        public void ShowAdverseYawTooltip()
        {
            TooltipText.text = "AdverseYaw\nYawing added to the vehicle with changes in throttle\n\nスロットルの変更により機体にヨーイングが追加される";
        }
        public void ShowAdverseRollTooltip()
        {
            TooltipText.text = "AdverseRoll\nRolling added to the vehicle with the throttle up flying below RotMultiMaxSpeed. Lower speed = more adverse roll.\n\nスロットルを上げてRotMultiMaxSpeedを下回ると、機体にローリングが追加されます。速度が低い=逆方向のロールが大きくなります。";
        }
        public void ShowRotMultiMaxSpeedTooltip()
        {
            TooltipText.text = "RotMultiMaxSpeed\nRotational inputs are multiplied by current speed to make flying at low speeds feel heavier. Above the speed input here, all inputs will be at 100%. Linear. (Meters/second)\n\n回転入力に現在の速度が乗算されるため、低速での飛行がより重く感じられます。ここで入力した速度を超えると、すべての入力が100%になります。リニア。(メートル/秒)";
        }
        public void ShowVelStraightenStrPitchTooltip()
        {
            TooltipText.text = "VelStraightenStrPitch\nHow much the the vehicle's nose is pulled toward the direction of movement on the pitch axis\n\n機体のノーズがピッチ軸上で進行方向にどれだけ引っ張られるか";
        }
        public void ShowVelStraightenStrYawTooltip()
        {
            TooltipText.text = "VelStraightenStrYaw\nHow much the the vehicle's nose is pulled toward the direction of movement on the yaw axis\n\n機体のノーズがヨー軸上の進行方向にどれだけ引っ張られるか";
        }
        public void ShowMaxAngleOfAttackPitchTooltip()
        {
            TooltipText.text = "MaxAngleOfAttackPitch\nAngle of attack on the yaw axis above which the plane will lose control\n\n飛行機が制御を失うヨー軸の迎角";
        }
        public void ShowMaxAngleOfAttackYawTooltip()
        {
            TooltipText.text = "MaxAngleOfAttackYaw\nAngle of attack on the pitch axis above which the plane will lose control\n\nピッチ軸上の迎え角。この角度を超えると飛行機は制御を失います。";
        }
        public void ShowAoaCurveStrengthTooltip()
        {
            TooltipText.text = "AoaCurveStrength\nShape of the angle of attack lift curve. 1= linear, high number = curve more vertical at the beginning, See this to understand (the 2 in the input represents this value, ignore everything outside the 0-1 range in the graph): https://www.wolframalpha.com/input/?i=-%28%281-x%29%5E2%29%2B1\n//1 = linear, >1 = convex, <1 = concave\n\n迎角リフトカーブの形状。1=線形、高い数値=最初はより垂直な曲線。これを参照して理解してください(入力の2はこの値を表し、グラフ内の0～1の範囲外はすべて無視します):https://www.wolframalpha.com/input/?i=-%28%281-x%29%5E2%29%2B1";
        }
        public void ShowHighPitchAoaMinControlTooltip()
        {
            TooltipText.text = "HighPitchAoaMinControl\nThe angle of attack curve is augmented by being MAX'd(taking the higher value) with a linear curve that is multiplied by this number. Use this value to decide how much control the plane has when beyond it's 'max' angle of attack. See AoALiftCurve.png. Pitch AoA and Yaw AoA are calculated seperately, control is reduced based on the worse value.\n\n迎え角曲線は、この数値を乗算した線形曲線でMAX'd(より高い値を取る)することによって強化されます。この値を使用して、「最大」迎え角を超えたときに飛行機がどの程度制御できるかを決定します。AoALiftCurve.pngを参照してください。ピッチAoAとヨーAoAは別々に計算され、悪い方の値に基づいて制御が低下します。";
        }
        public void ShowHighYawAoaMinControlTooltip()
        {
            TooltipText.text = "HighYawAoaMinControl\nSee above\n\n上記を参照";
        }
        public void ShowDoStallForcesTooltip()
        {
            TooltipText.text = "DoStallForces\nEnable YawAoaRollForce and PitchAoaPitchForce forces used to make vehicle rotate when in a stall, Both curves must be initialized or script will crash.\n\nYawAoaRollForceおよびPitchAoaPitchForceフォースを有効にして、失速時に機体を回転させるために使用します。両方のカーブを初期化する必要があります。初期化しないとスクリプトがクラッシュします。";
        }
        public void ShowPitchAoaPitchForceMultiTooltip()
        {
            TooltipText.text = "PitchAoaPitchForceMulti\nStrength of above force\n\n上記の力の強さ";
        }
        public void ShowYawAoaRollForceMultiTooltip()
        {
            TooltipText.text = "YawAoaRollForceMulti\nStrength of above force\n\n上記の力の強さ";
        }
        public void ShowHighPitchAoaMinLiftTooltip()
        {
            TooltipText.text = "HighPitchAoaMinLift\nWhen the plane is is at a high angle of attack you can give it a minimum amount of lift/drag, so that it doesn't just lose all air resistance.\n\n飛行機が高い迎え角にあるときは、空気抵抗をすべて失うことがないように、最小限の揚力/抗力を飛行機に与えることができます。";
        }
        public void ShowHighYawAoaMinLiftTooltip()
        {
            TooltipText.text = "HighYawAoaMinLift\nSee above\n\n上記を参照";
        }
        public void ShowZeroLiftAoATooltip()
        {
            TooltipText.text = "ZeroLiftAoA\nThe angle at which the aircraft's wings produce zero lift. Influenced by the wing's camber angle. A larger camber usually results in a lower ZeroLiftAoA. Helps simulate the plane's natural glide. Usually a negative value.\n\n航空機の翼がゼロ揚力を生み出す角度。翼のキャンバー角の影響を受けます。キャンバーが大きいほど、ZeroLiftAoAは通常低くなります。飛行機の自然な滑空をシミュレートするのに役立ちます。通常は負の値です。";
        }
        public void ShowTaxiRotationSpeedTooltip()
        {
            TooltipText.text = "TaxiRotationSpeed\nDegrees per second the vehicle rotates on the ground. Uses simple object rotation with a lerp, no real physics to it.\n\n機体が地面上で回転する1秒あたりの度数。lerpを使用した単純なオブジェクトの回転を使用します。実際の物理学は使用しません。";
        }
        public void ShowTaxiRotationResponseTooltip()
        {
            TooltipText.text = "TaxiRotationResponse\nHow lerped the taxi movement rotation is\n\nタクシーの移動ローテーションがどの程度緩いか";
        }
        public void ShowDisallowTaxiRotationWhileStillTooltip()
        {
            TooltipText.text = "DisallowTaxiRotationWhileStill\nMake taxiing more realistic by not allowing vehicle to rotate on the spot\n\n機体がその場で回転しないようにすることで、タキシングをより現実的にします";
        }
        public void ShowTaxiFullTurningSpeedTooltip()
        {
            TooltipText.text = "TaxiFullTurningSpeed\nWhen the above is ticked, This is the speed at which the vehicle will reach its full turning speed. Meters/second.\n\n上記にチェックを入れると、機体が全回転速度に達する速度になります。メートル/秒。";
        }
        public void ShowWheelSuspensionTooltip()
        {
            TooltipText.text = "WheelSuspension\nChanges suspension paramater ally to disable suspension down on V1 speed to avoid sticky wheels.\n\nサスペンション パラメータを変更して、V1 速度でサスペンションを無効にし、車輪の固着を回避します。";
        }
        public void ShowLiftTooltip()
        {
            TooltipText.text = "Lift\nAdjust how steep the lift curve is. Higher = more lift\n\nリフトカーブの急勾配を調整します。高い=揚力が大きい";
        }
        public void ShowSidewaysLiftTooltip()
        {
            TooltipText.text = "SidewaysLift\nHow much angle of attack on yaw turns the vehicle. Yaw steering strength in air\n\nヨーの迎え角が機体をどれだけ回転させるか。空中でのヨーステア強度";
        }
        public void ShowMaxLiftTooltip()
        {
            TooltipText.text = "MaxLift\nMaximum value for lift, as it's exponential it's wise to stop it at some point?\n\nリフトの最大値。指数関数的であるため、ある時点で停止するのが賢明ですか?";
        }
        public void ShowVelLiftTooltip()
        {
            TooltipText.text = "VelLift\nPush the vehicle up based on speed. Used to counter the fact that without it, the plane's nose will droop down due to gravity. Slower planes need a higher value.\n\n速度に応じて機体を押し上げます。これがないと飛行機の機首が重力によって垂れ下がってしまうという事実に対抗するために使用されます。速度が遅い飛行機には、より高い値が必要です。";
        }
        public void ShowVelLiftMaxTooltip()
        {
            TooltipText.text = "VelLiftMax\nMaximum Vel Lift, to stop the nose being pushed up. Technically should probably be 9.81 to counter gravity exactly\n\n最大ベルリフト、ノーズが押し上げられるのを防ぎます。重力に正確に対抗するには、技術的にはおそらく9.81でなければなりません";
        }
        public void ShowMaxGsTooltip()
        {
            TooltipText.text = "MaxGs\nVehicle will take damage if experiences more Gs that this (Internally Gs are calculated in all directions, the HUD shows only vertical Gs so it will differ slightly\n\nこれを超えるGを受けると機体はダメージを受けます(内部ではGは全方向で計算されますが、HUDには垂直Gのみが表示されるため、多少異なります)";
        }
        public void ShowGDamageTooltip()
        {
            TooltipText.text = "GDamage\nDamage taken Per G above maxGs, per second.\n(Gs - MaxGs) * GDamage = damage/second\n\nmaxGsを超えるGあたり、1秒あたりに受けるダメージ。\n(Gs-MaxGs)*GDamage=ダメージ/秒";
        }
        public void ShowCrash_Damage_SpeedTooltip()
        {
            TooltipText.text = "Crash_Damage_Speed\nSpeed at which vehicle will start to take damage from a crash (m/s)\n\n衝突により車両が損傷を受け始める速度 (m/s)";
        }
        public void ShowCrash_Death_SpeedTooltip()
        {
            TooltipText.text = "Crash_Death_Speed\nSpeed at which vehicle will take damage equal to its max health from a crash (m/s)\n\n衝突により車両が最大耐久力に等しいダメージを受ける速度 (m/s)";
        }
        public void ShowGroundEffectMaxDistanceTooltip()
        {
            TooltipText.text = "GroundEffectMaxDistance\nLength of the trace that looks for the ground to calculate ground effect\n\n地面効果を計算するために地面を探すトレースの長さ";
        }
        public void ShowGroundEffectStrengthTooltip()
        {
            TooltipText.text = "GroundEffectStrength\nMultiply the force of the ground effect\n\n地面効果の力を倍増する";
        }
        public void ShowGroundEffectLiftMaxTooltip()
        {
            TooltipText.text = "GroundEffectLiftMax\nLimit the force that can be applied by ground effect\n\n地面効果によって加えられる力を制限する";
        }
        public void ShowVTOLAngleTurnRateTooltip()
        {
            TooltipText.text = "VTOLAngleTurnRate\nDegrees per second which the angle of the thrusters on the vehicle rotate toward desired angle\n\n機体のスラスターの角度が希望の角度に向かって回転する1秒あたりの角度";
        }
        public void ShowVTOLDefaultValueTooltip()
        {
            TooltipText.text = "VTOLDefaultValue\nPosition between VTOL Min Angle and VTOL Max Angle that the plane is at by default. 0 = min, 1 = max.\n[Range(0.0f, 1f)]\n\n飛行機がデフォルトで存在するVTOL最小角度とVTOL最大角度の間の位置。0=最小、1=最大。";
        }
        public void ShowVTOLAllowAfterburnerTooltip()
        {
            TooltipText.text = "VTOLAllowAfterburner\nAllow after burner whilst VTOL is engaged, (VTOL angle is not 0), VTOL Min Angle must be 0 for afterburner to work if this is unticked.\n\nVTOLが作動している間、アフターバーナーを許可します(VTOL角度は0ではありません)。これがチェックされていない場合、アフターバーナーが機能するには、VTOL最小角度が0である必要があります。";
        }
        public void ShowVTOLThrottleStrengthMultiTooltip()
        {
            TooltipText.text = "VTOLThrottleStrengthMulti\nMultiply throttle strength by this value whilst vehicle is in VTOL mode, at VTOL angle of 90 degrees, this value is used, between 0 and 90 degrees the value is linearly transitioned towards this value, above 90 degrees it remains at this value\n\n機体がVTOLモードにある間、スロットル強度にこの値を乗算します。VTOL角度が90度の場合、この値が使用されます。0から90度の間では、値はこの値に向かって線形に移行します。90度を超えると、この値が維持されます。";
        }
        public void ShowVTOLMinAngleTooltip()
        {
            TooltipText.text = "VTOLMinAngle\nMinimum angle of thrust direction, 0 = straight backwards, 90 = straight down, 180 = straight forwards\n\nスラスト方向の最小角度、0=真っ直ぐ後方、90=真っ直ぐ下、180=真っ直ぐ前方";
        }
        public void ShowVTOLMaxAngleTooltip()
        {
            TooltipText.text = "VTOLMaxAngle\nMaximum angle of thrust direction, 0 = straight backwards, 90 = straight down, 180 = straight forwards\n[Range(0.0f, 360f)]\n\nスラスト方向の最大角度、0=真っ直ぐ後方、90=真っ直ぐ下、180=真っ直ぐ前方";
        }
        public void ShowVTOLPitchThrustVecMultiTooltip()
        {
            TooltipText.text = "VTOLPitchThrustVecMulti\nAmount of Thrust Vectoring the plane has whilst in VTOL mode. (Remember thrust vectoring is as a multiple of the normal rotation values, so best to keep below 1, usually below .4)\nLeave at 1 and adjust Pitch Strength etc for helicopters\n\nVTOLモード中に飛行機が持つ推力ベクタリングの量。(推力ベクタリングは通常の回転値の倍数であることに注意してください。そのため、1未満、通常は0.4未満に保つのが最善です)\n1のままにして、ヘリコプターのピッチ強度などを調整します。";
        }
        public void ShowVTOLYawThrustVecMultiTooltip()
        {
            TooltipText.text = "VTOLYawThrustVecMulti\nSee above\n\n上記を参照";
        }
        public void ShowVTOLRollThrustVecMultiTooltip()
        {
            TooltipText.text = "VTOLRollThrustVecMulti\nSee above\n\n上記を参照";
        }
        public void ShowVTOLLoseControlSpeedTooltip()
        {
            TooltipText.text = "VTOLLoseControlSpeed\nSpeed at which the VTOL Thrust Vec Multi values will stop taking affect, scaled linearly up to this speed. Doesn't have any effect if vehicle is VTOLOnly.\n\nVTOLThrustVecMulti値が影響を与えなくなる速度。この速度まで直線的にスケールアップされます。機体がVTOLOnlyの場合は効果がありません。";
        }
        public void ShowVTOLGroundEffectStrengthTooltip()
        {
            TooltipText.text = "VTOLGroundEffectStrength\nStrength of ground effect that doesn't depend on speed, and points in the direction of the thrust. Uses GroundEffectEmpty and GroundEffectMaxDistance. Only enabled if the vehicle has VTOL\n\n速度に依存せず、推力の方向を指す地面効果の強さ。GroundEffectEmptyとGroundEffectMaxDistanceを使用します。機体がVTOLを備えている場合にのみ有効になります";
        }
        public void ShowEnterVTOLEvent_AngleTooltip()
        {
            TooltipText.text = "EnterVTOLEvent_Angle\nReal angle offset from (0 == thrusting backwards) that the SFEXT_O_Enter/ExitVTOL is called. The event used for disabling cruise and flight limits\n\nSFEXT_O_Enter/ExitVTOLが呼び出される(0==後方への突き出し)からの実際の角度オフセット。巡航および飛行制限を無効にするために使用されるイベント";
        }
        public void ShowReverseThrustAllowAfterburnerTooltip()
        {
            TooltipText.text = "ReverseThrustAllowAfterburner\nReverse thrust allow afterburner\n\n逆推力によりアフターバーナーが使用可能";
        }
        public void ShowSoundBarrierStrengthTooltip()
        {
            TooltipText.text = "SoundBarrierStrength\nExtra drag added when airspeed approaches the speed of sound\n\n対気速度が音速に近づくと余分な抗力が追加される";
        }
        public void ShowSoundBarrierWidthTooltip()
        {
            TooltipText.text = "SoundBarrierWidth\nWithin how many meters per second of the speed of sound does the vehicle have to be before they experience extra drag. Extra drag is scaled linearly up to the speed of sound, and dowan after it\n\n機体が余分な抗力を経験する前に、音速が秒速何メートル以内になければなりません。追加の抗力は音速まで直線的にスケールアップされ、その後はダウンします。";
        }
        public void ShowFuelTooltip()
        {
            TooltipText.text = "Fuel\nFuel\n\n燃料";
        }
        public void ShowLowFuelTooltip()
        {
            TooltipText.text = "LowFuel\nAmount of fuel at which throttle will start reducing\n\nスロットルが減少し始める燃料の量";
        }
        public void ShowFuelConsumptionTooltip()
        {
            TooltipText.text = "FuelConsumption\nFuel consumed per second at max throttle, scales with throttle\n\n最大スロットル時の1秒あたりの燃料消費量、スロットルに応じて増減";
        }
        public void ShowMinFuelConsumptionTooltip()
        {
            TooltipText.text = "MinFuelConsumption\nFuel consumed per second at max throttle, scales with throttle\n\n最大スロットル時の1秒あたりの燃料消費量、スロットルに応じて増減";
        }
        public void ShowFuelConsumptionABMultiTooltip()
        {
            TooltipText.text = "FuelConsumptionABMulti\nMultiply FuelConsumption by this number when at full afterburner Scales with afterburner level\n\nアフターバーナーがフルの場合、燃料消費量にこの数値を掛けます。アフターバーナーレベルに応じてスケールします。";
        }
        public void ShowRefuelTimeTooltip()
        {
            TooltipText.text = "RefuelTime\nNumber of resupply ticks it takes to refuel fully from zero\n\nゼロから完全に燃料を補給するのに必要な再補給ティック数";
        }
        public void ShowRepairTimeTooltip()
        {
            TooltipText.text = "RepairTime\nNumber of resupply ticks it takes to repair fully from zero\n\nゼロから完全に修復するのに必要な再供給ティック数";
        }
        public void ShowRespawnDelayTooltip()
        {
            TooltipText.text = "RespawnDelay\nTime until vehicle reappears after exploding\n\n機体が爆発してから再出現するまでの時間";
        }
        public void ShowInvincibleAfterSpawnTooltip()
        {
            TooltipText.text = "InvincibleAfterSpawn\nTime after reappearing the vehicle is invincible for\n\n機体再登場後の無敵時間";
        }
        public void ShowBulletDamageTakenTooltip()
        {
            TooltipText.text = "BulletDamageTaken\nDamage taken when hit by a bullet\n\n弾丸が当たると受けるダメージ";
        }
        public void ShowPredictDamageTooltip()
        {
            TooltipText.text = "PredictDamage\nLocally destroy target if prediction thinks you killed them, should only ever cause problems if you have a system that repairs vehicles during a fight\n\n予測がターゲットを殺害したと判断した場合、ターゲットを局所的に破壊します。問題が発生するのは、戦闘中に機体を修復するシステムがある場合のみです。";
        }
        public void ShowSmallCrashSpeedTooltip()
        {
            TooltipText.text = "SmallCrashSpeed\nImpact speed that defines a small crash\n\n小さな衝突を定義する衝撃速度";
        }
        public void ShowMediumCrashSpeedTooltip()
        {
            TooltipText.text = "MediumCrashSpeed\nImpact speed that defines a medium crash\n\n中程度の衝突を定義する衝撃速度";
        }
        public void ShowBigCrashSpeedTooltip()
        {
            TooltipText.text = "BigCrashSpeed\nImpact speed that defines a big crash\n\n大クラッシュを定義する衝撃速度";
        }
        public void ShowMissileDamageTakenMultiplierTooltip()
        {
            TooltipText.text = "MissileDamageTakenMultiplier\nMultiply how much damage is done by missiles\n\nミサイルによるダメージを乗算します";
        }
        public void ShowMissilePushForceTooltip()
        {
            TooltipText.text = "MissilePushForce\nStrength of force that pushes the vehicle when a missile hits it\n\nミサイルが衝突したときに機体を押す力の強さ";
        }
        public void ShowSeaLevelTooltip()
        {
            TooltipText.text = "SeaLevel\nZero height of the calculation of atmosphere thickness and HUD altitude display\n\n大気の厚さの計算とHUD高度表示のゼロ高度";
        }
        public void ShowAtmosphereThinningStartTooltip()
        {
            TooltipText.text = "AtmosphereThinningStart\nAltitude above 'Sea Level' at which the atmosphere starts thinning, In meters. 12192 = 40,000~ feet\n//40,000 feet\n\n大気が薄くなり始める「海面」より上の高度(メートル単位)。12192=40,000~フィート";
        }
        public void ShowAtmosphereThinningEndTooltip()
        {
            TooltipText.text = "AtmosphereThinningEnd\nAltitude above 'Sea Level' at which the atmosphere reaches zero thickness. In meters. 19812 = 65,000~ feet\n//65,000 feet\n\n大気の厚さがゼロに達する「海面」より上の高度。メートル単位。19812=65,000~フィート";
        }
        public void ShowSquareJoyInputTooltip()
        {
            TooltipText.text = "SquareJoyInput\nWhen in desktop mode, make the joystick input square? (for game controllers, disable for actual joysticks\n\nデスクトップモードのとき、ジョイスティックの入力を正方形にしますか?(ゲームコントローラーの場合は、実際のジョイスティックの場合は無効にします)";
        }
        public void ShowDroneModeTooltip()
        {
            TooltipText.text = "DroneMode\nDrone mode\n\nドローンモード";
        }

        public void PresetDefault()
        {
            ArraysToInputs(_defaultFloatValues, _defaultBoolValues);
            HighlightButtonColor();
        }

        public void PresetSF1()
        {
            MassInputField.text = "19000";
            GroundDetectorRayDistanceInputField.text = "0.44";
            HealthInputField.text = "54";
            ExplodeHealthInputField.text = "-200";
            RepeatingWorldToggle.isOn = true;
            RepeatingWorldDistanceInputField.text = "20000";
            HasAfterburnerToggle.isOn = true;
            ThrottleAfterburnerPointInputField.text = "0.8";
            VTOLOnlyToggle.isOn = false;
            ThrottleStrengthInputField.text = "20";
            VerticalThrottleToggle.isOn = false;
            JoystickPushPullPitchToggle.isOn = false;
            JoystickPushPullDistanceInputField.text = "0.2";
            AfterburnerThrustMultiInputField.text = "1.5";
            AccelerationResponseInputField.text = "4.5";
            EngineSpoolDownSpeedMultiInputField.text = "0.5";
            AirFrictionInputField.text = "0.00058";
            PitchStrengthInputField.text = "5";
            PitchThrustVecMultiInputField.text = "0";
            PitchFrictionInputField.text = "24";
            PitchConstantFrictionInputField.text = "0";
            PitchResponseInputField.text = "20";
            ReversingPitchStrengthMultiInputField.text = "2";
            YawStrengthInputField.text = "3";
            YawThrustVecMultiInputField.text = "0";
            YawFrictionInputField.text = "15";
            YawConstantFrictionInputField.text = "0";
            YawResponseInputField.text = "20";
            ReversingYawStrengthMultiInputField.text = "2.4";
            RollStrengthInputField.text = "225";
            RollThrustVecMultiInputField.text = "0";
            RollFrictionInputField.text = "58.5";
            RollConstantFrictionInputField.text = "0";
            RollResponseInputField.text = "20";
            ReversingRollStrengthMultiInputField.text = "1.6";
            PitchDownStrMultiInputField.text = "0.8";
            PitchDownLiftMultiInputField.text = "0.8";
            InertiaTensorRotationMultiInputField.text = "1";
            InvertITRYawToggle.isOn = false;
            AdverseYawInputField.text = "0";
            AdverseRollInputField.text = "0";
            RotMultiMaxSpeedInputField.text = "220";
            VelStraightenStrPitchInputField.text = "0.035";
            VelStraightenStrYawInputField.text = "0.045";
            MaxAngleOfAttackPitchInputField.text = "25";
            MaxAngleOfAttackYawInputField.text = "40";
            AoaCurveStrengthInputField.text = "2";
            HighPitchAoaMinControlInputField.text = "0.2";
            HighYawAoaMinControlInputField.text = "0.2";
            DoStallForcesToggle.isOn = true;
            PitchAoaPitchForceMultiInputField.text = "4";
            YawAoaRollForceMultiInputField.text = "30";
            HighPitchAoaMinLiftInputField.text = "0.2";
            HighYawAoaMinLiftInputField.text = "0.2";
            ZeroLiftAoAInputField.text = "0";
            TaxiRotationSpeedInputField.text = "35";
            TaxiRotationResponseInputField.text = "2.5";
            DisallowTaxiRotationWhileStillToggle.isOn = false;
            TaxiFullTurningSpeedInputField.text = "20";
            WheelSuspensionToggle.isOn = false;
            LiftInputField.text = "0.00015";
            SidewaysLiftInputField.text = "0.17";
            MaxLiftInputField.text = "10";
            VelLiftInputField.text = "0.00015";
            VelLiftMaxInputField.text = "10";
            MaxGsInputField.text = "40";
            GDamageInputField.text = "10";
            Crash_Damage_SpeedInputField.text = "10";
            Crash_Death_SpeedInputField.text = "50";
            GroundEffectMaxDistanceInputField.text = "7";
            GroundEffectStrengthInputField.text = "4";
            GroundEffectLiftMaxInputField.text = "100";
            VTOLAngleTurnRateInputField.text = "90";
            VTOLDefaultValueInputField.text = "0";
            VTOLAllowAfterburnerToggle.isOn = false;
            VTOLThrottleStrengthMultiInputField.text = "0.7";
            VTOLMinAngleInputField.text = "0";
            VTOLMaxAngleInputField.text = "90";
            VTOLPitchThrustVecMultiInputField.text = "0.3";
            VTOLYawThrustVecMultiInputField.text = "0.3";
            VTOLRollThrustVecMultiInputField.text = "0.07";
            VTOLLoseControlSpeedInputField.text = "120";
            VTOLGroundEffectStrengthInputField.text = "0.14";
            EnterVTOLEvent_AngleInputField.text = "20";
            ReverseThrustAllowAfterburnerToggle.isOn = false;
            SoundBarrierStrengthInputField.text = "0.00015";
            SoundBarrierWidthInputField.text = "20";
            FuelInputField.text = "900";
            LowFuelInputField.text = "125";
            FuelConsumptionInputField.text = "1";
            MinFuelConsumptionInputField.text = "0.25";
            FuelConsumptionABMultiInputField.text = "3";
            RefuelTimeInputField.text = "25";
            RepairTimeInputField.text = "30";
            RespawnDelayInputField.text = "10";
            InvincibleAfterSpawnInputField.text = "2.5";
            BulletDamageTakenInputField.text = "10";
            PredictDamageToggle.isOn = true;
            SmallCrashSpeedInputField.text = "1";
            MediumCrashSpeedInputField.text = "8";
            BigCrashSpeedInputField.text = "25";
            MissileDamageTakenMultiplierInputField.text = "1";
            MissilePushForceInputField.text = "1";
            SeaLevelInputField.text = "-1080";
            AtmosphereThinningStartInputField.text = "12192";
            AtmosphereThinningEndInputField.text = "19812";
            SquareJoyInputToggle.isOn = true;
            DroneModeToggle.isOn = false;

            HighlightButtonColor();
        }

        public void PresetSH1()
        {
            MassInputField.text = "1350";
            GroundDetectorRayDistanceInputField.text = "0.44";
            HealthInputField.text = "113";
            ExplodeHealthInputField.text = "-200";
            RepeatingWorldToggle.isOn = true;
            RepeatingWorldDistanceInputField.text = "20000";
            HasAfterburnerToggle.isOn = false;
            ThrottleAfterburnerPointInputField.text = "0.8";
            VTOLOnlyToggle.isOn = true;
            ThrottleStrengthInputField.text = "13";
            VerticalThrottleToggle.isOn = true;
            JoystickPushPullPitchToggle.isOn = false;
            JoystickPushPullDistanceInputField.text = "0.2";
            AfterburnerThrustMultiInputField.text = "1.5";
            AccelerationResponseInputField.text = "4.5";
            EngineSpoolDownSpeedMultiInputField.text = "0.5";
            AirFrictionInputField.text = "0.000944";
            PitchStrengthInputField.text = "2.31";
            PitchThrustVecMultiInputField.text = "1";
            PitchFrictionInputField.text = "2";
            PitchConstantFrictionInputField.text = "1";
            PitchResponseInputField.text = "40";
            ReversingPitchStrengthMultiInputField.text = "2";
            YawStrengthInputField.text = "0.5";
            YawThrustVecMultiInputField.text = "1";
            YawFrictionInputField.text = "14";
            YawConstantFrictionInputField.text = "1";
            YawResponseInputField.text = "40";
            ReversingYawStrengthMultiInputField.text = "2.4";
            RollStrengthInputField.text = "3.5";
            RollThrustVecMultiInputField.text = "1";
            RollFrictionInputField.text = "0";
            RollConstantFrictionInputField.text = "1";
            RollResponseInputField.text = "40";
            ReversingRollStrengthMultiInputField.text = "1.6";
            PitchDownStrMultiInputField.text = "1";
            PitchDownLiftMultiInputField.text = "0.175";
            InertiaTensorRotationMultiInputField.text = "1";
            InvertITRYawToggle.isOn = false;
            AdverseYawInputField.text = "2.9";
            AdverseRollInputField.text = "0";
            RotMultiMaxSpeedInputField.text = "220";
            VelStraightenStrPitchInputField.text = "0.02";
            VelStraightenStrYawInputField.text = "0.09";
            MaxAngleOfAttackPitchInputField.text = "40";
            MaxAngleOfAttackYawInputField.text = "40";
            AoaCurveStrengthInputField.text = "1.5";
            HighPitchAoaMinControlInputField.text = "1";
            HighYawAoaMinControlInputField.text = "1";
            DoStallForcesToggle.isOn = false;
            PitchAoaPitchForceMultiInputField.text = "0";
            YawAoaRollForceMultiInputField.text = "0";
            HighPitchAoaMinLiftInputField.text = "0.5";
            HighYawAoaMinLiftInputField.text = "0.5";
            ZeroLiftAoAInputField.text = "0";
            TaxiRotationSpeedInputField.text = "25";
            TaxiRotationResponseInputField.text = "0.5";
            DisallowTaxiRotationWhileStillToggle.isOn = false;
            TaxiFullTurningSpeedInputField.text = "20";
            WheelSuspensionToggle.isOn = false;
            LiftInputField.text = "0.0024";
            SidewaysLiftInputField.text = "0.0012";
            MaxLiftInputField.text = "10";
            VelLiftInputField.text = "0.0036";
            VelLiftMaxInputField.text = "7";
            MaxGsInputField.text = "10";
            GDamageInputField.text = "22";
            Crash_Damage_SpeedInputField.text = "10";
            Crash_Death_SpeedInputField.text = "50";
            GroundEffectMaxDistanceInputField.text = "4";
            GroundEffectStrengthInputField.text = "3";
            GroundEffectLiftMaxInputField.text = "100";
            VTOLAngleTurnRateInputField.text = "10";
            VTOLDefaultValueInputField.text = "1";
            VTOLAllowAfterburnerToggle.isOn = false;
            VTOLThrottleStrengthMultiInputField.text = "0.7";
            VTOLMinAngleInputField.text = "80";
            VTOLMaxAngleInputField.text = "90";
            VTOLPitchThrustVecMultiInputField.text = "1";
            VTOLYawThrustVecMultiInputField.text = "1";
            VTOLRollThrustVecMultiInputField.text = "1";
            VTOLLoseControlSpeedInputField.text = "1";
            VTOLGroundEffectStrengthInputField.text = "0.3";
            EnterVTOLEvent_AngleInputField.text = "20";
            ReverseThrustAllowAfterburnerToggle.isOn = false;
            SoundBarrierStrengthInputField.text = "0.00015";
            SoundBarrierWidthInputField.text = "20";
            FuelInputField.text = "1400";
            LowFuelInputField.text = "125";
            FuelConsumptionInputField.text = "1.5";
            MinFuelConsumptionInputField.text = "0.25";
            FuelConsumptionABMultiInputField.text = "4.4";
            RefuelTimeInputField.text = "25";
            RepairTimeInputField.text = "30";
            RespawnDelayInputField.text = "10";
            InvincibleAfterSpawnInputField.text = "2.5";
            BulletDamageTakenInputField.text = "10";
            PredictDamageToggle.isOn = true;
            SmallCrashSpeedInputField.text = "1";
            MediumCrashSpeedInputField.text = "8";
            BigCrashSpeedInputField.text = "25";
            MissileDamageTakenMultiplierInputField.text = "1";
            MissilePushForceInputField.text = "1";
            SeaLevelInputField.text = "-1080";
            AtmosphereThinningStartInputField.text = "6604";
            AtmosphereThinningEndInputField.text = "9144";
            SquareJoyInputToggle.isOn = true;
            DroneModeToggle.isOn = false;

            HighlightButtonColor();
        }

        public void PresetWX3()
        {
            MassInputField.text = "1250";
            GroundDetectorRayDistanceInputField.text = "0.44";
            HealthInputField.text = "43";
            ExplodeHealthInputField.text = "-200";
            RepeatingWorldToggle.isOn = true;
            RepeatingWorldDistanceInputField.text = "20000";
            HasAfterburnerToggle.isOn = false;
            ThrottleAfterburnerPointInputField.text = "0.8";
            VTOLOnlyToggle.isOn = false;
            ThrottleStrengthInputField.text = "5";
            VerticalThrottleToggle.isOn = false;
            JoystickPushPullPitchToggle.isOn = false;
            JoystickPushPullDistanceInputField.text = "0.2";
            AfterburnerThrustMultiInputField.text = "1.5";
            AccelerationResponseInputField.text = "0.6";
            EngineSpoolDownSpeedMultiInputField.text = "0.5";
            AirFrictionInputField.text = "0.000912";
            PitchStrengthInputField.text = "1";
            PitchThrustVecMultiInputField.text = "0";
            PitchFrictionInputField.text = "2";
            PitchConstantFrictionInputField.text = "0";
            PitchResponseInputField.text = "12";
            ReversingPitchStrengthMultiInputField.text = "2";
            YawStrengthInputField.text = "3";
            YawThrustVecMultiInputField.text = "0";
            YawFrictionInputField.text = "12";
            YawConstantFrictionInputField.text = "0";
            YawResponseInputField.text = "12";
            ReversingYawStrengthMultiInputField.text = "2.4";
            RollStrengthInputField.text = "15";
            RollThrustVecMultiInputField.text = "0";
            RollFrictionInputField.text = "10";
            RollConstantFrictionInputField.text = "0";
            RollResponseInputField.text = "12";
            ReversingRollStrengthMultiInputField.text = "1.6";
            PitchDownStrMultiInputField.text = "0.7";
            PitchDownLiftMultiInputField.text = "0.5";
            InertiaTensorRotationMultiInputField.text = "1";
            InvertITRYawToggle.isOn = false;
            AdverseYawInputField.text = "0";
            AdverseRollInputField.text = "2.5";
            RotMultiMaxSpeedInputField.text = "70";
            VelStraightenStrPitchInputField.text = "0.08";
            VelStraightenStrYawInputField.text = "0.3";
            MaxAngleOfAttackPitchInputField.text = "37";
            MaxAngleOfAttackYawInputField.text = "30";
            AoaCurveStrengthInputField.text = "1.5";
            HighPitchAoaMinControlInputField.text = "0.2";
            HighYawAoaMinControlInputField.text = "0.2";
            DoStallForcesToggle.isOn = false;
            PitchAoaPitchForceMultiInputField.text = "0.9";
            YawAoaRollForceMultiInputField.text = "5";
            HighPitchAoaMinLiftInputField.text = "0.5";
            HighYawAoaMinLiftInputField.text = "0.4";
            ZeroLiftAoAInputField.text = "0";
            TaxiRotationSpeedInputField.text = "20";
            TaxiRotationResponseInputField.text = "2.5";
            DisallowTaxiRotationWhileStillToggle.isOn = false;
            TaxiFullTurningSpeedInputField.text = "20";
            WheelSuspensionToggle.isOn = false;
            LiftInputField.text = "0.0009";
            SidewaysLiftInputField.text = "0.2";
            MaxLiftInputField.text = "5";
            VelLiftInputField.text = "0.0036";
            VelLiftMaxInputField.text = "10";
            MaxGsInputField.text = "10";
            GDamageInputField.text = "15";
            Crash_Damage_SpeedInputField.text = "10";
            Crash_Death_SpeedInputField.text = "50";
            GroundEffectMaxDistanceInputField.text = "4";
            GroundEffectStrengthInputField.text = "0.2";
            GroundEffectLiftMaxInputField.text = "15";
            VTOLAngleTurnRateInputField.text = "90";
            VTOLDefaultValueInputField.text = "0";
            VTOLAllowAfterburnerToggle.isOn = false;
            VTOLThrottleStrengthMultiInputField.text = "0.7";
            VTOLMinAngleInputField.text = "0";
            VTOLMaxAngleInputField.text = "90";
            VTOLPitchThrustVecMultiInputField.text = "0.3";
            VTOLYawThrustVecMultiInputField.text = "0.3";
            VTOLRollThrustVecMultiInputField.text = "0.07";
            VTOLLoseControlSpeedInputField.text = "120";
            VTOLGroundEffectStrengthInputField.text = "4";
            EnterVTOLEvent_AngleInputField.text = "20";
            ReverseThrustAllowAfterburnerToggle.isOn = false;
            SoundBarrierStrengthInputField.text = "0.0015";
            SoundBarrierWidthInputField.text = "100";
            FuelInputField.text = "2500";
            LowFuelInputField.text = "125";
            FuelConsumptionInputField.text = "1";
            MinFuelConsumptionInputField.text = "0.25";
            FuelConsumptionABMultiInputField.text = "4.4";
            RefuelTimeInputField.text = "25";
            RepairTimeInputField.text = "30";
            RespawnDelayInputField.text = "10";
            InvincibleAfterSpawnInputField.text = "2.5";
            BulletDamageTakenInputField.text = "10";
            PredictDamageToggle.isOn = true;
            SmallCrashSpeedInputField.text = "1";
            MediumCrashSpeedInputField.text = "8";
            BigCrashSpeedInputField.text = "25";
            MissileDamageTakenMultiplierInputField.text = "1";
            MissilePushForceInputField.text = "1";
            SeaLevelInputField.text = "-1080";
            AtmosphereThinningStartInputField.text = "2700";
            AtmosphereThinningEndInputField.text = "3300";
            SquareJoyInputToggle.isOn = true;
            DroneModeToggle.isOn = false;

            HighlightButtonColor();
        }

        public void PresetSH2()
        {
            MassInputField.text = "22296";
            GroundDetectorRayDistanceInputField.text = "0.44";
            HealthInputField.text = "553";
            ExplodeHealthInputField.text = "-452";
            RepeatingWorldToggle.isOn = true;
            RepeatingWorldDistanceInputField.text = "20000";
            HasAfterburnerToggle.isOn = false;
            ThrottleAfterburnerPointInputField.text = "0.8";
            VTOLOnlyToggle.isOn = true;
            ThrottleStrengthInputField.text = "20";
            VerticalThrottleToggle.isOn = true;
            JoystickPushPullPitchToggle.isOn = false;
            JoystickPushPullDistanceInputField.text = "0.2";
            AfterburnerThrustMultiInputField.text = "1.5";
            AccelerationResponseInputField.text = "4.5";
            EngineSpoolDownSpeedMultiInputField.text = "0.5";
            AirFrictionInputField.text = "0.000944";
            PitchStrengthInputField.text = "96";
            PitchThrustVecMultiInputField.text = "1";
            PitchFrictionInputField.text = "2";
            PitchConstantFrictionInputField.text = "1";
            PitchResponseInputField.text = "50";
            ReversingPitchStrengthMultiInputField.text = "2";
            YawStrengthInputField.text = "25.2";
            YawThrustVecMultiInputField.text = "1";
            YawFrictionInputField.text = "14";
            YawConstantFrictionInputField.text = "1";
            YawResponseInputField.text = "18";
            ReversingYawStrengthMultiInputField.text = "2.4";
            RollStrengthInputField.text = "10";
            RollThrustVecMultiInputField.text = "1";
            RollFrictionInputField.text = "0";
            RollConstantFrictionInputField.text = "1";
            RollResponseInputField.text = "40";
            ReversingRollStrengthMultiInputField.text = "1.6";
            PitchDownStrMultiInputField.text = "1";
            PitchDownLiftMultiInputField.text = "0.175";
            InertiaTensorRotationMultiInputField.text = "1";
            InvertITRYawToggle.isOn = false;
            AdverseYawInputField.text = "2.9";
            AdverseRollInputField.text = "0";
            RotMultiMaxSpeedInputField.text = "220";
            VelStraightenStrPitchInputField.text = "0.02";
            VelStraightenStrYawInputField.text = "0.09";
            MaxAngleOfAttackPitchInputField.text = "40";
            MaxAngleOfAttackYawInputField.text = "40";
            AoaCurveStrengthInputField.text = "1.5";
            HighPitchAoaMinControlInputField.text = "1";
            HighYawAoaMinControlInputField.text = "1";
            DoStallForcesToggle.isOn = false;
            PitchAoaPitchForceMultiInputField.text = "0";
            YawAoaRollForceMultiInputField.text = "0";
            HighPitchAoaMinLiftInputField.text = "0.5";
            HighYawAoaMinLiftInputField.text = "0.5";
            ZeroLiftAoAInputField.text = "0";
            TaxiRotationSpeedInputField.text = "25";
            TaxiRotationResponseInputField.text = "0.5";
            DisallowTaxiRotationWhileStillToggle.isOn = false;
            TaxiFullTurningSpeedInputField.text = "20";
            WheelSuspensionToggle.isOn = false;
            LiftInputField.text = "0.0015";
            SidewaysLiftInputField.text = "0.0012";
            MaxLiftInputField.text = "10";
            VelLiftInputField.text = "0.00225";
            VelLiftMaxInputField.text = "7";
            MaxGsInputField.text = "10";
            GDamageInputField.text = "100";
            Crash_Damage_SpeedInputField.text = "10";
            Crash_Death_SpeedInputField.text = "50";
            GroundEffectMaxDistanceInputField.text = "4";
            GroundEffectStrengthInputField.text = "3";
            GroundEffectLiftMaxInputField.text = "100";
            VTOLAngleTurnRateInputField.text = "10";
            VTOLDefaultValueInputField.text = "1";
            VTOLAllowAfterburnerToggle.isOn = false;
            VTOLThrottleStrengthMultiInputField.text = "0.7";
            VTOLMinAngleInputField.text = "80";
            VTOLMaxAngleInputField.text = "90";
            VTOLPitchThrustVecMultiInputField.text = "1";
            VTOLYawThrustVecMultiInputField.text = "1";
            VTOLRollThrustVecMultiInputField.text = "1";
            VTOLLoseControlSpeedInputField.text = "1";
            VTOLGroundEffectStrengthInputField.text = "0.3";
            EnterVTOLEvent_AngleInputField.text = "20";
            ReverseThrustAllowAfterburnerToggle.isOn = false;
            SoundBarrierStrengthInputField.text = "0.00015";
            SoundBarrierWidthInputField.text = "20";
            FuelInputField.text = "1400";
            LowFuelInputField.text = "125";
            FuelConsumptionInputField.text = "1.5";
            MinFuelConsumptionInputField.text = "0.25";
            FuelConsumptionABMultiInputField.text = "4.4";
            RefuelTimeInputField.text = "25";
            RepairTimeInputField.text = "30";
            RespawnDelayInputField.text = "10";
            InvincibleAfterSpawnInputField.text = "2.5";
            BulletDamageTakenInputField.text = "10";
            PredictDamageToggle.isOn = true;
            SmallCrashSpeedInputField.text = "1";
            MediumCrashSpeedInputField.text = "8";
            BigCrashSpeedInputField.text = "25";
            MissileDamageTakenMultiplierInputField.text = "1";
            MissilePushForceInputField.text = "1";
            SeaLevelInputField.text = "-1080";
            AtmosphereThinningStartInputField.text = "6604";
            AtmosphereThinningEndInputField.text = "9144";
            SquareJoyInputToggle.isOn = true;
            DroneModeToggle.isOn = false;

            HighlightButtonColor();
        }

        void Start()
        {
            NormalButtonColor = ApplyAndRespawnButton.colors;

            ColorBlock colorBlock = ApplyAndRespawnButton.colors;
            Color32 orange = new Color32(255, 179, 102, 255);
            colorBlock.normalColor = orange;
            colorBlock.highlightedColor = orange;
            colorBlock.selectedColor = orange;
            HighlightedButtonColor = colorBlock;
        }

        public override void OnDeserialization()
        {
            ArraysToInputs(_syncedFloatValues, _syncedBoolValues);
            ArraysToVehicleValues(_syncedFloatValues, _syncedBoolValues);
            NormalizeButtonColor();
        }

        private void Initialize()
        {
            if (VehicleEntity == null)
            {
                VehicleEntity = Vehicle.transform.parent.GetComponent<SaccEntity>();
                VehicleRigidbody = VehicleEntity.GetComponent<Rigidbody>();
            }

            if (HUDController == null)
            {
                Transform InVehicleOnly = VehicleEntity.transform.Find("InVehicleOnly");
                if (InVehicleOnly) //InVehicleOnlyは無いこともある
                {
                    Transform t = InVehicleOnly.Find("HUDController");
                    if (t) //HUDControllerは無いこともある
                    {
                        HUDController = t.GetComponent<SAV_HUDController>();
                    }
                }
            }

            if (EffectsController == null)
            {
                Transform t = VehicleEntity.transform.Find("EffectsController");
                if (t) //EffectsControllerが無いのは考えにくいが一応
                {
                    EffectsController = t.GetComponent<SAV_EffectsController>();
                }
            }

            VehicleValuesToDefaultArrays();
            ArraysToInputs(_defaultFloatValues, _defaultBoolValues);

            NormalizeButtonColor();

            ShowActualValues();
        }

        public void NotifyChange()
        {
            HighlightButtonColor();
        }

        private void HighlightButtonColor()
        {
            ApplyAndRespawnButton.colors = HighlightedButtonColor;
        }

        private void NormalizeButtonColor()
        {
            ApplyAndRespawnButton.colors = NormalButtonColor;
        }

        public void ApplyAndRespawn()
        {
            if (CanRespawnVehicle())
            {
                Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
                InputsToSyncedArrays();
                RequestSerialization();

                ArraysToVehicleValues(_syncedFloatValues, _syncedBoolValues);

                RespawnVehicle();

                NormalizeButtonColor();
            }
        }

        private bool CanRespawnVehicle()
        {
            VRCPlayerApi currentOwner = Networking.GetOwner(VehicleEntity.gameObject);
            bool BlockedCheck = (currentOwner != null && currentOwner.GetBonePosition(HumanBodyBones.Hips) == Vector3.zero) && Vehicle.Speed > .2f;
            if (Vehicle.Occupied || VehicleEntity._dead || BlockedCheck)
            {
                return false;
            }
            return true;
        }

        private void RespawnVehicle()
        {
            VehicleEntity.SendCustomEvent("EntityRespawn");
        }


        private void ReAdjustVehicleValues()
        {
            Vehicle.SendCustomEvent("ReAdjustValues");

            if (HUDController)
            {
                HUDController.SendCustomEvent("ReAdjustValues");
            }

            if (EffectsController)
            {
                EffectsController.SendCustomEvent("ReAdjustValues");
            }
        }

        private float ToFloat(InputField inputField)
        {
            float f;
            if(float.TryParse(inputField.text, out f))
            {
                return f;
            }
            else
            {
                return 0;
            }
        }

        private bool ToBool(Toggle toggle)
        {
            return toggle.isOn;
        }

        private void Update()
        {
            if (!Initialized)
            {
                if(Vehicle.FullFuel > 0) //SFEXT_L_EntityStartのAutoAdjustValuesToMassが済んでいたら
                {
                    Initialize();
                    Initialized = true;
                }
            }
        }

    }
}
