using UnityEngine;
using TMPro;
using Photon.Pun;

public class Timer : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI txt_TimerText;
   [SerializeField] private GameObject teacherPanel, participant;
    private float fl_TimerInitialValue;
    private bool bl_StartTimer;

    PhotonView PV;

    //To limit bandwith usage this is done locally

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void InitialiseTimer(float timerValue, bool state)
    {
        fl_TimerInitialValue = timerValue * 3600;   //Converts hour input into seconds
        txt_TimerText.text = fl_TimerInitialValue.ToString();
        bl_StartTimer = state;
    }

    void Update()
    {
        if (bl_StartTimer)
        {
            if (fl_TimerInitialValue > 0f)
            {
                fl_TimerInitialValue -= Time.deltaTime;
                txt_TimerText.text = fl_TimerInitialValue.ToString();
            }
            else
            {
                PV.RPC("EndTimer", RpcTarget.MasterClient); //Only triggers for host
            }
        }
        else
        {
            fl_TimerInitialValue = 0f;  //Reset timer
        }
    }

    [PunRPC]
    public void EndTimer()
    {
        teacherPanel.GetComponent<TeacherPannel>().TimerEnded();
    }
}
