using UnityEngine;
using System.Collections;

public class InputListener : UnitySceneSingleton<InputListener>
{

    private static IGameInput _gameInput;
    float xSpeed = 1.0f;
    float ySpeed = 1.0f;
    float x = 0.0f;
    float y = 0.0f;
    private static InputListener ins;
    public static void Init()
    {
        ins = InputListener.Instance;
    }
    void Start()
    {
        if (PlatformUtil.IsTouchDevice)
        {
            _gameInput = new SingleTouchGameInput();
        }
        else
        {
            _gameInput = new WinGameInput();
        }
    }

    float deltaX = 0;
    float deltaY = 0;
    Vector3 lastPostion;
    float lastScale;
    void Update()
    {
        //exclude ui
    }

    /// <summary> X轴向移动速度 </summary>
    float lastMoveSpeedX = 0.0f;
    /// <summary> Y轴向移动速度 </summary>
    float lastMoveSpeedY = 0.0f;
    /// <summary> X轴向移动加速度 </summary>
    float ax = 0.0f;
    /// <summary> Y轴向移动加速度 </summary>
    float ay = 0.0f;
    /// <summary> 移动惯性 </summary>
    bool isTwing = false;

    /// <summary>
    /// 惯性处理
    /// </summary>
    void LateMove()
    {
        if(Mathf.Abs(lastMoveSpeedX)-0.0f<WinGameInput.EdgeWidth)
        {
            lastMoveSpeedX = 0.0f;
            ax = 0.0f;
            isTwing = false;
        }
        if(Mathf.Abs(lastMoveSpeedY)-0.0f<WinGameInput.EdgeWidth)
        {
            lastMoveSpeedY = 0.0f;
            ay = 0.0f;
            isTwing = false;
        }
        if(ax!=0.0f)
        {
            if(lastMoveSpeedX<0)
            {
                lastMoveSpeedX += ax * Time.deltaTime;
            }
            else
            {
                lastMoveSpeedX -= ax * Time.deltaTime;
            }
            if(Camera.main.transform.localPosition.x>WinGameInput.EdgeLeftX
                &&Camera.main.transform.localPosition.x<WinGameInput.EdgeRightX)
            {
                Camera.main.transform.Translate(lastMoveSpeedX,0,0);
            }
            else
            {
                lastMoveSpeedX = 0.0f;
                isTwing=false;
            }
        }
        else if(ay!=0.0f)
        {
            if (lastMoveSpeedY < 0)
            {
                lastMoveSpeedY += ay * Time.deltaTime;
            }
            else
            {
                lastMoveSpeedY -= ay * Time.deltaTime;
            }
            if (Camera.main.transform.localPosition.y > WinGameInput.EdgeDownY
                && Camera.main.transform.localPosition.y < WinGameInput.EdgeUpY)
            {
                Camera.main.transform.Translate(0, lastMoveSpeedY, 0);
            }
            else
            {
                lastMoveSpeedY = 0.0f;
                isTwing = false;
            }
        }
    }
    void LateUpdate()
    {
        if (_gameInput.IsClickDown)
        {
            if (!PlatformUtil.IsTouchDevice)
            {
                lastPostion = _gameInput.MousePosition;
            }
            else
            {
                //计算加速度
                ax = (Mathf.Abs(WinGameInput.CameraMoveSpeed) - 0.0f) / 1.0f;
                ay = (Mathf.Abs(WinGameInput.CameraMoveSpeed) - 0.0f) / 1.0f;
                isTwing = true;
            }
        }
        //twing
        if (_gameInput.IsMove)
        {
            if (!PlatformUtil.IsTouchDevice)
            {
                deltaX = -(_gameInput.MousePosition - lastPostion).x;
                deltaY = -(_gameInput.MousePosition - lastPostion).y;

                //实际挪移位置
                float realDeltaX = 0;
                float realDeltaY = 0;
                if (Mathf.Abs(deltaX) > 9.99999944E-11f)//判断精度，9.99999944E-11f是一个极小值，随便起的
                {
                    realDeltaX = deltaX;
                }
                if (Mathf.Abs(deltaY) > 9.99999944E-11f)
                {
                    realDeltaY = deltaY;
                }
                Vector3 newPos = new Vector3(Camera.main.transform.position.x + realDeltaX * xSpeed * 0.02f
                    , Camera.main.transform.position.y + realDeltaY * ySpeed * 0.02f
                    , Camera.main.transform.position.z);
                if (!(newPos.x > WinGameInput.EdgeRightX || newPos.x < WinGameInput.EdgeLeftX || newPos.y < WinGameInput.EdgeDownY || newPos.y > WinGameInput.EdgeUpY))
                {
                    Camera.main.transform.position = newPos;
                }
                lastPostion = _gameInput.MousePosition;
            }
            else
            {
                if (!isTwing)
                {
                    if (Mathf.Abs(Input.GetTouch(0).deltaPosition.x) > Mathf.Abs(Input.GetTouch(0).deltaPosition.y))
                    {
                        lastMoveSpeedY = 0.0f;
                        float deltaX = Mathf.Abs(Input.GetTouch(0).deltaPosition.x * 0.2f);
                        //正向移动
                        if (Input.GetTouch(0).deltaPosition.x < 0 && Camera.main.transform.localPosition.x < WinGameInput.EdgeRightX)
                        {
                            lastMoveSpeedX = WinGameInput.CameraMoveSpeed * deltaX;
                            //Unity给Translate包装过，所以移动会平滑点
                            Camera.main.transform.Translate(lastMoveSpeedX, 0, 0);
                        }
                        //反向移动
                        else if (Camera.main.transform.localPosition.x > WinGameInput.EdgeLeftX)
                        {
                            lastMoveSpeedX = -WinGameInput.CameraMoveSpeed * deltaX;
                            Camera.main.transform.Translate(lastMoveSpeedX, 0, 0);
                        }
                    }
                    else
                    {
                        lastMoveSpeedX = 0.0f;
                        float deltaX = Mathf.Abs(Input.GetTouch(0).deltaPosition.y * 0.2f);
                        //正向移动
                        if (Input.GetTouch(0).deltaPosition.y < 0 && Camera.main.transform.localPosition.y < WinGameInput.EdgeDownY)
                        {
                            lastMoveSpeedY = -WinGameInput.CameraMoveSpeed * deltaY;
                            //Unity给Translate包装过，所以移动会平滑点
                            Camera.main.transform.Translate(lastMoveSpeedY, 0, 0);
                        }
                        //反向移动
                        else if (Camera.main.transform.localPosition.y > WinGameInput.EdgeUpY)
                        {
                            lastMoveSpeedY = WinGameInput.CameraMoveSpeed * deltaY;
                            Camera.main.transform.Translate(lastMoveSpeedY, 0, 0);
                        }
                    }
                }

                if (_gameInput.TouchCount > 1)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
                    {
                        Vector2 curDist = Input.GetTouch(0).position - Input.GetTouch(1).position;
                        Vector2 preDist = (Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);
                        float touchDelta = curDist.magnitude - preDist.magnitude;

                        if (touchDelta > 0)
                        {
                            if (Camera.main.orthographicSize - 0.5f > 5.0f && Camera.main.orthographicSize - 5.0f < 12.0f)
                            {
                                Camera.main.orthographicSize += 0.5f;
                            }
                        }

                        if (touchDelta < 0)
                        {
                            if (Camera.main.orthographicSize + 0.5f > 5.0f && Camera.main.orthographicSize + 5.0f < 12.0f)
                            {
                                Camera.main.orthographicSize += 0.5f;
                            }
                        }
                    }
                }
            }
        }
    }

}
