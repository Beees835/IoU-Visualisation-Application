using UnityEngine;

public class CartesianPlane : MonoBehaviour
{
    public int gridSize = 10;
    public float lineSpacing = 1f;
    public Material lineMaterial;
    public float numberZOffset = -0.1f;
    public float lineThickness = 0.02f;

    void Start()
    {
        DrawAxes();
        DrawGrid();
    }

    void DrawAxes()
    {
        DrawLine(Vector3.left * gridSize, Vector3.right * gridSize, Color.black); // X axis
        DrawLine(Vector3.up * gridSize, Vector3.down * gridSize, Color.black); // Y axis
    }

    void DrawGrid()
    {
        // Draw vertical and horizontal grid lines
        for (int i = -gridSize; i <= gridSize; i++)
        {
            // Horizontal lines
            DrawLine(new Vector3(-gridSize, i * lineSpacing, 0), new Vector3(gridSize, i * lineSpacing, 0), Color.gray);
            CreateNumber(new Vector3(0.1f, i * lineSpacing, numberZOffset), i.ToString(), Color.gray); // Y axis numbering

            // Vertical lines
            DrawLine(new Vector3(i * lineSpacing, -gridSize, 0), new Vector3(i * lineSpacing, gridSize, 0), Color.gray);

            // We only want one zero
            if (i != 0)
            {
                CreateNumber(new Vector3(i * lineSpacing, 0.1f, numberZOffset), i.ToString(), Color.gray); // X axis numbering
            }
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.SetParent(transform);
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.startWidth = lineThickness;
        lineRenderer.endWidth = lineThickness;
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.useWorldSpace = true;
    }

    void CreateNumber(Vector3 position, string text, Color textColor)
    {
        GameObject textObj = new GameObject("Number");
        textObj.transform.SetParent(transform);
        textObj.transform.position = position;

        TextMesh textMesh = textObj.AddComponent<TextMesh>();
        textMesh.text = text;
        textMesh.fontSize = 24;
        textMesh.color = textColor;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.characterSize = 0.1f;

        // Move the number in front of the grid lines
        textObj.transform.position = new Vector3(position.x, position.y, position.z + numberZOffset);
    }
}