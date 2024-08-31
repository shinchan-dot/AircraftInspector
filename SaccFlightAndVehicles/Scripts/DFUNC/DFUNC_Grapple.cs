﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace SaccFlightAndVehicles
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class DFUNC_Grapple : UdonSharpBehaviour
    {
        public Animator GrappleAnimator;
        public Transform Hook;
        public Transform HookRopePoint;
        [Tooltip("Object enabled when function is active (used on MFD)")]
        public GameObject Dial_Funcon;
        public float HookSpeed = 300f;
        public float SwingStrength = 20f;
        [System.NonSerialized] public SaccFlightAndVehicles.SaccEntity EntityControl;
        private Rigidbody VehicleRB;
        [Tooltip("Hook launches from here, in this transform's forward direction")]
        public Transform HookLaunchPoint;
        [Tooltip("Pull grappled rigidbodies towards the vehicle?")]
        public bool TwoWayForces = true;
        [Tooltip("Force is applied by the client in the vehicle that got hooked, less technical issues, but very janky movement. Requires TwoWayForces_LocalForceMode to be false.")]
        public bool TwoWayForces_LocalForceMode = true;
        [Tooltip("Don't apply two way forces if someone else is in the vehicle")]
        public bool TwoWayForces_DisableIfOccupied = false;
        [Tooltip("Apply the forces to this vehicle at ForceApplyPoint or CoM?")]
        public bool UseForceApplyPoint = true;
        [Tooltip("Apply the forces at this point, requires tickbox above to be ticked")]
        public Transform ForceApplyPoint;
        public float PullReductionStrength = 5f;
        [Tooltip("Snap rigidbody target connection points to just above their CoM?")]
        public bool HoldTargetUpright = false;
        [Tooltip("Select the function instead of just instantly firing it with keyboard input?")]
        public bool ResetHookOnExitVehicle = false;
        public LineRenderer Rope_Line;
        public Transform RopeBasePoint;
        public LayerMask HookLayers;
        public float HookStrength = 10f;
        public float MaxExtraStrByDist = 10f;
        public float HookRange = 340f;
        public float SphereCastAccuracy = 0.125f;
        public AnimationCurve PullStrOverDist;
        public AudioSource HookLaunch;
        public AudioSource HookAttach;
        public AudioSource HookAttachConfirm;
        public AudioSource HookReelIn;
        public GameObject[] EnableOnSelect;
        public bool HandHeldGunMode;
        [Tooltip("Apply force at player instead of gun (prevent player from 'pulling' themself faster for competitive)")]
        public bool DisablePulling = false;
        [Tooltip("If player drops gun in the air, teleport it to them until they land?")]
        public bool MoveGunToSelfOnAirDrop = true;
        [Tooltip("How heavy the player is for interactions with rigidbodies")]
        public float PlayerMass = 150f;
        [Tooltip("Disable these objects whilst object is held (SaccFlight?)")]
        public GameObject[] DisableOnPickup;
        [Tooltip("Object that points towards launch direction")]
        public Transform TargetingLaser;
        private float HookLaunchTime;
        public Transform PredictedHitPoint;
        public bool KeyboardSelectMode = false;
        [Tooltip("Run an event on objects that the grappling hook hits?")]
        public bool RunEventOnTargets = false;
        [Tooltip("Run Event on any triggers the hook flies through?")]
        public bool RunEventOnTargets_Triggers = false;
        [Tooltip("Name of event to run")]
        public string HitEventName = "_interact";
        public string HitEventName_Trigger = "_interact";
        private float AprHookFlyTime;
        private bool DoHitPrediction;
        private Vector3 HookStartPos;
        private Transform HookedTransform;
        private Vector3 HookedTransformOffset;
        private Collider HookedCollider;
        private GameObject HookedGameObject;
        private Rigidbody HookedRB;
        private VRC_Pickup EntityPickup;
        private bool InVr;
        private VRCPlayerApi localPlayer;
        private SaccFlightAndVehicles.SaccEntity HookedEntity;
        //these 2 variables are only used if TwoWayForces_LocalForceMode is true
        private bool NonLocalAttached;//if you are in a vehicle that is attached
        private bool PlayReelIn = true;
        private bool Occupied = false;
        private bool LeftDial = false;
        private int DialPosition = -999;
        private bool KeepingHEAwake = false;
        private bool Overriding_DisallowOwnerShipTransfer = false;
        private Vector3 PredictedHitPointStartScale;
        private Vector3 HoldHeight;
        public override void OnDeserialization()
        {
            if (_HookLaunched != _HookLaunchedPrev)
            {
                HookLaunched = _HookLaunchedPrev = _HookLaunched;
            }
            if (_HookAttachPoint != _HookAttachPointPrev)
            {
                HookAttachPoint = _HookAttachPointPrev = _HookAttachPoint;
            }
        }
        private Vector3 _HookAttachPointPrev;
        [UdonSynced] private Vector3 _HookAttachPoint;
        public Vector3 HookAttachPoint
        {
            set
            {
                if (!Initialized || !_HookLaunched) { _HookAttachPoint = value; return; }

                //first just check if there's a collider at the exact coordinates
                Vector3 rayDir = (value - HookLaunchPoint.position).normalized * .1f;
                RaycastHit firstRay;
                bool firstrayhit = Physics.Raycast(value - rayDir, rayDir, out firstRay, .11f, HookLayers, QueryTriggerInteraction.Ignore);

                float spheresize = SphereCastAccuracy;
                int hitlen = 0;
                RaycastHit[] hits = new RaycastHit[0];
                bool hitSelf = false;
                while (spheresize < 17 && hitlen == 0 && !hitSelf)
                {
                    hits = Physics.SphereCastAll(value, spheresize, Vector3.up, 0, HookLayers, QueryTriggerInteraction.Ignore);
                    spheresize *= 2;
                    RaycastHit[] hitstemp = new RaycastHit[hits.Length + 1];
                    if (firstrayhit)//add the collider at the coordinates to the list to check against
                    {
                        hits.CopyTo(hitstemp, 0);
                        hitstemp[hits.Length] = firstRay;
                        hits = hitstemp;
                    }

                    hitlen = hits.Length;
                    if (hitlen > 0)
                    {
                        HookedTransform = null;
                        if (Dial_Funcon) { Dial_Funcon.SetActive(true); }
                        foreach (RaycastHit hit in hits)
                        {
                            hitSelf = false;
                            if (hit.collider)
                            {
                                float NearestDist = float.MaxValue;
                                float tempdist = Vector3.Distance(hit.collider.ClosestPoint(value), value);
                                if (tempdist < NearestDist)
                                {
                                    if (HookedEntity) { UndoHookOverrides(); }
                                    if (hit.collider.attachedRigidbody)
                                    {
                                        HookedEntity = hit.collider.attachedRigidbody.GetComponent<SaccFlightAndVehicles.SaccEntity>();
                                        if (HookedEntity)
                                        {
                                            if (HookedEntity == EntityControl) { hitSelf = true; continue; } //skip if raycast finds own vehicle
                                            else
                                            {
                                                if (!KeepingHEAwake)
                                                {
                                                    KeepingHEAwake = true;
                                                    HookedEntity.KeepAwake_++;
                                                    HookedEntity.SendEventToExtensions("SFEXT_L_GrappleAttach");
                                                }
                                            }
                                        }
                                    }
                                    else { HookedEntity = null; }
                                    NearestDist = tempdist;
                                    HookedCollider = hit.collider;
                                    HookedGameObject = hit.collider.gameObject;
                                    HookedTransform = hit.collider.transform;
                                    HookedTransformOffset = HookedTransform.InverseTransformPoint(hit.collider.ClosestPoint(value));
                                    if (TwoWayForces && (!HookedEntity || (!TwoWayForces_DisableIfOccupied || (!HookedEntity.Occupied && (!HookedEntity.EntityPickup || !HookedEntity.EntityPickup.IsHeld)))))
                                    {
                                        HookedRB = HookedCollider.attachedRigidbody;
                                        if (HookedRB)
                                        {
                                            if (HoldTargetUpright)
                                            {
                                                HookedTransform = HookedRB.transform;
                                                Vector3 targCoMPos = HookedRB.position + HookedRB.centerOfMass;
                                                Vector3 abovedist = (HookedTransform.up * Vector3.Distance(targCoMPos, HookLaunchPoint.position) /* / 2f */);
                                                Vector3 raypoint = targCoMPos + abovedist;
                                                Vector3 raydir = targCoMPos - raypoint;
                                                RaycastHit hit2;
                                                if (Physics.Raycast(raypoint, raydir, out hit2, abovedist.magnitude + 10f, HookLayers, QueryTriggerInteraction.Ignore))
                                                {
                                                    HookedTransformOffset = HookedTransform.InverseTransformPoint(hit2.point);
                                                }
                                            }
                                            if (TwoWayForces_LocalForceMode)
                                            {
                                                if (IsOwner)
                                                {
                                                    if ((!EntityPickup || !EntityPickup.IsHeld) && (!HookedEntity || (!HookedEntity.Occupied && (!HookedEntity.EntityPickup || !HookedEntity.EntityPickup.IsHeld))))
                                                    {
                                                        if (!Overriding_DisallowOwnerShipTransfer)
                                                        {
                                                            Networking.SetOwner(Networking.LocalPlayer, HookedRB.gameObject);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (Networking.LocalPlayer.IsOwner(HookedRB.gameObject))
                                                    {
                                                        NonLocalAttached = true;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (IsOwner && !Overriding_DisallowOwnerShipTransfer)
                                                {
                                                    Networking.SetOwner(Networking.LocalPlayer, HookedRB.gameObject);
                                                }
                                            }
                                            //people cant take ownership while vehicle is being held.
                                            //localforcemode is only active if someone is in the vehicle when its grabbed
                                            if (HookedEntity && (!TwoWayForces_LocalForceMode || (!HookedEntity.Occupied && (!HookedEntity.EntityPickup || !HookedEntity.EntityPickup.IsHeld))))
                                            {
                                                if (!Overriding_DisallowOwnerShipTransfer)
                                                {
                                                    HookedEntity.SetProgramVariable("DisallowOwnerShipTransfer", (int)HookedEntity.GetProgramVariable("DisallowOwnerShipTransfer") + 1);
                                                    Overriding_DisallowOwnerShipTransfer = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!HookedTransform)//nothing was found (terrain colliders dont work with spherecastall?)
                        {
                            HookWorldPos(value);
                        }
                    }
                    else
                    {//extreme lag/late joiners if object targeted was in air/sea/not near anything (or if trying to lock terrain apparently)
                        HookWorldPos(value);
                    }
                }
                Vector3 hookedpoint = HookedTransform.TransformPoint(HookedTransformOffset);
                HookLength = Vector3.Distance(HookLaunchPoint.position, hookedpoint);
                HookAttached = true;
                SkippedFrames = 1;
                SetHookPos();
                HookAttach.Play();
                _HookAttachPoint = hookedpoint;
            }
            get => _HookAttachPoint;
        }
        private void HookWorldPos(Vector3 val)
        {
            HookedTransform = transform.root;//should be a non-moving object
            HookedTransformOffset = HookedTransform.InverseTransformPoint(val);
            HookedCollider = null;
            HookedRB = null;
        }
        public void SetHookPos()
        {
            if (HookAttached)
            {
                if (IsOwner)
                {
                    //jittery if done in fixedupdate
                    if (Rope_Line)
                    {
                        Rope_Line.SetPosition(0, RopeBasePoint.position);
                        Rope_Line.SetPosition(1, HookRopePoint.position);
                    }
                }
                else
                {
                    if ((HookedCollider && !HookedCollider.enabled) || !HookedTransform.gameObject.activeInHierarchy) { ResetHook(); return; }
                    Hook.position = HookedTransform.TransformPoint(HookedTransformOffset);
                    if (Rope_Line)
                    {
                        Rope_Line.SetPosition(0, RopeBasePoint.position);
                        Rope_Line.SetPosition(1, HookRopePoint.position);
                    }
                }
                SendCustomEventDelayedFrames(nameof(SetHookPos), 1);
            }
        }
        private Quaternion HookStartRot;
        private Quaternion HookLaunchRot;
        private float HookLength;
        private bool HookAttached = false;
        private Vector3 LaunchVec;
        private float LaunchSpeed;
        private Transform HookParentStart;
        private bool IsOwner;
        private bool Initialized;
        private bool _HookLaunchedPrev;
        [UdonSynced] private bool _HookLaunched;
        public bool HookLaunched
        {
            set
            {
                if (value)
                {
                    _HookLaunched = value;
                    if (!HookAttached)
                    { LaunchHook(); }
                }
                else
                {
                    ResetHook();
                    _HookLaunched = value;
                    if (!Occupied && !IsOwner) { gameObject.SetActive(false); }
                }
            }
            get => _HookLaunched;
        }
        public void SFEXT_L_EntityStart()
        {
            Initialized = true;
            if (!ForceApplyPoint) { ForceApplyPoint = HookLaunchPoint; }
            VehicleRB = EntityControl.GetComponent<Rigidbody>();
            IsOwner = (bool)EntityControl.GetProgramVariable("IsOwner");
            EntityPickup = EntityControl.GetComponent<VRC_Pickup>();
            HookedTransform = transform;//avoid null
            HookParentStart = Hook.parent;
            HookStartPos = Hook.localPosition;
            HookStartRot = Hook.localRotation;
            if (PredictedHitPoint) { PredictedHitPointStartScale = PredictedHitPoint.localScale; }
            localPlayer = Networking.LocalPlayer;
            InVr = localPlayer.IsUserInVR();
            if (Dial_Funcon) Dial_Funcon.SetActive(false);
            foreach (GameObject obj in EnableOnSelect) { obj.SetActive(false); }
            FindSelf();
            gameObject.SetActive(true);
            SendCustomEventDelayedSeconds(nameof(DisableThis), 10f);
            InitializeChangeableValues();
        }
        public void InitializeChangeableValues()
        {
            AprHookFlyTime = HookRange / HookSpeed;
            DoHitPrediction = PredictedHitPoint || GrappleAnimator;
        }
        public void LaunchHook()
        {
            if (!Initialized) { return; }
            Rope_Line.gameObject.SetActive(true);
            HookLaunchTime = Time.time;
            HookLaunchRot = Hook.rotation;
            LaunchVec = ((HandHeldGunMode ? localPlayer.GetVelocity() : VehicleRB ? VehicleRB.velocity : Vector3.zero)) + (HookLaunchPoint.forward * HookSpeed);
            LaunchSpeed = LaunchVec.magnitude;
            Hook.parent = EntityControl.transform.parent;
            Hook.position = HookLaunchPoint.position;
            HookFlyLoop();
            HookLaunch.Play();
            //Something can probably be done here to do with HookFlyLoops()'s timing to make it look smoother when fired at high speed
        }
        public void HookFlyLoop()
        {
            if (!HookLaunched || HookAttached)
            {
                return;
            }
            RaycastHit hookhit;
            if (IsOwner)
            {
                if (RunEventOnTargets_Triggers)
                {
                    if (Physics.Raycast(Hook.position, LaunchVec, out hookhit, LaunchSpeed * Time.deltaTime, HookLayers, QueryTriggerInteraction.Collide))
                    {
                        if (hookhit.collider)//in vrc this will be null if it hits a player
                        {
                            UdonSharpBehaviour hitScript = (UdonSharpBehaviour)hookhit.collider.gameObject.GetComponent(typeof(UdonSharpBehaviour));
                            if (hitScript) { hitScript.SendCustomEvent(HitEventName_Trigger); }
                        }
                    }
                }
                if (Physics.Raycast(Hook.position, LaunchVec, out hookhit, LaunchSpeed * Time.deltaTime, HookLayers, QueryTriggerInteraction.Ignore))
                {
                    float checklength;
                    if (HandHeldGunMode && DisablePulling)
                    { checklength = Vector3.Distance(localPlayer.GetPosition() + HoldHeight, hookhit.point); }
                    else
                    { checklength = Vector3.Distance(HookLaunchPoint.position, hookhit.point); }
                    if (checklength < HookRange)
                    {
                        if (RunEventOnTargets)
                        {
                            UdonSharpBehaviour hitScript;
                            Rigidbody ARB = hookhit.collider.attachedRigidbody;
                            if (ARB)
                            { Debug.Log("ASS"); hitScript = (UdonSharpBehaviour)ARB.GetComponent(typeof(UdonSharpBehaviour)); }
                            else
                            { Debug.Log("TITS"); hitScript = (UdonSharpBehaviour)hookhit.collider.gameObject.GetComponent(typeof(UdonSharpBehaviour)); }
                            Debug.Log(hitScript);
                            if (hitScript) { hitScript.SendCustomEvent(HitEventName); }
                        }
                        HookAttachPoint = hookhit.point;
                        if (HookAttachConfirm) { HookAttachConfirm.Play(); }
                        RequestSerialization();
                        Hook.position = hookhit.point;
                        if (HandHeldGunMode && DisablePulling)
                        { HookLength = Vector3.Distance(localPlayer.GetPosition() + HoldHeight, _HookAttachPoint); }
                        else
                        { HookLength = Vector3.Distance(HookLaunchPoint.position, Hook.position); }
                        return;
                    }
                }
                Hook.position += LaunchVec * Time.deltaTime;
                if (HandHeldGunMode && DisablePulling)
                { HookLength = Vector3.Distance(localPlayer.GetPosition() + HoldHeight, Hook.position); }
                else
                { HookLength = Vector3.Distance(HookLaunchPoint.position, Hook.position); }
                if (HookLength > HookRange)
                {
                    HookLaunched = false;
                    if (!Occupied) { SendCustomEventDelayedSeconds(nameof(DisableThis), 2f); }
                    RequestSerialization();
                    return;
                }
            }
            else
            {
                Hook.position += LaunchVec * Time.deltaTime;
            }
            if (Rope_Line)
            {
                Rope_Line.SetPosition(0, RopeBasePoint.position);
                Rope_Line.SetPosition(1, HookRopePoint.position);
            }
            SendCustomEventDelayedFrames(nameof(HookFlyLoop), 1);
        }
        private void UndoHookOverrides()
        {
            if (Overriding_DisallowOwnerShipTransfer)
            {
                HookedEntity.SetProgramVariable("DisallowOwnerShipTransfer", (int)HookedEntity.GetProgramVariable("DisallowOwnerShipTransfer") - 1);
                Overriding_DisallowOwnerShipTransfer = false;
            }
        }
        public void ResetHook()
        {
            if (Dial_Funcon) { Dial_Funcon.SetActive(false); }
            Rope_Line.gameObject.SetActive(false);
            Hook.parent = HookParentStart;
            HookAttached = false;
            NonLocalAttached = false;
            Hook.localPosition = HookStartPos;
            Hook.localRotation = HookStartRot;
            if (HookedEntity)
            {
                HookedEntity.SendEventToExtensions("SFEXT_L_GrappleDetach");
                if (KeepingHEAwake)
                {
                    KeepingHEAwake = false;
                    HookedEntity.KeepAwake_--;
                }
                UndoHookOverrides();
                if (HookedEntity.Using)
                {
                    Networking.SetOwner(Networking.LocalPlayer, HookedEntity.gameObject);
                    HookedEntity.SendEventToExtensions("SFEXT_L_SetEngineOn");
                }
                HookedEntity = null;
            }
            if (PlayReelIn && HookReelIn) { HookReelIn.Play(); }
            if (HookAttach && HookAttach.isPlaying) { HookAttach.Stop(); }
        }
        public void UpdateRopeLine()
        {
            if (Rope_Line)
            {
                Rope_Line.SetPosition(0, RopeBasePoint.position);
                Rope_Line.SetPosition(1, HookRopePoint.position);
            }
        }
        private int SkippedFrames; // delete me if bug is fixed
        private Vector3 PlayerPosLast; // delete me if bug is fixed
        private void FixedUpdate()
        {
            if (HookAttached && IsOwner)
            {
                if ((HookedCollider && !HookedCollider.enabled)
                 || !HookedTransform.gameObject.activeInHierarchy
                 || (HookedEntity && (HookedEntity.dead)))
                {
                    HookLaunched = false;
                    RequestSerialization(); return;
                }
                Hook.position = _HookAttachPoint = HookedTransform.TransformPoint(HookedTransformOffset);

                float dist;
                Vector3 forceDirection;
                Vector3 holdpos;
                int _skippedFrames = 1;
                Vector3 playerpos = localPlayer.GetPosition();
                // this 'SkippedFrames' business is to workaround this bug. It's not perfect but it works well enough
                // https://feedback.vrchat.com/vrchat-udon-closed-alpha-feedback/p/vrcplayerapis-physics-values-are-only-updated-in-update
                // Remove if fixed
                if (HandHeldGunMode)
                {
                    if (playerpos == PlayerPosLast)
                    {
                        if (Time.fixedDeltaTime * (SkippedFrames + 1) > 0.0501f) { return; }
                        SkippedFrames = SkippedFrames++; return;
                    }
                    else
                    {
                        _skippedFrames = SkippedFrames;
                        SkippedFrames = 1;
                    }
                    PlayerPosLast = playerpos;
                }
                if (HandHeldGunMode && DisablePulling)
                {
                    holdpos = localPlayer.GetPosition() + HoldHeight;
                    dist = Vector3.Distance(holdpos, _HookAttachPoint);
                    forceDirection = (_HookAttachPoint - holdpos).normalized * _skippedFrames;//simpler just to pre-multiply this than do it later
                }
                else
                {
                    holdpos = HookLaunchPoint.position;
                    dist = Vector3.Distance(holdpos, _HookAttachPoint);
                    forceDirection = (_HookAttachPoint - HookLaunchPoint.position).normalized * _skippedFrames;//simpler just to pre-multiply this than do it later
                }
                float PullReduction = 0f;

                float SwingForce = (dist - HookLength) / _skippedFrames;//this measures the difference between frames so it has 'deltatime' including skipped frames built in, so we need to remove the pre-multiplication
                if (SwingForce < 0)
                {
                    PullReduction = SwingForce;
                    SwingForce = 0;
                }
                else { SwingForce *= SwingStrength; }
                HookLength = Mathf.Min(dist, HookRange);

                float WeightRatio = 1;

                if (HookedRB && !HookedRB.isKinematic)
                {
                    if (HandHeldGunMode)
                    {
                        WeightRatio = HookedRB.mass / (HookedRB.mass + PlayerMass);
                    }
                    else
                    {
                        if (!VehicleRB || VehicleRB.isKinematic)
                        { WeightRatio = 0f; }
                        else
                        { WeightRatio = HookedRB.mass / (HookedRB.mass + VehicleRB.mass); }
                    }
                    Vector3 forceDirection_HookedRB = -forceDirection;
                    HookedRB.AddForceAtPosition((forceDirection_HookedRB * HookStrength * PullStrOverDist.Evaluate(dist) * Time.fixedDeltaTime + (forceDirection_HookedRB * SwingForce)) * (1f - WeightRatio), _HookAttachPoint, ForceMode.VelocityChange);
                }
                if (HandHeldGunMode)
                {
                    Vector3 newPlayerVelocity = localPlayer.GetVelocity() + (((forceDirection * HookStrength * Time.fixedDeltaTime) + (forceDirection * SwingForce)) * WeightRatio * PullStrOverDist.Evaluate(dist));
#if UNITY_EDITOR
                    //SetVelocity overrides all other forces in clientsim so we need to add gravity ourselves
                    newPlayerVelocity += -Vector3.up * 9.81f * Time.fixedDeltaTime;
#endif
                    localPlayer.SetVelocity(newPlayerVelocity);
                }
                else if (VehicleRB)
                {
                    if (UseForceApplyPoint)
                    {
                        VehicleRB.AddForceAtPosition((forceDirection * HookStrength * PullStrOverDist.Evaluate(dist) * Time.fixedDeltaTime + (forceDirection * SwingForce) + (forceDirection * PullReduction * PullReductionStrength)) * WeightRatio, ForceApplyPoint.position, ForceMode.VelocityChange);
                    }
                    else
                    {
                        VehicleRB.AddForce((forceDirection * HookStrength * PullStrOverDist.Evaluate(dist) * Time.fixedDeltaTime + (forceDirection * SwingForce) + (forceDirection * PullReduction * PullReductionStrength)) * WeightRatio, ForceMode.VelocityChange);
                    }
                }

            }
            else if (NonLocalAttached)
            {
                if (HookedRB)
                {
                    float dist = Vector3.Distance(HookLaunchPoint.position, _HookAttachPoint);
                    float PullReduction = 0f;

                    float SwingForce = dist - HookLength;
                    if (SwingForce < 0)
                    {
                        PullReduction = SwingForce;
                        SwingForce = 0;
                    }
                    else { SwingForce *= SwingStrength; }
                    HookLength = dist;

                    Vector3 forceDirection = (_HookAttachPoint - HookLaunchPoint.position).normalized;

                    float WeightRatio;
                    if (HandHeldGunMode)
                    {
                        WeightRatio = HookedRB.mass / (HookedRB.mass + PlayerMass);
                    }
                    else
                    {
                        if (!VehicleRB || VehicleRB.isKinematic)
                        { WeightRatio = 0f; }
                        else
                        { WeightRatio = HookedRB.mass / (HookedRB.mass + VehicleRB.mass); }
                    }
                    Vector3 forceDirection_HookedRB = (HookLaunchPoint.position - _HookAttachPoint).normalized;
                    HookedRB.AddForceAtPosition((forceDirection_HookedRB * HookStrength * PullStrOverDist.Evaluate(dist) * Time.deltaTime + (forceDirection_HookedRB * SwingForce) + (forceDirection_HookedRB * PullReduction * PullReductionStrength)) * (1f - WeightRatio), _HookAttachPoint, ForceMode.VelocityChange);
                }
            }
        }
        public void SFEXT_G_Explode()
        {
            if (IsOwner)
            {
                if (_HookLaunched)
                { HookLaunched = false; RequestSerialization(); }
            }
            //make sure this happens because the one in the HookLaunched Set may not be reliable because synced variables are faster than events
            SendCustomEventDelayedSeconds(nameof(DisableThis), 2f);
        }
        public void DisableThis() { if (!Occupied && !_HookLaunched && (!EntityPickup || !EntityPickup.IsHeld)) { gameObject.SetActive(false); } }
        public void SFEXT_O_PilotExit()
        {
            Selected = false;
            if (ResetHookOnExitVehicle)
            {
                if (_HookLaunched)
                { ResetHook(); }
            }
            if (!InVr && !KeyboardSelectMode) { foreach (GameObject obj in EnableOnSelect) { obj.SetActive(false); } }
        }
        public void SFEXT_O_TakeOwnership()
        {
            if (HandHeldGunMode && _HookLaunched)
            { ResetHook(); }
            IsOwner = true;
        }
        public void SFEXT_O_LoseOwnership()
        {
            IsOwner = false;
        }
        public void SFEXT_O_PilotEnter()
        {
            if (!InVr && !KeyboardSelectMode) { foreach (GameObject obj in EnableOnSelect) { obj.SetActive(true); } }
        }
        public void SFEXT_G_PilotEnter()
        {
            Occupied = true;
            gameObject.SetActive(true);
        }
        public void SFEXT_G_RespawnButton()
        {
            gameObject.SetActive(true);
            SendCustomEventDelayedSeconds(nameof(DisableThis), 2f);
        }
        public void SFEXT_O_RespawnButton()
        {
            PlayReelIn = false;
            HookLaunched = false;
            PlayReelIn = true;
            RequestSerialization();
        }
        public void SFEXT_G_PilotExit()
        {
            Occupied = false;
            if (!_HookLaunched)
            { SendCustomEventDelayedSeconds(nameof(DisableThis), 2f); }
        }
        public void KeyboardInput()
        {
            if (KeyboardSelectMode)
            {
                if (LeftDial)
                {
                    if (EntityControl.LStickSelection == DialPosition)
                    { EntityControl.LStickSelection = -1; }
                    else
                    { EntityControl.LStickSelection = DialPosition; }
                }
                else
                {
                    if (EntityControl.RStickSelection == DialPosition)
                    { EntityControl.RStickSelection = -1; }
                    else
                    { EntityControl.RStickSelection = DialPosition; }
                }
            }
            else
            {
                FireHook();
            }
        }
        public void SFEXT_O_OnPickup()
        {
            HoldHeight = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position - localPlayer.GetPosition();//imrpove this if there's ever a way to get player avatar height or something
            if (_HookLaunched) { ResetHook(); }
            SFEXT_O_PilotEnter();
            for (int i = 0; i < DisableOnPickup.Length; i++)
            {
                if (DisableOnPickup[i]) { DisableOnPickup[i].SetActive(false); }
            }
            if (GrappleAnimator) { GrappleAnimator.SetBool("held", true); }
        }
        public void SFEXT_O_OnDrop()
        {
            SFEXT_O_PilotExit();
            if (_HookLaunched)
            { HookLaunched = false; RequestSerialization(); }
            PredictionOff();
            SendCustomEventDelayedSeconds(nameof(DisableThis), 2f);
            for (int i = 0; i < DisableOnPickup.Length; i++)
            {
                if (DisableOnPickup[i]) { DisableOnPickup[i].SetActive(true); }
            }
            if (GrappleAnimator) { GrappleAnimator.SetBool("held", false); }
            if (MoveGunToSelfOnAirDrop && !localPlayer.IsPlayerGrounded())
            {
                TeleportingGunToMe = true;
                TelePortGunToMe();
            }
        }
        private bool TeleportingGunToMe;
        public void TelePortGunToMe()
        {
            if (TeleportingGunToMe && !EntityControl.Holding && !localPlayer.IsPlayerGrounded())
            {
                SendCustomEventDelayedFrames(nameof(TelePortGunToMe), 1, VRC.Udon.Common.Enums.EventTiming.LateUpdate);
                var head = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
                float tall = Vector3.Distance(localPlayer.GetPosition(), head.position);
                EntityControl.transform.position = head.position + ((head.rotation * Vector3.forward) * tall * .5f);
            }
            else { TeleportingGunToMe = false; }
        }
        public void SFEXT_G_OnPickup()
        {
            SFEXT_G_PilotEnter();
        }
        public void SFEXT_G_OnDrop()
        {
            SFEXT_G_PilotExit();
        }
        public void SFEXT_O_OnPickupUseDown()
        {
            KeyboardInput();
        }
        public void SFEXT_O_OnPickupUseUp()
        {
            if (_HookLaunched)
            { HookLaunched = false; RequestSerialization(); }
        }
        public void FireHook()
        {
            HookLaunched = !HookLaunched;
            RequestSerialization();
        }
        private void FindSelf()
        {
            int x = 0;
            foreach (UdonSharpBehaviour usb in EntityControl.Dial_Functions_R)
            {
                if (this == usb)
                {
                    DialPosition = x;
                    return;
                }
                x++;
            }
            LeftDial = true;
            x = 0;
            foreach (UdonSharpBehaviour usb in EntityControl.Dial_Functions_L)
            {
                if (this == usb)
                {
                    DialPosition = x;
                    return;
                }
                x++;
            }
            DialPosition = -999;
            Debug.LogWarning("DFUNC_AAM: Can't find self in dial functions");
        }
        public void DFUNC_LeftDial() { UseLeftTrigger = true; }
        public void DFUNC_RightDial() { UseLeftTrigger = false; }
        private bool TriggerLastFrame;
        private bool Selected;
        private bool UseLeftTrigger;
        public void DFUNC_Selected()
        {
            Selected = true;
            foreach (GameObject obj in EnableOnSelect) { obj.SetActive(true); }
        }
        public void DFUNC_Deselected()
        {
            Selected = false;
            foreach (GameObject obj in EnableOnSelect) { obj.SetActive(false); }
        }
        private void Update()
        {
            if (Selected)
            {
                if (!HandHeldGunMode)
                {
                    float Trigger;
                    if (UseLeftTrigger)
                    { Trigger = Input.GetAxisRaw("Oculus_CrossPlatform_PrimaryIndexTrigger"); }
                    else
                    { Trigger = Input.GetAxisRaw("Oculus_CrossPlatform_SecondaryIndexTrigger"); }
                    if (Trigger > 0.75 || Input.GetKey(KeyCode.Space))
                    {
                        if (!TriggerLastFrame)
                        {
                            FireHook();
                        }
                        TriggerLastFrame = true;
                    }
                    else { TriggerLastFrame = false; }
                }
                if (DoHitPrediction)
                {
                    Vector3 playervel = localPlayer.GetVelocity();
                    Vector3 checkdir = (((HandHeldGunMode ? playervel : VehicleRB ? VehicleRB.velocity : Vector3.zero)) + (HookLaunchPoint.forward * HookSpeed)).normalized;
                    RaycastHit targetcheck;
                    Vector3 predictedhookflight = checkdir * HookRange + playervel * AprHookFlyTime;
                    float predictedflightdist = predictedhookflight.magnitude;
                    if (DisablePulling && HandHeldGunMode) { predictedflightdist -= (HookLaunchPoint.position - (localPlayer.GetPosition() + HoldHeight)).magnitude; }
                    bool InRange = false;
                    if (Physics.Raycast(HookLaunchPoint.position, checkdir, out targetcheck, predictedflightdist, HookLayers, QueryTriggerInteraction.Ignore))
                    {
                        InRange = true;
                        if (GrappleAnimator) { GrappleAnimator.SetBool("hitpredicted", true); }
                        if (PredictedHitPoint)
                        {
                            PredictedHitPoint.gameObject.SetActive(true);
                            PredictedHitPoint.position = targetcheck.point;
                            Vector3 eyespoint = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
                            PredictedHitPoint.localScale = PredictedHitPointStartScale * Vector3.Distance(eyespoint, PredictedHitPoint.position);
                        }
                    }
                    else
                    {
                        PredictionOff();
                    }
                    if (TargetingLaser)
                    {
                        if (InRange)
                        {
                            TargetingLaser.LookAt(PredictedHitPoint, Vector3.up);
                        }
                        else
                        {
                            TargetingLaser.LookAt(TargetingLaser.position + checkdir, Vector3.up);
                        }
                    }
                }
            }
        }
        private void PredictionOff()
        {
            if (GrappleAnimator) { GrappleAnimator.SetBool("hitpredicted", false); }
            if (PredictedHitPoint)
            { PredictedHitPoint.gameObject.SetActive(false); }
        }
        public override void OnPlayerRespawn(VRCPlayerApi player)
        {
            if (player.isLocal && IsOwner)
            {
                if (_HookLaunched)
                { HookLaunched = false; RequestSerialization(); }
            }
        }
    }
}