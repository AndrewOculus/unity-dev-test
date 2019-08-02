using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    public bool useSlider;
    [SerializeField]
    public bool useTrace;
    [SerializeField]
    private bool useDoubleClick;   
    [SerializeField]
    private bool useSwitchTarget;
    [SerializeField]
    public bool useCameraRotation;
    [SerializeField]
    private string[] paths;
    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private CameraBehavour cameraPrefab;
    private List<GameObject> gameObjects;
    private const float doubleClickTime = 0.2f;
    private float timer = 0f;
    private bool isDoubleClick;
    private int cameraTargetIdx = 0;

    void Start(){
        instance = this;
        gameObjects = new List<GameObject>();
        foreach(var path in paths){
            var go = Instantiate(ballPrefab);
            go.GetComponent<BallBehavour>().Load(path);
            gameObjects.Add(go);
        }
        cameraPrefab.SetBallFollow(gameObjects[cameraTargetIdx]);
    }
    void OneClick(){
		MakeRaycast ()?.Begin();
    }
    void TwoClick(){
        if(useDoubleClick)
            MakeRaycast ()?.BackToStart();
    }
    void Update () {
        DoubleClickLogic();
        CameraSwitchTargetLogic();
	}
    void DoubleClickLogic(){
        var click = Input.GetKeyDown (KeyCode.Mouse0);

        if(isDoubleClick){
            timer += Time.deltaTime;
        }
        if(timer > doubleClickTime && isDoubleClick){
            isDoubleClick = false;
            timer = 0;
            OneClick();
            return;
        }
        if(timer < doubleClickTime && click && isDoubleClick){
            isDoubleClick = false;
            timer = 0;
            TwoClick();
            return;
        }
        if(click){
            isDoubleClick = true;
        }
    }
    void CameraSwitchTargetLogic(){
        if(Input.GetKeyDown(KeyCode.LeftArrow) && useSwitchTarget){
            LeftTarget();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow) && useSwitchTarget){
            RightTarget();
        }
    }
    public void LeftTarget(){
        gameObjects[cameraTargetIdx].GetComponent<BallBehavour>().SetVelocity(0);
        cameraTargetIdx--;
        if(cameraTargetIdx == -1)
            cameraTargetIdx = gameObjects.Count-1;
        cameraPrefab.SetBallFollow(gameObjects[cameraTargetIdx]);
    }
    public void RightTarget(){
        gameObjects[cameraTargetIdx].GetComponent<BallBehavour>().SetVelocity(0);
        cameraTargetIdx++;
        if(cameraTargetIdx == gameObjects.Count)
            cameraTargetIdx = 0;
        cameraPrefab.SetBallFollow(gameObjects[cameraTargetIdx]);
    }
	BallBehavour MakeRaycast () {
		RaycastHit hit;
		Ray ray = cameraPrefab.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit)) {
            return hit.transform.gameObject.GetComponent<BallBehavour>();
		}
        return null;
	}
}
