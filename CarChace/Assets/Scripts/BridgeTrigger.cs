using UnityEngine;

public class BridgeTrigger : MonoBehaviour
{
    [SerializeField] Bridge _connectedBridge;

    public void ToggleBridge(bool open)
    {
        _connectedBridge.ToggleBridge(open);
    }
}
