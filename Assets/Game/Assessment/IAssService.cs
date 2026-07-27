using SUG.Essentials;
using System;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// 单个题目记录包
/// </summary>
public class TopicRecordPkg
{
    /// <summary>
    /// 答题情况记录：
    /// 0: 未作答， 1：正确， 2：失败
    /// </summary>
    public int mark = 0;

    /// <summary>
    /// 选择的内容
    /// </summary>
    public List<string> selectContents = new List<string>();

    // 构造函数
    public TopicRecordPkg()
    {
        mark = 0;
    }

    public TopicRecordPkg(int mark, List<string> content)
    {
        this.mark = mark;
        selectContents = content;
    }

    /// <summary>
    /// 记录当前题目选择的内容
    /// </summary>
    /// <param name="isSave">保存还是删除</param>
    /// <param name="content">选项内容</param>
    /// <param name="isSingle">这个包是否为单选题</param>
    public void Record(bool isSave, string content, bool isSingle)
    {
        UnityEngine.Debug.Log($"isSave: {isSave}, content: {content}, isSingle: {isSingle}");
        // 如果这个题时单选题
        // 那么需要把之前的记录内容列表清空
        if (isSingle)
            selectContents.Clear();

        // 添加/删除这个选项
        if (!isSave && selectContents.Contains(content))
            selectContents.Remove(content);
        
        if (isSave)
            selectContents.Add(content);
    }
}

[Injectable] public interface IAssService
{
    public int currIdx { get; }

    /// <summary>
    /// 答题情况记录：
    /// 0: 未作答， 1：正确， 2：失败
    /// </summary>
    public TopicRecordPkg[] recordArr { get; }
    public List<QuestionData> questionList { get; }

    // 最终得分
    public float finalScore { get; }

    // 错题数
    public int wrongCount { get; }
    
    // 对题数
    public int correctCount { get; }

    // 获取当前题目
    public QuestionData GetCurrQuestion();

    // 获取index索引的题目
    public QuestionData GetIndexQuestion(int index);

    // 获得总题目数量
    public int GetTotalQuestion();

    // 获取当前完成题目数量
    public int GetFinishQestionCount();

    // 设置当前题目索引，在设置之前需要检查
    public void SetQuestionIndex(int index);

    /// <summary>
    /// 下一题
    /// </summary>
    /// <returns> 当前题目列表索引 </returns>
    public int NextQuestion();

    /// <summary>
    /// 上一题
    /// </summary>
    /// <returns> 当前题目列表索引 </returns>
    public int PrevQuestion();

    // 判断该索引的题目对错
    public void ValidateIndexTitle(int index);

    // 重置
    public void ResetData();

    #region 工具

    /// <summary>
    /// 获取字符串开头的选项字母，并返回大写字母。
    /// </summary>
    public char GetOptionLetter(string text);

    #endregion
}
