using TMPro;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class CSVMaker : MonoBehaviour
{
    public string str_CSVName;
    public bool bl_CSVCreated;
    [SerializeField] TMP_InputField TMP_InputField;

    public void CreateMarksCSV(List<Marks> studentMarks)
    {
        if(!bl_CSVCreated)
        {
            str_CSVName = Application.dataPath + TMP_InputField.text;
            TextWriter markCSVMaker = new StreamWriter(str_CSVName, true);
        }

        for (int i = 0; i < studentMarks.Count; i++)
        {
            foreach (Marks marks in studentMarks)
            {
                markCSVMaker.WriteLine();
            }
        }
    }
}
