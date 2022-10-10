using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class MarkingManager : MonoBehaviour
{
    /// <summary>
    /// This script file starts and manages the both peer and group marking activities
    /// </summary>

    [SerializeField] public Canvas markingCanvas;
    [SerializeField] public Transform markingPrefabGroup;
    [SerializeField] public GameObject groupMarkPrefab, peerMarkPrefab;
    [SerializeField] public GameObject[] drp_MarksInput, intxt_Feedback;

    private List<GameObject> studentGroupMembers = new List<GameObject>();
    private List<GameObject> markingGameObjectList = new List<GameObject>();

    PhotonView PV;
    GameObject participant;
    string str_activityName;
    [SerializeField] int int_numberOfGroups;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        participant = GameObject.Find("CurrentParticipant");
    }

    //Start the relevant marking activity (either group or peer)
    public void StartMarkingActivity(string activityName, int numberOfGroups)
    {
        LineGen.CanDraw(false);     //Ensure that students cannot draw
        int_numberOfGroups = numberOfGroups;
        str_activityName = activityName;
        InstantiateUIAssets();
        Debug.Log("StartMarking Activity started");
    }

    public void EndMarkingActivity(string activityName)
    {
        LineGen.CanDraw(false);     //Ensure that students cannot draw
        str_activityName = activityName;

        foreach (GameObject uiObject in markingGameObjectList)
        {
            Destroy(uiObject);
        }

        markingGameObjectList.Clear();

        Debug.Log("Marking Activity Ended");
    }

    //Depending on the marking activity chosen, instantiate the relevant marking UI's
    //Example the whiteboard group marking activity will instantiate the group mark prefab, allowing
    //for students to mark work produced by other groups but not their own
    public void InstantiateUIAssets()
    {
        if (str_activityName.Equals("whiteboard"))
        {
            if (int_numberOfGroups == 0)
            {
                int_numberOfGroups = 1;
            }

            for (int i = 0; i <= int_numberOfGroups - 1; i++)
            {
                if (i + 1 != participant.GetComponent<GroupData>().GetGroupID())
                {
                    GameObject groupMarkingUI = Instantiate(groupMarkPrefab, markingPrefabGroup);
                    groupMarkingUI.GetComponentInChildren<TextMeshProUGUI>().text = "Group " + (i + 1);
                    markingGameObjectList.Add(groupMarkingUI);
                    drp_MarksInput[i] = groupMarkingUI;
                }
            }
        }
        else if (str_activityName.Equals("peerAssessment"))
        {
            int index = 0;

            foreach (GameObject student in TeacherPannel.GetList("studentList"))
            {
                if ((student.GetComponent<GroupData>().GetGroupID() == participant.GetComponent<GroupData>().GetGroupID()) && (student.GetComponent<Participant>().CheckData("name") != participant.GetComponent<Participant>().CheckData("name")))
                {
                    GameObject peerMarkingUI = Instantiate(peerMarkPrefab, markingPrefabGroup);
                    peerMarkingUI.GetComponentInChildren<TextMeshProUGUI>().text = student.GetComponent<Participant>().CheckData("name");
                    markingGameObjectList.Add(peerMarkingUI);
                    studentGroupMembers.Add(student);
                    intxt_Feedback[index] = peerMarkingUI;
                    index++;
                }
            }
        }
    }

    //Once the submit button has been pressed, package the inputted marks into an array to be sent to the teacher
    public void AssignMarks()
    {
        //Wanted to do this with a custom type called Marks that had several sections
        //relevant to each field of the marks, but photon does not allow the serialization of custom types
        //so had to do a string array instead

        if (str_activityName.Equals("whiteboard"))
        {
            string[] m = new string[3 + int_numberOfGroups];

            m[0] = str_activityName;
            m[1] = participant.GetComponent<Participant>().CheckData("name");
            m[2] = participant.GetComponent<GroupData>().GetGroupID().ToString();

            for (int i = 0; i < int_numberOfGroups; i++)
            {
                if (drp_MarksInput[i] != null)
                {
                    m[i + 3] = drp_MarksInput[i].GetComponentInChildren<TMP_Dropdown>().value.ToString();
                }
                else
                {
                    m[i + 3] = "Marks not applicable";
                }
            }

            Debug.Log("Marks Submitted");

            PV.RPC("SubmitMarks", RpcTarget.Others, "addMarksGroup", m);
        }
        else if (str_activityName.Equals("peerAssessment"))
        {
            string[] m = new string[5];
            int index = 0;

            foreach (GameObject student in studentGroupMembers)
            {
                m[0] = str_activityName;
                m[1] = participant.GetComponent<Participant>().CheckData("name");
                m[2] = participant.GetComponent<GroupData>().GetGroupID().ToString();
                m[3] = student.GetComponent<Participant>().CheckData("name");

                if (intxt_Feedback[index] != null || !intxt_Feedback[index].Equals(""))
                {
                    m[4] = intxt_Feedback[index].GetComponentInChildren<TMP_InputField>().text;
                }
                else
                {
                    m[4] = "student did not supply feedback";
                }

                Debug.Log("Marks Submitted");
                PV.RPC("SubmitMarks", RpcTarget.Others, "addMarksPeer", m); //Send the marks to all other clients, incase there is more than one teacher
                index++;
            }
        }
    }

    //Function assigns the marks to a list handled by the teacher(s)
    [PunRPC]
    public void SubmitMarks(string listType, string[] outGoingMarkList)
    {
        PV = GetComponent<PhotonView>();

        if (participant.tag.Equals("Teacher"))
        {
            Debug.Log(listType + " Marks Recieved");
            TeacherPannel.ModifyList(listType, outGoingMarkList, null);
        }
    }
}
