using System.Runtime.InteropServices;

namespace auxiliar
{
    class AudioMixerHelper
    {
        public const int MMSYSERR_NOERROR = 0;
        public const int MAXPNAMELEN = 32;
        public const int MIXER_SHORT_NAME_CHARS = 16;
        public const int MIXER_LONG_NAME_CHARS = 64;
        public const int MIXER_GETLINECONTROLSF_ONEBYTYPE = 2;
        public const int MIXER_GETLINEINFOF_COMPONENTTYPE = 3;
        public const int MIXER_GETCONTROLDETAILSF_VALUE = 0;
        public const int MIXER_SETCONTROLDETAILSF_VALUE = 0;
        public const int MIXERCONTROL_CT_CLASS_SWITCH = 0x20000000;
        public const int MIXERCONTROL_CT_CLASS_FADER = 0x50000000;
        public const int MIXERCONTROL_CT_UNITS_BOOLEAN = 0x10000;
        public const int MIXERCONTROL_CT_UNITS_UNSIGNED = 0x30000;
        public const int MIXERCONTROL_CONTROLTYPE_FADER = (MIXERCONTROL_CT_CLASS_FADER | MIXERCONTROL_CT_UNITS_UNSIGNED);
        public const int MIXERCONTROL_CONTROLTYPE_VOLUME = (MIXERCONTROL_CONTROLTYPE_FADER + 1);
        public const int MIXERCONTROL_CONTROLTYPE_BOOLEAN = (MIXERCONTROL_CT_CLASS_SWITCH | MIXERCONTROL_CT_UNITS_BOOLEAN);
        public const int MIXERCONTROL_CONTROLTYPE_MUTE = (MIXERCONTROL_CONTROLTYPE_BOOLEAN + 2);
        public const int MIXERLINE_COMPONENTTYPE_SRC_FIRST = 0x1000;
        public const int MIXERLINE_COMPONENTTYPE_SRC_LINE = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 2);
        public const int MIXERLINE_COMPONENTTYPE_DST_SPEAKERS = 4;
        public const int MM_MIXM_CONTROL_CHANGE = 0x3D1;
        public const int CALLBACK_WINDOW = 0x10000;

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        private static extern int mixerOpen(out int phmx, int uMxId, IntPtr dwCallback, IntPtr dwInstance, int fdwOpen);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        private static extern int mixerClose(int hmx);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        private static extern int mixerGetControlDetailsA(int hmxobj, ref MIXERCONTROLDETAILS pmxcd, int fdwDetails);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        private static extern int mixerGetLineControlsA(int hmxobj, ref MIXERLINECONTROLS pmxlc, int fdwControls);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        private static extern int mixerGetLineInfoA(int hmxobj, ref MIXERLINE pmxl, int fdwInfo);

        [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
        private static extern int mixerSetControlDetails(int hmxobj, ref MIXERCONTROLDETAILS pmxcd, int fdwDetails);

        public struct MIXERCONTROL
        {
            public int cbStruct = 0;
            public int dwControlID = 0;
            public int dwControlType = 0;
            public int fdwControl = 0;
            public int cMultipleItems = 0;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_SHORT_NAME_CHARS)]
            public string szShortName = string.Empty;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_LONG_NAME_CHARS)]
            public string szName = string.Empty;

            public int lMinimum = 0;
            public int lMaximum = 0;

            [MarshalAs(UnmanagedType.U4, SizeConst = 10)]
            public int reserved = 0;

            public MIXERCONTROL() { }
        }

        public struct MIXERCONTROLDETAILS
        {
            public int cbStruct = 0;
            public int dwControlID = 0;
            public int cChannels = 0;
            public int item = 0;
            public int cbDetails = 0;
            public IntPtr paDetails = IntPtr.Zero;

            public MIXERCONTROLDETAILS() { }
        }

        public struct MIXERCONTROLDETAILS_UNSIGNED
        {
            public int dwValue = 0;
            public MIXERCONTROLDETAILS_UNSIGNED() { }
        }

        public struct MIXERLINE
        {
            public int cbStruct = 0;
            public int dwDestination = 0;
            public int dwSource = 0;
            public int dwLineID = 0;
            public int fdwLine = 0;
            public int dwUser = 0;
            public int dwComponentType = 0;
            public int cChannels = 0;
            public int cConnections = 0;
            public int cControls = 0;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_SHORT_NAME_CHARS)]
            public string szShortName = string.Empty;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MIXER_LONG_NAME_CHARS)]
            public string szName = string.Empty;

            public int dwType = 0;
            public int dwDeviceID = 0;
            public int wMid = 0;
            public int wPid = 0;
            public int vDriverVersion = 0;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAXPNAMELEN)]
            public string szPname = string.Empty;

            public MIXERLINE() { }
        }

        public struct MIXERLINECONTROLS
        {
            public int cbStruct = 0;
            public int dwLineID = 0;
            public int dwControl = 0;
            public int cControls = 0;
            public int cbmxctrl = 0;
            public IntPtr pamxctrl = IntPtr.Zero;

            public MIXERLINECONTROLS() { }
        }

        private static bool GetMixerControl(int hmixer, int component, int control, out MIXERCONTROL mxc, out int vCurrentVol)
        {
            bool retValue = false;
            vCurrentVol = -1;
            mxc = new MIXERCONTROL();

            MIXERLINECONTROLS mxlc = new MIXERLINECONTROLS();
            MIXERLINE mxl = new MIXERLINE();
            MIXERCONTROLDETAILS pmxcd = new MIXERCONTROLDETAILS();

            mxl.cbStruct = Marshal.SizeOf(mxl);
            mxl.dwComponentType = component;

            int rc = mixerGetLineInfoA(hmixer, ref mxl, MIXER_GETLINEINFOF_COMPONENTTYPE);

            if (MMSYSERR_NOERROR == rc)
            {
                int sizeofMIXERCONTROL = 152;
                mxlc.pamxctrl = Marshal.AllocCoTaskMem(sizeofMIXERCONTROL);
                mxlc.cbStruct = Marshal.SizeOf(mxlc);
                mxlc.dwLineID = mxl.dwLineID;
                mxlc.dwControl = control;
                mxlc.cControls = 1;
                mxlc.cbmxctrl = sizeofMIXERCONTROL;

                rc = mixerGetLineControlsA(hmixer, ref mxlc, MIXER_GETLINECONTROLSF_ONEBYTYPE);

                if (MMSYSERR_NOERROR == rc)
                {
                    retValue = true;
                    mxc = Marshal.PtrToStructure<MIXERCONTROL>(mxlc.pamxctrl);
                }

                int sizeofMIXERCONTROLDETAILS = Marshal.SizeOf(typeof(MIXERCONTROLDETAILS));
                int sizeofMIXERCONTROLDETAILS_UNSIGNED = Marshal.SizeOf(typeof(MIXERCONTROLDETAILS_UNSIGNED));

                pmxcd.cbStruct = sizeofMIXERCONTROLDETAILS;
                pmxcd.dwControlID = mxc.dwControlID;
                pmxcd.paDetails = Marshal.AllocCoTaskMem(sizeofMIXERCONTROLDETAILS_UNSIGNED);
                pmxcd.cChannels = 1;
                pmxcd.item = 0;
                pmxcd.cbDetails = sizeofMIXERCONTROLDETAILS_UNSIGNED;

                rc = mixerGetControlDetailsA(hmixer, ref pmxcd, MIXER_GETCONTROLDETAILSF_VALUE);

                if (MMSYSERR_NOERROR == rc)
                {
                    var du = Marshal.PtrToStructure<MIXERCONTROLDETAILS_UNSIGNED>(pmxcd.paDetails);
                    vCurrentVol = du.dwValue;
                }

                Marshal.FreeCoTaskMem(mxlc.pamxctrl);
                Marshal.FreeCoTaskMem(pmxcd.paDetails);
            }

            return retValue;
        }

        private static bool SetVolumeControl(int hmixer, MIXERCONTROL mxc, int volume)
        {
            MIXERCONTROLDETAILS mxcd = new MIXERCONTROLDETAILS();
            MIXERCONTROLDETAILS_UNSIGNED vol = new MIXERCONTROLDETAILS_UNSIGNED();

            mxcd.item = 0;
            mxcd.dwControlID = mxc.dwControlID;
            mxcd.cbStruct = Marshal.SizeOf(mxcd);
            mxcd.cbDetails = Marshal.SizeOf(vol);
            mxcd.cChannels = 1;
            vol.dwValue = volume;

            mxcd.paDetails = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(MIXERCONTROLDETAILS_UNSIGNED)));
            Marshal.StructureToPtr(vol, mxcd.paDetails, false);

            int rc = mixerSetControlDetails(hmixer, ref mxcd, MIXER_SETCONTROLDETAILSF_VALUE);
            
            Marshal.FreeCoTaskMem(mxcd.paDetails);

            return MMSYSERR_NOERROR == rc;
        }

        public static int GetVolume(int control, int component)
        {
            int hmixer = 0;
            int currVol = -1;
            int rc = mixerOpen(out hmixer, 0, IntPtr.Zero, IntPtr.Zero, 0);
            if (rc == MMSYSERR_NOERROR)
            {
                GetMixerControl(hmixer, component, control, out _, out currVol);
                mixerClose(hmixer);
            }
            return currVol;
        }

        public static void SetVolume(int control, int component, int newVol)
        {
            int hmixer = 0;
            int rc = mixerOpen(out hmixer, 0, IntPtr.Zero, IntPtr.Zero, 0);
            if (rc == MMSYSERR_NOERROR)
            {
                if (GetMixerControl(hmixer, component, control, out var volCtrl, out _))
                {
                    SetVolumeControl(hmixer, volCtrl, newVol);
                }
                mixerClose(hmixer);
            }
        }

        public static bool MonitorControl(int iw)
        {
            int rc = mixerOpen(out _, 0, (IntPtr)iw, IntPtr.Zero, CALLBACK_WINDOW);
            return MMSYSERR_NOERROR == rc;
        }

        public static int CheckMixer()
        {
            int rc1 = mixerOpen(out int hmixer, 0, IntPtr.Zero, IntPtr.Zero, 0);
            int rc2 = (rc1 == MMSYSERR_NOERROR) ? mixerClose(hmixer) : -1;
            return (MMSYSERR_NOERROR == rc1 && MMSYSERR_NOERROR == rc2) ? MMSYSERR_NOERROR : -1;
        }

        public static int GetControlID(int component, int control)
        {
            if (GetMixerControl(0, component, control, out var mxc, out _))
            {
                return mxc.dwControlID;
            }
            return -1;
        }
    }
}