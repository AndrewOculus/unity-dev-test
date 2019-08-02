using UnityEngine;
using UnityEngine.UI;

public class CameraBehavour : MonoBehaviour
{
    [SerializeField]
    private BallBehavour ballFollowBehavour;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Text notification;
    private Vector3 cameraPosition;
    [SerializeField]
    private float axisVelocity = 100;
    private bool useSlider;
    private bool useCameraRotations;

    #if UNITY_IOS || UNITY_ANDROID
    private Vector2 oldTouchPosition = new Vector2();
    private const float touchSpeed = 0.2f;
    #endif
    void Start(){
        this.useCameraRotations = GameManager.instance.useCameraRotation;
        this.useSlider = GameManager.instance.useSlider;
        this.SetCameraPosition();
        this.slider.transform.gameObject.SetActive(useSlider);
        this.notification.transform.gameObject.SetActive(useCameraRotations);
    }
    void Update(){
        FollowBehavour();
        CameraRotationBehavour();
    }
    void CameraRotationBehavour(){
        if(ballFollowBehavour!=null && useCameraRotations){
            #if UNITY_EDITOR || UNITY_STANDALONE
            if(Input.GetKey(KeyCode.V)){
                CameraDeltaRotation(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }
            #endif

            #if UNITY_IOS || UNITY_ANDROID
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Began){
                    Vector2 pos = touch.position;
                    oldTouchPosition.Set(pos.x, pos.y);
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 pos = touch.position;
                    CameraDeltaRotation((pos.x - oldTouchPosition.x )*touchSpeed,-(pos.y - oldTouchPosition.y)*touchSpeed);
                    oldTouchPosition.Set(pos.x, pos.y);
                }     
            }
            #endif
        }
    }
    void CameraDeltaRotation(float deltaX, float deltaY){
        cameraPosition = Quaternion.AngleAxis(axisVelocity*Time.deltaTime*deltaX, Vector3.up) * cameraPosition;
        cameraPosition = Quaternion.AngleAxis(-axisVelocity*Time.deltaTime*deltaY, this.transform.right) * cameraPosition;        
    }
    void FollowBehavour(){
        if(ballFollowBehavour!=null){
            var ballTransform = ballFollowBehavour.GetTransform();
            this.transform.position = ballTransform.position + cameraPosition;
            this.transform.rotation = Quaternion.LookRotation(ballTransform.transform.position - this.transform.position);
            if(useSlider){
                ballFollowBehavour.SetVelocity(slider.value);
            }
        }
    }
    public void SetBallFollow(GameObject gameObject){
        this.ballFollowBehavour = gameObject.GetComponent<BallBehavour>();
    }
    public Slider GetSlider(){
        return slider;
    }
    void SetCameraPosition(){
        if(ballFollowBehavour!=null)
            this.cameraPosition = -ballFollowBehavour.GetTransform().transform.position + this.transform.position;
    }
}
