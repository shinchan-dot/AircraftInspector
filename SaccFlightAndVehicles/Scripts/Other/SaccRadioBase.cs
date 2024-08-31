
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

namespace SaccFlightAndVehicles
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SaccRadioBase : UdonSharpBehaviour
    {
        [Header("Vehicles must have SAV_Radio extension for this to work")]
        private SaccEntity[] _AllPlanes_ENT;
        private SAV_Radio[] _AllPlanes_RD;
        public float VoiceNear = 199999;
        public float VoiceFar = 200000;
        // public float VoiceVolumetric = 1500;
        public float VoiceGain = .05f;
        // public float VoiceLowPass;
        [Tooltip("Make this text object darker when radio is disabled. Not required.")]
        public TextMeshProUGUI RadioEnabledTxt;
        public bool RadioEnabled = true;
        private byte CurrentChannel = 1;
        public byte MyChannel = 1;
        [Header("All Planes and RadioZones are filled automatically on build.")]
        public Transform[] AllPlanes;
        public SaccRadioZone[] RadioZones;
        public TextMeshProUGUI ChannelText;
        [Header("Debug, leave empty:")]
        public SaccEntity MyVehicle;
        [System.NonSerialized] public int MyVehicleSetTimes;//number of times MyVehicle has been set (for when holding 2 objects with radio, and dropping one) //Used by SAV_Radio
        [System.NonSerialized] public SaccRadioZone MyZone;
        private int NextPlane;
        private int NextZone;
        private int NumZones;
        private bool DoZones = false;
        void Start()
        {
            SendCustomEventDelayedSeconds(nameof(SetRadioVoiceVolumes), 5);
            CurrentChannel = MyChannel;
            if (ChannelText) { ChannelText.text = MyChannel.ToString(); }
            _AllPlanes_ENT = new SaccEntity[AllPlanes.Length];
            _AllPlanes_RD = new SAV_Radio[AllPlanes.Length];
            for (int i = 0; i < AllPlanes.Length; i++)
            {
                _AllPlanes_ENT[i] = (SaccEntity)AllPlanes[i].GetComponent<SaccEntity>();
                if (_AllPlanes_ENT[i]) { _AllPlanes_RD[i] = (SAV_Radio)_AllPlanes_ENT[i].GetExtention("SAV_Radio"); }
            }
            NumZones = RadioZones.Length;
            if (NumZones != 0) { DoZones = true; }
        }
        public void SetRadioVoiceVolumes()
        {
            SendCustomEventDelayedFrames(nameof(SetRadioVoiceVolumes), 5);
            if ((!MyVehicle || !RadioEnabled) && !MyZone) { return; }
            if (DoZones)
            { SendCustomEventDelayedFrames(nameof(SetRadioVoiceVolumes_Zones), 2); }//separate in frames for optimization
            NextPlane++;
            if (NextPlane == _AllPlanes_RD.Length) { NextPlane = 0; }
            if (_AllPlanes_RD[NextPlane])
            {
                if (MyVehicle == _AllPlanes_ENT[NextPlane]
                    || (byte)_AllPlanes_RD[NextPlane].Channel != CurrentChannel
                    || CurrentChannel == 0) { return; }
                for (int o = 0; o < _AllPlanes_ENT[NextPlane].VehicleSeats.Length; o++)
                {
                    VRCPlayerApi thisplayer = _AllPlanes_ENT[NextPlane].VehicleSeats[o].SeatedPlayer;
                    if (thisplayer != null)
                    {
                        thisplayer.SetVoiceDistanceNear(VoiceNear);
                        thisplayer.SetVoiceDistanceFar(VoiceFar);
                        thisplayer.SetVoiceGain(VoiceGain);
                    }
                }
                if (_AllPlanes_ENT[NextPlane].EntityPickup && _AllPlanes_ENT[NextPlane].EntityPickup.IsHeld)
                {
                    VRCPlayerApi thisplayer = Networking.GetOwner(_AllPlanes_ENT[NextPlane].gameObject);
                    if (thisplayer != null)
                    {
                        thisplayer.SetVoiceDistanceNear(VoiceNear);
                        thisplayer.SetVoiceDistanceFar(VoiceFar);
                        thisplayer.SetVoiceGain(VoiceGain);
                    }
                }
            }
        }
        public void SetRadioVoiceVolumes_Zones()
        {
            if ((!MyVehicle || !RadioEnabled) && !MyZone) { return; }
            NextZone++;
            if (NextZone >= NumZones) { NextZone = 0; }
            SaccRadioZone NextRZ = RadioZones[NextZone];
            VRCPlayerApi[] RZ_players = NextRZ.playersinside;
            if (CurrentChannel != NextRZ.Channel)
            {
                for (int i = 0; i < NextRZ.numPlayersInside; i++)
                {
                    RZ_players[i].SetVoiceDistanceNear(0);
                    RZ_players[i].SetVoiceDistanceFar(25);
                    RZ_players[i].SetVoiceGain(15);
                }
                return;
            }
            if (NextRZ != MyZone)
            {
                for (int i = 0; i < NextRZ.numPlayersInside; i++)
                {
                    RZ_players[i].SetVoiceDistanceNear(VoiceNear);
                    RZ_players[i].SetVoiceDistanceFar(VoiceFar);
                    RZ_players[i].SetVoiceGain(VoiceGain);
                }
            }
        }
        public void SetAllVoiceVolumesDefault()
        {
            VRCPlayerApi[] AllPlayers = new VRCPlayerApi[100];
            VRCPlayerApi.GetPlayers(AllPlayers);
            int numplayers = VRCPlayerApi.GetPlayerCount();
            for (int i = 0; i < numplayers; i++)
            {
                AllPlayers[i].SetVoiceDistanceNear(0);
                AllPlayers[i].SetVoiceDistanceFar(25);
                AllPlayers[i].SetVoiceGain(15);
            }
        }
        private int SetSingleVoiceVolumeID;
        public void SetSingleVoiceVolumeDefault()
        {
            VRCPlayerApi SingleVV = VRCPlayerApi.GetPlayerById(SetSingleVoiceVolumeID);
            if (SingleVV == null) { return; }
            SingleVV.SetVoiceDistanceNear(0);
            SingleVV.SetVoiceDistanceFar(25);
            SingleVV.SetVoiceGain(15);
        }
        public void ToggleRadio()
        {
            RadioEnabled = !RadioEnabled;
            if (RadioEnabledTxt) RadioEnabledTxt.color = RadioEnabled ? Color.white : Color.gray;
        }
        public void IncreaseChannel()
        {
            if (MyChannel + 1 > 16) { return; }
            MyChannel++;
            if (ChannelText) { ChannelText.text = MyChannel.ToString(); }
            CurrentChannel = MyChannel;
        }
        public void DecreaseChannel()
        {
            if (MyChannel - 1 < 1) { return; }
            MyChannel--;
            if (ChannelText) { ChannelText.text = MyChannel.ToString(); }
            CurrentChannel = MyChannel;
        }
    }
}