using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Gestimates a size for the text element and shares it with all 
/// </summary>
//[ExecuteInEditMode]
[RequireComponent(typeof(Text))]
public class TextFitGroup : MonoBehaviour
{
    public static Dictionary<int, int> smallestMax = new Dictionary<int, int>();

    [Tooltip("A shared TextGroupId that sets the font size of all that have the value to the smallest best fit font size")]
    public int TextGroupId;

    [Tooltip("the number of rows of text you expect to be used to when filling the avaiable text area")]
    public int TargetRowsOfText;

    private Text text;
    private RectTransform rectTrans;

    IEnumerator Start()
    {
        if (TargetRowsOfText < 1)
            TargetRowsOfText = 1;

        text = this.GetComponent<Text>();
        
        rectTrans = this.transform as RectTransform;
        int charPerLine = text.text.Length / TargetRowsOfText;
        int line = (int)rectTrans.rect.width;
        int fontSize = (int)((line / charPerLine) * 1.3);
        if (smallestMax.ContainsKey(TextGroupId) == false)
        {
            smallestMax.Add(TextGroupId, fontSize);
        }
        else if (fontSize < smallestMax[TextGroupId])
        {
            smallestMax[TextGroupId] = fontSize;
        }

        // wait a frame to ensure all other members of the group agree on a size
        yield return null;

        // use the determined size
        text.fontSize = smallestMax[TextGroupId];
        //Debug.Log(string.Format("#{0} chars{1} lines{2} size{3} used{4}", TextGroupId, charPerLine, line, fontSize, smallestMax[TextGroupId]));
    }
}
