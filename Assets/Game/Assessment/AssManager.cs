using LitJson;
using SUG.Essentials;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.UI;

/// <summary>
///  考核模块管理器
/// </summary>
[Service(ServiceLifetime.Scene)]
public class AssManager : MonoBehaviour, IAssService
{
    // 当前列表索引
    private int _currIdx = 0;
    public int currIdx { get => _currIdx; }

    // 题目列表
    private List<QuestionData> _questionList = new List<QuestionData>();
    public List<QuestionData> questionList { get => _questionList; }

    // 答题进度记录容器
    private TopicRecordPkg[] _recordArr;
    public TopicRecordPkg[] recordArr { get => _recordArr; }

    // 总得分
    public float finalScore { get => CalTheFinalScore(); }

    // 正确题目数
    public int correctCount { get => GetCorrectCount(); }

    // 错误题目数
    public int wrongCount { get => GetWrongCount(); }

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
            _recordArr = new TopicRecordPkg[_questionList.Count];

            for (int i = 0; i < recordArr.Length; ++i)
                recordArr[i] = new TopicRecordPkg();
        });
    }

    public void ResetData()
    {
        _recordArr = null;
        _recordArr = new TopicRecordPkg[_questionList.Count];

        _currIdx = 0;
    }

    #region 工具方法

    public QuestionData GetCurrQuestion() => questionList[currIdx];

    public QuestionData GetIndexQuestion(int index)
    {
        QuestionData data = null;

        if (index >= 0 && index < questionList.Count())
            data = questionList[index];
        
        return data;
    }

    public int GetTotalQuestion() => recordArr.Count();

    public int GetFinishQestionCount()
    {
        int finishedCnt = 0;

        foreach (var i in recordArr)
        {
            if (i == null)
                continue;

            finishedCnt += i.mark == 0 ? 0 : 1;
        }

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

    public int NextQuestion()
    {
        _currIdx = (_currIdx + 1) % _questionList.Count;
        return _currIdx;
    }

    public int PrevQuestion()
    {
        _currIdx = (_currIdx - 1 + _questionList.Count) % _questionList.Count;
        return _currIdx;
    }

    public void ValidateIndexTitle(int index)
    {
        // 如果_recordArr[index].selectContents为空则标记为未作答
        if (_recordArr[index].selectContents.Count == 0)
        {
            _recordArr[index].mark = 0;
            return;
        }

        // 判断题目是否正确
        var data = _questionList[index];
        foreach (OptionData op in data.options)
        {
            bool a = op.isAnswer;
            bool b = _recordArr[index].selectContents.Contains(op.content);
            Debug.Log($"{a} : {b}");
            _recordArr[index].mark = ((a && b) || (!a && !b)) ? 1 : 2;

            // 如果出现一个选项是有问题的，那么不在验证直接返回
            if (_recordArr[index].mark == 2) 
                break;
        }
        Debug.Log($"Rcore: {index} : {_recordArr[index].mark}");
    }

    /// <summary>
    /// 计算最终得分
    /// </summary>
    private float CalTheFinalScore()
    {
        float _finalScore = 0.0f;
        for (int i = 0; i < _recordArr.Count(); ++ i)
        {
            QuestionData data = null;
            TopicRecordPkg pkg = _recordArr[i];
            if (i >= 0 && i < _questionList.Count)
            {
                data = _questionList[i];
                _finalScore += pkg.mark == 1 ? data.score : 0.0f;
            }
        }
        return _finalScore;
    }

    /// <summary>
    /// 正确题目数量
    /// </summary>
    /// <returns></returns>
    private int GetCorrectCount() => _recordArr.Count(x => x.mark == 1);

    /// <summary>
    /// 错误题目数量
    /// </summary>
    /// <returns></returns>
    private int GetWrongCount() => _recordArr.Count(x => x.mark == 2);

    #endregion

    #region 工具方法

    public char GetOptionLetter(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Input cannot be null or empty.", nameof(text));

        char c = text[0];

        if ((c < 'A' || c > 'Z') && (c < 'a' || c > 'z'))
            throw new FormatException("The first character must be an English letter.");

        return char.ToUpperInvariant(c);
    }

    #endregion
}
