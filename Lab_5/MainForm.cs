using System;
using System.Numerics;
using System.Windows.Forms;

namespace Lab_5
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            _helper = new EncryptionHelper();
            _helper.K = 2;
            _helper.L = 3;

            UpdateSettingsDisplay();
            UpdateKeyDisplay();
        }

        private void OnKeyPress(object sender, KeyEventArgs e)
        {
            Button simulated_btn = null;
            switch (e.KeyCode)
            {
                case Keys.Q:
                    simulated_btn = btn_q;
                    break;
                case Keys.W:
                    simulated_btn = btn_w;
                    break;
                case Keys.E:
                    simulated_btn = btn_e;
                    break;
                case Keys.R:
                    simulated_btn = btn_r;
                    break;
                case Keys.T:
                    simulated_btn = btn_t;
                    break;
                case Keys.Y:
                    simulated_btn = btn_y;
                    break;
                case Keys.U:
                    simulated_btn = btn_u;
                    break;
                case Keys.I:
                    simulated_btn = btn_i;
                    break;
                case Keys.O:
                    simulated_btn = btn_o;
                    break;
                case Keys.P:
                    simulated_btn = btn_p;
                    break;
                case Keys.A:
                    simulated_btn = btn_a;
                    break;
                case Keys.S:
                    simulated_btn = btn_s;
                    break;
                case Keys.D:
                    simulated_btn = btn_d;
                    break;
                case Keys.F:
                    simulated_btn = btn_f;
                    break;
                case Keys.G:
                    simulated_btn = btn_g;
                    break;
                case Keys.H:
                    simulated_btn = btn_h;
                    break;
                case Keys.J:
                    simulated_btn = btn_j;
                    break;
                case Keys.K:
                    simulated_btn = btn_k;
                    break;
                case Keys.L:
                    simulated_btn = btn_l;
                    break;
                case Keys.Z:
                    simulated_btn = btn_z;
                    break;
                case Keys.X:
                    simulated_btn = btn_x;
                    break;
                case Keys.C:
                    simulated_btn = btn_c;
                    break;
                case Keys.V:
                    simulated_btn = btn_v;
                    break;
                case Keys.B:
                    simulated_btn = btn_b;
                    break;
                case Keys.N:
                    simulated_btn = btn_n;
                    break;
                case Keys.M:
                    simulated_btn = btn_m;
                    break;
                case Keys.Space:
                    simulated_btn = btn_space;
                    break;
                case Keys.Back:
                    OnBackspaceClicked(null, null);
                    break;
                default:
                    break;
            }

            if (simulated_btn == null)
                return;

            OnBtnClicked(simulated_btn, null);
        }

        private void OnBtnClicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            _input += btn.Text;
            label_input.Text = "Input: " + _input;

            if (!label_output.Equals("Output: "))
                label_output.Text = "Output: ";

            CheckUndoStatus();
            Focus();
        }

        void CheckUndoStatus()
        {
            btn_backspace.Enabled = _input.Length > 0 ? true : false;
        }


        private void OnBackspaceClicked(object sender, EventArgs e)
        {
            if (_input.Length > 0)
            {
                _input = _input.Remove(_input.Length - 1);
                UpdateInput(_input);
            }

            CheckUndoStatus();
            UpdateOutput("");
        }

        private void UpdateInput(string msg)
        {
            label_input.Text = "Input: " + msg;
        }

        private void UpdateOutput(string msg)
        {
            label_output.Text = "Output: " + msg;
        }

        private void OnEncryptClicked(object sender, EventArgs e)
        {
            UpdateOutput(_helper.EncryptRSA(_input));
        }

        private void OnDecryptClicked(object sender, EventArgs e)
        {
            UpdateOutput(_helper.DecryptRSA(_input));
        }

        private void UpdateSettingsDisplay()
        {
            input_k.Value = _helper.K;
            input_l.Value = _helper.L;
        }

        private void UpdateKeyDisplay()
        {
            label_keys.Text = "Ke: (" + _helper.Ke.Item1.ToString() + ", " + _helper.Ke.Item2.ToString() + ") | Kd: " + _helper.Kd;
        }

        private void OnGenerateKeysClicked(object sender, MouseEventArgs e)
        {
            _helper.InitRSA();
            UpdateKeyDisplay();
        }

        private void ForceInputAsNumeric(NumericUpDown input)
        {
            decimal val = input.Value;
            if (val - (int)val != 0.0m)
                input.Value = (int)val;
        }

        private void OnInputKValueChanged(object sender, EventArgs e)
        {
            ForceInputAsNumeric(sender as NumericUpDown);
            _helper.K = (int)input_k.Value;
        }

        private void OnInputLValueChanged(object sender, EventArgs e)
        {
            ForceInputAsNumeric(sender as NumericUpDown);
            _helper.L = (int)input_l.Value;
        }

        private string _input;

        private EncryptionHelper _helper;
    }
}
