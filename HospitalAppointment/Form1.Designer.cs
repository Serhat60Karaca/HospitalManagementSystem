namespace HospitalAppointment
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnregister = new System.Windows.Forms.Button();
            this.btndoctor = new System.Windows.Forms.Button();
            this.btnpatient = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.register1 = new HospitalAppointment.register();
            this.doctorlogin1 = new HospitalAppointment.doctorlogin();
            this.patientlogin1 = new HospitalAppointment.patientlogin();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(171)))), ((int)(((byte)(194)))));
            this.panel1.Controls.Add(this.btnregister);
            this.panel1.Controls.Add(this.btndoctor);
            this.panel1.Controls.Add(this.btnpatient);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 688);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnregister
            // 
            this.btnregister.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnregister.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnregister.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnregister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnregister.Location = new System.Drawing.Point(0, 271);
            this.btnregister.Name = "btnregister";
            this.btnregister.Size = new System.Drawing.Size(200, 48);
            this.btnregister.TabIndex = 2;
            this.btnregister.Text = "Register";
            this.btnregister.UseVisualStyleBackColor = true;
            this.btnregister.Click += new System.EventHandler(this.btnregister_Click);
            // 
            // btndoctor
            // 
            this.btndoctor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btndoctor.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btndoctor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btndoctor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btndoctor.Location = new System.Drawing.Point(0, 202);
            this.btndoctor.Name = "btndoctor";
            this.btndoctor.Size = new System.Drawing.Size(200, 48);
            this.btndoctor.TabIndex = 1;
            this.btndoctor.Text = "Doctor Login";
            this.btndoctor.UseVisualStyleBackColor = true;
            this.btndoctor.Click += new System.EventHandler(this.btndoctor_Click);
            // 
            // btnpatient
            // 
            this.btnpatient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnpatient.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnpatient.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnpatient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnpatient.Location = new System.Drawing.Point(0, 129);
            this.btnpatient.Name = "btnpatient";
            this.btnpatient.Size = new System.Drawing.Size(200, 50);
            this.btnpatient.TabIndex = 0;
            this.btnpatient.Text = "Patient Login";
            this.btnpatient.UseVisualStyleBackColor = true;
            this.btnpatient.Click += new System.EventHandler(this.btnpatient_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // register1
            // 
            this.register1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.register1.Location = new System.Drawing.Point(200, 0);
            this.register1.Name = "register1";
            this.register1.Size = new System.Drawing.Size(999, 688);
            this.register1.TabIndex = 4;
            this.register1.Visible = false;
            this.register1.Load += new System.EventHandler(this.register1_Load);
            // 
            // doctorlogin1
            // 
            this.doctorlogin1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doctorlogin1.Location = new System.Drawing.Point(200, 0);
            this.doctorlogin1.Name = "doctorlogin1";
            this.doctorlogin1.Size = new System.Drawing.Size(999, 688);
            this.doctorlogin1.TabIndex = 3;
            this.doctorlogin1.Visible = false;
            // 
            // patientlogin1
            // 
            this.patientlogin1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patientlogin1.Location = new System.Drawing.Point(200, 0);
            this.patientlogin1.Name = "patientlogin1";
            this.patientlogin1.Size = new System.Drawing.Size(999, 688);
            this.patientlogin1.TabIndex = 2;
            this.patientlogin1.Visible = false;
            this.patientlogin1.Load += new System.EventHandler(this.patientlogin1_Load);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(43)))), ((int)(((byte)(58)))));
            this.panel2.Controls.Add(this.button4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(200, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(999, 36);
            this.panel2.TabIndex = 5;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(949, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(38, 27);
            this.button4.TabIndex = 0;
            this.button4.Text = "X";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 688);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.register1);
            this.Controls.Add(this.doctorlogin1);
            this.Controls.Add(this.patientlogin1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnregister;
        private System.Windows.Forms.Button btndoctor;
        private System.Windows.Forms.Button btnpatient;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private patientlogin patientlogin1;
        private doctorlogin doctorlogin1;
        private register register1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button4;
    }
}

