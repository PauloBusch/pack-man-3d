using UnityEngine;

public class Mirror : MonoBehaviour
{
    public GameObject TargetPortal;

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = new Vector3(
            TargetPortal.transform.position.x,
            other.gameObject.transform.position.y, 
            TargetPortal.transform.position.z
        );
    }
}
