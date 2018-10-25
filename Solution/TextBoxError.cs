#region Using Directives
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace LeagueSpectator
{
    public class TextBoxError : TextBox
    {
        #region Members
        private Boolean m_Error;
        private Color m_BackgroundColor = SystemColors.Window;
        private Color m_ErrorBackgroundColor = Color.MistyRose;
        private Color m_ErrorBorderColor = Color.IndianRed;
        #endregion

        #region Properties
        protected sealed override Boolean DoubleBuffered => true;

        [Browsable(false)]
        public Boolean Error
        {
            get => m_Error;
            set
            {
                if (value == m_Error)
                    return;

                m_Error = value;

                if (!Enabled || ReadOnly)
                    return;

                if (m_Error)
                    BackColor = m_ErrorBackgroundColor;
                else
                    BackColor = m_BackgroundColor;

                NativeMethods.RedrawControl(this);
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        [Category("Appearance")]
        [Description("The background color of the component.")]
        [DefaultValue(typeof(SystemColors),"Window")]
        public Color BackgroundColor
        {
            get => m_BackgroundColor;
            set
            {
                if (ReadOnly)
                    return;

                if (m_BackgroundColor == value)
                    return;
        
                m_BackgroundColor = value;

                if (!Error)
                    BackColor = value;
            }
        }

        [Category("Appearance")]
        [Description("The background color of the component in case of validation error.")]
        [DefaultValue(typeof(Color),"MistyRose")]
        public Color ErrorBackgroundColor
        {
            get => m_ErrorBackgroundColor;
            set
            {
                if (value == m_ErrorBorderColor)
                    return;

                m_ErrorBackgroundColor = value;

                if (Error)
                    BackColor = value;
            }
        }

        [Category("Appearance")]
        [Description("The border color of the component in case of validation error.")]
        [DefaultValue(typeof(Color),"IndianRed")]
        public Color ErrorBorderColor
        {
            get => m_ErrorBorderColor;
            set
            {
                if (value == m_ErrorBorderColor)
                    return;

                m_ErrorBorderColor = value;

                if (Error)
                    NativeMethods.RedrawControl(this);
            }
        }
        #endregion

        #region Constructors
        public TextBoxError()
        {
            base.BackColor = SystemColors.Window;
            base.DoubleBuffered = true;
        }
        #endregion

        #region Methods
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            if (Enabled)
            {
                if (ReadOnly)
                    base.BackColor = SystemColors.Control;
                else if (Error)
                    base.BackColor = m_ErrorBackgroundColor;
                else
                    base.BackColor = m_BackgroundColor;
            }
            else
            {
                if (ReadOnly)
                    base.BackColor = SystemColors.Control;
                else
                    base.BackColor = m_BackgroundColor;
            }
        }

        protected override void OnReadOnlyChanged(EventArgs e)
        {
            base.OnReadOnlyChanged(e);

            if (ReadOnly)
                base.BackColor = SystemColors.Control;
            else if (!Enabled && Error)
                base.BackColor = m_ErrorBackgroundColor;
            else
                base.BackColor = m_BackgroundColor;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            NativeMethods.RedrawControl(this);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (!NativeMethods.IsPaintMessage(m) || !Enabled || !Error || (ErrorBackgroundColor == Color.Transparent))
                return;
            
            IntPtr dcHandle = NativeMethods.GetControlGraphics(this);
            
            using (Graphics graphics = Graphics.FromHdcInternal(dcHandle))
            using (Pen pen = new Pen(m_ErrorBorderColor))
                graphics.DrawRectangle(pen, (new Rectangle(0, 0, Width - 1, Height - 1)));
            
            NativeMethods.ReleaseControlGraphics(this, dcHandle);
        }
        #endregion
    }
}