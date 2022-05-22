using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RTSS_time_reader.WindowsInterop;

namespace RTSS_time_reader
{
    public partial class HotkeyEditorDialog : Form
    {
        public HotkeyEditorDialog()
        {
            InitializeComponent();
        }

        private bool m_firstPressing = true;
        public ushort NewHotkeyAtom { get; protected set; }

        public Hotkey NewHotkey { get; protected set; }
        public Hotkey? RegistredHotkey
        {
            get { return m_registredHotkey; }
            set
            {
                m_registredHotkey = value;
                if (value.HasValue)
                    NewHotkey = value.Value;
                ShowPressedKeys();
            }
        }


        public MainForm HotkeyProcessor { get; set; }

        private bool m_hotkeyAccepted;
        private Hotkey? m_registredHotkey;
        private Win32A.KeyModifiers m_pressedKeyModifiers;

        protected override bool ProcessKeyPreview(ref Message p_message)
        {
            if (p_message.HWnd != txtHotkeyEditor.Handle)
                return base.ProcessKeyPreview(ref p_message);

            var msg = (Win32A.WindowsMessages) p_message.Msg;
            var newHotkey = NewHotkey;

            Keys key;
            switch (msg)
            {
                case Win32A.WindowsMessages.WM_KEYDOWN:
                case Win32A.WindowsMessages.WM_SYSKEYDOWN:
                    if (ReadFlag(p_message.LParam, KF_REPEAT))
                        return true;

                    break;
                default:
                    break;
            }

            Debug.Print(msg.ToString());

            switch (msg)
            {
                case Win32A.WindowsMessages.WM_SYSKEYDOWN:
                    Debug.Print(p_message.LParam.ToString("X16"));

                    newHotkey.Modifiers |= Win32A.KeyModifiers.Alt;
                    AcceptNewHotkey(p_message);
                    ShowPressedKeys();
                    return true;

                case Win32A.WindowsMessages.WM_KEYDOWN:
                    Debug.Print(p_message.LParam.ToString("X16"));

                    AcceptNewHotkey(p_message);
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
                        newHotkey.Modifiers &= ~Win32A.KeyModifiers.Alt;
                    }

                    NewHotkey = newHotkey;
                    ShowPressedKeys();
                    return true;



                case Win32A.WindowsMessages.WM_KEYUP:
                    key = GetVkKey(p_message);
                    Debug.Print(key +" -keyup");
                    if (false == m_hotkeyAccepted)
                    {
                        if (key == Keys.ControlKey)
                        {
                            newHotkey.Modifiers &= ~Win32A.KeyModifiers.Ctrl;
                        }
                        else if (key == Keys.ShiftKey)
                        {
                            newHotkey.Modifiers &= ~Win32A.KeyModifiers.Shift;
                        }
                        else if ((key == Keys.LWin) || (key == Keys.RWin))
                        {
                            newHotkey.Modifiers &= ~Win32A.KeyModifiers.Win;
                        }
                        else if (key == Keys.Menu)
                        {
                            newHotkey.Modifiers &= ~Win32A.KeyModifiers.Alt;
                        }
                        else
                        {
                            if (newHotkey.Modifiers == Win32A.KeyModifiers.None)
                                newHotkey.Key &= ~key;
                            else if (newHotkey.Key != Keys.None)
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

                    NewHotkey = newHotkey;
                    ShowPressedKeys();
                    return true;

                case Win32A.WindowsMessages.WM_CHAR:
                    return true;
                default:
                    return base.ProcessKeyPreview(ref p_message);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool ReadFlag(IntPtr p_lparam, int p_flag)
        {
            return 0 != (p_lparam.ToInt32() & p_flag);
        }

        // ReSharper disable InconsistentNaming
        // ReSharper disable CommentTypo
        private const int KF_EXTENDED = 0x1 << 24;
        private const int KF_ALTDOWN = 0x1 << 29;

        private const int KF_REPEAT = 0x1 << 30;
        private const int KF_UP = 0x1 << 31;

        //private const int KF_EXTENDED         0x0100
        //private const int KF_DLGMODE          0x0800

        //private const int KF_MENUMODE         0x1000
        //private const int KF_ALTDOWN          0x2000 
        //private const int KF_REPEAT           0x4000
        //private const int KF_UP               0x8000
        //------

        // ReSharper restore CommentTypo
        // ReSharper restore InconsistentNaming

        private void AcceptNewHotkey(Message p_message)
        {
            var newHotkey = NewHotkey;
            if (m_firstPressing)
            {
                m_firstPressing = false;
                newHotkey.Key = Keys.None;
                newHotkey.Modifiers = Win32A.KeyModifiers.None;
            }

            Keys key;
            key = GetVkKey(p_message);
            Debug.Print(key.ToString());
            if (key == Keys.ControlKey)
            {
                newHotkey.Modifiers |= Win32A.KeyModifiers.Ctrl;
                m_pressedKeyModifiers |= Win32A.KeyModifiers.Ctrl;
            }
            else if (key == Keys.ShiftKey)
            {
                newHotkey.Modifiers |= Win32A.KeyModifiers.Shift;
                m_pressedKeyModifiers |= Win32A.KeyModifiers.Shift;
            }
            else if ((key == Keys.LWin) || (key == Keys.RWin))
            {
                newHotkey.Modifiers |= Win32A.KeyModifiers.Win;
                m_pressedKeyModifiers |= Win32A.KeyModifiers.Win;
            }
            else if (key == Keys.Menu)
            {
                newHotkey.Modifiers |= Win32A.KeyModifiers.Alt;
                m_pressedKeyModifiers |= Win32A.KeyModifiers.Alt;
            }
            else if ((key == Keys.Back) && (m_pressedKeyModifiers == Win32A.KeyModifiers.None))
            {
                newHotkey.Key = Keys.None;
                newHotkey.Modifiers = Win32A.KeyModifiers.None;
                m_hotkeyAccepted = false;
            }
            else
            {
                newHotkey.Key = key;
                if (newHotkey.Modifiers != Win32A.KeyModifiers.None)
                {
                    m_hotkeyAccepted = true;
                }
            }

            NewHotkey = newHotkey;
            OnHotkeyAccepted();
        }

        private void OnHotkeyAccepted()
        {
            if (!m_hotkeyAccepted)
            {
                btnOk.Enabled = false;
            }
            else
            {
                if (RegistredHotkey.HasValue)
                {
                    btnOk.Enabled = RegistredHotkey.Value != NewHotkey;
                }
                else
                {
                    btnOk.Enabled = (false == NewHotkey.IsEmpty);
                }
            }
        }

        private void ShowPressedKeys()
        {
            if (NewHotkey.Modifiers == Win32A.KeyModifiers.None)
            {
                txtHotkeyEditor.Text = Win32A.KeyModifiers.None.ToString();
                return;
            }
            if (NewHotkey.Key != Keys.None)
                txtHotkeyEditor.Text = NewHotkey.Modifiers.GetDescription() + "+" + NewHotkey.Key;
            else
                txtHotkeyEditor.Text = NewHotkey.Modifiers.GetDescription();
        }

        private static Keys GetVkKey(Message p_message)
        {
            return (Keys) p_message.WParam;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            NewHotkeyAtom = Win32A.GlobalAddAtom("RTSS_time_reader"+GetHashCode().ToString());
            
            var registred = HotkeyProcessor.RegisterHotkey(NewHotkeyAtom, NewHotkey);
            if (false == registred)
                DialogResult = DialogResult.None;
            else
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}
