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

        public Keys? NewHotkey { get; private set; }
        public Keys? RegistredHotkey
        {
            get { return m_registredHotkey; }
            set
            {
                m_registredHotkey = value;
                if (value.HasValue)
                    NewHotkey = value.Value;
            }
        }

        public Win32A.KeyModifiers? RegistredHotkeyModifiers
        {
            get { return m_registredHotkeyModifiers; }
            set
            {
                m_registredHotkeyModifiers = value;
                if (value.HasValue)
                    NewKeyModifiers = value.Value;
            }
        }

        public Win32A.KeyModifiers NewKeyModifiers { get; private set; }
        public MainForm HotkeyProcessor { get; set; }

        private bool m_hotkeyAccepted;
        private Keys? m_registredHotkey;
        private Win32A.KeyModifiers? m_registredHotkeyModifiers;
        private Win32A.KeyModifiers m_pressedKeyModifiers;

        protected override bool ProcessKeyPreview(ref Message p_message)
        {
            if (p_message.HWnd != txtHotkeyEditor.Handle)
                return base.ProcessKeyPreview(ref p_message);

            var msg = (Win32A.WindowsMessages) p_message.Msg;
            switch (msg)
            {
                case Win32A.WindowsMessages.WM_SYSKEYDOWN:
                    NewKeyModifiers |= Win32A.KeyModifiers.Alt;
                    ShowPressedKeys();
                    return true;

                case Win32A.WindowsMessages.WM_KEYDOWN:
                    var key = GetVkKey(p_message);
                    if (key == Keys.ControlKey)
                    {
                        NewKeyModifiers |= Win32A.KeyModifiers.Ctrl;
                        m_pressedKeyModifiers |= Win32A.KeyModifiers.Ctrl;
                    }
                    else if (key == Keys.ShiftKey)
                    {
                        NewKeyModifiers |= Win32A.KeyModifiers.Shift;
                        m_pressedKeyModifiers |= Win32A.KeyModifiers.Shift;
                    }
                    else if ((key == Keys.LWin) || (key == Keys.RWin))
                    {
                        NewKeyModifiers |= Win32A.KeyModifiers.Win;
                        m_pressedKeyModifiers |= Win32A.KeyModifiers.Win;
                    }
                    else if (key == Keys.Menu)
                    {
                        NewKeyModifiers |= Win32A.KeyModifiers.Alt;
                        m_pressedKeyModifiers |= Win32A.KeyModifiers.Alt;
                    }
                    else if ((key == Keys.Back) && (m_pressedKeyModifiers == Win32A.KeyModifiers.None))
                    {
                        NewHotkey = Keys.None;
                        NewKeyModifiers = Win32A.KeyModifiers.None;
                        m_hotkeyAccepted = false;
                    }
                    else
                    {
                        NewHotkey = key;
                        if (NewKeyModifiers != Win32A.KeyModifiers.None)
                            m_hotkeyAccepted = true;
                    }

                    ShowPressedKeys();
                    return true;



                case Win32A.WindowsMessages.WM_SYSKEYUP:
                    key = GetVkKey(p_message);
                    if (m_hotkeyAccepted)
                    {
                        if (key == Keys.ControlKey)
                        {
                            m_pressedKeyModifiers &= ~Win32A.KeyModifiers.Ctrl;
                        }
                        else if (key == Keys.ShiftKey)
                        {
                            m_pressedKeyModifiers &= ~Win32A.KeyModifiers.Shift;
                        }
                        else if ((key == Keys.LWin) || (key == Keys.RWin))
                        {
                            m_pressedKeyModifiers &= ~Win32A.KeyModifiers.Win;
                        }

                        m_pressedKeyModifiers &= ~Win32A.KeyModifiers.Alt;
                        ShowPressedKeys();
                    }
                    else
                    {
                        NewKeyModifiers &= ~Win32A.KeyModifiers.Alt;
                    }
                    ShowPressedKeys();
                    return true;

                case Win32A.WindowsMessages.WM_KEYUP:
                    key = GetVkKey(p_message);
                    if (false == m_hotkeyAccepted)
                    {
                        if (key == Keys.ControlKey)
                        {
                            NewKeyModifiers &= ~Win32A.KeyModifiers.Ctrl;
                        }
                        else if (key == Keys.ShiftKey)
                        {
                            NewKeyModifiers &= ~Win32A.KeyModifiers.Shift;
                        }
                        else if ((key == Keys.LWin) || (key == Keys.RWin))
                        {
                            NewKeyModifiers &= ~Win32A.KeyModifiers.Win;
                        }
                        else if (key == Keys.Menu)
                        {
                            NewKeyModifiers &= ~Win32A.KeyModifiers.Alt;
                        }
                        else
                        {
                            if (NewKeyModifiers == Win32A.KeyModifiers.None)
                                NewHotkey &= ~key;
                            else
                                m_hotkeyAccepted = true;
                        }
                    }
                    else
                    {
                        if (key == Keys.ControlKey)
                        {
                            m_pressedKeyModifiers &= ~Win32A.KeyModifiers.Ctrl;
                        }
                        else if (key == Keys.ShiftKey)
                        {
                            m_pressedKeyModifiers &= ~Win32A.KeyModifiers.Shift;
                        }
                        else if ((key == Keys.LWin) || (key == Keys.RWin))
                        {
                            m_pressedKeyModifiers &= ~Win32A.KeyModifiers.Win;
                        }
                        else if (key == Keys.Menu)
                        {
                            m_pressedKeyModifiers &= ~Win32A.KeyModifiers.Alt;
                        }
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
            if (NewKeyModifiers == Win32A.KeyModifiers.None)
            {
                txtHotkeyEditor.Text = Win32A.KeyModifiers.None.ToString();
                return;
            }
            if (NewHotkey != Keys.None)
                txtHotkeyEditor.Text = NewKeyModifiers.GetDescription() + "+" + NewHotkey;
            else
                txtHotkeyEditor.Text = NewKeyModifiers.GetDescription();
        }
/*
        private void DebugPrint(Message p_message, Keys key)
        {
            Win32A.WindowsMessages msg = (Win32A.WindowsMessages)p_message.Msg;

            Debug.Print(msg + "_key: " + key);
            Debug.Print(msg + "_lparam: " + p_message.LParam.ToInt32().ToString("X8"));

            int scanCode = (p_message.LParam.ToInt32() >> 16) & 0xff;
            Debug.Print(msg + "_scancode: " + scanCode.ToString("X8"));

            Debug.Print("allPressed: " + NewHotkey);

            /*
                    var sb = new StringBuilder(32);
                    Win32A.GetKeyNameText(p_message.LParam.ToInt32(), sb, sb.Capacity);
                    Debug.Print(sb.ToString());
             
             -/
        }
*/

        private static Keys GetVkKey(Message p_message)
        {
            return (Keys) p_message.WParam;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (m_hotkeyAccepted)
            {
                var result = HotkeyProcessor.RegisterHotkey(Atom.Value, NewKeyModifiers, NewHotkey.Value);
                if (false == result)
                    DialogResult = DialogResult.None;
            }
            else
            {
                DialogResult = DialogResult.None;
            }
        }
    }
}
