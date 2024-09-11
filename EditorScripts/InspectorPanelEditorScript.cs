#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using VRC.Udon;
using UdonSharp;

namespace SaccFlightAndVehicles
{

    public class InspectorPanelEditorScript : MonoBehaviour
    {
        public GameObject LabelPrefab;
        public GameObject InputFieldPrefab;
        public GameObject TogglePrefab;
        public GameObject ValueTextPrefab;
        public RectTransform LabelParent;
        public RectTransform InputParent;
        public RectTransform ValueTextParent;
        public UdonBehaviour AircraftInspectorUB;

        private AircraftInspector AirclaftInspector;
        private Type AirclaftInspectorType;
        public InputField TestInputField;

        [ContextMenu("TEST")]
        public void Test()
        {
            FieldInfo field = AirclaftInspectorType.GetField("GroundDetectorRayDistanceInputField");
            field.SetValue(AirclaftInspector, TestInputField);
        }


        [ContextMenu("PUT_OBJECTS")]
        public void PutObjects()
        {
            AirclaftInspector = AircraftInspectorUB.GetComponent<AircraftInspector>();
            AirclaftInspectorType = AirclaftInspector.GetType();

            float dx = 400;
            float dy = -25;
            float initX = 0;
            float initY = 0 - dy;
            float x = initX;
            float y = initY;

            PutInputField("Mass", x, y += dy);
            PutInputField("GroundDetectorRayDistance", x, y += dy);
            PutInputField("Health", x, y += dy);
            PutToggle("RepeatingWorld", x, y += dy);
            PutInputField("RepeatingWorldDistance", x, y += dy);
            PutToggle("HasAfterburner", x, y += dy);
            PutInputField("ThrottleAfterburnerPoint", x, y += dy);
            PutToggle("VTOLOnly", x, y += dy);
            PutInputField("ThrottleStrength", x, y += dy);
            PutToggle("VerticalThrottle", x, y += dy);
            PutToggle("JoystickPushPullPitch", x, y += dy);
            PutInputField("JoystickPushPullDistance", x, y += dy);
            PutInputField("AfterburnerThrustMulti", x, y += dy);
            PutInputField("AccelerationResponse", x, y += dy);
            PutInputField("EngineSpoolDownSpeedMulti", x, y += dy);
            PutInputField("AirFriction", x, y += dy);
            PutInputField("PitchStrength", x, y += dy);
            PutInputField("PitchThrustVecMulti", x, y += dy);
            PutInputField("PitchFriction", x, y += dy);
            PutInputField("PitchConstantFriction", x, y += dy);
            PutInputField("PitchResponse", x, y += dy);
            PutInputField("ReversingPitchStrengthMulti", x, y += dy);
            PutInputField("YawStrength", x, y += dy);
            PutInputField("YawThrustVecMulti", x, y += dy);
            PutInputField("YawFriction", x, y += dy);
            PutInputField("YawConstantFriction", x, y += dy);
            PutInputField("YawResponse", x, y += dy);
            PutInputField("ReversingYawStrengthMulti", x, y += dy);
            PutInputField("RollStrength", x, y += dy);
            PutInputField("RollThrustVecMulti", x, y += dy);
            PutInputField("RollFriction", x, y += dy);
            PutInputField("RollConstantFriction", x, y += dy);
            PutInputField("RollResponse", x, y += dy);
            PutInputField("ReversingRollStrengthMulti", x, y += dy);
            PutInputField("PitchDownStrMulti", x, y += dy);
            PutInputField("PitchDownLiftMulti", x, y += dy);
            PutInputField("InertiaTensorRotationMulti", x, y += dy);
            PutToggle("InvertITRYaw", x, y += dy);
            PutInputField("AdverseYaw", x, y += dy);
            PutInputField("AdverseRoll", x, y += dy);
            x += dx;
            y = initY;
            PutInputField("RotMultiMaxSpeed", x, y += dy);
            PutInputField("VelStraightenStrPitch", x, y += dy);
            PutInputField("VelStraightenStrYaw", x, y += dy);
            PutInputField("MaxAngleOfAttackPitch", x, y += dy);
            PutInputField("MaxAngleOfAttackYaw", x, y += dy);
            PutInputField("AoaCurveStrength", x, y += dy);
            PutInputField("HighPitchAoaMinControl", x, y += dy);
            PutInputField("HighYawAoaMinControl", x, y += dy);
            PutToggle("DoStallForces", x, y += dy);
            PutInputField("PitchAoaPitchForceMulti", x, y += dy);
            PutInputField("YawAoaRollForceMulti", x, y += dy);
            PutInputField("HighPitchAoaMinLift", x, y += dy);
            PutInputField("HighYawAoaMinLift", x, y += dy);
            PutInputField("TaxiRotationSpeed", x, y += dy);
            PutInputField("TaxiRotationResponse", x, y += dy);
            PutToggle("DisallowTaxiRotationWhileStill", x, y += dy);
            PutInputField("TaxiFullTurningSpeed", x, y += dy);
            PutInputField("Lift", x, y += dy);
            PutInputField("SidewaysLift", x, y += dy);
            PutInputField("MaxLift", x, y += dy);
            PutInputField("VelLift", x, y += dy);
            PutInputField("VelLiftMax", x, y += dy);
            PutInputField("MaxGs", x, y += dy);
            PutInputField("GDamage", x, y += dy);
            PutInputField("GroundEffectMaxDistance", x, y += dy);
            PutInputField("GroundEffectStrength", x, y += dy);
            PutInputField("GroundEffectLiftMax", x, y += dy);
            PutInputField("VTOLAngleTurnRate", x, y += dy);
            PutInputField("VTOLDefaultValue", x, y += dy);
            PutToggle("VTOLAllowAfterburner", x, y += dy);
            PutInputField("VTOLThrottleStrengthMulti", x, y += dy);
            PutInputField("VTOLMinAngle", x, y += dy);
            PutInputField("VTOLMaxAngle", x, y += dy);
            PutInputField("VTOLPitchThrustVecMulti", x, y += dy);
            PutInputField("VTOLYawThrustVecMulti", x, y += dy);
            PutInputField("VTOLRollThrustVecMulti", x, y += dy);
            PutInputField("VTOLLoseControlSpeed", x, y += dy);
            PutInputField("VTOLGroundEffectStrength", x, y += dy);
            PutInputField("EnterVTOLEvent_Angle", x, y += dy);
            PutToggle("AutoAdjustValuesToMass", x, y += dy);
            x += dx;
            y = initY;
            PutInputField("SoundBarrierStrength", x, y += dy);
            PutInputField("SoundBarrierWidth", x, y += dy);
            PutInputField("Fuel", x, y += dy);
            PutInputField("LowFuel", x, y += dy);
            PutInputField("FuelConsumption", x, y += dy);
            PutInputField("MinFuelConsumption", x, y += dy);
            PutInputField("FuelConsumptionABMulti", x, y += dy);
            PutInputField("RefuelTime", x, y += dy);
            PutInputField("RepairTime", x, y += dy);
            PutInputField("RespawnDelay", x, y += dy);
            PutInputField("InvincibleAfterSpawn", x, y += dy);
            PutInputField("BulletDamageTaken", x, y += dy);
            PutToggle("PredictDamage", x, y += dy);
            PutInputField("SmallCrashSpeed", x, y += dy);
            PutInputField("MediumCrashSpeed", x, y += dy);
            PutInputField("BigCrashSpeed", x, y += dy);
            PutInputField("MissileDamageTakenMultiplier", x, y += dy);
            PutInputField("MissilePushForce", x, y += dy);
            PutInputField("SeaLevel", x, y += dy);
            PutInputField("AtmosphereThinningStart", x, y += dy);
            PutInputField("AtmosphereThinningEnd", x, y += dy);
            PutToggle("SquareJoyInput", x, y += dy);
        }

        private void PutInputField(string variableName, float x, float y)
        {
            PutLabel(variableName, x, y);
            PutValueText(variableName, x, y);

            string objectName = variableName + "InputField";

            GameObject gameObject = PrefabUtility.InstantiatePrefab(InputFieldPrefab) as GameObject;
            gameObject.transform.SetParent(InputParent);
            gameObject.name = objectName;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = new Vector3(x, y, 0);
            rectTransform.localScale = new Vector3(1, 1, 1);

            InputField inputField = gameObject.GetComponent<InputField>();
            UnityEditor.Events.UnityEventTools.RegisterStringPersistentListener(inputField.onValueChanged, 0, AircraftInspectorUB.SendCustomEvent, "NotifyChange");

            FieldInfo field = AirclaftInspectorType.GetField(objectName);
            if(field != null)
            {
                field.SetValue(AirclaftInspector, inputField);
            }
        }

        private void PutToggle(string variableName, float x, float y)
        {
            PutLabel(variableName, x, y);
            PutValueText(variableName, x, y);

            string objectName = variableName + "Toggle";

            GameObject gameObject = PrefabUtility.InstantiatePrefab(TogglePrefab) as GameObject;
            gameObject.transform.SetParent(InputParent);
            gameObject.name = objectName;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = new Vector3(x, y, 0);
            rectTransform.localScale = new Vector3(1, 1, 1);

            Toggle toggle = gameObject.GetComponent<Toggle>();
            UnityEditor.Events.UnityEventTools.RegisterStringPersistentListener(toggle.onValueChanged, 0, AircraftInspectorUB.SendCustomEvent, "NotifyChange");

            FieldInfo field = AirclaftInspectorType.GetField(objectName);
            if (field != null)
            {
                field.SetValue(AirclaftInspector, toggle);
            }
        }

        private void PutLabel(string variableName, float x, float y)
        {
            GameObject label = PrefabUtility.InstantiatePrefab(LabelPrefab) as GameObject;
            label.transform.SetParent(LabelParent);
            label.name = variableName + "Label";

            RectTransform rectTransform = label.GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = new Vector3(x, y, 0);
            rectTransform.localScale = new Vector3(1, 1, 1);

            Text text = label.transform.GetChild(0).gameObject.GetComponent<Text>();
            text.text = variableName;

            string methodName = "Show" + variableName + "Tooltip";
            Button button = label.GetComponent<Button>();
            UnityEditor.Events.UnityEventTools.RegisterStringPersistentListener(button.onClick, 0, AircraftInspectorUB.SendCustomEvent, methodName);
        }

        private void PutValueText(string variableName, float x, float y)
        {
            string objectName = variableName + "ValueText";

            GameObject gameObject = PrefabUtility.InstantiatePrefab(ValueTextPrefab) as GameObject;
            gameObject.transform.SetParent(ValueTextParent);
            gameObject.name = objectName;

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = new Vector3(x, y, 0);
            rectTransform.localScale = new Vector3(0.1f, 0.1f, 1);

            Text text = gameObject.GetComponent<Text>();
            text.text = "";

            FieldInfo field = AirclaftInspectorType.GetField(objectName);
            if (field != null)
            {
                field.SetValue(AirclaftInspector, text);
            }
        }

    }
}
#endif
