// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    /// <summary>
    /// Action version of Unity's builtin MouseLook behaviour.
    /// TODO: Expose invert Y option.
    /// </summary>
    [ActionCategory(ActionCategory.Input)]
    [Tooltip("Rotates a GameObject based on mouse movement. Minimum and Maximum values can be used to constrain the rotation.")]
    public class FlowCharacterCamera : FsmStateAction
    {

        [RequiredField]
        [Tooltip("The GameObject to rotate.")]
        public FsmOwnerDefault gameObject;

        [RequiredField]
        [Tooltip("The GameObject to look at.")]
        public FsmGameObject lookAtObject;

        /// <summary>
        /// 摄像机与目标点的距离
        /// </summary>
        public float objectDistance;

        /// <summary>
        /// 摄像机碰撞半径
        /// </summary>
        public float sphereCastRadius = 0.5f;

        [RequiredField]
        public FsmFloat sensitivityX;

        [RequiredField]
        public FsmFloat sensitivityY;

        [RequiredField]
        [HasFloatSlider(-360, 360)]
        public FsmFloat minimumX;

        [RequiredField]
        [HasFloatSlider(-360, 360)]
        public FsmFloat maximumX;

        [RequiredField]
        [HasFloatSlider(-360, 360)]
        public FsmFloat minimumY;

        [RequiredField]
        [HasFloatSlider(-360, 360)]
        public FsmFloat maximumY;

        public bool invertYAxis;

        [Tooltip("Repeat every frame.")]
        public bool everyFrame;



        float rotationX;
        float rotationY;

        public override void Reset()
        {
            gameObject = null;
            sensitivityX = 15f;
            sensitivityY = 15f;
            minimumX = -360f;
            maximumX = 360f;
            minimumY = -60f;
            maximumY = 60f;
            everyFrame = true;
            invertYAxis = false;
        }

        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                Finish();
                return;
            }

            // Make the rigid body not change rotation
            // TODO: Original Unity script had this. Expose as option?
            if (go.rigidbody)
            {
                go.rigidbody.freezeRotation = true;
            }

            DoMouseLook();

            if (!everyFrame)
            {
                Finish();
            }
        }

        public override void OnUpdate()
        {
            DoMouseLook();
        }

        void DoMouseLook()
        {
            var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (go == null)
            {
                return;
            }

            var transform = go.transform;


            if (invertYAxis)
            {
                transform.localEulerAngles = new Vector3(GetYRotation(), GetXRotation(), 0);
            }
            else
            {
                transform.localEulerAngles = new Vector3(-GetYRotation(), GetXRotation(), 0);

            }
            RaycastHit hit;
            if (Physics.SphereCast(lookAtObject.Value.transform.position, 0.5f, -transform.forward, out hit, objectDistance))
            {
                float distanceToObstacle = hit.distance;
                transform.position = lookAtObject.Value.transform.position + transform.forward * -distanceToObstacle;
            }
            else
            {
                transform.position = lookAtObject.Value.transform.position + transform.forward * -objectDistance;
            }   
        }

        float GetXRotation()
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX.Value;
            //rotationX = ClampAngle(rotationX, minimumX, maximumX);
            return rotationX;
        }

        float GetYRotation()
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY.Value;
            rotationY = ClampAngle(rotationY, minimumY, maximumY);
            return rotationY;
        }

        // Clamp function that respects IsNone
        static float ClampAngle(float angle, FsmFloat min, FsmFloat max)
        {
            if (!min.IsNone && angle < min.Value)
            {
                angle = min.Value;
            }

            if (!max.IsNone && angle > max.Value)
            {
                angle = max.Value;
            }

            return angle;
        }
    }
}