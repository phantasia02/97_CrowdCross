using UnityEngine;
using UnityEditor;
using System.Collections;
using DG.Tweening;

#if UNITY_EDITOR


[CustomPropertyDrawer(typeof(DOTweenModTest))]
public class CEditorDOTweenModTest : PropertyDrawer
{
    DOTweenModTest m_DOTweenModTestData;

    private SerializedProperty m_OnTriggerEventProp;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect lTempCurRect;

        var MySequenceRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        var MySequenceType = property.FindPropertyRelative("m_MySequenceType");
       // MySequenceType.intValue     = EditorGUI.Popup(MySequenceRect, "MySequenceType"  , MySequenceType.intValue, MySequenceType.enumNames);
        EditorGUI.PropertyField(MySequenceRect, MySequenceType, new GUIContent("MySequenceType"));
        lTempCurRect = MySequenceRect;

        switch ((DOTweenModTest.ESequenceType)MySequenceType.intValue)
        {
            case DOTweenModTest.ESequenceType.eAppend:
            case DOTweenModTest.ESequenceType.eInsert:
            case DOTweenModTest.ESequenceType.eJoin:
            case DOTweenModTest.ESequenceType.eLoop:
                {
                    if (DOTweenModTest.ESequenceType.eInsert == (DOTweenModTest.ESequenceType)MySequenceType.intValue)
                    {
                        var IntervalTimeRect = new Rect(position.x, lTempCurRect.y + lTempCurRect.height, position.width, EditorGUIUtility.singleLineHeight);
                        var IntervalTime = property.FindPropertyRelative("m_StartTime");
                       // IntervalTime.floatValue = EditorGUI.FloatField(IntervalTimeRect, "StartTime", IntervalTime.floatValue);
                        EditorGUI.PropertyField(IntervalTimeRect, IntervalTime, new GUIContent("StartTime"));
                        lTempCurRect = IntervalTimeRect;
                    }
                    else if (DOTweenModTest.ESequenceType.eLoop == (DOTweenModTest.ESequenceType)MySequenceType.intValue)
                    {
                        var LoopTypeRect = new Rect(position.x, lTempCurRect.y + lTempCurRect.height, position.width, EditorGUIUtility.singleLineHeight);
                        var LoopType = property.FindPropertyRelative("m_ELoopTweenType");
                        //LoopType.intValue = EditorGUI.Popup(LoopTypeRect, "LoopTweenType", LoopType.intValue, LoopType.enumNames);
                        EditorGUI.PropertyField(LoopTypeRect, LoopType, new GUIContent("LoopTweenType"));
                        lTempCurRect = LoopTypeRect;

                        var LoopCountRect = new Rect(position.x, lTempCurRect.y + lTempCurRect.height, position.width, EditorGUIUtility.singleLineHeight);
                        var LoopCount = property.FindPropertyRelative("m_LoopCount");
                       // LoopCount.intValue = EditorGUI.IntField(LoopCountRect, "LoopCount", LoopCount.intValue);
                        EditorGUI.PropertyField(LoopCountRect, LoopCount, new GUIContent("LoopCount"));
                        lTempCurRect = LoopCountRect;
                    }

                    var TweenTypetRect = new Rect(position.x, lTempCurRect.y + lTempCurRect.height, position.width, EditorGUIUtility.singleLineHeight);
                    var TweenType = property.FindPropertyRelative("m_TweenType");
                    //TweenType.intValue = EditorGUI.Popup(TweenTypetRect, "TweenType", TweenType.intValue, TweenType.enumNames);
                    EditorGUI.PropertyField(TweenTypetRect, TweenType, new GUIContent("TweenType"));
                    lTempCurRect = TweenTypetRect;

                    var DurationRect = new Rect(position.x, lTempCurRect.y + lTempCurRect.height, position.width, EditorGUIUtility.singleLineHeight);
                    var Duration = property.FindPropertyRelative("m_Duration");
                   // Duration.floatValue = EditorGUI.FloatField(DurationRect, "Duration", Duration.floatValue);
                    EditorGUI.PropertyField(DurationRect, Duration, new GUIContent("Duration"));
                    lTempCurRect = DurationRect;


                    {
                        switch ((DOTweenModTest.ETweenType)TweenType.intValue)
                        {
                            case DOTweenModTest.ETweenType.eCanvasGroupAlpha:
                                {
                                    var TargetAlphaRect = new Rect(position.x, lTempCurRect.y + lTempCurRect.height, position.width, EditorGUIUtility.singleLineHeight);
                                    var TargetAlpha = property.FindPropertyRelative("m_TargetAlpha");
                                   // TargetAlpha.floatValue = EditorGUI.FloatField(TargetAlphaRect, "TargetAlpha", TargetAlpha.floatValue);
                                    EditorGUI.PropertyField(TargetAlphaRect, TargetAlpha, new GUIContent("TargetAlpha"));
                                    lTempCurRect = TargetAlphaRect;
                                }
                                break;
                            case DOTweenModTest.ETweenType.eLocalMovePos:
                                {
                                    var LocalTargetPosRect = new Rect(position.x, lTempCurRect.y + lTempCurRect.height, position.width, EditorGUIUtility.singleLineHeight);
                                    var LocalTargetPos = property.FindPropertyRelative("m_TargetV3");
                                   // LocalTargetPos.vector3Value = EditorGUI.Vector3Field(LocalTargetPosRect, "LocalTargetPos", LocalTargetPos.vector3Value);
                                    EditorGUI.PropertyField(LocalTargetPosRect, LocalTargetPos, new GUIContent("LocalTargetPos"));
                                    lTempCurRect = LocalTargetPosRect;
                                }
                                break;
                            case DOTweenModTest.ETweenType.eLocalRotate:
                            case DOTweenModTest.ETweenType.eRotate:
                            case DOTweenModTest.ETweenType.eScale:
                                {
                                    var LocalTargetRotateRect = new Rect(position.x, lTempCurRect.y + lTempCurRect.height, position.width, EditorGUIUtility.singleLineHeight);
                                    var LocalTargetRotate = property.FindPropertyRelative("m_TargetV3");

                                    DOTweenModTest.ETweenType lTempETweenType = (DOTweenModTest.ETweenType)TweenType.intValue;
                                    string lTempstring = "";

                                    if (lTempETweenType == DOTweenModTest.ETweenType.eLocalRotate)
                                        lTempstring = "LocalTargetRotate";
                                    else if (lTempETweenType == DOTweenModTest.ETweenType.eRotate)
                                        lTempstring = "TargetRotate";
                                    else if (lTempETweenType == DOTweenModTest.ETweenType.eScale)
                                        lTempstring = "TargetScaleSize";

                                   // LocalTargetRotate.vector3Value = EditorGUI.Vector3Field(LocalTargetRotateRect, lTempstring, LocalTargetRotate.vector3Value);
                                    EditorGUI.PropertyField(LocalTargetRotateRect, LocalTargetRotate, new GUIContent(lTempstring));
                                    lTempCurRect = LocalTargetRotateRect;

                                    if (lTempETweenType == DOTweenModTest.ETweenType.eScale)
                                    {
                                        var startZeroRect = new Rect(position.x, lTempCurRect.y + lTempCurRect.height, position.width, EditorGUIUtility.singleLineHeight);
                                        var startZero = property.FindPropertyRelative("m_startZero");
                                       // startZero.boolValue = EditorGUI.PropertyField(startZeroRect, startZero);
                                        EditorGUI.PropertyField(startZeroRect, startZero, new GUIContent("StartZero"));
                                        lTempCurRect = startZeroRect;
                                    }
                                }
                                break;
                            case DOTweenModTest.ETweenType.eMaterialAlpha:
                                {
                                    var TargetAlphaRect = new Rect(position.x, lTempCurRect.y + lTempCurRect.height, position.width, EditorGUIUtility.singleLineHeight);
                                    var TargetAlpha = property.FindPropertyRelative("m_TargetAlpha");
                                  //  TargetAlpha.floatValue = EditorGUI.FloatField(TargetAlphaRect, "TargetAlpha", TargetAlpha.floatValue);
                                    EditorGUI.PropertyField(TargetAlphaRect, TargetAlpha, new GUIContent("TargetAlpha"));
                                    lTempCurRect = TargetAlphaRect;
                                }
                                break;
                            case DOTweenModTest.ETweenType.eMaterialColorAlpha:
                                {
                                    var TargetColorRect = new Rect(position.x, lTempCurRect.y + lTempCurRect.height, position.width, EditorGUIUtility.singleLineHeight);
                                    var TargetColor = property.FindPropertyRelative("m_TargetColor");
                                    EditorGUI.PropertyField(TargetColorRect, TargetColor, new GUIContent("TargetColor"));
                                    lTempCurRect = TargetColorRect;

                                    var MaterialKeywordRect = new Rect(position.x, lTempCurRect.y + lTempCurRect.height, position.width, EditorGUIUtility.singleLineHeight);
                                    var MaterialKeyword = property.FindPropertyRelative("m_MaterialKeywordToID");
                                    EditorGUI.PropertyField(MaterialKeywordRect, MaterialKeyword, new GUIContent("MaterialKeywordToID"));
                                    lTempCurRect = MaterialKeywordRect;
                                }
                                break;
                            case DOTweenModTest.ETweenType.eMoveWordpos:
                                {
                                    var TargetObjRect = new Rect(position.x, lTempCurRect.y + lTempCurRect.height, position.width, EditorGUIUtility.singleLineHeight);
                                    var TargetObj = property.FindPropertyRelative("m_TargetTransform");
                                   // TargetObj.objectReferenceValue = EditorGUI.ObjectField(TargetObjRect, "TargetTransform", TargetObj.objectReferenceValue, typeof(Transform));
                                    EditorGUI.PropertyField(TargetObjRect, TargetObj, new GUIContent("TargetTransform"));
                                    lTempCurRect = TargetObjRect;
                                }
                                break;
                        }
                    }

                    var animationCurveValueRect = new Rect(position.x, lTempCurRect.y + lTempCurRect.height, position.width, EditorGUIUtility.singleLineHeight);
                    var animationCurveValue = property.FindPropertyRelative("m_MyAnimationCurve");
                   // animationCurveValue.animationCurveValue = EditorGUI.CurveField(animationCurveValueRect, "Curve", animationCurveValue.animationCurveValue);
                    EditorGUI.PropertyField(animationCurveValueRect, animationCurveValue, new GUIContent("Curve"));
                    lTempCurRect = animationCurveValueRect;
                }
                break;

            case DOTweenModTest.ESequenceType.eInterval:
                {
                    var IntervalTimeRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
                    var IntervalTime = property.FindPropertyRelative("m_StartTime");
                    EditorGUI.PropertyField(IntervalTimeRect, IntervalTime, new GUIContent("IntervalTime"));
                    lTempCurRect = IntervalTimeRect;
                }
                break;

            case DOTweenModTest.ESequenceType.eCallback:
                {
                    var IntervalTimeRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, EditorGUIUtility.singleLineHeight);
                    var lTempBarkEvent = property.FindPropertyRelative("m_UnityEvent");
                    EditorGUI.PropertyField(IntervalTimeRect, lTempBarkEvent, new GUIContent("OnTrigger Event"));
                    lTempCurRect = IntervalTimeRect;
                }
                break;
        }


        

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();

    }


    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var MySequenceType = property.FindPropertyRelative("m_MySequenceType");
        float lTempH = 2.0f;

        switch ((DOTweenModTest.ESequenceType)MySequenceType.intValue)
        {
            case DOTweenModTest.ESequenceType.eAppend:
            case DOTweenModTest.ESequenceType.eInsert:
            case DOTweenModTest.ESequenceType.eJoin:
            case DOTweenModTest.ESequenceType.eLoop:
                lTempH = 8.0f;
                break;
            case DOTweenModTest.ESequenceType.eInterval:
                lTempH = 3.0f;
                break;
            case DOTweenModTest.ESequenceType.eCallback:
                lTempH = 8.0f;
                break;
        }


        return (20 - EditorGUIUtility.singleLineHeight) + (EditorGUIUtility.singleLineHeight * lTempH);
        //return (20 - EditorGUIUtility.singleLineHeight) +TotleHigh;
    }
}
#endif