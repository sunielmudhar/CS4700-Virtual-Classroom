using TMPro;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CSVMaker : MonoBehaviour
{
    public string str_CSVName;
    public bool bl_CSVCreated = false;
    [SerializeField] TMP_InputField intxt_CSVName;

    public void CreateMarksCSV(List<string[]> studentMarks, string currentTask)
    {

        if (intxt_CSVName.text.Equals(null) || intxt_CSVName.Equals(""))
            str_CSVName = "Marks.csv";
        else
            str_CSVName = intxt_CSVName.text + ".csv";

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
