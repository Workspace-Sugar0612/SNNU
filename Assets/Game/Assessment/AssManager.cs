using SUG.Essentials;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  考核模块管理器
/// </summary>
public class AssManager : MonoBehaviour, IAssService, ILocalService
{
    private int _currIdx = 0;
    public int currIdx { get => _currIdx; set => _currIdx = value; }

    private List<QuestionData> _questionList = new List<QuestionData>();
    public List<QuestionData> questionList { get => _questionList; set => throw new System.NotImplementedException(); }

    // 获取当前问题
    public QuestionData GetCurrQuestion()
    {
        return questionList[currIdx];
    }
}
