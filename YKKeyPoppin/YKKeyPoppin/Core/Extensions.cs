namespace YKKeyPoppin
{
    using System.Collections.Generic;
    using System.Timers;
    using System.Windows.Input;
    using YKKeyPoppin.Models;
    using YKToolkit.Controls;

    internal static class Extensions
    {
        public static void Restart(this Timer timer)
        {
            timer.Stop();
            timer.Start();
        }

        public static bool IsModifierKey(this User32.VKs key)
        {
            if (key == User32.VKs.VK_SHIFT) return true;
            if (key == User32.VKs.VK_LSHIFT) return true;
            if (key == User32.VKs.VK_RSHIFT) return true;
            if (key == User32.VKs.VK_CONTROL) return true;
            if (key == User32.VKs.VK_LCONTROL) return true;
            if (key == User32.VKs.VK_RCONTROL) return true;
            if (key == User32.VKs.VK_MENU) return true;
            if (key == User32.VKs.VK_LMENU) return true;
            if (key == User32.VKs.VK_RMENU) return true;
            if (key == User32.VKs.VK_LWIN) return true;
            if (key == User32.VKs.VK_RWIN) return true;

            return false;
        }

        public static ModifierKeys ToModifierKeys(this User32.VKs key)
        {
            if (key == User32.VKs.VK_SHIFT) return ModifierKeys.Shift;
            if (key == User32.VKs.VK_LSHIFT) return ModifierKeys.Shift;
            if (key == User32.VKs.VK_RSHIFT) return ModifierKeys.Shift;
            if (key == User32.VKs.VK_CONTROL) return ModifierKeys.Control;
            if (key == User32.VKs.VK_LCONTROL) return ModifierKeys.Control;
            if (key == User32.VKs.VK_RCONTROL) return ModifierKeys.Control;
            if (key == User32.VKs.VK_MENU) return ModifierKeys.Alt;
            if (key == User32.VKs.VK_LMENU) return ModifierKeys.Alt;
            if (key == User32.VKs.VK_RMENU) return ModifierKeys.Alt;
            if (key == User32.VKs.VK_LWIN) return ModifierKeys.Windows;
            if (key == User32.VKs.VK_RWIN) return ModifierKeys.Windows;

            return ModifierKeys.None;
        }

        public static string GetString(this KeyInfo info)
        {
            var keyOnly = new KeyInfo() { Key = info.Key };
            if (KeyConf.Current.ContainsKey(info))
            {
                return KeyConf.Current[info];
            }
            else if (info.ModifierKeys == ModifierKeys.None)
            {
                return info.Key.ChangeString();
            }

            return info.ChangeString();
        }

        public static string ChangeString(this KeyInfo info)
        {
            var str = "";
            if (info.ModifierKeys.HasFlag(ModifierKeys.Control)) str += "Ctrl+";
            if (info.ModifierKeys.HasFlag(ModifierKeys.Alt)) str += "Alt+";
            if (info.ModifierKeys.HasFlag(ModifierKeys.Shift)) str += "Shift+";
            if (info.ModifierKeys.HasFlag(ModifierKeys.Windows)) str += "Win+";
            str += info.Key.ChangeString();
            return str;
        }

        public static string ChangeString(this User32.VKs key)
        {
            switch (key)
            {
                //case User32.VKs.VK_0x00: return "NULL";
                case User32.VKs.VK_LBUTTON: return "LBUTTON";
                case User32.VKs.VK_RBUTTON: return "RBUTTON";
                case User32.VKs.VK_CANCEL: return "Cancel";
                case User32.VKs.VK_MBUTTON: return "MBUTTON";
                case User32.VKs.VK_XBUTTON1: return "XBUTTON1";
                case User32.VKs.VK_XBUTTON2: return "XBUTTON2";
                //case User32.VKs.VK_0x07: return "NULL";
                case User32.VKs.VK_BACK: return "BS";
                case User32.VKs.VK_TAB: return "Tab";
                //case User32.VKs.VK_0x0A: return "NULL";
                //case User32.VKs.VK_0x0B: return "NULL";
                case User32.VKs.VK_CLEAR: return "Clear";
                case User32.VKs.VK_RETURN: return "Enter";
                //case User32.VKs.VK_0x0E: return "NULL";
                //case User32.VKs.VK_0x0F: return "NULL";
                case User32.VKs.VK_SHIFT: return "Shift";
                case User32.VKs.VK_CONTROL: return "Ctrl";
                case User32.VKs.VK_MENU: return "Alt";
                case User32.VKs.VK_PAUSE: return "Pause";
                case User32.VKs.VK_CAPITAL: return "Caps";
                //case User32.VKs.VK_HANGUEL: return "Hanguel";
                case User32.VKs.VK_KANA: return "KANA";
                //case User32.VKs.VK_HANGUL: return "Hangul";
                case User32.VKs.VK_JUNJA: return "Junja";
                case User32.VKs.VK_FINAL: return "Final";
                case User32.VKs.VK_HANJA: return "IME";
                //case User32.VKs.VK_KANJI: return "IME";
                case User32.VKs.VK_ESCAPE: return "Esc";
                case User32.VKs.VK_CONVERT: return "IME";
                case User32.VKs.VK_NONCONVERT: return "NonConvert";
                case User32.VKs.VK_ACCEPT: return "Accept";
                case User32.VKs.VK_MODECHANGE: return "ModeChange";
                case User32.VKs.VK_SPACE: return "Space";
                case User32.VKs.VK_PRIOR: return "PageUp";
                case User32.VKs.VK_NEXT: return "PageDown";
                case User32.VKs.VK_END: return "End";
                case User32.VKs.VK_HOME: return "Home";
                case User32.VKs.VK_LEFT: return "Left";
                case User32.VKs.VK_UP: return "Up";
                case User32.VKs.VK_RIGHT: return "Right";
                case User32.VKs.VK_DOWN: return "Down";
                case User32.VKs.VK_SELECT: return "Select";
                case User32.VKs.VK_PRINT: return "Print";
                case User32.VKs.VK_EXECUTE: return "Execute";
                case User32.VKs.VK_SNAPSHOT: return "PrintScreen";
                case User32.VKs.VK_INSERT: return "Ins";
                case User32.VKs.VK_DELETE: return "Del";
                case User32.VKs.VK_HELP: return "Help";
                case User32.VKs.K_0: return "0";
                case User32.VKs.K_1: return "1";
                case User32.VKs.K_2: return "2";
                case User32.VKs.K_3: return "3";
                case User32.VKs.K_4: return "4";
                case User32.VKs.K_5: return "5";
                case User32.VKs.K_6: return "6";
                case User32.VKs.K_7: return "7";
                case User32.VKs.K_8: return "8";
                case User32.VKs.K_9: return "9";
                //case User32.VKs.VK_0x3A: return "NULL";
                //case User32.VKs.VK_0x3B: return "NULL";
                //case User32.VKs.VK_0x3C: return "NULL";
                //case User32.VKs.VK_0x3D: return "NULL";
                //case User32.VKs.VK_0x3E: return "NULL";
                //case User32.VKs.VK_0x3F: return "NULL";
                //case User32.VKs.VK_0x40: return "NULL";
                case User32.VKs.K_A: return "A";
                case User32.VKs.K_B: return "B";
                case User32.VKs.K_C: return "C";
                case User32.VKs.K_D: return "D";
                case User32.VKs.K_E: return "E";
                case User32.VKs.K_F: return "F";
                case User32.VKs.K_G: return "G";
                case User32.VKs.K_H: return "H";
                case User32.VKs.K_I: return "I";
                case User32.VKs.K_J: return "J";
                case User32.VKs.K_K: return "K";
                case User32.VKs.K_L: return "L";
                case User32.VKs.K_M: return "M";
                case User32.VKs.K_N: return "N";
                case User32.VKs.K_O: return "O";
                case User32.VKs.K_P: return "P";
                case User32.VKs.K_Q: return "Q";
                case User32.VKs.K_R: return "R";
                case User32.VKs.K_S: return "S";
                case User32.VKs.K_T: return "T";
                case User32.VKs.K_U: return "U";
                case User32.VKs.K_V: return "V";
                case User32.VKs.K_W: return "W";
                case User32.VKs.K_X: return "X";
                case User32.VKs.K_Y: return "Y";
                case User32.VKs.K_Z: return "Z";
                case User32.VKs.VK_LWIN: return "Win";
                case User32.VKs.VK_RWIN: return "Win";
                case User32.VKs.VK_APPS: return "Apps";
                //case User32.VKs.VK_0x5E: return "NULL";
                case User32.VKs.VK_SLEEP: return "Sleep";
                case User32.VKs.VK_NUMPAD0: return "0";
                case User32.VKs.VK_NUMPAD1: return "1";
                case User32.VKs.VK_NUMPAD2: return "2";
                case User32.VKs.VK_NUMPAD3: return "3";
                case User32.VKs.VK_NUMPAD4: return "4";
                case User32.VKs.VK_NUMPAD5: return "5";
                case User32.VKs.VK_NUMPAD6: return "6";
                case User32.VKs.VK_NUMPAD7: return "7";
                case User32.VKs.VK_NUMPAD8: return "8";
                case User32.VKs.VK_NUMPAD9: return "9";
                case User32.VKs.VK_MULTIPLY: return "Multiply";
                case User32.VKs.VK_ADD: return "+";
                case User32.VKs.VK_SEPARATOR: return "|";
                case User32.VKs.VK_SUBTRACT: return "-";
                case User32.VKs.VK_DECIMAL: return "Decimal";
                case User32.VKs.VK_DIVIDE: return "/";
                case User32.VKs.VK_F1: return "F1";
                case User32.VKs.VK_F2: return "F2";
                case User32.VKs.VK_F3: return "F3";
                case User32.VKs.VK_F4: return "F4";
                case User32.VKs.VK_F5: return "F5";
                case User32.VKs.VK_F6: return "F6";
                case User32.VKs.VK_F7: return "F7";
                case User32.VKs.VK_F8: return "F8";
                case User32.VKs.VK_F9: return "F9";
                case User32.VKs.VK_F10: return "F10";
                case User32.VKs.VK_F11: return "F11";
                case User32.VKs.VK_F12: return "F12";
                case User32.VKs.VK_F13: return "F13";
                case User32.VKs.VK_F14: return "F14";
                case User32.VKs.VK_F15: return "F15";
                case User32.VKs.VK_F16: return "F16";
                case User32.VKs.VK_F17: return "F17";
                case User32.VKs.VK_F18: return "F18";
                case User32.VKs.VK_F19: return "F19";
                case User32.VKs.VK_F20: return "F20";
                case User32.VKs.VK_F21: return "F21";
                case User32.VKs.VK_F22: return "F22";
                case User32.VKs.VK_F23: return "F23";
                case User32.VKs.VK_F24: return "F24";
                //case User32.VKs.VK_0x88: return "NULL";
                //case User32.VKs.VK_0x89: return "NULL";
                //case User32.VKs.VK_0x8A: return "NULL";
                //case User32.VKs.VK_0x8B: return "NULL";
                //case User32.VKs.VK_0x8C: return "NULL";
                //case User32.VKs.VK_0x8D: return "NULL";
                //case User32.VKs.VK_0x8E: return "NULL";
                //case User32.VKs.VK_0x8F: return "NULL";
                case User32.VKs.VK_NUMLOCK: return "NumLock";
                case User32.VKs.VK_SCROLL: return "ScrollLock";
                //case User32.VKs.VK_0x92: return "NULL";
                //case User32.VKs.VK_0x93: return "NULL";
                //case User32.VKs.VK_0x94: return "NULL";
                //case User32.VKs.VK_0x95: return "NULL";
                //case User32.VKs.VK_0x96: return "NULL";
                //case User32.VKs.VK_0x97: return "NULL";
                //case User32.VKs.VK_0x98: return "NULL";
                //case User32.VKs.VK_0x99: return "NULL";
                //case User32.VKs.VK_0x9A: return "NULL";
                //case User32.VKs.VK_0x9B: return "NULL";
                //case User32.VKs.VK_0x9C: return "NULL";
                //case User32.VKs.VK_0x9D: return "NULL";
                //case User32.VKs.VK_0x9E: return "NULL";
                //case User32.VKs.VK_0x9F: return "NULL";
                case User32.VKs.VK_LSHIFT: return "Shift";
                case User32.VKs.VK_RSHIFT: return "Shift";
                case User32.VKs.VK_LCONTROL: return "Ctrl";
                case User32.VKs.VK_RCONTROL: return "Ctrl";
                case User32.VKs.VK_LMENU: return "Alt";
                case User32.VKs.VK_RMENU: return "Alt";
                case User32.VKs.VK_BROWSER_BACK: return "Back";
                case User32.VKs.VK_BROWSER_FORWARD: return "Forward";
                case User32.VKs.VK_BROWSER_REFRESH: return "Refresh";
                case User32.VKs.VK_BROWSER_STOP: return "Stop";
                case User32.VKs.VK_BROWSER_SEARCH: return "Search";
                case User32.VKs.VK_BROWSER_FAVORITES: return "Favorites";
                case User32.VKs.VK_BROWSER_HOME: return "Home";
                case User32.VKs.VK_VOLUME_MUTE: return "Mute";
                case User32.VKs.VK_VOLUME_DOWN: return "VolDown";
                case User32.VKs.VK_VOLUME_UP: return "VolUp";
                case User32.VKs.VK_MEDIA_NEXT_TRACK: return "NextTrack";
                case User32.VKs.VK_MEDIA_PREV_TRACK: return "PrevTrack";
                case User32.VKs.VK_MEDIA_STOP: return "Stop";
                case User32.VKs.VK_MEDIA_PLAY_PAUSE: return "Pause";
                case User32.VKs.VK_LAUNCH_MAIL: return "Mail";
                case User32.VKs.VK_LAUNCH_MEDIA_SELECT: return "Select";
                case User32.VKs.VK_LAUNCH_APP1: return "App1";
                case User32.VKs.VK_LAUNCH_APP2: return "App2";
                //case User32.VKs.VK_0xB8: return "NULL";
                //case User32.VKs.VK_0xB9: return "NULL";
                case User32.VKs.VK_OEM_1: return ";";
                case User32.VKs.VK_OEM_PLUS: return "+";
                case User32.VKs.VK_OEM_COMMA: return ",";
                case User32.VKs.VK_OEM_MINUS: return "-";
                case User32.VKs.VK_OEM_PERIOD: return ".";
                case User32.VKs.VK_OEM_2: return "/";
                case User32.VKs.VK_OEM_3: return "`";
                //case User32.VKs.VK_0xC1: return "NULL";
                //case User32.VKs.VK_0xC2: return "NULL";
                //case User32.VKs.VK_0xC3: return "NULL";
                //case User32.VKs.VK_0xC4: return "NULL";
                //case User32.VKs.VK_0xC5: return "NULL";
                //case User32.VKs.VK_0xC6: return "NULL";
                //case User32.VKs.VK_0xC7: return "NULL";
                //case User32.VKs.VK_0xC8: return "NULL";
                //case User32.VKs.VK_0xC9: return "NULL";
                //case User32.VKs.VK_0xCA: return "NULL";
                //case User32.VKs.VK_0xCB: return "NULL";
                //case User32.VKs.VK_0xCC: return "NULL";
                //case User32.VKs.VK_0xCD: return "NULL";
                //case User32.VKs.VK_0xCE: return "NULL";
                //case User32.VKs.VK_0xCF: return "NULL";
                //case User32.VKs.VK_0xD0: return "NULL";
                //case User32.VKs.VK_0xD1: return "NULL";
                //case User32.VKs.VK_0xD2: return "NULL";
                //case User32.VKs.VK_0xD3: return "NULL";
                //case User32.VKs.VK_0xD4: return "NULL";
                //case User32.VKs.VK_0xD5: return "NULL";
                //case User32.VKs.VK_0xD6: return "NULL";
                //case User32.VKs.VK_0xD7: return "NULL";
                //case User32.VKs.VK_0xD8: return "NULL";
                //case User32.VKs.VK_0xD9: return "NULL";
                //case User32.VKs.VK_0xDA: return "NULL";
                case User32.VKs.VK_OEM_4: return "[";
                case User32.VKs.VK_OEM_5: return "\\";
                case User32.VKs.VK_OEM_6: return "]";
                case User32.VKs.VK_OEM_7: return "'";
                case User32.VKs.VK_OEM_8: return "OEM8";
                //case User32.VKs.VK_0xE0: return "NULL";
                //case User32.VKs.VK_0xE1: return "NULL";
                case User32.VKs.VK_OEM_102: return "OEM102";
                //case User32.VKs.VK_0xE3: return "NULL";
                //case User32.VKs.VK_0xE4: return "NULL";
                case User32.VKs.VK_PROCESSKEY: return "Process";
                //case User32.VKs.VK_0xE6: return "NULL";
                case User32.VKs.VK_PACKET: return "Packet";
                //case User32.VKs.VK_0xE8: return "NULL";
                //case User32.VKs.VK_0xE9: return "NULL";
                //case User32.VKs.VK_0xEA: return "NULL";
                //case User32.VKs.VK_0xEB: return "NULL";
                //case User32.VKs.VK_0xEC: return "NULL";
                //case User32.VKs.VK_0xED: return "NULL";
                //case User32.VKs.VK_0xEE: return "NULL";
                //case User32.VKs.VK_0xEF: return "NULL";
                //case User32.VKs.VK_0xF0: return "NULL";
                //case User32.VKs.VK_0xF1: return "NULL";
                //case User32.VKs.VK_0xF2: return "NULL";
                //case User32.VKs.VK_0xF3: return "NULL";
                //case User32.VKs.VK_0xF4: return "NULL";
                //case User32.VKs.VK_0xF5: return "NULL";
                case User32.VKs.VK_ATTN: return "Attn";
                case User32.VKs.VK_CRSEL: return "CrSel";
                case User32.VKs.VK_EXSEL: return "ExSel";
                case User32.VKs.VK_EREOF: return "ErEOF";
                case User32.VKs.VK_PLAY: return "Play";
                case User32.VKs.VK_ZOOM: return "Zoom";
                case User32.VKs.VK_NONAME: return "NonName";
                case User32.VKs.VK_PA1: return "PA1";
                case User32.VKs.VK_OEM_CLEAR: return "OEMCLEAR";
                //case User32.VKs.VK_0xFF: return "NULL";
                default: return "NULL";
            }
        }

        public static double GetPosition(this User32.VKs key)
        {
            //var unit = App.Instance.Bounds.Width / 17.0;

            switch (key)
            {
                case User32.VKs.K_0: return 11;
                case User32.VKs.K_1: return 2;
                case User32.VKs.K_2: return 3;
                case User32.VKs.K_3: return 4;
                case User32.VKs.K_4: return 5;
                case User32.VKs.K_5: return 6;
                case User32.VKs.K_6: return 7;
                case User32.VKs.K_7: return 8;
                case User32.VKs.K_8: return 9;
                case User32.VKs.K_9: return 10;
                case User32.VKs.K_A: return 2;
                case User32.VKs.K_B: return 7;
                case User32.VKs.K_C: return 5;
                case User32.VKs.K_D: return 5;
                case User32.VKs.K_E: return 4;
                case User32.VKs.K_F: return 6;
                case User32.VKs.K_G: return 7;
                case User32.VKs.K_H: return 8;
                case User32.VKs.K_I: return 9;
                case User32.VKs.K_J: return 9;
                case User32.VKs.K_K: return 10;
                case User32.VKs.K_L: return 11;
                case User32.VKs.K_M: return 9;
                case User32.VKs.K_N: return 8;
                case User32.VKs.K_O: return 10;
                case User32.VKs.K_P: return 11;
                case User32.VKs.K_Q: return 2;
                case User32.VKs.K_R: return 5;
                case User32.VKs.K_S: return 3;
                case User32.VKs.K_T: return 6;
                case User32.VKs.K_U: return 8;
                case User32.VKs.K_V: return 6;
                case User32.VKs.K_W: return 3;
                case User32.VKs.K_X: return 4;
                case User32.VKs.K_Y: return 7;
                case User32.VKs.K_Z: return 3;
                case User32.VKs.VK_ADD: return 14;
                case User32.VKs.VK_BACK: return 15;
                case User32.VKs.VK_CAPITAL: return 1;
                case User32.VKs.VK_CONTROL: return 1;
                case User32.VKs.VK_CONVERT: return 1;
                case User32.VKs.VK_DELETE: return 14;
                case User32.VKs.VK_DIVIDE: return 12;
                case User32.VKs.VK_DOWN: return 12;
                case User32.VKs.VK_END: return 11;
                case User32.VKs.VK_ESCAPE: return 1;
                case User32.VKs.VK_F1: return 2;
                case User32.VKs.VK_F2: return 3;
                case User32.VKs.VK_F3: return 4;
                case User32.VKs.VK_F4: return 5;
                case User32.VKs.VK_F5: return 6;
                case User32.VKs.VK_F6: return 7;
                case User32.VKs.VK_F7: return 8;
                case User32.VKs.VK_F8: return 9;
                case User32.VKs.VK_F9: return 10;
                case User32.VKs.VK_F10: return 11;
                case User32.VKs.VK_F11: return 12;
                case User32.VKs.VK_F12: return 13;
                case User32.VKs.VK_F13: return 14;
                case User32.VKs.VK_F14: return 15;
                case User32.VKs.VK_F15: return 15;
                case User32.VKs.VK_F16: return 15;
                case User32.VKs.VK_F17: return 15;
                case User32.VKs.VK_F18: return 15;
                case User32.VKs.VK_F19: return 15;
                case User32.VKs.VK_F20: return 15;
                case User32.VKs.VK_F21: return 15;
                case User32.VKs.VK_F22: return 15;
                case User32.VKs.VK_F23: return 15;
                case User32.VKs.VK_F24: return 15;
                case User32.VKs.VK_HOME: return 10;
                case User32.VKs.VK_INSERT: return 14;
                case User32.VKs.VK_LCONTROL: return 1;
                case User32.VKs.VK_LEFT: return 12;
                case User32.VKs.VK_LMENU: return 4;
                case User32.VKs.VK_LSHIFT: return 1;
                case User32.VKs.VK_LWIN: return 13;
                case User32.VKs.VK_MENU: return 11;
                case User32.VKs.VK_PRIOR: return 11;
                case User32.VKs.VK_NEXT: return 11;
                case User32.VKs.VK_NUMLOCK: return 15;
                case User32.VKs.VK_NUMPAD0: return 11;
                case User32.VKs.VK_NUMPAD1: return 2;
                case User32.VKs.VK_NUMPAD2: return 3;
                case User32.VKs.VK_NUMPAD3: return 4;
                case User32.VKs.VK_NUMPAD4: return 5;
                case User32.VKs.VK_NUMPAD5: return 6;
                case User32.VKs.VK_NUMPAD6: return 7;
                case User32.VKs.VK_NUMPAD7: return 8;
                case User32.VKs.VK_NUMPAD8: return 9;
                case User32.VKs.VK_NUMPAD9: return 10;
                case User32.VKs.VK_OEM_1: return 11;
                case User32.VKs.VK_OEM_2: return 12;
                case User32.VKs.VK_OEM_3: return 15;
                case User32.VKs.VK_OEM_4: return 12;
                case User32.VKs.VK_OEM_5: return 14;
                case User32.VKs.VK_OEM_6: return 13;
                case User32.VKs.VK_OEM_7: return 11;
                case User32.VKs.VK_OEM_COMMA: return 10;
                case User32.VKs.VK_OEM_MINUS: return 12;
                case User32.VKs.VK_OEM_PERIOD: return 11;
                case User32.VKs.VK_OEM_PLUS: return 13;
                case User32.VKs.VK_PAUSE: return 13;
                case User32.VKs.VK_RCONTROL: return 1;
                case User32.VKs.VK_RETURN: return 15;
                case User32.VKs.VK_RIGHT: return 13;
                case User32.VKs.VK_RMENU: return 11;
                case User32.VKs.VK_RSHIFT: return 1;
                case User32.VKs.VK_RWIN: return 13;
                case User32.VKs.VK_SCROLL: return 10;
                case User32.VKs.VK_SHIFT: return 1;
                case User32.VKs.VK_SNAPSHOT: return 9;
                case User32.VKs.VK_SPACE: return 7;
                case User32.VKs.VK_SUBTRACT: return 12;
                case User32.VKs.VK_TAB: return 1;
                case User32.VKs.VK_UP: return 12;
                default: return 1;
            }
        }
    }
}
