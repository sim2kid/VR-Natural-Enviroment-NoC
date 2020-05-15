using UnityEngine;

public class Collider : MonoBehaviour
{
    [SerializeField]
    private ColliderFollower _colliderFollowerPrefab;

    private void SpawnColliderFollower() {
        var follower = Instantiate(_colliderFollowerPrefab);
        follower.transform.position = transform.position;
        follower.SetFollowTarget(this);
    }

    void Start()
    {
        SpawnColliderFollower();
    }
}
