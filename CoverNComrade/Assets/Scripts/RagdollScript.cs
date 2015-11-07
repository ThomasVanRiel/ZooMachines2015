using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollScript : MonoBehaviour {

    private Animator _animController;

    public float BlendTime = 0.5f;
    private float _toGetUpTransitionTime = 0.25f;
    private float _ragdollEndTime = -1;

    public float GetUpAfter = 2.0f;

    enum RagdollState {
        Animated,   //Mecanim
        Ragdoll,    //Physics
        Blending    //Mecanim + Blending in LateUpdate()
    }
    RagdollState _state = RagdollState.Animated;

    public class Limb {
        public Transform Transform;
        public Vector3 Position;
        public Quaternion Rotation;

        public Limb(Transform transform) {
            Transform = transform;
        }
    }

    private List<Limb> _limbs = new List<Limb>();

    Vector3 _ragdolledHipPosition, _ragdolledHeadPosition, _ragdolledFeetPosition;

    public bool Ragdolled {
        get {
            return _state != RagdollState.Animated;
        }
        set {
            if (value == true) {
                if (_state == RagdollState.Animated) {
                    SetKinematic(false);
                    _animController.enabled = false;
                    _state = RagdollState.Ragdoll;
                }
            } else {
                if (_state == RagdollState.Ragdoll) {
                    SetKinematic(true);
                    _animController.enabled = true;
                    _state = RagdollState.Blending;
                    _ragdollEndTime = Time.time;

                    //Store ragdoll position for blending
                    foreach (var limb in _limbs) {
                        limb.Position = limb.Transform.position;
                        limb.Rotation = limb.Transform.rotation;
                    }

                    //Set positions of key bones
                    _ragdolledFeetPosition = 0.5f * (_animController.GetBoneTransform(HumanBodyBones.LeftFoot).position + _animController.GetBoneTransform(HumanBodyBones.RightFoot).position);
                    _ragdolledHeadPosition = _animController.GetBoneTransform(HumanBodyBones.Head).position;
                    _ragdolledHipPosition = _animController.GetBoneTransform(HumanBodyBones.Hips).position;

                    //if pointing up, character is on back
                    if (_animController.GetBoneTransform(HumanBodyBones.Spine).forward.y > 0) {
                        Debug.Log("Getting up from back");
                        _animController.SetTrigger("GetUpBack");
                    } else {
                        Debug.Log("Getting up from front");
                        _animController.SetTrigger("GetUpFront");
                    }

                }
            }
        }
    }

    void SetKinematic(bool value) {
        Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var item in rigidBodies) {
            item.isKinematic = value;
        }
    }

    // Use this for initialization
    void Awake() {
        _animController = GetComponent<Animator>();
        SetKinematic(true);

        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms) {
            _limbs.Add(new Limb(t));
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Ragdolled = true;
            Invoke("SetAnimTrue", GetUpAfter);
        }

    }

    void SetAnimTrue() {
        Ragdolled = false;
    }
    

    void LateUpdate() {
        if (_state == RagdollState.Blending) {
            if (Time.time <= _ragdollEndTime + _toGetUpTransitionTime) {
                //If we are waiting for Mecanim to start playing the get up animations, update the root of the mecanim
                //character to the best match with the ragdoll
                Vector3 animatedToRagdolled = _ragdolledHipPosition - _animController.GetBoneTransform(HumanBodyBones.Hips).position;
                Vector3 newRootPosition = transform.position + animatedToRagdolled;

                //Now cast a ray from the computed position downwards and find the highest hit that does not belong to the character 
                RaycastHit[] hits = Physics.RaycastAll(new Ray(newRootPosition, Vector3.down));
                newRootPosition.y = 0;
                foreach (RaycastHit hit in hits) {
                    if (!hit.transform.IsChildOf(transform)) {
                        newRootPosition.y = Mathf.Max(newRootPosition.y, hit.point.y);
                    }
                }
                transform.position = newRootPosition;

                //Get body orientation in ground plane for both the ragdolled pose and the animated get up pose
                Vector3 ragdolledDirection = _ragdolledHeadPosition - _ragdolledFeetPosition;
                ragdolledDirection.y = 0;

                Vector3 meanFeetPosition = 0.5f * (_animController.GetBoneTransform(HumanBodyBones.LeftFoot).position + _animController.GetBoneTransform(HumanBodyBones.RightFoot).position);
                Vector3 animatedDirection = _animController.GetBoneTransform(HumanBodyBones.Head).position - meanFeetPosition;
                animatedDirection.y = 0;

                //Try to match the rotations. Note that we can only rotate around Y axis, as the animated characted must stay upright,
                //hence setting the y components of the vectors to zero. 
                transform.rotation *= Quaternion.FromToRotation(animatedDirection.normalized, ragdolledDirection.normalized);
            }


            float blend = Mathf.Clamp01(1.0f - (Time.time - _ragdollEndTime - _toGetUpTransitionTime) / BlendTime);
            foreach (var l in _limbs) {

                if (l.Transform != transform) { //this if is to prevent us from modifying the root of the character, only the actual body parts
                    //position is only interpolated for the hips
                    if (l.Transform == _animController.GetBoneTransform(HumanBodyBones.Hips))
                        l.Transform.position = Vector3.Lerp(l.Transform.position, l.Position, blend);
                    //rotation is interpolated for all body parts
                    l.Transform.rotation = Quaternion.Slerp(l.Transform.rotation, l.Rotation, blend);
                }
            }

            //if the ragdoll blend amount has decreased to zero, move to animated state
            if (blend == 0) {
                _state = RagdollState.Animated;
                return;
            }
        }
    }
}
