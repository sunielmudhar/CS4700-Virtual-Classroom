using TMPro;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CSVMaker : MonoBehaviour
{
    public string str_CSVName = "/test.csv";
    public bool bl_CSVCreated = false;
    [SerializeField] TMP_InputField TMP_InputField;

    //Need to change this to check which marks are being exported, group or student?

    public void CreateMarksCSV(List<string[]> studentMarks, string currentTask)
    {
        TextWriter markCSVMaker = new StreamWriter(str_CSVName, true);

        if (currentTask.Equals("EndWhiteboardMarking"))
        {
            markCSVMaker.WriteLine("Activity Name, Participant Name, Participant Group Number, Group Marks");
            markCSVMaker.WriteLine(",,,1,2,3,4");
        }
        else if (currentTask.Equals("EndPeerMarking"))
        {
            markCSVMaker.WriteLine("Activity Name, Participant Name, Participant Group Number, Peer Name, Peer Feedback");
        }

        foreach (string[] marks in studentMarks)
        {
            markCSVMaker.WriteLine();

            for (int i = 0; i < marks.Length; i++)
            {
                markCSVMaker.Write(marks[i] + ",");
            }
        }

        markCSVMaker.Close();
    }
}
