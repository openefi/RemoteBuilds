using System;

namespace OpenEFI_RemoteBuild
{
    public class BuildRequest
    {
        // Version de firmware
        public string OPENEFI_VER_MAJOR { get; set; }
        public string OPENEFI_VER_MINOR { get; set; }
        public string OPENEFI_VER_REV { get; set; }

        // /*-----( Globales )-----*/
        // public int mtr { get; set; }
        // public int CIL { get; set; }
        // public int L_CIL { get; set; }
        // public int DNT { get; set; }
        // public int Alpha { get; set; }
        // public int ED { get; set; }

        // /*-----( C_PWM )-----*/
        // public int PMSI { get; set; }

        // /*-----( Inyeción )-----*/
        // public int AVCI { get; set; }
        // public int ECNT { get; set; }

        // /*-----( _LMB )-----*/
        // public int CTA { get; set; }
        // public int CTB { get; set; }
        // public int P_LMB { get; set; }
        // public int T_LMB { get; set; }
        // public int FLMBA { get; set; }
        // public int FLMBB { get; set; }

        BuildRequest()
        {

        }
    }
}