﻿namespace YKKeyPoppin.Models
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using YKToolkit.Controls;

    internal class KeyHook
    {
        public static KeyHook Current { get; private set; }

        static KeyHook()
        {
            Current = new KeyHook();
        }

        private KeyHook()
        {
            this._keyHook.KeyDown += OnKeyDown;
            this._keyHook.KeyUp += OnKeyUp;
            this._keyHook.Hook();
        }

        /// <summary>
        /// キーボードが押されたときのグローバルフック
        /// </summary>
        /// <param name="key">押されたキー</param>
        private void OnKeyDown(User32.VKs key)
        {
            if (this._downKeys.Contains(key)) return;

            if (key.IsModifierKey())
            {
                // Shift/Ctrl/Alt/Windows が押された
                this._isModifierKeyDown = true;
            }
            else
            {
                // Shift/Ctrl/Alt/Windows 以外が押された
                this._isModifierKeyDown = false;
            }

            this._modifierKeys |= key.ToModifierKeys();
            this._downKeys.Add(key);
        }

        /// <summary>
        /// キーボードが離されたときのグローバルフック
        /// </summary>
        /// <param name="key">離されたキー</param>
        private void OnKeyUp(User32.VKs key)
        {
            this._modifierKeys &= ~(key.ToModifierKeys());

            if (key.IsModifierKey())
            {
                // Shift/Ctrl/Alt/Windows が離された

                if (this._isModifierKeyDown)
                {
                    if (this._modifierKeys == ModifierKeys.None)
                    {
                        // Shift/Ctrl/Alt/Windows が押された状態で
                        // 他のキーが押されていなかったとき
                        // 他の Shift/Ctrl/Alt/Windows の操作ではなく
                        // 単独で Shift/Ctrl/Alt/Windows が離されたときに
                        // Shift/Ctrl/Alt/Windows を Poppin' する
                        RaiseKeyUp(key, this._modifierKeys);
                        this._isModifierKeyDown = false;
                    }
                }
            }
            else
            {
                // Shift/Ctrl/Alt/Windows 以外が離された

                RaiseKeyUp(key, this._modifierKeys);
            }

            this._downKeys.Remove(key);
        }

        internal delegate void KeyUpHandler(KeyInfo info);

        public event KeyUpHandler KeyUp;

        private void RaiseKeyUp(User32.VKs key, ModifierKeys modifierKeys)
        {
            var h = this.KeyUp;
            if (h != null) h(new KeyInfo() { Key = key, ModifierKeys = modifierKeys });
        }

        /// <summary>
        /// キーボードフック
        /// </summary>
        private KeyboardHook _keyHook = new KeyboardHook();

        /// <summary>
        /// 押下しているキー
        /// </summary>
        private List<User32.VKs> _downKeys = new List<User32.VKs>();

        /// <summary>
        /// 変換キー押下状態
        /// </summary>
        private ModifierKeys _modifierKeys;

        /// <summary>
        /// 変換キーによる操作開始
        /// </summary>
        private bool _isModifierKeyDown;
    }
}
