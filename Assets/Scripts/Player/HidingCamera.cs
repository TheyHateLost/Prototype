using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingCamera: MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    [SerializeField] float MaxLeftRotation;
    [SerializeField] float MaxRightRotation;

    //HeartBeat
    [SerializeField] Transform monsterTransform;
    public float Heartbeat_Timer = 2f;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -40f, 30f);
        yRotation = Mathf.Clamp(yRotation, MaxLeftRotation, MaxRightRotation);

        // rotate cam and orientation
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.localRotation = Quaternion.Euler(0, yRotation, 0);

        Heartbeat();
    }

    void Heartbeat()
    {
        float DistanceFromMonster = Vector3.Distance(monsterTransform.position, gameObject.transform.position);

        if (DistanceFromMonster <= 50f)
        {
            Heartbeat_Timer -= Time.deltaTime;

            if (Heartbeat_Timer <= 0)
            {
                SoundManager.PlaySound(SoundSource.SoundManager, SoundType.Player_Heartbeat, 2f / DistanceFromMonster);

                if (DistanceFromMonster > 30 && DistanceFromMonster < 35)
                {
                    Heartbeat_Timer = 1.9f;
                }
                else if (DistanceFromMonster > 25 && DistanceFromMonster < 30)
                {
                    Heartbeat_Timer = 1.75f;
                }
                else if (DistanceFromMonster > 20 && DistanceFromMonster < 25)
                {
                    Heartbeat_Timer = 1.50f;
                }
                else if (DistanceFromMonster > 15 && DistanceFromMonster < 20)
                {
                    Heartbeat_Timer = 1.30f;
                }
                else if (DistanceFromMonster > 10 && DistanceFromMonster < 15)
                {
                    Heartbeat_Timer = 1.15f;
                }
                else if (DistanceFromMonster > 5 && DistanceFromMonster < 10)
                {
                    Heartbeat_Timer = 0.9f;
                }
                else if (DistanceFromMonster > 0 && DistanceFromMonster < 5)
                {
                    Heartbeat_Timer = .75f;
                }
            }
        }
        else
        {
            Heartbeat_Timer = 1.9f;
        }
    }
}
