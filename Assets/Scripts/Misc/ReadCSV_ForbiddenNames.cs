using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReadCSV_ForbiddenNames : MonoBehaviour
{

    [SerializeField] TextAsset namesCSV;
    [SerializeField] private string[] str_Names;

    // Start is called before the first frame update
    void Start()
    {
        str_Names = namesCSV.text.Split(new char[] { '\n' });
        Debug.Log("Forbidden names CSV loaded");
    }

    public bool CheckName(string name)
    {

        if (name.Length < 1)
        {
            return false;
        }

        foreach (string x in str_Names)
        {
            if (x.Contains(name))
            {
                return false;
            }
        }

        return true;

    }

}
