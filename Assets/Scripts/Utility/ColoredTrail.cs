using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColoredTrail
{
    public Material LineMaterial { get; set; }

    private Vector3 lastTrailVertex;

    public ColoredTrail(Material lineMaterial)
    {
        this.LineMaterial = lineMaterial;
    }

    public IEnumerator StartTrail(Transform objectToTrail, Color color)
    {
        color.a = 0.5f;
        lastTrailVertex = objectToTrail.position;

        while (true)
        {
            // TODO: purge all create cube lines when a new level has begun
            if (Vector3.Distance(lastTrailVertex, objectToTrail.position) > .5)
            {
                addRuntimeLine(lastTrailVertex, objectToTrail.position, color);
                lastTrailVertex = objectToTrail.position;
            }
            yield return null;
        }
    }

    #region [ Debug with lines ]
    private IList<GameObject> lineSegments = new List<GameObject>();

    private void addRuntimeLine(Vector3 start, Vector3 end, Color color)
    {
        removeSegements(lineSegments.Count - 48); //set a maximum number 48 line segements.
        lineSegments.Add(
            createLineSegment(start, end, color)
        );
    }

    /// <summary>
    /// from fishing game Catch.cs which got it from http://answers.unity3d.com/questions/285040/draw-a-line-in-game.html
    /// </summary>
    private GameObject createLineSegment(Vector3 start, Vector3 end, Color lineColor, float lineWidth = 0.3f)
    {
        // TODO: reposition exisitng lines instead of destroying
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);  // TODO: try Quad type
        GameObject.Destroy(cube.GetComponent<Collider>());
        cube.transform.position = Vector3.Lerp(start, end, 0.5f);
        cube.transform.LookAt(end);
        cube.transform.localScale = new Vector3(lineWidth, lineWidth, Vector3.Distance(start, end));

        var cubeRenderer = cube.GetComponent<Renderer>();
        cubeRenderer.material = LineMaterial;
        cubeRenderer.material.color = lineColor;
        cubeRenderer.receiveShadows = false;
        cubeRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        return cube;
    }

    private void removeSegements(int howManyToRemove)
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
