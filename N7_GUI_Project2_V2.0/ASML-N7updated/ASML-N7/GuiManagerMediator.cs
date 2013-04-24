using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASML_N7
{
    class GuiManagerMediator : EventArgs
    {
        private double phi;
        private double theta;
        private string message;
        private bool enemiesLeft;

        private static GuiManagerMediator m_guiManagerMediator = null;

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
