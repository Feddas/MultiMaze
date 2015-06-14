using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColoredTrail
{
    private const int segmentPoolSize = 48;

    /// <summary>
    /// Material to use on the colored trail meshes
    /// </summary>
    public Material LineMaterial { get; set; }

    private int poolIndex;
    private Vector3 lastTrailVertex;

    public ColoredTrail(Material lineMaterial)
    {
        this.LineMaterial = lineMaterial;
    }

    public IEnumerator StartTrail(Transform objectToTrail)
    {
        // set y to 0.5 for a constant y value. Ensuring the trail doesn't follow the y-axis of the ball popping up or falling down
        lastTrailVertex = objectToTrail.position.SetY(0.5f);
        Vector3 currentPosition;

        while (true)
        {
            currentPosition = objectToTrail.position.SetY(0.5f);

            if (Vector3.Distance(lastTrailVertex, currentPosition) > .5)
            {
                addRuntimeLine(lastTrailVertex, currentPosition, Color.clear);
                lastTrailVertex = objectToTrail.position.SetY(0.5f);
            }
            yield return null;
        }
    }

    public void PurgeLines()
    {
        poolIndex = 0;
        removeSegments(lineSegments.Count);
    }

    #region [ Debug with lines ]
    private IList<GameObject> lineSegments = new List<GameObject>();

    private void addRuntimeLine(Vector3 start, Vector3 end, Color color)
    {
        if (replaceSegment(start, end) == false)
        {
            lineSegments.Add(
                createLineSegment(start, end, color)
            );
        }
    }

    /// <summary>
    /// from fishing game Catch.cs which got it from http://answers.unity3d.com/questions/285040/draw-a-line-in-game.html
    /// </summary>
    private GameObject createLineSegment(Vector3 start, Vector3 end, Color lineColor, float lineWidth = 0.3f)
    {
        GameObject lineSegment = GameObject.CreatePrimitive(PrimitiveType.Quad);
        GameObject.Destroy(lineSegment.GetComponent<Collider>());
        lineSegment.name += LineMaterial.name;
        lineSegment.isStatic = true;
        stretchQuad(lineSegment, start, end, lineWidth);

        var cubeRenderer = lineSegment.GetComponent<Renderer>();
        cubeRenderer.material = LineMaterial;
        //cubeRenderer.material.color = lineColor; // don't modify material to ensure static batching of material
        cubeRenderer.receiveShadows = false;
        cubeRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        return lineSegment;
    }

    private bool replaceSegment(Vector3 start, Vector3 end)
    {
        if (lineSegments.Count >= segmentPoolSize)
        {
            if (poolIndex == lineSegments.Count)
                poolIndex = 0;
            stretchQuad(lineSegments[poolIndex++], start, end);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void stretchCube(GameObject cube, Vector3 start, Vector3 end, float lineWidth = 0.3f)
    {
        cube.transform.position = Vector3.Lerp(start, end, 0.5f);
        cube.transform.LookAt(end);
        cube.transform.localScale = new Vector3(lineWidth, lineWidth, Vector3.Distance(start, end));
    }

    private void stretchQuad(GameObject quad, Vector3 start, Vector3 end, float lineWidth = 0.3f)
    {
        quad.transform.position = Vector3.Lerp(start, end, 0.5f);
        quad.transform.LookAt(end);
        quad.transform.Rotate(90, 0, 0);
        quad.transform.localScale = new Vector3(lineWidth, Vector3.Distance(start, end), 1);
    }

    private void removeSegments(int howManyToRemove)
    {
        if (howManyToRemove <= 0)
            return;

        for (int i = 0; i < howManyToRemove; i++)
        {
            UnityEngine.Object.Destroy(lineSegments[0]);
            lineSegments.RemoveAt(0);
        }
    }
    #endregion [ Debug with lines ]
}
