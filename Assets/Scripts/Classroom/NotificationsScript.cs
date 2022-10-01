using TMPro;
using UnityEngine;

public class NotificationsScript : MonoBehaviour
{
    [SerializeField] public GameObject notificationsPrefab;
    [SerializeField] public Transform notificationCanvas;

    //Upon recieving a notification, instantiate a prefab (create notification for teacher to read)
    public void RecieveNotification(string participantName, int groupID)
    {
        GameObject notificationUI = Instantiate(notificationsPrefab, notificationCanvas);
        notificationUI.GetComponentInChildren<TextMeshProUGUI>().text = participantName + " from Group " + groupID + " has requested help.";
    }

    //Delete notification once read
    public void OnPressDelete()
    {
        Destroy(this.gameObject);
    }
}
