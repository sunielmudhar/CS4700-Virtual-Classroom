using TMPro;
using UnityEngine;

public class NotificationsScript : MonoBehaviour
{
    [SerializeField] public GameObject notificationsPrefab;
    [SerializeField] public Transform notificationCanvas;

    public void RecieveNotification(string participantName, int groupID)
    {
        GameObject notificationUI = Instantiate(notificationsPrefab, notificationCanvas);
        notificationUI.GetComponentInChildren<TextMeshProUGUI>().text = participantName + " from Group " + groupID + " has requested help.";
    }

    public void OnPressDelete()
    {
        Destroy(this.gameObject);
    }
}
