using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Serialization;

namespace Dreamteck.Splines
{
    [ExecuteInEditMode]
    [AddComponentMenu("Dreamteck/Splines/Spline Projector")]
    public class SplineProjector : SplineTracer
    {
        public enum Mode {Accurate, Cached}
        public Mode mode
        {
            get { return _mode; }
            set
            {
                if(value != _mode)
                {
                    _mode = value;
                    Rebuild();
                }
            }
        }

        public bool autoProject
        {
            get { return _autoProject; }
            set
            {
                if(value != _autoProject)
                {
                    _autoProject = value;
                    if (_autoProject) Rebuild();
                }
            }
        }

        public int subdivide
        {
            get { return _subdivide; }
            set
            {
                if (value != _subdivide)
                {
                    _subdivide = value;
                    if (_mode == Mode.Accurate) Rebuild();
                }
            }
        }

        public Transform projectTarget
        {
            get {
                if (_projectTarget == null) return transform;
                return _projectTarget; 
            }
            set
            {
                if (value != _projectTarget)
                {
                    _projectTarget = value;
                    Rebuild();
                }
            }
        }

        public GameObject targetObject
        {
            get
            {
                if (_targetObject == null)
                {
                    if (applyTarget != null) //Temporary check to migrate SplineProjectors that use target
                    {
                        _targetObject = applyTarget.gameObject;
                        applyTarget = null;
                        return _targetObject;
                    }
                }
                return _targetObject;
            }

            set
            {
                if (value != _targetObject)
                {
                    _targetObject = value;
                    RefreshTargets();
                    Rebuild();
                }
            }
        }

        [SerializeField]
        [HideInInspector]
        private Mode _mode = Mode.Cached;
        [SerializeField]
        [HideInInspector]
        private bool _autoProject = true;
        [SerializeField]
        [HideInInspector]
        [Range(3, 8)]
        private int _subdivide = 4;
        [SerializeField]
        [HideInInspector]
        private Transform _projectTarget;


        [SerializeField]
        [HideInInspector]
        private Transform applyTarget = null;
        [SerializeField]
        [HideInInspector]
        private GameObject _targetObject;

        double traceFromA = -1.0, traceToA = -1.0, traceFromB = -1.0;

        [SerializeField]
        [HideInInspector]
        public Vector2 _offset;
        [SerializeField]
        [HideInInspector]
        public Vector3 _rotationOffset = Vector3.zero;

        public event SplineReachHandler onEndReached;
        public event SplineReachHandler onBeginningReached;

        [SerializeField]
        [HideInInspector]
        Vector3 lastPosition = Vector3.zero;

        protected override void Reset()
        {
            base.Reset();
            _projectTarget = transform;
        }

        protected override Transform GetTransform()
        {
            if (targetObject == null) return null;
            return targetObject.transform;
        }

        protected override Rigidbody GetRigidbody()
        {
            if (targetObject == null) return null;
            return targetObject.GetComponent<Rigidbody>();
        }

        protected override Rigidbody2D GetRigidbody2D()
        {
            if (targetObject == null) return null;
            return targetObject.GetComponent<Rigidbody2D>();
        }

        protected override void LateRun()
        {
            base.LateRun();
            if (autoProject)
            {
                if (projectTarget == null) return;
                if (lastPosition != projectTarget.position)
                {
                    lastPosition = projectTarget.position;
                    CalculateProjection();
                }
            }
         }

        protected override void PostBuild()
        {
            base.PostBuild();
            CalculateProjection();
        }

        private void CheckTriggers()
        {
            if (traceFromA >= 0f)
            {
                if (clipTo - traceFromA > traceFromB)
                {
                    traceToA = clipTo;
                    traceFromB = clipFrom;
                }
                else
                {
                    traceToA = clipFrom;
                    traceFromB = clipTo;
                }
                if (System.Math.Abs(traceToA - traceFromA) + System.Math.Abs(result.percent - traceFromB) < System.Math.Abs(result.percent - traceFromA))
                {
                    CheckTriggers(traceFromA, traceToA);
                    CheckTriggers(traceFromB, result.percent);
                }
                else CheckTriggers(traceFromA, result.percent);
            }
        }

        public void CalculateProjection()
        {
            if (_projectTarget == null) return;
            traceFromA = -1.0;
            traceToA = -1.0;
            traceFromB = -1.0;
            double lastPercent = _result.percent;
            traceFromA = _result.percent;
            if (_mode == Mode.Accurate && spline != null)
            {
                spline.Project(_result, _projectTarget.position, clipFrom, clipTo, SplineComputer.EvaluateMode.Calculate);
            } else Project(_projectTarget.position, _result);
            if (onBeginningReached != null && _result.percent <= clipFrom)
            {
                if (!Mathf.Approximately((float)lastPercent, (float)_result.percent)) onBeginningReached();
            }
            else if (onEndReached != null && _result.percent >= clipTo)
            {
                if (!Mathf.Approximately((float)lastPercent, (float)_result.percent)) onEndReached();
            }
            if (targetObject != null) ApplyMotion();
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                CheckTriggers();
                InvokeTriggers();
            }
#else
            CheckTriggers();
            InvokeTriggers();
#endif
            lastPosition = projectTarget.position;
        }
    }
}
