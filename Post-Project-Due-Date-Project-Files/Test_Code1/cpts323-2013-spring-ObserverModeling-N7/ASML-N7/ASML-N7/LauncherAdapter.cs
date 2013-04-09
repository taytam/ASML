using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASMLEngineSdk;
using UsbLibrary;

namespace ASML_N7
{
    public class LauncherAdapter : IMissileLauncher
    {
        private MissileLauncher missileLauncher;

        public LauncherAdapter()
        {
            missileLauncher = new MissileLauncher();
        }

        /// <summary>
        /// Resets the missile launcher 
        /// </summary>
        void IMissileLauncher.Reset()
        {
            missileLauncher.command_reset();
        }

        /// <summary>
        /// Moves the missile launcher by a relative amount.
        /// </summary>
        /// <param name="phi"></param>
        /// <param name="phi"></param>
        void IMissileLauncher.MoveBy(double phi, double psi)
        {
            int x_phi = Convert.ToInt32(phi);
            int y_psi = Convert.ToInt32(psi);

            if (x_phi <= 0)
            {
                x_phi = x_phi * -1;
                missileLauncher.command_Left(x_phi);
            }
            else if (x_phi > 0)
            {
                missileLauncher.command_Right(x_phi);
            }
            else
            {
                missileLauncher.command_Left(0);
            }

            if (y_psi <= 0)
            {
                y_psi = y_psi*-1;
                missileLauncher.command_Down(y_psi);
            }
            else if (y_psi > 0)
            {
                missileLauncher.command_Up(y_psi);
            }
            else
            {
                missileLauncher.command_Down(0);
            }
        }

        /// <summary>
        /// Moves the missile launcher to an absolute position.
        /// </summary>
        /// <param name="phi"></param>
        /// <param name="psi"></param>
        void IMissileLauncher.MoveTo(double phi, double psi)
        {
            int x_phi = Convert.ToInt32(phi);
            int y_psi = Convert.ToInt32(psi);

            missileLauncher.command_reset();

            if (x_phi <= 0)
            {
                x_phi = x_phi * -1;
                missileLauncher.command_Left(x_phi);
            }
            else if (x_phi > 0)
            {
                missileLauncher.command_Right(x_phi);
            }
            else
            {
                missileLauncher.command_Left(0);
            }

            if (y_psi <= 0)
            {
                y_psi = y_psi * -1;
                missileLauncher.command_Down(y_psi);
            }
            else if (y_psi > 0)
            {
                missileLauncher.command_Up(y_psi);
            }
            else
            {
                missileLauncher.command_Down(0);
            }
        }

        /// <summary>
        /// Fires a missile.
        /// </summary>
        void IMissileLauncher.Fire()
        {
            missileLauncher.command_Fire();
        }

        /// <summary>
        /// Gets the phi position of the missile launcher.
        /// </summary>
        double IMissileLauncher.Phi 
        {
            get { return missileLauncher.Phi; }
        }
        /// <summary>
        /// Gets the psi position of the missile launcher.
        /// </summary>
        double IMissileLauncher.Psi
        {
            get { return missileLauncher.Psi; }
        }
    }
}
