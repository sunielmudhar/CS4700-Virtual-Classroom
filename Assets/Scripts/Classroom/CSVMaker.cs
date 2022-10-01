using TMPro;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CSVMaker : MonoBehaviour
{
    [SerializeField] TMP_InputField intxt_CSVName;
    public string str_CSVName;

    public void CreateMarksCSV(List<string[]> studentMarks, string currentTask)
    {
        //Check if teacher has inputted a name for the CSV file
        if (intxt_CSVName.text.Equals(null) || intxt_CSVName.Equals(""))
            str_CSVName = "Marks.csv";
        else
            str_CSVName = intxt_CSVName.text + ".csv";

        TextWriter markCSVMaker = new StreamWriter(str_CSVName, true);  //Create CSV

        //Depending on the current task, set up the CSV in a particular way
        if (currentTask.Equals("EndWhiteboardMarking"))
        {
            markCSVMaker.WriteLine("Activity Name, Participant Name, Participant Group Number, Group Marks");
            markCSVMaker.WriteLine(",,,1,2,3,4");
        }
        else if (currentTask.Equals("EndPeerMarking"))
        {
            markCSVMaker.WriteLine("Activity Name, Participant Name, Participant Group Number, Peer Name, Peer Feedback");
        }

        //Write the marks from the string array into the CSV
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
