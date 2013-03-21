using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ASML_N7
{
    public class TargetManager
    {
        static TargetManager m_TargetManager = null;
        private List<Target> TargetList;

        public static TargetManager GetInstance()
        {
            if(m_TargetManager == null)
            {
                m_TargetManager = new TargetManager();
            }

            return m_TargetManager;
        }

        private TargetManager()
        {
            TargetList = new List<Target>();
        }

        public void Add_Targets(Target target)
        {
            TargetList.Insert(target._targetCount, target);
        }
    }                                                                                               
}
