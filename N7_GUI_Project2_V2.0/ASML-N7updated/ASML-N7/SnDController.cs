using System.Collections.Generic;

namespace ASML_N7
{
    public class SnDController
    {
        private static SnDController m_SnDController = null;
        private IMissileLauncher launcher = new LauncherAdapter();
        private List<Target> killList = new List<Target>();
        //public GuiManagerMediator guiManger;

        //public event UpdateSubscriber updater;
        //public delegate void updateSubcriber();

        public SnDController(string mode)
        {
            SearchAndDestroyFactory searchAndDestroy = new SearchAndDestroyFactory();
            DestroyMode destroyMode = searchAndDestroy.Check_Mode(mode);

            killList = destroyMode.getTargets();
        }

        public void SearchAndDestroy()
        {
            //guiManger = GuiManagerMediator.getInstance();
            launcher.Reset();
            int firedFourTimes = 0;
            //bool targetsLeft = true;
            foreach (Target target in killList)
            {
                launcher.MoveTo(target.Phi, target.Theta);
                launcher.Fire();
                //firedFourTimes += 1;
                if (firedFourTimes >= 4)
                {
                    break;
                    //targetsLeft = false;
                    //return targetsLeft;
                }

                launcher.Reset();

                //mLStop += new RoutedEventHandler();
            }

            //targetsLeft = false;
            //return targetsLeft;
        }

        public void ChangeMode(string mode)
        {
            SearchAndDestroyFactory searchAndDestroy = new SearchAndDestroyFactory();
            DestroyMode destroyMode = searchAndDestroy.Check_Mode(mode);

            killList = destroyMode.getTargets();
        }

        public static SnDController getInstance(string mode)
        {
            if (m_SnDController == null)
            {
                m_SnDController = new SnDController(mode);
                return m_SnDController;
            }

            m_SnDController.ChangeMode(mode);

            return m_SnDController;
        }
    }
}