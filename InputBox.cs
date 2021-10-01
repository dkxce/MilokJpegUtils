//////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////
//
// Milok Zbrozek InputBox Class
// milokz@gmail.com
// Last Modified: 09.09.2019
//
//////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public class InputBox
    {
        public static string pOk_Text = "OK";
        public static string pCancel_Text = "Cancel";
        public static bool pShowInTaskBar = false;

        private string _title;
        private string _promptText;
        private string[] _values;
        private int _valueIndex = 0;
        private string _prevValue;
        private bool _readOnly;
        private string _inputMaskOrRegex;
        private System.Drawing.Image _icon;
        private DialogResult _result = DialogResult.None;
        private object[] _additData = new object[6];

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }
        public string PromptText
        {
            get
            {
                return this._promptText;
            }
            set
            {
                this._promptText = value;
            }
        }
        public string Value
        {
            get
            {
                return _values[this._valueIndex];
            }
            set
            {
                if (_values.Length == 1)
                    this._values[0] = value;
                else
                {
                    for (int i = 0; i < this._values.Length; i++)
                        if (this._values[i] == value)
                            this._valueIndex = i;
                };
            }
        }
        public string[] Values
        {
            get
            {
                return this._values;
            }
            set
            {
                if (value == null) throw new Exception("Invalid length");
                if (value.Length == 0) throw new Exception("Invalid length");
                this._values = value;
                this._valueIndex = 0;
            }
        }
        public int SelectedIndex
        {
            get
            {
                return this._valueIndex;
            }
            set
            {
                if ((this._values.Length > 1) && (value >= 0) && (value < this._values.Length))
                    this._valueIndex = value;
            }
        }
        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                this._readOnly = value;
            }
        }
        public string InputMaskOrRegex
        {
            get
            {
                return this._inputMaskOrRegex;
            }
            set
            {
                this._inputMaskOrRegex = value;
            }
        }
        public string InputMask
        {
            get
            {
                if (String.IsNullOrEmpty(this._inputMaskOrRegex))
                    return this._inputMaskOrRegex;

                string[] mr = this._inputMaskOrRegex.Split(new char[] { '\0' }, 2);
                for (int i = 0; i < mr.Length; i++)
                    if (mr[i].StartsWith("M"))
                        return mr[i].Substring(1);
                return "";
            }
            set
            {
                this._inputMaskOrRegex = "M" + value;
            }
        }
        public string InputRegex
        {
            get
            {
                if (String.IsNullOrEmpty(this._inputMaskOrRegex))
                    return this._inputMaskOrRegex;

                string[] mr = this._inputMaskOrRegex.Split(new char[] { '\0' }, 2);
                for (int i = 0; i < mr.Length; i++)
                    if (mr[i].StartsWith("R"))
                        return mr[i].Substring(1);
                return "";
            }
            set
            {
                this._inputMaskOrRegex = "R" + value;
            }
        }
        public System.Drawing.Image Icon
        {
            get
            {
                return this._icon;
            }
            set
            {
                this._icon = value;
            }
        }
        public DialogResult Result
        {
            get
            {
                return _result;
            }
        }

        private InputBox(string Title, string PromptText)
        {
            this._title = Title;
            this._promptText = PromptText;
        }

        private InputBox(string Title, string PromptText, string Value)
        {
            this._title = Title;
            this._promptText = PromptText;
            this._values = new string[] { Value };
        }

        private InputBox(string Title, string PromptText, string[] Values)
        {
            this._title = Title;
            this._promptText = PromptText;
            this.Values = Values;
        }

        private InputBox(string Title, string PromptText, string[] Values, int SelectedIndex)
        {
            this._title = Title;
            this._promptText = PromptText;
            this.Values = Values;
            this.SelectedIndex = SelectedIndex;
        }

        private DialogResult Show()
        {
            if (this._values.Length == 1)
                return ShowMaskedTextBoxed();
            else
                return ShowComboBoxed();
        }

        private DialogResult ShowNumericBoxed(ref int val, int min, int max)
        {
            Form form = new InputBoxForm();
            form.ShowInTaskbar = pShowInTaskBar;
            Label label = new Label();
            NumericUpDown digitBox = new NumericUpDown();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            PictureBox picture = new PictureBox();

            form.Text = _title;
            label.Text = _promptText;
            digitBox.BorderStyle = BorderStyle.FixedSingle;
            digitBox.Minimum = min;
            digitBox.Maximum = max;
            digitBox.Value = val;
            digitBox.Select(0, digitBox.Value.ToString().Length);
            if (_icon != null) picture.Image = _icon;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;

            buttonOk.Text = pOk_Text;
            buttonCancel.Text = pCancel_Text;
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            digitBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);
            picture.SetBounds(12, 72, 22, 22);

            label.AutoSize = true;
            digitBox.Anchor = digitBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, digitBox, buttonOk, buttonCancel, picture });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterParent;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            _result = form.ShowDialog();
            if (picture.Image != null) picture.Image.Dispose();
            form.Dispose();
            _values[0] = ((val = (int)digitBox.Value)).ToString();
            return _result;
        }

        private DialogResult ShowMaskedTextBoxed()
        {
            Form form = new InputBoxForm();
            form.ShowInTaskbar = pShowInTaskBar;
            Label label = new Label();
            MaskedTextBox textBox = new MaskedTextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            PictureBox picture = new PictureBox();

            form.Text = _title;
            label.Text = _promptText;
            textBox.Text = _prevValue = _values[0];
            if (!String.IsNullOrEmpty(this.InputMask))
                textBox.Mask = this.InputMask;
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.Text.Length;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            Color bc = textBox.BackColor;
            if (_readOnly) textBox.ReadOnly = true;
            textBox.BackColor = bc;
            textBox.TextChanged += new EventHandler(MaskOrComboTextChanged);
            if (_icon != null) picture.Image = _icon;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;

            buttonOk.Text = pOk_Text;
            buttonCancel.Text = pCancel_Text;
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);
            picture.SetBounds(12, 72, 22, 22);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel, picture });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterParent;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            _result = form.ShowDialog();
            if (picture.Image != null) picture.Image.Dispose();
            form.Dispose();
            _values[0] = textBox.Text;
            return _result;
        }

        private DialogResult ShowComboBoxed()
        {
            Form form = new InputBoxForm();
            form.ShowInTaskbar = pShowInTaskBar;
            Label label = new Label();
            ComboBox comboBox = new ComboBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            PictureBox picture = new PictureBox();

            form.Text = _title;
            label.Text = _promptText;
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.DropDownHeight = 200;
            if (this._readOnly)
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            else
                comboBox.DropDownStyle = ComboBoxStyle.DropDown;
            foreach(string str in this._values)
                comboBox.Items.Add(str);            
            comboBox.SelectedIndex = this._valueIndex;
            this._prevValue = comboBox.Text;
            comboBox.TextChanged += new EventHandler(MaskOrComboTextChanged);
            if (_icon != null) picture.Image = _icon;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;

            buttonOk.Text = pOk_Text;
            buttonCancel.Text = pCancel_Text;
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            comboBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);
            picture.SetBounds(12, 72, 22, 22);

            label.AutoSize = true;
            comboBox.Anchor = comboBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, comboBox, buttonOk, buttonCancel, picture });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterParent;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            _result = form.ShowDialog();
            if (picture.Image != null) picture.Image.Dispose();
            form.Dispose();
            if (comboBox.SelectedIndex == -1)
            {
                List<string> tmp = new List<string>(this._values);
                tmp.Add(comboBox.Text);
                this._values = tmp.ToArray();
                this._valueIndex = this._values.Length -1;
            }
            else
                this._valueIndex = comboBox.SelectedIndex;
            return _result;
        }

        private DialogResult ShowSelectDir()
        {
            Form form = new InputBoxForm();
            form.ShowInTaskbar = pShowInTaskBar;
            Label label = new Label();
            MaskedTextBox textBox = new MaskedTextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            Button buttonAddit = new Button();
            PictureBox picture = new PictureBox();

            form.Text = _title;
            label.Text = _promptText;
            textBox.Text = _values[0];
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.Text.Length;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            Color bc = textBox.BackColor;
            if (_readOnly) textBox.ReadOnly = true;
            textBox.BackColor = bc;
            if (_icon != null) picture.Image = _icon;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            buttonAddit.Click += new EventHandler(buttonAdditD_Click);
            _additData[0] = textBox;
            
            buttonOk.Text = pOk_Text;
            buttonCancel.Text = pCancel_Text;
            buttonAddit.Text = "..";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
            
            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 345, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);
            buttonAddit.SetBounds(360, 36, 24, 20);
            picture.SetBounds(12, 72, 22, 22);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonAddit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonAddit, buttonOk, buttonCancel, picture });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterParent;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            _result = form.ShowDialog();
            if (picture.Image != null) picture.Image.Dispose();
            form.Dispose();
            _values[0] = textBox.Text;
            return _result;
        }

        private DialogResult ShowSelectFile(string filter)
        {
            Form form = new InputBoxForm();
            form.ShowInTaskbar = pShowInTaskBar;
            Label label = new Label();
            MaskedTextBox textBox = new MaskedTextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            Button buttonAddit = new Button();
            PictureBox picture = new PictureBox();

            form.Text = _title;
            label.Text = _promptText;
            textBox.Text = _values[0];
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.Text.Length;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            Color bc = textBox.BackColor;
            if (_readOnly) textBox.ReadOnly = true;
            textBox.BackColor = bc;
            if (_icon != null) picture.Image = _icon;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            buttonAddit.Click += new EventHandler(buttonAdditF_Click);
            _additData[0] = textBox;
            if (String.IsNullOrEmpty(filter))
                _additData[1] = "All Types|*.*";
            else
                _additData[1] = filter;
            _additData[2] = _title;

            buttonOk.Text = pOk_Text;
            buttonCancel.Text = pCancel_Text;
            buttonAddit.Text = "..";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 345, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);
            buttonAddit.SetBounds(360, 36, 24, 20);
            picture.SetBounds(12, 72, 22, 22);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonAddit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonAddit, buttonOk, buttonCancel, picture });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterParent;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            _result = form.ShowDialog();
            if (picture.Image != null) picture.Image.Dispose();
            form.Dispose();
            _values[0] = textBox.Text;
            return _result;
        }

        private DialogResult ShowSelectColor(ref Color color)
        {
            Form form = new InputBoxForm();
            form.ShowInTaskbar = pShowInTaskBar;
            Label label = new Label();
            MaskedTextBox textBox = new MaskedTextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            Button buttonAddit = new Button();
            PictureBox picture = new PictureBox();

            form.Text = _title;
            label.Text = _promptText;
            textBox.Text = HexConverter(color);
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.Text.Length;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Mask = @"\#AAAAAA";            
            this.InputRegex = @"^(#[\dA-Fa-f]{0,6})$";
            textBox.TextChanged += new EventHandler(colorBox_TextChanged);
            if (_icon != null) picture.Image = _icon;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            buttonAddit.Text = "..";
            buttonAddit.FlatStyle = FlatStyle.Flat;
            buttonAddit.BackColor = color;
            buttonAddit.Click += new EventHandler(buttonAdditC_Click);
            _additData[0] = textBox;
            _additData[1] = buttonAddit;
            
            buttonOk.Text = pOk_Text;
            buttonCancel.Text = pCancel_Text;
            buttonAddit.Text = "..";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 345, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);
            buttonAddit.SetBounds(360, 36, 24, 20);
            picture.SetBounds(12, 72, 22, 22);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonAddit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonAddit, buttonOk, buttonCancel, picture });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterParent;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            _result = form.ShowDialog();
            if (picture.Image != null) picture.Image.Dispose();
            form.Dispose();
            color = RGBConverter(textBox.Text);
            return _result;
        }

        private void colorBox_TextChanged(object sender, EventArgs e)
        {
            ((Button)_additData[1]).BackColor = RGBConverter((string)((MaskedTextBox)_additData[0]).Text);
        }

        private void buttonAdditC_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ColorDialog cd = new System.Windows.Forms.ColorDialog();
            cd.FullOpen = true;
            cd.Color = RGBConverter((string)((MaskedTextBox)_additData[0]).Text);
            if (cd.ShowDialog() == DialogResult.OK)
            {
                ((MaskedTextBox)_additData[0]).Text = HexConverter(cd.Color);
                ((Button)_additData[1]).BackColor = cd.Color;
            };
            cd.Dispose();
        }

        private void buttonAdditF_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = ((MaskedTextBox)_additData[0]).Text;
            ofd.Title = (string)_additData[2];
            ofd.Filter = (string)_additData[1];
            ofd.FileName = (string)((MaskedTextBox)_additData[0]).Text;
            if (ofd.ShowDialog() == DialogResult.OK)
                ((MaskedTextBox)_additData[0]).Text = ofd.FileName;
            ofd.Dispose();
        }

        private void buttonAdditD_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = (string)((MaskedTextBox)_additData[0]).Text;
            if (fbd.ShowDialog() == DialogResult.OK)
                ((MaskedTextBox)_additData[0]).Text = fbd.SelectedPath;
            fbd.Dispose();
        }

        private void MaskOrComboTextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(InputRegex)) return;

            if (sender is MaskedTextBox)
            {
                MaskedTextBox tb = (MaskedTextBox)sender;
                int index = tb.SelectionStart > 0 ? tb.SelectionStart - 1 : 0;
                if (String.IsNullOrEmpty(tb.Text)) return;
                if (Regex.IsMatch(tb.Text, InputRegex))
                {
                    _prevValue = tb.Text;
                    return;
                }
                else
                {
                    tb.Text = _prevValue;
                    tb.SelectionStart = index;
                };
            };
            if (sender is ComboBox)
            {
                ComboBox cb = (ComboBox)sender;
                int index = cb.SelectionStart > 0 ? cb.SelectionStart - 1 : 0;
                if (String.IsNullOrEmpty(cb.Text)) return;
                if (Regex.IsMatch(cb.Text, InputRegex))
                {
                    _prevValue = cb.Text;
                    return;
                }
                else
                {
                    cb.Text = _prevValue;
                    cb.SelectionStart = index;
                };
            };
        }

        /// <summary>
        ///     Show ReadOnly Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>DialogResult</returns>6
        public static DialogResult Show(string title, string promptText, string value)
        {
            InputBox ib = new InputBox(title, promptText, value);
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            value = ib.Value;
            return dr;
        }
        /// <summary>
        ///     Show ReadOnly Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="icon">Icon Image</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, string value, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value);
            ib.ReadOnly = true;
            ib.Icon = icon;
            DialogResult dr = ib.Show();
            value = ib.Value;
            return dr;
        }
        /// <summary>
        ///     Show Editable Input Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, ref string value)
        {
            InputBox ib = new InputBox(title, promptText, value);
            DialogResult dr = ib.Show();
            value = ib.Value;
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="icon">Icon Image</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, ref string value, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value);
            ib.Icon = icon;
            DialogResult dr = ib.Show();
            value = ib.Value;
            return dr;

        }
        /// <summary>
        ///     Show Editable Masked Input Box Dialog 
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="InputMaskOrRegex">Input Mask or Regex for Value
        /// <para>If text is Input Mask it starts from M. If text is Regex it starts from R.</para>
        /// <para>Input Mask symbols: 0 - digits; 9 - digits and spaces; # - digits, spaces, +, -</para>
        /// <para>  L - letter; ? - letters if need; A - letter or digit; . - decimal separator;</para>
        /// <para>  , - space for digits; / - date separator; $ - currency symbol</para>
        /// <para>Regex: http://regexstorm.net/reference</para>
        /// </param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, ref string value, string InputMaskOrRegex)
        {
            InputBox ib = new InputBox(title, promptText, value);
            ib.InputMaskOrRegex = InputMaskOrRegex;
            DialogResult dr = ib.Show();
            value = ib.Value;
            return dr;
        }
        /// <summary>
        ///     Show Editable Masked Input Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="InputMaskOrRegex">Input Mask or Regex for Value
        /// <para>If text is Input Mask it starts from M. If text is Regex it starts from R.</para>
        /// <para>Input Mask symbols: 0 - digits; 9 - digits and spaces; # - digits, spaces, +, -</para>
        /// <para>  L - letter; ? - letters if need; A - letter or digit; . - decimal separator;</para>
        /// <para>  , - space for digits; / - date separator; $ - currency symbol</para>
        /// <para>Regex: http://regexstorm.net/reference</para>
        /// </param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, ref string value, string InputMaskOrRegex, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value);
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            value = ib.Value.ToString();
            return dr;
        }

        /// <summary>
        ///     Show Editable Input Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryStringBox(string title, string promptText, ref string value)
        {
            InputBox ib = new InputBox(title, promptText, value);
            DialogResult dr = ib.Show();
            value = ib.Value;
            return dr;
        }
        /// <summary>
        ///     Show Editable Input Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="icon">Icon Image</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryStringBox(string title, string promptText, ref string value, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value);
            ib.Icon = icon;
            DialogResult dr = ib.Show();
            value = ib.Value;
            return dr;

        }
        /// <summary>
        ///     Show Editable Masked Input Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="InputMaskOrRegex">Input Mask or Regex for Value
        /// <para>If text is Input Mask it starts from M. If text is Regex it starts from R.</para>
        /// <para>Input Mask symbols: 0 - digits; 9 - digits and spaces; # - digits, spaces, +, -</para>
        /// <para>  L - letter; ? - letters if need; A - letter or digit; . - decimal separator;</para>
        /// <para>  , - space for digits; / - date separator; $ - currency symbol</para>
        /// <para>Regex: http://regexstorm.net/reference</para>
        /// </param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryStringBox(string title, string promptText, ref string value, string InputMaskOrRegex)
        {
            InputBox ib = new InputBox(title, promptText, value);
            ib.InputMaskOrRegex = InputMaskOrRegex;
            DialogResult dr = ib.Show();
            value = ib.Value;
            return dr;
        }
        /// <summary>
        ///     Show Editable Masked Input Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="InputMaskOrRegex">Input Mask or Regex for Value
        /// <para>If text is Input Mask it starts from M. If text is Regex it starts from R.</para>
        /// <para>Input Mask symbols: 0 - digits; 9 - digits and spaces; # - digits, spaces, +, -</para>
        /// <para>  L - letter; ? - letters if need; A - letter or digit; . - decimal separator;</para>
        /// <para>  , - space for digits; / - date separator; $ - currency symbol</para>
        /// <para>Regex: http://regexstorm.net/reference</para>
        /// </param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryStringBox(string title, string promptText, ref string value, string InputMaskOrRegex, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value);
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            value = ib.Value.ToString();
            return dr;
        }

        /// <summary>
        ///     Show ReadOnly Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, int value)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString());
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            return dr;

        }
        /// <summary>
        ///     Show ReadOnly Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, int value, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString());
            ib.Icon = icon;
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, ref int value)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString());
            DialogResult dr = ib.ShowNumericBoxed(ref value, int.MinValue, int.MaxValue);
            value = int.Parse(ib.Value);
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, ref int value, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString());
            ib.Icon = icon;
            DialogResult dr = ib.ShowNumericBoxed(ref value, int.MinValue, int.MaxValue);
            value = int.Parse(ib.Value);
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="min">Minimum Allowed Value</param>
        /// <param name="max">Maximum Allowed Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, ref int value, int min, int max)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString());
            DialogResult dr = ib.ShowNumericBoxed(ref value, min, max);
            value = int.Parse(ib.Value);
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="min">Minimum Allowed Value</param>
        /// <param name="max">Maximum Allowed Value</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, ref int value, int min, int max, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString());
            ib.Icon = icon;
            DialogResult dr = ib.ShowNumericBoxed(ref value, min, max);
            value = int.Parse(ib.Value);
            return dr;

        }

        /// <summary>
        ///     Show Editable Input Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryNumberBox(string title, string promptText, ref int value)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString());
            DialogResult dr = ib.ShowNumericBoxed(ref value, int.MinValue, int.MaxValue);
            value = int.Parse(ib.Value);
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryNumberBox(string title, string promptText, ref int value, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString());
            ib.Icon = icon;
            DialogResult dr = ib.ShowNumericBoxed(ref value, int.MinValue, int.MaxValue);
            value = int.Parse(ib.Value);
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="min">Minimum Allowed Value</param>
        /// <param name="max">Maximum Allowed Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryNumberBox(string title, string promptText, ref int value, int min, int max)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString());
            DialogResult dr = ib.ShowNumericBoxed(ref value, min, max);
            value = int.Parse(ib.Value);
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="min">Minimum Allowed Value</param>
        /// <param name="max">Maximum Allowed Value</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryNumberBox(string title, string promptText, ref int value, int min, int max, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString());
            ib.Icon = icon;
            DialogResult dr = ib.ShowNumericBoxed(ref value, min, max);
            value = int.Parse(ib.Value);
            return dr;

        }


        /// <summary>
        ///     Show Selectable List Box
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Index</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, string[] values, ref int selectedValue)
        {
            InputBox ib = new InputBox(title, promptText, values, selectedValue);
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            selectedValue = ib.SelectedIndex;
            return dr;

        }
        /// <summary>
        ///     Show Selectable List Box with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Index</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, string[] values, ref int selectedValue, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, values, selectedValue);
            ib.ReadOnly = true;
            ib.Icon = icon;
            DialogResult dr = ib.Show();
            selectedValue = ib.SelectedIndex;
            return dr;

        }
        /// <summary>
        ///     Show Selectable List Box
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Text</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, string[] values, ref string selectedValue)
        {
            InputBox ib = new InputBox(title, promptText, values);
            ib.ReadOnly = true;
            ib.Value = selectedValue;
            DialogResult dr = ib.Show();
            selectedValue = ib.Value;
            return dr;

        }
        /// <summary>
        ///     Show Selectable List Box with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Text</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, string[] values, ref string selectedValue, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, values);
            ib.ReadOnly = true;
            ib.Icon = icon;
            ib.Value = selectedValue;
            DialogResult dr = ib.Show();
            selectedValue = ib.Value;
            return dr;

        }
        /// <summary>
        ///     Show Changable List Box 
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Text</param>
        /// <param name="InputMaskOrRegex">Input Mask or Regex for Value
        /// <para>If text is Input Mask it starts from M. If text is Regex it starts from R.</para>
        /// <para>Input Mask symbols: 0 - digits; 9 - digits and spaces; # - digits, spaces, +, -</para>
        /// <para>  L - letter; ? - letters if need; A - letter or digit; . - decimal separator;</para>
        /// <para>  , - space for digits; / - date separator; $ - currency symbol</para>
        /// <para>Regex: http://regexstorm.net/reference</para>
        /// </param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, string[] values, ref string selectedValue, string InputMaskOrRegex)
        {
            InputBox ib = new InputBox(title, promptText, values);
            ib.InputMaskOrRegex = InputMaskOrRegex;
            ib.ReadOnly = false;
            ib.Value = selectedValue;
            DialogResult dr = ib.Show();
            selectedValue = ib.Value;
            return dr;
        }
        /// <summary>
        ///     Show Changable List Box with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Text</param>
        /// <param name="InputMaskOrRegex">Input Mask or Regex for Value
        /// <para>If text is Input Mask it starts from M. If text is Regex it starts from R.</para>
        /// <para>Input Mask symbols: 0 - digits; 9 - digits and spaces; # - digits, spaces, +, -</para>
        /// <para>  L - letter; ? - letters if need; A - letter or digit; . - decimal separator;</para>
        /// <para>  , - space for digits; / - date separator; $ - currency symbol</para>
        /// <para>Regex: http://regexstorm.net/reference</para>
        /// </param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, string[] values, ref string selectedValue, string InputMaskOrRegex, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, values);
            ib.InputMaskOrRegex = InputMaskOrRegex;
            ib.ReadOnly = false;
            ib.Icon = icon;
            ib.Value = selectedValue;
            DialogResult dr = ib.Show();
            selectedValue = ib.Value;
            return dr;
        }
        /// <summary>
        ///      Show Listable or Changable List
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Text</param>
        /// <param name="allowNewValue">Changable or Editable</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, string[] values, ref string selectedValue, bool allowNewValue)
        {
            InputBox ib = new InputBox(title, promptText, values);
            ib.ReadOnly = !allowNewValue;
            ib.Value = selectedValue;
            DialogResult dr = ib.Show();
            selectedValue = ib.Value;
            return dr;
        }
        /// <summary>
        ///     Show Listable or Changable List with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Text</param>
        /// <param name="allowNewValue">Changable or Editable</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, string[] values, ref string selectedValue, bool allowNewValue, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, values);
            ib.ReadOnly = !allowNewValue;
            ib.Icon = icon;
            ib.Value = selectedValue;
            DialogResult dr = ib.Show();
            selectedValue = ib.Value;
            return dr;
        }

        /// <summary>
        ///     Show Selectable List Box
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Index</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryListBox(string title, string promptText, string[] values, ref int selectedValue)
        {
            InputBox ib = new InputBox(title, promptText, values, selectedValue);
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            selectedValue = ib.SelectedIndex;
            return dr;

        }
        /// <summary>
        ///     Show Selectable List Box with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Index</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryListBox(string title, string promptText, string[] values, ref int selectedValue, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, values, selectedValue);
            ib.ReadOnly = true;
            ib.Icon = icon;
            DialogResult dr = ib.Show();
            selectedValue = ib.SelectedIndex;
            return dr;

        }
        /// <summary>
        ///     Show Selectable List Box
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Text</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryListBox(string title, string promptText, string[] values, ref string selectedValue)
        {
            InputBox ib = new InputBox(title, promptText, values);
            ib.ReadOnly = true;
            ib.Value = selectedValue;
            DialogResult dr = ib.Show();
            selectedValue = ib.Value;
            return dr;

        }
        /// <summary>
        ///     Show Selectable List Box with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Text</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryListBox(string title, string promptText, string[] values, ref string selectedValue, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, values);
            ib.ReadOnly = true;
            ib.Icon = icon;
            ib.Value = selectedValue;
            DialogResult dr = ib.Show();
            selectedValue = ib.Value;
            return dr;

        }
        /// <summary>
        ///     Show Changable List Box 
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Text</param>
        /// <param name="InputMaskOrRegex">Input Mask or Regex for Value
        /// <para>If text is Input Mask it starts from M. If text is Regex it starts from R.</para>
        /// <para>Input Mask symbols: 0 - digits; 9 - digits and spaces; # - digits, spaces, +, -</para>
        /// <para>  L - letter; ? - letters if need; A - letter or digit; . - decimal separator;</para>
        /// <para>  , - space for digits; / - date separator; $ - currency symbol</para>
        /// <para>Regex: http://regexstorm.net/reference</para>
        /// </param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryListBox(string title, string promptText, string[] values, ref string selectedValue, string InputMaskOrRegex)
        {
            InputBox ib = new InputBox(title, promptText, values);
            ib.InputMaskOrRegex = InputMaskOrRegex;
            ib.ReadOnly = false;
            ib.Value = selectedValue;
            DialogResult dr = ib.Show();
            selectedValue = ib.Value;
            return dr;
        }
        /// <summary>
        ///     Show Changable List Box with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Text</param>
        /// <param name="InputMaskOrRegex">Input Mask or Regex for Value
        /// <para>If text is Input Mask it starts from M. If text is Regex it starts from R.</para>
        /// <para>Input Mask symbols: 0 - digits; 9 - digits and spaces; # - digits, spaces, +, -</para>
        /// <para>  L - letter; ? - letters if need; A - letter or digit; . - decimal separator;</para>
        /// <para>  , - space for digits; / - date separator; $ - currency symbol</para>
        /// <para>Regex: http://regexstorm.net/reference</para>
        /// </param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryListBox(string title, string promptText, string[] values, ref string selectedValue, string InputMaskOrRegex, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, values);
            ib.InputMaskOrRegex = InputMaskOrRegex;
            ib.ReadOnly = false;
            ib.Icon = icon;
            ib.Value = selectedValue;
            DialogResult dr = ib.Show();
            selectedValue = ib.Value;
            return dr;
        }
        /// <summary>
        ///      Show Listable or Changable List
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Text</param>
        /// <param name="allowNewValue">Changable or Editable</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryListBox(string title, string promptText, string[] values, ref string selectedValue, bool allowNewValue)
        {
            InputBox ib = new InputBox(title, promptText, values);
            ib.ReadOnly = !allowNewValue;
            ib.Value = selectedValue;
            DialogResult dr = ib.Show();
            selectedValue = ib.Value;
            return dr;
        }
        /// <summary>
        ///     Show Listable or Changable List with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="values">List Values</param>
        /// <param name="selectedValue">Selected Value Text</param>
        /// <param name="allowNewValue">Changable or Editable</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryListBox(string title, string promptText, string[] values, ref string selectedValue, bool allowNewValue, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, values);
            ib.ReadOnly = !allowNewValue;
            ib.Icon = icon;
            ib.Value = selectedValue;
            DialogResult dr = ib.Show();
            selectedValue = ib.Value;
            return dr;
        }
        
        /// <summary>
        ///     Show Two-List DropBox
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">boolean value</param>
        /// <param name="textFalse">Text for False Value</param>
        /// <param name="textTrue">Text for True Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, ref bool value, string textFalse, string textTrue)
        {
            InputBox ib = new InputBox(title, promptText, new string[]{ textFalse, textTrue }, value ? 1 : 0);
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            value = ib.SelectedIndex == 1;
            return dr;
        }
        /// <summary>
        ///     Show Two-List DropBox with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">boolean value</param>
        /// <param name="textFalse">Text for False Value</param>
        /// <param name="textTrue">Text for True Value</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, ref bool value, string textFalse, string textTrue, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, new string[] { textFalse, textTrue }, value ? 1 : 0);
            ib.Icon = icon;
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            value = ib.SelectedIndex == 1;
            return dr;
        }

        /// <summary>
        ///     Show ReadOnly Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, float value)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            return dr;

        }
        /// <summary>
        ///     Show ReadOnly Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, float value, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.Icon = icon;
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, ref float value)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.InputMaskOrRegex = "^(([+-]?)|([+-]?[0-9]{1,}[.]?[0-9]{0,})|([+-]?[.][0-9]{0,}))$";
            DialogResult dr = ib.Show();
            if ((ib.Value == "-") || (ib.Value == "."))
                value = 0;
            else
                value = float.Parse(ib.Value, System.Globalization.CultureInfo.InvariantCulture);
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, ref float value, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.InputMaskOrRegex = "^(([+-]?)|([+-]?[0-9]{1,}[.]?[0-9]{0,})|([+-]?[.][0-9]{0,}))$";
            ib.Icon = icon;
            DialogResult dr = ib.Show();
            if ((ib.Value == "-") || (ib.Value == "."))
                value = 0;
            else
                value = float.Parse(ib.Value, System.Globalization.CultureInfo.InvariantCulture);
            return dr;
        }

        /// <summary>
        ///     Show ReadOnly Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryNumberBox(string title, string promptText, float value)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            return dr;

        }
        /// <summary>
        ///     Show ReadOnly Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryNumberBox(string title, string promptText, float value, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.Icon = icon;
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryNumberBox(string title, string promptText, ref float value)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.InputMaskOrRegex = "^(([+-]?)|([+-]?[0-9]{1,}[.]?[0-9]{0,})|([+-]?[.][0-9]{0,}))$";
            DialogResult dr = ib.Show();
            if ((ib.Value == "-") || (ib.Value == "."))
                value = 0;
            else
                value = float.Parse(ib.Value, System.Globalization.CultureInfo.InvariantCulture);
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryNumberBox(string title, string promptText, ref float value, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.InputMaskOrRegex = "^(([+-]?)|([+-]?[0-9]{1,}[.]?[0-9]{0,})|([+-]?[.][0-9]{0,}))$";
            ib.Icon = icon;
            DialogResult dr = ib.Show();
            if ((ib.Value == "-") || (ib.Value == "."))
                value = 0;
            else
                value = float.Parse(ib.Value, System.Globalization.CultureInfo.InvariantCulture);
            return dr;
        }

        /// <summary>
        ///     Show ReadOnly Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, double value)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            return dr;

        }
        /// <summary>
        ///     Show ReadOnly Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, double value, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.Icon = icon;
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, ref double value)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.InputMaskOrRegex = "^(([+-]?)|([+-]?[0-9]{1,}[.]?[0-9]{0,})|([+-]?[.][0-9]{0,}))$";
            DialogResult dr = ib.Show();
            if ((ib.Value == "-") || (ib.Value == "."))
                value = 0;
            else
                value = double.Parse(ib.Value, System.Globalization.CultureInfo.InvariantCulture);
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, ref double value, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.InputMaskOrRegex = "^(([+-]?)|([+-]?[0-9]{1,}[.]?[0-9]{0,})|([+-]?[.][0-9]{0,}))$";
            ib.Icon = icon;
            DialogResult dr = ib.Show();
            if ((ib.Value == "-") || (ib.Value == "."))
                value = 0;
            else
                value = double.Parse(ib.Value, System.Globalization.CultureInfo.InvariantCulture);
            return dr;
        }

        /// <summary>
        ///     Show ReadOnly Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryNumberBox(string title, string promptText, double value)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            return dr;

        }
        /// <summary>
        ///     Show ReadOnly Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryNumberBox(string title, string promptText, double value, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.Icon = icon;
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryNumberBox(string title, string promptText, ref double value)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.InputMaskOrRegex = "^(([+-]?)|([+-]?[0-9]{1,}[.]?[0-9]{0,})|([+-]?[.][0-9]{0,}))$";
            DialogResult dr = ib.Show();
            if ((ib.Value == "-") || (ib.Value == "."))
                value = 0;
            else
                value = double.Parse(ib.Value, System.Globalization.CultureInfo.InvariantCulture);
            return dr;

        }
        /// <summary>
        ///     Show Editable Input Box Dialog with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="value">Parameter Value</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryNumberBox(string title, string promptText, ref double value, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            ib.InputMaskOrRegex = "^(([+-]?)|([+-]?[0-9]{1,}[.]?[0-9]{0,})|([+-]?[.][0-9]{0,}))$";
            ib.Icon = icon;
            DialogResult dr = ib.Show();
            if ((ib.Value == "-") || (ib.Value == "."))
                value = 0;
            else
                value = double.Parse(ib.Value, System.Globalization.CultureInfo.InvariantCulture);
            return dr;
        }

        /// <summary>
        ///     Show Listable Input Box Dialog for typeof(Enum)
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="enumType">Type of Enum</param>
        /// <param name="selectedValue">Selected Value</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, Type enumType, ref object selectedValue)
        {
            Array vls = Enum.GetValues((enumType));

            List<string> vals = new List<string>();
            foreach (string element in Enum.GetNames(enumType))
                vals.Add(element);

            InputBox ib = new InputBox(title, promptText, vals.ToArray());
            ib.Value = selectedValue.ToString();
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            selectedValue = vls.GetValue(ib.SelectedIndex);
            return dr;
        }
        /// <summary>
        ///     Show Listable Input Box Dialog for typeof(Enum)
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="enumType">Type of Enum</param>
        /// <param name="selectedValue">Selected Value</param>
        /// /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(string title, string promptText, Type enumType, ref object selectedValue, Bitmap icon)
        {
            Array vls = Enum.GetValues((enumType));

            List<string> vals = new List<string>();
            foreach (string element in Enum.GetNames(enumType))
                vals.Add(element);

            InputBox ib = new InputBox(title, promptText, vals.ToArray());
            ib.Icon = icon;
            ib.Value = selectedValue.ToString();
            ib.ReadOnly = true;
            DialogResult dr = ib.Show();
            selectedValue = vls.GetValue(ib.SelectedIndex);
            return dr;
        }

        /// <summary>
        ///     Show Editable Directory Input Box With Browse Button
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="path">DIrectory</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryDirectoryBox(string title, string promptText, ref string path)
        {
            InputBox ib = new InputBox(title, promptText, path);
            DialogResult dr = ib.ShowSelectDir();
            path = ib.Value;
            return dr;
        }
        /// <summary>
        ///     Show Editable Directory Input Box With Browse Button
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="path">DIrectory</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryDirectoryBox(string title, string promptText, ref string path, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, path);
            ib.Icon = icon;
            DialogResult dr = ib.ShowSelectDir();
            path = ib.Value;
            return dr;
        }

        /// <summary>
        ///     Show Editable File Input Box
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="file">File Path</param>
        /// <param name="filter">OpenFileDialog Filter</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryFileBox(string title, string promptText, ref string file, string filter)
        {
            InputBox ib = new InputBox(title, promptText, file);
            DialogResult dr = ib.ShowSelectFile(filter);
            file = ib.Value;
            return dr;
        }

        /// <summary>
        ///     Show Editable File Input Box with Icon
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="file">File Path</param>
        /// <param name="filter">OpenFileDialog Filter</param>
        /// <param name="icon">Image Icon</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryFileBox(string title, string promptText, ref string file, string filter, Bitmap icon)
        {
            InputBox ib = new InputBox(title, promptText, file);
            DialogResult dr = ib.ShowSelectFile(filter);
            file = ib.Value;
            return dr;
        }

        /// <summary>
        ///     Show Editable Color Box
        /// </summary>
        /// <param name="title">Dialog Window Title</param>
        /// <param name="promptText">Parameter Prompt Text</param>
        /// <param name="color">Color</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QueryColorBox(string title, string promptText, ref Color color)
        {
            InputBox ib = new InputBox(title, promptText);
            DialogResult dr = ib.ShowSelectColor(ref color);
            return dr;
        }

        private static String HexConverter(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private static Color RGBConverter(string hex)
        {
            Color rtn = Color.Black;
            try
            {
                return Color.FromArgb(
                    int.Parse(hex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber));
            }
            catch (Exception ex)
            {
                //doing nothing
            }

            return rtn;
        }
    }

    public class InputBoxForm : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
    }
}
