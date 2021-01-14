using UnityEngine;

public class FakeWallScript : MonoBehaviour
{
    public GameObject targetWall;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
            if(collider.gameObject.GetComponent<Movement>().getMovementVelocity() > 20)
                targetWall.GetComponent<MeshCollider>().isTrigger = true;
    }
}
