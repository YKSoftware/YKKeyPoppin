namespace YKKeyPoppin
{
    using System.Collections.Generic;
    using System.Windows.Input;

    internal static class Extensions
    {
        public static string GetString(this Dictionary<KeyInfo, string> dic, KeyInfo info)
        {
            var keyOnly = new KeyInfo() { Key = info.Key };
            if (dic.ContainsKey(info))
            {
                return dic[info];
            }
            else if ((info.ModifierKeys != ModifierKeys.None) && dic.ContainsKey(keyOnly))
            {
                return info.ModifierKeys.ToString() + "+" + dic[keyOnly];
            }

            return info.Key.ChangeString();
        }

        public static string ChangeString(this User32.VKs key)
        {
            switch (key)
            {
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
                case User32.VKs.VK_ADD: return "+";
                case User32.VKs.VK_BACK: return "BS";
                case User32.VKs.VK_CAPITAL: return "Caps";
                case User32.VKs.VK_CONTROL: return "Ctrl";
                case User32.VKs.VK_CONVERT: return "IME";
                case User32.VKs.VK_DELETE: return "Del";
                case User32.VKs.VK_DIVIDE: return "/";
                case User32.VKs.VK_DOWN: return "Down";
                case User32.VKs.VK_END: return "End";
                case User32.VKs.VK_ESCAPE: return "Esc";
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
                case User32.VKs.VK_HANJA: return "IME";
                case User32.VKs.VK_HOME: return "Home";
                case User32.VKs.VK_INSERT: return "Ins";
                case User32.VKs.VK_LCONTROL: return "Ctrl";
                case User32.VKs.VK_LEFT: return "Left";
                case User32.VKs.VK_LMENU: return "Alt";
                case User32.VKs.VK_LSHIFT: return "Shift";
                case User32.VKs.VK_LWIN: return "Win";
                case User32.VKs.VK_MENU: return "Alt";
                case User32.VKs.VK_NEXT: return "PageDown";
                case User32.VKs.VK_NUMLOCK: return "NumLock";
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
                case User32.VKs.VK_OEM_1: return ";";
                case User32.VKs.VK_OEM_2: return "/";
                case User32.VKs.VK_OEM_3: return "`";
                case User32.VKs.VK_OEM_4: return "[";
                case User32.VKs.VK_OEM_5: return "\\";
                case User32.VKs.VK_OEM_6: return "]";
                case User32.VKs.VK_OEM_7: return "'";
                case User32.VKs.VK_OEM_COMMA: return ",";
                case User32.VKs.VK_OEM_MINUS: return "-";
                case User32.VKs.VK_OEM_PERIOD: return ".";
                case User32.VKs.VK_OEM_PLUS: return "+";
                case User32.VKs.VK_PAUSE: return "Pause";
                case User32.VKs.VK_RCONTROL: return "Ctrl";
                case User32.VKs.VK_RETURN: return "Enter";
                case User32.VKs.VK_RIGHT: return "Right";
                case User32.VKs.VK_RMENU: return "Alt";
                case User32.VKs.VK_RSHIFT: return "Shift";
                case User32.VKs.VK_RWIN: return "Win";
                case User32.VKs.VK_SCROLL: return "ScrollLock";
                case User32.VKs.VK_SHIFT: return "Shift";
                case User32.VKs.VK_SNAPSHOT: return "PrintScreen";
                case User32.VKs.VK_SPACE: return "Space";
                case User32.VKs.VK_SUBTRACT: return "-";
                case User32.VKs.VK_TAB: return "Tab";
                case User32.VKs.VK_UP: return "Up";
                default: return "NULL";
            }
        }

        public static double GetPosition(this User32.VKs key)
        {
            var unit = App.Instance.Bounds.Width / 17.0;

            switch (key)
            {
                case User32.VKs.K_0: return 11 * unit;
                case User32.VKs.K_1: return 2 * unit;
                case User32.VKs.K_2: return 3 * unit;
                case User32.VKs.K_3: return 4 * unit;
                case User32.VKs.K_4: return 5 * unit;
                case User32.VKs.K_5: return 6 * unit;
                case User32.VKs.K_6: return 7 * unit;
                case User32.VKs.K_7: return 8 * unit;
                case User32.VKs.K_8: return 9 * unit;
                case User32.VKs.K_9: return 10 * unit;
                case User32.VKs.K_A: return 2 * unit;
                case User32.VKs.K_B: return 7 * unit;
                case User32.VKs.K_C: return 5 * unit;
                case User32.VKs.K_D: return 5 * unit;
                case User32.VKs.K_E: return 4 * unit;
                case User32.VKs.K_F: return 6 * unit;
                case User32.VKs.K_G: return 7 * unit;
                case User32.VKs.K_H: return 8 * unit;
                case User32.VKs.K_I: return 9 * unit;
                case User32.VKs.K_J: return 9 * unit;
                case User32.VKs.K_K: return 10 * unit;
                case User32.VKs.K_L: return 11 * unit;
                case User32.VKs.K_M: return 9 * unit;
                case User32.VKs.K_N: return 8 * unit;
                case User32.VKs.K_O: return 10 * unit;
                case User32.VKs.K_P: return 11 * unit;
                case User32.VKs.K_Q: return 2 * unit;
                case User32.VKs.K_R: return 5 * unit;
                case User32.VKs.K_S: return 3 * unit;
                case User32.VKs.K_T: return 6 * unit;
                case User32.VKs.K_U: return 8 * unit;
                case User32.VKs.K_V: return 6 * unit;
                case User32.VKs.K_W: return 3 * unit;
                case User32.VKs.K_X: return 4 * unit;
                case User32.VKs.K_Y: return 7 * unit;
                case User32.VKs.K_Z: return 3 * unit;
                case User32.VKs.VK_ADD: return 14 * unit;
                case User32.VKs.VK_BACK: return 15 * unit;
                case User32.VKs.VK_CAPITAL: return 1 * unit;
                case User32.VKs.VK_CONTROL: return 1 * unit;
                case User32.VKs.VK_CONVERT: return 1 * unit;
                case User32.VKs.VK_DELETE: return 14 * unit;
                case User32.VKs.VK_DIVIDE: return 12 * unit;
                case User32.VKs.VK_DOWN: return 12 * unit;
                case User32.VKs.VK_END: return 11 * unit;
                case User32.VKs.VK_ESCAPE: return 1 * unit;
                case User32.VKs.VK_F1: return 2 * unit;
                case User32.VKs.VK_F2: return 3 * unit;
                case User32.VKs.VK_F3: return 4 * unit;
                case User32.VKs.VK_F4: return 5 * unit;
                case User32.VKs.VK_F5: return 6 * unit;
                case User32.VKs.VK_F6: return 7 * unit;
                case User32.VKs.VK_F7: return 8 * unit;
                case User32.VKs.VK_F8: return 9 * unit;
                case User32.VKs.VK_F9: return 10 * unit;
                case User32.VKs.VK_F10: return 11 * unit;
                case User32.VKs.VK_F11: return 12 * unit;
                case User32.VKs.VK_F12: return 13 * unit;
                case User32.VKs.VK_F13: return 14 * unit;
                case User32.VKs.VK_F14: return 15 * unit;
                case User32.VKs.VK_F15: return 15 * unit;
                case User32.VKs.VK_F16: return 15 * unit;
                case User32.VKs.VK_F17: return 15 * unit;
                case User32.VKs.VK_F18: return 15 * unit;
                case User32.VKs.VK_F19: return 15 * unit;
                case User32.VKs.VK_F20: return 15 * unit;
                case User32.VKs.VK_F21: return 15 * unit;
                case User32.VKs.VK_F22: return 15 * unit;
                case User32.VKs.VK_F23: return 15 * unit;
                case User32.VKs.VK_F24: return 15 * unit;
                case User32.VKs.VK_HOME: return 10 * unit;
                case User32.VKs.VK_INSERT: return 14 * unit;
                case User32.VKs.VK_LCONTROL: return 1 * unit;
                case User32.VKs.VK_LEFT: return 12 * unit;
                case User32.VKs.VK_LMENU: return 4 * unit;
                case User32.VKs.VK_LSHIFT: return 1 * unit;
                case User32.VKs.VK_LWIN: return 13 * unit;
                case User32.VKs.VK_MENU: return 11 * unit;
                case User32.VKs.VK_NEXT: return 11 * unit;
                case User32.VKs.VK_NUMLOCK: return 15 * unit;
                case User32.VKs.VK_NUMPAD0: return 11 * unit;
                case User32.VKs.VK_NUMPAD1: return 2 * unit;
                case User32.VKs.VK_NUMPAD2: return 3 * unit;
                case User32.VKs.VK_NUMPAD3: return 4 * unit;
                case User32.VKs.VK_NUMPAD4: return 5 * unit;
                case User32.VKs.VK_NUMPAD5: return 6 * unit;
                case User32.VKs.VK_NUMPAD6: return 7 * unit;
                case User32.VKs.VK_NUMPAD7: return 8 * unit;
                case User32.VKs.VK_NUMPAD8: return 9 * unit;
                case User32.VKs.VK_NUMPAD9: return 10 * unit;
                case User32.VKs.VK_OEM_1: return 11 * unit;
                case User32.VKs.VK_OEM_2: return 12 * unit;
                case User32.VKs.VK_OEM_3: return 15 * unit;
                case User32.VKs.VK_OEM_4: return 12 * unit;
                case User32.VKs.VK_OEM_5: return 14 * unit;
                case User32.VKs.VK_OEM_6: return 13 * unit;
                case User32.VKs.VK_OEM_7: return 11 * unit;
                case User32.VKs.VK_OEM_COMMA: return 10 * unit;
                case User32.VKs.VK_OEM_MINUS: return 12 * unit;
                case User32.VKs.VK_OEM_PERIOD: return 11 * unit;
                case User32.VKs.VK_OEM_PLUS: return 13 * unit;
                case User32.VKs.VK_PAUSE: return 13 * unit;
                case User32.VKs.VK_RCONTROL: return 1 * unit;
                case User32.VKs.VK_RETURN: return 15 * unit;
                case User32.VKs.VK_RIGHT: return 13 * unit;
                case User32.VKs.VK_RMENU: return 11 * unit;
                case User32.VKs.VK_RSHIFT: return 1 * unit;
                case User32.VKs.VK_RWIN: return 13 * unit;
                case User32.VKs.VK_SCROLL: return 10 * unit;
                case User32.VKs.VK_SHIFT: return 1 * unit;
                case User32.VKs.VK_SNAPSHOT: return 9 * unit;
                case User32.VKs.VK_SPACE: return 7 * unit;
                case User32.VKs.VK_SUBTRACT: return 12 * unit;
                case User32.VKs.VK_TAB: return 1 * unit;
                case User32.VKs.VK_UP: return 12 * unit;
                default: return unit;
            }
        }
    }
}
