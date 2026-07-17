using SUG.Essentials;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Injectable] public interface IAssService
{
    public int currIdx { get; }

    /// <summary>
    /// 答题情况记录：
    /// 0: 未作答， 1：正确， 2：失败
    /// </summary>
    public int[] recordArr { get; }
    public List<QuestionData> questionList { get; }

    // 获取当前题目
    public QuestionData GetCurrQuestion();

    // 获得总题目数量
    public int GetTotalQuestion();

    // 获取当前完成题目数量
    public int GetFinishQestionCount();

    // 设置当前题目索引，在设置之前需要检查
    public void SetQuestionIndex(int index);
}
