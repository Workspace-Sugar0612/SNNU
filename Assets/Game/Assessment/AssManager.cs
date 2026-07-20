using SUG.Essentials;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.Rendering;
using System.Linq;

/// <summary>
///  考核模块管理器
/// </summary>
public class AssManager : MonoBehaviour, IAssService, ILocalService
{
    // 当前列表索引
    private int _currIdx = 0;
    public int currIdx { get => _currIdx; }

    // 题目列表
    private List<QuestionData> _questionList = new List<QuestionData>();
    public List<QuestionData> questionList { get => _questionList; }

    // 答题进度记录容器
    private int[] _recordArr;
    public int[] recordArr { get => _recordArr; }

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

            // 记录有多少题目
            _recordArr = new int[_questionList.Count];
        });
    }

    public void ResetData()
    {
        _recordArr = null;
        _recordArr = new int[_questionList.Count];

        _currIdx = 0;
    }

    #region 工具方法

    public QuestionData GetCurrQuestion() => questionList[currIdx];

    public int GetTotalQuestion() => recordArr.Count();

    public int GetFinishQestionCount()
    {
        int finishedCnt = 0;
        foreach (var i in recordArr) finishedCnt += i == 0 ? 0 : 1;
        return finishedCnt;
    }

    public void SetQuestionIndex(int setIdx)
    {
        if (setIdx - 1 >= 0 && setIdx + 1 < _questionList.Count 
            || (setIdx == 0) 
            || (setIdx == _questionList.Count - 1)
           )
            _currIdx = setIdx;
    }

    #endregion
}
