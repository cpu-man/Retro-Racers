using UnityEngine;

public class Cams : MonoBehaviour
{
    Camera cam1 ;
    Camera cam2 ;

    void Start()
    {
        cam1.enabled = true;
        cam2.enabled = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            cam1.enabled = !cam1.enabled;
            cam2.enabled = !cam2.enabled;
        }
    }
}