using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class DrawLine : MonoBehaviour
{
    public int TouchIndex;
    public float SegmentSize = 0.5f;
    public Text DebugText;
    public Shader LineShader;

    private LineRenderer lineRenderer;
    private List<Vector3> linePositions = new List<Vector3>(); // http://answers.unity3d.com/questions/422186/remove-points-of-linerenderer.html
    private bool[] isPlayerDrawing = new bool[] { false, false, false, false };

    void Start()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        DebugText.text += " Line" + TouchIndex + " is " + lineRenderer.material.color;
        lineRenderer.SetColors(lineRenderer.material.color, lineRenderer.material.color);
        lineRenderer.material = new Material(LineShader);
        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));  // this shader isn't found on Android
        lineRenderer.SetWidth(0.1F, 0.2F);

        StartCoroutine(MonitorDrag(null, () => Debug.Log("released")));
    }

    void Update() { }

    /// <summary> Performs an action if a finger is dragged </summary>
    /// <param name="onDrag">argument sent to the action is the delta along the x-axis</param>
    public IEnumerator MonitorDrag(Action<float> onDrag, Action onRelease = null)
    {
        isPlayerDrawing[0] = true;

        //Debug.Log("MonitorHorizontalDrag=" + isPlayerDrawing[0]);
        while (true)
        {
            if (isPlayerDrawing[0])
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
        Vector3 touchPos = touchPosition;
        touchPos.z = 5;
        // only add point if it's delta is at least 1 from the last point
        Vector3 newPoint = Camera.main.ScreenToWorldPoint(touchPos);
        Debug.Log(touchPos + " vs " + newPoint + " dpi" + Screen.dpi);
        //Debug.Log("AddPoint distance=" + linePositions[linePositions.Count - 1] + " and " + newPoint + " dist" + Vector3.Distance(linePositions[linePositions.Count - 1], newPoint));
        if (linePositions.Count == 0 || Vector2.Distance(linePositions[linePositions.Count - 1], newPoint) > SegmentSize)
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
