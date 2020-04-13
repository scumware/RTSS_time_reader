using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTSS_time_reader
{
    public partial class HotkeyEditorDialog : Form
    {
        public HotkeyEditorDialog()
        {
            InitializeComponent();
        }

        public ushort? Atom { get; set; }
        public int? RegistredHotkeyId { get; set; }
        public Keys? Hotkey { get; set; }
        public Keys? RegistredHotkey { get; set; }

        private Keys m_pressedKey = 0;
        private Win32A.KeyModifiers m_keyModifiers;

        protected override bool ProcessKeyPreview(ref Message p_message)
        {
            if (p_message.HWnd != txtHotkeyEditor.Handle)
                return base.ProcessKeyPreview(ref p_message);

            var msg = (Win32A.WindowsMessages) p_message.Msg;
            switch (msg)
            {
                case Win32A.WindowsMessages.WM_SYSKEYDOWN:
                    m_keyModifiers |= Win32A.KeyModifiers.Alt;
                    ShowPressedKeys();
                    return true;

                case Win32A.WindowsMessages.WM_KEYDOWN:
                    var key = GetVkKey(p_message);
                    if (key == Keys.ControlKey)
                    {
                        m_keyModifiers |= Win32A.KeyModifiers.Ctrl;
                    }
                    else if (key == Keys.ShiftKey)
                    {
                        m_keyModifiers |= Win32A.KeyModifiers.Shift;
                    }
                    else if ((key == Keys.LWin) || (key == Keys.RWin))
                    {
                        m_keyModifiers |= Win32A.KeyModifiers.Win;
                    }
                    else if (key == Keys.Menu)
                    {
                        m_keyModifiers |= Win32A.KeyModifiers.Alt;
                    }
                    else
                    {
                        m_pressedKey = key;
                    }

                    ShowPressedKeys();
                    return true;



                case Win32A.WindowsMessages.WM_SYSKEYUP:
                    m_keyModifiers &= ~Win32A.KeyModifiers.Alt;
                    ShowPressedKeys();
                    return true;

                case Win32A.WindowsMessages.WM_KEYUP:
                    key = GetVkKey(p_message);

                    if (key == Keys.ControlKey)
                    {
                        m_keyModifiers &= ~Win32A.KeyModifiers.Ctrl;
                    }
                    else if (key == Keys.ShiftKey)
                    {
                        m_keyModifiers &= ~Win32A.KeyModifiers.Shift;
                    }
                    else if ((key == Keys.LWin) || (key == Keys.RWin))
                    {
                        m_keyModifiers &= ~Win32A.KeyModifiers.Win;
                    }
                    else if (key == Keys.Menu)
                    {
                        m_keyModifiers &= ~Win32A.KeyModifiers.Alt;
                    }
                    else
                    {
                        m_pressedKey &= ~key;
                    }
                    ShowPressedKeys();
                    return true;

                case Win32A.WindowsMessages.WM_CHAR:
                    return true;
                default:
                    return base.ProcessKeyPreview(ref p_message);
            }
        }

        private void ShowPressedKeys()
        {
            if (m_keyModifiers == Win32A.KeyModifiers.None)
            {
                txtHotkeyEditor.Text = Win32A.KeyModifiers.None.ToString();
                return;
            }
            if (m_pressedKey != Keys.None)
                txtHotkeyEditor.Text = m_keyModifiers.GetDescription() + "+" + m_pressedKey;
            else
                txtHotkeyEditor.Text = m_keyModifiers.GetDescription();
        }

        private void DebugPrint(Message p_message, Keys key)
        {
            Win32A.WindowsMessages msg = (Win32A.WindowsMessages)p_message.Msg;

            Debug.Print(msg + "_key: " + key);
            Debug.Print(msg + "_lparam: " + p_message.LParam.ToInt32().ToString("X8"));

            int scanCode = (p_message.LParam.ToInt32() >> 16) & 0xff;
            Debug.Print(msg + "_scancode: " + scanCode.ToString("X8"));

            Debug.Print("allPressed: " + m_pressedKey);

            /*
                    var sb = new StringBuilder(32);
                    Win32A.GetKeyNameText(p_message.LParam.ToInt32(), sb, sb.Capacity);
                    Debug.Print(sb.ToString());
             
             */
        }


        private static Keys GetVkKey(Message p_message)
        {
            return (Keys) p_message.WParam;
        }
    }
}
