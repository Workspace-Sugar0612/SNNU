using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 题目数据
public class QuestionData
{
    public string title;
    public List<OptionData> options = new List<OptionData>();
    public string analysis;
    public bool isSingle = true;
}

// 选择题数据
public class OptionData
{
    public bool isAnswer = false;
    public string content;
}
