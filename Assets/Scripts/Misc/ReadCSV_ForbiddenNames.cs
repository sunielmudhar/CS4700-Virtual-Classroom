using UnityEngine;
using System.Linq;

public class ReadCSV_ForbiddenNames : MonoBehaviour
{

    [SerializeField] TextAsset namesCSV;
    [SerializeField] private string[] str_Names;

    // Start is called before the first frame update
    void Start()
    {
        str_Names = namesCSV.text.Split(new char[] { '\n' });   //Load CSV contents into a string array
        Debug.Log("Forbidden names CSV loaded");
    }

    //Check if the entered name meets the length requirement and is not in the forbidden names CSV
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
