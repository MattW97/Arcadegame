using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
    const float VERTICAL_LINE_GRADIENT = 1e5f;

    bool approachSide;

    float gradient;
    float yIntercept;
    Vector2 pointOnLine1;
    Vector2 pointOnLine2;
    float gradientPerpendicular;

    public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
    {
        float dx = pointOnLine.x - pointPerpendicularToLine.x;
        float dy = pointOnLine.y - pointPerpendicularToLine.y;

        if(dx == 0)
        {
            gradientPerpendicular = VERTICAL_LINE_GRADIENT;
        }
        else
        {
            gradientPerpendicular = dy / dx;
        }

        if(gradientPerpendicular == 0)
        {
            gradient = VERTICAL_LINE_GRADIENT;
        }
        else
        {
            gradient = -1 / gradientPerpendicular;
        }

        yIntercept = pointOnLine.y - gradient * pointOnLine.x;
        pointOnLine1 = pointOnLine;
        pointOnLine2 = pointOnLine + new Vector2(1, gradient);

        approachSide = false;
        approachSide = GetSide(pointPerpendicularToLine);
    }

    bool GetSide(Vector2 p)
    {
        return (p.x - pointOnLine1.x) * (pointOnLine2.y - pointOnLine1.y) > (p.y - pointOnLine1.y) * (pointOnLine2.x - pointOnLine1.x);
    }

    public bool HasCrossedLine(Vector2 p)
    {
        return GetSide(p) != approachSide;
    }

    public void DrawWithGizmos(float length)
    {
        Vector3 lineDir = new Vector3(1, 0, gradient).normalized;
        Vector3 lineCentre = new Vector3(pointOnLine1.x, 0, pointOnLine1.y) + Vector3.up;
        Gizmos.DrawLine(lineCentre - lineDir * length / 2.0f, lineCentre + lineDir * length / 2.0f);
    }
}
