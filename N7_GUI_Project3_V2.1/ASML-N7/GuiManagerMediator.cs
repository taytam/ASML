using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASML_N7
{
    public class GuiManagerMediator : EventArgs
    {
        private double phi;
        private double theta;
        private string message;
        private bool enemiesLeft;

        private object ThetaLabel;
        private object PhiLabel;
        private object MissileLabel;

        //public delegate void Update(object sender);
        //public event Update UpdateGUICoordinates;
        private static GuiManagerMediator m_guiManagerMediator = null;

        //public void UpdateGUI()
        //{
        //    ThetaLabel = Phi;
        //    PhiLabel = Phi;
        //    //MissileLabel = 
        //}

        //public void loadLabels(object theta, object phi, object missiles)
        //{
        //    ThetaLabel = theta;
        //    PhiLabel = phi;
        //    MissileLabel = missiles;
        //}

        public double Phi
        {
            get { return phi; }
            set { phi = value; }
        }

        public double Theta
        {
            get { return theta; }
            set { theta = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public bool EnemiesLeft
        {
            get { return enemiesLeft; }
            set { enemiesLeft = value; }
        }

        public static GuiManagerMediator getInstance()
        {
            if (m_guiManagerMediator == null)
            {
                m_guiManagerMediator = new GuiManagerMediator();
            }

            return m_guiManagerMediator;
        }

        private GuiManagerMediator()
        {
            phi = 0;
            theta = 0;
            message = "";
            enemiesLeft = false;
        }
    }
}
