using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class MarkingManager : MonoBehaviour
{
    [SerializeField] public Transform markingCanvas;
    [SerializeField] public GameObject groupMarkPrefab;
    [SerializeField] public TMP_Dropdown[] drp_MarksInput;

    PhotonView PV;
    GameObject participant;

    void Start()
    {
        PV = GetComponent<PhotonView>();    
    }

    public void StartMarkingActivity(string activityName, int numberOfGroups)
    {
        if (activityName.Equals("whiteboard"))   //Getting the end activity name as this means that the whiteboard task has ended and theres something to mark
        {
            LineGen.CanDraw(false);     //Ensure that students cannot draw
            markingCanvas.gameObject.SetActive(true);
            InstantiateUIAssets(numberOfGroups);
            //marksArray = new int[numberOfGroups];
            Debug.Log("StartMarking Activity started");
        }
    }

    public void InstantiateUIAssets(int numberOfGroups)
    {
        for (int i = 0; i < numberOfGroups; i++)
        {
            Debug.Log("Instantiating participants");
            GameObject groupMarkingUI = Instantiate(groupMarkPrefab, markingCanvas);
        }
    }

    public void AssignMarks(string activityName)
    {
        Marks m = new Marks();

        for (int i = 0; i < drp_MarksInput.Length; i++)
        {
            m.int_Marks[i] = drp_MarksInput[i].value;
        }

        m.str_ActivityName = activityName;
        m.str_StudentName = participant.GetComponent<Participant>().CheckData("name");
        m.int_StudentGroupNumber = participant.GetComponent<GroupData>().GetGroupID();

        PV.RPC("SendMarks", RpcTarget.MasterClient, m);
    }

    [PunRPC]
    public void SubmitMarks(Marks outGoingMarkList)
    {
        TeacherPannel.ModifyList("addMarks", outGoingMarkList);
    }
}
