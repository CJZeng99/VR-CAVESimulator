using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform leftAnchor;
    public Transform rightAnchor;
    public Transform leftOffscreenAnchor;
    public Transform rightOffscreenAnchor;

    private bool frozen;

    // Start is called before the first frame update
    void Start()
    {
        frozen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            frozen = !frozen;
        }

        if (!frozen)
        {
            leftOffscreenAnchor.position = leftAnchor.position;
            rightOffscreenAnchor.position = rightAnchor.position;
        }
    }
}
