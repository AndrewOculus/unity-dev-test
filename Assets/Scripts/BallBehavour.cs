using UnityEngine;

public class BallBehavour : MonoBehaviour
{
    private SerializableModel serializableModel;
    private LineRenderer lineRenderer;
    private bool isMove;
    private int idx = 1;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private const float startVelocity = 8f;
    private float smoothVelocity = startVelocity;

    void Awake(){
        lineRenderer = GetComponent<LineRenderer>();
    }
    public void Load(string resourceName)
    {
        lineRenderer.enabled = GameManager.instance.useTrace;
        var json = (TextAsset)Resources.Load(resourceName);
        serializableModel = JsonUtility.FromJson<SerializableModel>(json.text);
        SetZeroPosition();
    }
    void Update()
    {
        if(isMove){

            if((transform.position - targetPosition).sqrMagnitude < 0.05f){
                if(idx>1){
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, serializableModel.GetVector(idx));
                } 
                else {
                    lineRenderer.positionCount = 0;
                }
                idx++;
            }
            if(serializableModel.x.Length <= idx){
                idx=1;
                isMove = false;
                return;
            }
            targetPosition = serializableModel.GetVector(idx);
            this.transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime*smoothVelocity);
            if(lineRenderer.positionCount == 2)
                lineRenderer.SetPosition(1, transform.position);
        }
    }
    void SetZeroPosition(){
        this.transform.position = serializableModel.GetVector(0);
    }
    public void Begin(){
        if(!isMove && idx == 1){
            isMove = true;
            SetZeroPosition();
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
            targetPosition = transform.position;
            smoothVelocity = startVelocity;
        }
    }
    public void BackToStart(){
        idx = 1;
        isMove = false;
        lineRenderer.positionCount = 0;
        SetZeroPosition();
    }
    public void SetVelocity(float velocity){
        this.smoothVelocity = velocity*30f;
    }
    public Transform GetTransform(){
        return this.transform;
    }
}
