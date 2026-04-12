using System;
using System.Collections.Generic;

namespace MKU.Scripts.SkillSystem
{
    [Serializable]
    public class SkillTree
    {
        public SkillTree(){}
        public List<SkillCollection> _Skills = new ();
        public Skills _debuff;
    }
}