using SUG.Essentials;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

/// <summary>
///  考核模块管理器
/// </summary>
public class AssManager : MonoBehaviour, IAssService, ILocalService
{
    // 当前列表索引
    private int _currIdx = 0;
    public int currIdx { get => _currIdx; set => _currIdx = value; }

    // 题目列表
    private List<QuestionData> _questionList = new List<QuestionData>();
    public List<QuestionData> questionList { get => _questionList; set => throw new System.NotImplementedException(); }

    // 题库位置路径
    private readonly string _titlePath = Application.streamingAssetsPath + "/QuestionList.json";

    // Inject
    [Inject] private IFileService _fileMgr;


    // life cycyle
    private void Awake()
    {
        Initializtion();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Initializtion()
    {
        _fileMgr.ReadText(_titlePath, (t) => 
        {
            _questionList = JsonMapper.ToObject<List<QuestionData>>(t);
        });
    }

    // 获取当前问题
    public QuestionData GetCurrQuestion()
    {
        return questionList[currIdx];
    }
}
