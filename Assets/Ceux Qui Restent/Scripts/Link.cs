using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link  {
    // Attributes
    public float length;
    public List<GameObject> objects;
    public List<Vector2> lines = new List<Vector2>();

    // Constructors
    public Link(float _length, List<GameObject> _objects, List<Vector2> _lines)
    {
        this.length = _length;
        this.objects = _objects;
        this.lines = _lines;
    }

    // Methods
    public void Clear()
    {
        for(int o = 0; o < objects.Count; o++)
        {
            GameObject.Destroy(objects[o]);
        }
        lines.Clear();
        objects.Clear();

        // Refill Energy
        //technician = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllerColliderHit>();
    }

    public bool Intersect(List<Vector2> otherLines)
    {
        for(int l = 0; l < lines.Count - 1; l++)
        {
            Vector2 a = lines[l];
            Vector2 b = lines[l + 1];
            Debug.DrawRay(new Vector3(a.x, 8, a.y), new Vector3(b.x, 8, b.y) - new Vector3(a.x, 8, a.y), Color.blue, 1);
            for (int ol = 0; ol < otherLines.Count - 1; ol++)
            {
                Vector2 c = otherLines[ol];
                Vector2 d = otherLines[ol + 1];
                Debug.DrawRay(new Vector3(c.x, 8, c.y), new Vector3(d.x, 8, d.y) - new Vector3(c.x, 8, c.y), Color.yellow, 1);
                if (LineLineIntersect(a, b, c, d))
                    return true;
            }
        }
        return false;
    }

    private static bool LineLineIntersect(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4)
    {
        float x1 = v1.x;
        float y1 = v1.y;
        float x2 = v2.x;
        float y2 = v2.y;
        float x3 = v3.x;
        float y3 = v3.y;
        float x4 = v4.x;
        float y4 = v4.y;

        bool over = false;
        float a1 = y2 - y1;
        float b1 = x1 - x2;
        float c1 = a1 * x1 + b1 * y1;

        float a2 = y4 - y3;
        float b2 = x3 - x4;
        float c2 = a2 * x3 + b2 * y3;

        float det = a1 * b2 - a2 * b1;

        if (det == 0)
        {
            // Lines are parallel
        }
        else
        {
            float x = (b2 * c1 - b1 * c2) / det;
            float y = (a1 * c2 - a2 * c1) / det;
            if (x > Mathf.Min(x1, x2) && x < Mathf.Max(x1, x2) &&
                x > Mathf.Min(x3, x4) && x < Mathf.Max(x3, x4) &&
                y > Mathf.Min(y1, y2) && y < Mathf.Max(y1, y2) &&
                y > Mathf.Min(y3, y4) && y < Mathf.Max(y3, y4))
            {
                Debug.DrawRay(new Vector3(v1.x, 8.2f, v1.y), new Vector3(v2.x, 8.2f, v2.y) - new Vector3(v1.x, 8.2f, v1.y), Color.red, 2);
                Debug.DrawRay(new Vector3(v3.x, 8.2f, v3.y), new Vector3(v4.x, 8.2f, v4.y) - new Vector3(v3.x, 8.2f, v3.y), Color.red, 2);
                over = true;
            }
        }
        return over;
    }

    private static bool IntersectLineLine(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        bool intersect = false;

        //The direction of the line
        Vector3 lineDir = p2 - p1;

        //The normal to a line is just flipping x and z and making z negative
        Vector3 lineNormal = new Vector3(-lineDir.z, lineDir.y, lineDir.x);

        //Now we need to take the dot product between the normal and the points on the other line
        float dot1 = Vector3.Dot(lineNormal, p3 - p1);
        float dot2 = Vector3.Dot(lineNormal, p4 - p1);

        //If you multiply them and get a negative value then p3 and p4 are on different sides of the line
        if (dot1 * dot2 < 0f)
        {
            intersect = true;
            Debug.DrawRay(new Vector3(p1.x, 8.2f, p1.y), new Vector3(p2.x, 8.2f, p2.y) - new Vector3(p1.x, 8.2f, p1.y), Color.red, 2);
            Debug.DrawRay(new Vector3(p3.x, 8.2f, p3.y), new Vector3(p4.x, 8.2f, p4.y) - new Vector3(p3.x, 8.2f, p3.y), Color.red, 2);
        }

        return intersect;
    }
}
