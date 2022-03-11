using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class otherbal : MonoBehaviour
{
    MeshRenderer mesh;
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mat = mesh.material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Sphere")
        {
            mat.color = new Color(0, 0, 0);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Sphere")
        {
            mat.color = new Color(1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
