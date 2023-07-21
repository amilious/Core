
using Amilious.Core;
using Amilious.Core.Attributes;
using TMPro;

using UnityEngine;
using Amilious.Core.Extensions;

public class PositionTracker : AmiliousBehavior {

    [SerializeField] private TMP_Text topLeft;
    [SerializeField] private TMP_Text topRight;
    [SerializeField] private TMP_Text bottomLeft;
    [SerializeField] private TMP_Text bottomRight;
    [SerializeField] private TMP_Text center;

    [SerializeField] private MoveSource position;
    [SerializeField] private float rotation = 90;
    
    private RectTransform _rectTransform;
    private readonly Vector3[] _cornersW = new Vector3[4];
    private readonly Vector3[] _cornersL = new Vector3[4];
    private Vector3 _centerPosW;
    private Vector3 _centerPosL;

    public RectTransform RectTransform => this.GetCacheComponent(ref _rectTransform);
    private void Update() {
        RectTransform.GetWorldCorners(_cornersW);
        RectTransform.GetLocalCorners(_cornersL);
        _centerPosW = _cornersW[0] + (_cornersW[2] - _cornersW[0]) * .5f;
        _centerPosL = _cornersL[0] + (_cornersL[2] - _cornersL[0]) * .5f;
        bottomLeft.SetText(CornerText("Bottom Left",_cornersW[0],_cornersL[0]));
        topLeft.SetText(CornerText("Top Left",_cornersW[1],_cornersL[1]));
        topRight.SetText(CornerText("Top Right",_cornersW[2],_cornersL[2]));
        bottomRight.SetText(CornerText("Bottom Right",_cornersW[2],_cornersL[3]));
        center.SetText(
    CornerText("Position",RectTransform.position,RectTransform.localPosition) + "\n\n"+
            CornerText("Center",_centerPosW,_centerPosL));
    }

    [AmiButton("Rotate")]
    private void Rotate() {
        RectTransform.RotateAroundCorner(position,rotation);
    }

    private string CornerText(string name, Vector2 world, Vector2 local) => name + "\n" + world + "\n" + local;

}
