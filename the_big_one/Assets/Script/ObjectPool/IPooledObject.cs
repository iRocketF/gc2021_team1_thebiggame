using UnityEngine;

public interface IPooledObject
{
    /// <summary>
    /// Called automatically by ObjectPooler when this object is fetched from the pool.
    /// </summary>
    /// <param name="position">The starting position of the game object</param>
    /// <param name="rotation">The starting rotation of the game object</param>
    public void Activate(Vector3 position, Quaternion rotation);

    /// <summary>
    /// Call this to deactivate object from pool instead of destroying it.
    /// </summary>
    public void Deactivate();

    /// <summary>
    /// Returns gameObject from the component that implements this interface.
    /// </summary>
    /// <returns>GameObject connected to this pooled object</returns>
    public GameObject ReturnGameObject();
}
