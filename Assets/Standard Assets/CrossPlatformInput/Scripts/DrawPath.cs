using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class DrawPath : MonoBehaviour
{
    /// <summary>
    /// True when the player has dragged a continous line with their cursor that started over their ball.
    /// </summary>
    public bool IsPlayerDrawing { get; private set; }

    /// <summary>
    /// Touch Id used to track multiple touch points at once
    /// </summary>
    public int TouchIndex
    {
        get { return _touchIndex; }
        set
        {
            if (_touchIndex == value)
                return;

            _touchIndex = value;

            // turn off drawing if index is now invalid. This happens when the touch point is up.
            if (IsPlayerDrawing)
                IsPlayerDrawing = value != -1;
        }
    }
    private int _touchIndex;
    
    public float SegmentSize = 0.5f;
    public Text DebugText;
    public Shader LineShader;
    public Transform LinkedBall;

    [SerializeField][Tooltip("use to manually set the TouchIndex")]
    private int startingTouchIndex = -1;
    private LineRenderer lineRenderer;
    private List<Vector3> linePositions = new List<Vector3>(); // http://answers.unity3d.com/questions/422186/remove-points-of-linerenderer.html

    void Start()
    {
        TouchIndex = startingTouchIndex;

        lineRenderer = this.GetComponent<LineRenderer>();
        //DebugText.text += " Line" + TouchIndex + " is " + lineRenderer.material.color;
        lineRenderer.SetColors(lineRenderer.material.color, lineRenderer.material.color);
        lineRenderer.material = new Material(LineShader);
        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));  // this shader isn't found on Android
        lineRenderer.SetWidth(0.1F, 0.2F);
        
        StartCoroutine(MonitorDrag(null, onRelease));
    }

    void Update() { }

    /// <summary> Performs an action if a finger is dragged </summary>
    /// <param name="onDrag">argument sent to the action is the delta along the x-axis</param>
    public IEnumerator MonitorDrag(Action<float> onDrag, Action onRelease = null)
    {
        //Debug.Log("MonitorHorizontalDrag=" + isPlayerDrawing[0]);
        while (true)
        {
            if (TouchIndex > -1) // then a touch point is being monitored, but may not have yet touched its linked ball
            {
                //Debug.Log("if (isP MonitorHorizontalDrag=" + isPlayerDrawing[0]);
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
                horizontalMouseDrag(onDrag, onRelease);
#else
                horizontalDrag(onDrag, onRelease);
#endif
            }
            yield return null;
        }
    }

    private void horizontalMouseDrag(Action<float> onDrag, Action onRelease)
    {
        TouchPhase touchPhase = TouchPhase.Stationary;
        if (Input.GetMouseButtonDown(0)) // Began
        {
            touchPhase = TouchPhase.Began;
        }
        else if (Input.GetMouseButton(0))  // Moved
        {
            touchPhase = TouchPhase.Moved;
        }
        else if (Input.GetMouseButtonUp(0)) // End
        {
            touchPhase = TouchPhase.Ended;
        }
        else
        {
            return;
        }

        switchOnPhase(touchPhase, Input.mousePosition, onRelease);
    }

    private void horizontalDrag(Action<float> onDrag, Action onRelease)
    {
        //Debug.Log("horizontalDrag=" + Input.touches.Length);
        if (Input.touches.Length > TouchIndex)
        {
            // only care about the TouchIndex touch
            Touch touch = Input.touches[TouchIndex];
            //DebugText.text += " touch" + TouchIndex + "@" + touch.position;

            switchOnPhase(touch.phase, touch.position, onRelease);
        }
    }

    private void switchOnPhase(TouchPhase touchPhase, Vector3 touchPosition, Action onRelease)
    {
        switch (touchPhase)
        {
            case TouchPhase.Began:
                //Debug.Log("Began=" + Input.mousePosition);
                linePositions.Clear();
                AddPoint(touchPosition);
                break;
            case TouchPhase.Moved:
                //Debug.Log("Moved=" + Input.mousePosition);
                AddPoint(touchPosition);
                break;
            case TouchPhase.Ended:
                //Debug.Log("End=" + Input.mousePosition);
                AddPoint(touchPosition);
                if (onRelease != null)
                    onRelease();
                break;
            case TouchPhase.Stationary:
            case TouchPhase.Canceled:
            default:
                break;
        }
    }

    private void AddPoint(Vector3 touchPosition)
    {
        // convert the touch position to worl position
        Vector3 touchPos = touchPosition;
        touchPos.z = 5; // ensure the screen point is visible by the camera
        Vector3 newPoint = Camera.main.ScreenToWorldPoint(touchPos);
        newPoint.y = 4; // move the points down just above the walls of the maze

        // enable path drawing if the world position is close enough to the linked ball
        if (IsPlayerDrawing == false)
            IsPlayerDrawing = isCursorInRange(newPoint, 3.65f);
        if (IsPlayerDrawing == false)
            return;

        //if (linePositions.Count > 0)
        //    Debug.Log("AddPoint distance=" + linePositions[linePositions.Count - 1] + " and " + newPoint + " dist" + Vector3.Distance(linePositions[linePositions.Count - 1], newPoint) + " Seg" + SegmentSize + " cond" + (Vector3.Distance(linePositions[linePositions.Count - 1], newPoint) > SegmentSize));
        
        // center the first vertex on the players ball
        if (linePositions.Count == 0)
        {
            newPoint = LinkedBall.position;
            newPoint.y = 4;
        }

        // add a new point to the line
        if (linePositions.Count == 0 || isBeyondLastVertex(newPoint, SegmentSize))
        {
            linePositions.Add(newPoint);

            lineRenderer.SetVertexCount(linePositions.Count);
            for (int i = 0; i < linePositions.Count; i++)
            {
                lineRenderer.SetPosition(i, linePositions[i]);
            }
            //Debug.Log("AddPoint distance=" + Vector3.Distance(linePositions[linePositions.Count - 1], newPoint));
        }
        else
        {
            //Debug.Log("No AddPoint distance=" + Vector2.Distance(linePositions[linePositions.Count - 1], newPoint));
        }
    }

    /// <summary>
    /// only add point if it's delta is at least SegmentSize from the last added point
    /// </summary>
    bool isBeyondLastVertex(Vector3 newPoint2, float segmentSize)
    {
        float distance = Vector3.Distance(linePositions[linePositions.Count - 1], newPoint2);

        if (linePositions.Count == 1) // handle snapping to center of the players ball causing extra distance from the cursor
            return distance > (2 * SegmentSize);
        else
            return distance > SegmentSize;
    }

    private bool isCursorInRange(Vector3 cursorWorldPosition, float distance)
    {
        //Debug.Log(TouchIndex + "isCursorInRange" + distance + " " + Vector3.Distance(cursorWorldPosition, LinkedBall.position)
        //    + " result:" + (Vector3.Distance(cursorWorldPosition, LinkedBall.position) < distance));
        return Vector3.Distance(cursorWorldPosition, LinkedBall.position) < distance;
    }

    private void onRelease()
    {
        //Debug.Log("released");

        //isPlayerDrawing = false; // TODO: comment this line in when isPlayerDrawing is set to true by touching the cooresponding ball
    }

    //public Color c1 = Color.yellow;
    //public Color c2 = Color.red;
    //public int lengthOfLineRenderer = 20;
    //void testStart() //http://docs.unity3d.com/ScriptReference/LineRenderer.SetPosition.html
    //{
    //   // LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
    //    lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
    //    lineRenderer.SetColors(c1, c2);
    //    lineRenderer.SetWidth(0.2F, 0.2F);
    //    lineRenderer.SetVertexCount(lengthOfLineRenderer);
    //}
    //void Update()
    //{
    //    LineRenderer lineRenderer = GetComponent<LineRenderer>();
    //    int i = 0;
    //    while (i < lengthOfLineRenderer)
    //    {
    //        Vector3 pos = new Vector3(i * 0.5F, Mathf.Sin(i + Time.time), 0);
    //        lineRenderer.SetPosition(i, pos);
    //        i++;
    //    }
    //}
}
