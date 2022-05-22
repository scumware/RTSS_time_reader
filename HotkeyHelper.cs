using System.Text;
using System.Windows.Forms;
using RTSS_time_reader.WindowsInterop;

namespace RTSS_time_reader
{
    public static class HotkeyHelper
    {
        public static string GetDescription(this Win32A.KeyModifiers p_value)
        {
            if (p_value == Win32A.KeyModifiers.None)
                return p_value.ToString();

            var result = string.Empty;
            var firstMod = true;

            result += KeyModifierToString(p_value, Win32A.KeyModifiers.Ctrl, ref firstMod);
            result += KeyModifierToString(p_value, Win32A.KeyModifiers.Alt, ref firstMod);
            result += KeyModifierToString(p_value, Win32A.KeyModifiers.Shift, ref firstMod);
            result += KeyModifierToString(p_value, Win32A.KeyModifiers.Win, ref firstMod);

            return result;
        }

        private static string KeyModifierToString(Win32A.KeyModifiers p_value, Win32A.KeyModifiers modifier, ref bool firstMod)
        {
            string result = string.Empty;

            if ((p_value & modifier) != 0)
            {
                if (firstMod)
                    result += modifier;
                else
                    result += "+" + modifier;

                firstMod = false;
            }

            return result;
        }
    }

    public struct Hotkey
    {
        public Hotkey(Win32A.KeyModifiers p_modifiers, Keys p_key)
        {
            Modifiers = p_modifiers;
            Key = p_key;
        }
        public Win32A.KeyModifiers Modifiers;
        public System.Windows.Forms.Keys Key;

        public bool IsEmpty
        {
            get { return (Modifiers == Win32A.KeyModifiers.None) && (Key == Keys.None); }
        }

        public static bool operator ==(Hotkey p_left, Hotkey p_right)
        {
            return (p_left.Key == p_right.Key) && (p_left.Modifiers == p_right.Modifiers);
        }
        public static bool operator !=(Hotkey p_left, Hotkey p_right)
        {
            return false == (p_left == p_right);
        }
    }
}