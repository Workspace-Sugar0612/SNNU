using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TheoryMode
{
    None = 0,
    CourseIntro = 1 << 0, // 课程介绍
    SafetySpec = 1 << 1, // 安全规范
    TheoryKnowledge = 1 << 2, // 理论规范
    TheoryAssessment = 1 << 3 // 理论考核
}

public enum TheoryBackMode
{ 
    None = 0, 
    Normal = 1 << 0, // 普通模式
    Assess = 1 << 1 // 考核模式
}