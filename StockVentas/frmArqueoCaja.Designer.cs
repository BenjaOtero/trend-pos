namespace StockVentas
{
    partial class frmArqueoCaja
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvVentas = new System.Windows.Forms.DataGridView();
            this.lblFecha = new System.Windows.Forms.Label();
            this.lblLocal = new System.Windows.Forms.Label();
            this.dgvTesoreria = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblEfectivoEntregar = new System.Windows.Forms.Label();
            this.lblCajaFinal = new System.Windows.Forms.Label();
            this.lblEfectivoExistente = new System.Windows.Forms.Label();
            this.lblTesoreria = new System.Windows.Forms.Label();
            this.lblCajaInicial = new System.Windows.Forms.Label();
            this.lblTarjeta = new System.Windows.Forms.Label();
            this.lblEfectivo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTesoreria)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvVentas
            // 
            this.dgvVentas.AllowUserToAddRows = false;
            this.dgvVentas.AllowUserToDeleteRows = false;
            this.dgvVentas.AllowUserToResizeColumns = false;
            this.dgvVentas.AllowUserToResizeRows = false;
            this.dgvVentas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvVentas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVentas.Location = new System.Drawing.Point(12, 46);
            this.dgvVentas.Name = "dgvVentas";
            this.dgvVentas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVentas.Size = new System.Drawing.Size(992, 283);
            this.dgvVentas.TabIndex = 0;
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFecha.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblFecha.Location = new System.Drawing.Point(13, 13);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(47, 15);
            this.lblFecha.TabIndex = 1;
            this.lblFecha.Text = "label1";
            // 
            // lblLocal
            // 
            this.lblLocal.AutoSize = true;
            this.lblLocal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocal.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblLocal.Location = new System.Drawing.Point(880, 13);
            this.lblLocal.Name = "lblLocal";
            this.lblLocal.Size = new System.Drawing.Size(47, 15);
            this.lblLocal.TabIndex = 1;
            this.lblLocal.Text = "label1";
            // 
            // dgvTesoreria
            // 
            this.dgvTesoreria.AllowUserToAddRows = false;
            this.dgvTesoreria.AllowUserToDeleteRows = false;
            this.dgvTesoreria.AllowUserToOrderColumns = true;
            this.dgvTesoreria.AllowUserToResizeColumns = false;
            this.dgvTesoreria.AllowUserToResizeRows = false;
            this.dgvTesoreria.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTesoreria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTesoreria.Location = new System.Drawing.Point(13, 355);
            this.dgvTesoreria.Name = "dgvTesoreria";
            this.dgvTesoreria.Size = new System.Drawing.Size(659, 225);
            this.dgvTesoreria.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label3.Location = new System.Drawing.Point(13, 337);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Movimientos de tesorería";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblTotal);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblEfectivoEntregar);
            this.groupBox1.Controls.Add(this.lblCajaFinal);
            this.groupBox1.Controls.Add(this.lblEfectivoExistente);
            this.groupBox1.Controls.Add(this.lblTesoreria);
            this.groupBox1.Controls.Add(this.lblCajaInicial);
            this.groupBox1.Controls.Add(this.lblTarjeta);
            this.groupBox1.Controls.Add(this.lblEfectivo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(689, 350);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 230);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(220, 198);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(39, 15);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "Total";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(17, 198);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 15);
            this.label11.TabIndex = 1;
            this.label11.Text = "Venta total";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 175);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Efectivo a entregar";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 151);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Caja final";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Efectivo existente";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(126, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Movimientos de tesorería";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Caja inicial";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Ventas con tarjeta";
            // 
            // lblEfectivoEntregar
            // 
            this.lblEfectivoEntregar.AutoSize = true;
            this.lblEfectivoEntregar.Location = new System.Drawing.Point(220, 175);
            this.lblEfectivoEntregar.Name = "lblEfectivoEntregar";
            this.lblEfectivoEntregar.Size = new System.Drawing.Size(46, 13);
            this.lblEfectivoEntregar.TabIndex = 1;
            this.lblEfectivoEntregar.Text = "Efectivo";
            // 
            // lblCajaFinal
            // 
            this.lblCajaFinal.AutoSize = true;
            this.lblCajaFinal.Location = new System.Drawing.Point(220, 151);
            this.lblCajaFinal.Name = "lblCajaFinal";
            this.lblCajaFinal.Size = new System.Drawing.Size(46, 13);
            this.lblCajaFinal.TabIndex = 1;
            this.lblCajaFinal.Text = "Efectivo";
            // 
            // lblEfectivoExistente
            // 
            this.lblEfectivoExistente.AutoSize = true;
            this.lblEfectivoExistente.Location = new System.Drawing.Point(220, 126);
            this.lblEfectivoExistente.Name = "lblEfectivoExistente";
            this.lblEfectivoExistente.Size = new System.Drawing.Size(46, 13);
            this.lblEfectivoExistente.TabIndex = 1;
            this.lblEfectivoExistente.Text = "Efectivo";
            // 
            // lblTesoreria
            // 
            this.lblTesoreria.AutoSize = true;
            this.lblTesoreria.Location = new System.Drawing.Point(220, 100);
            this.lblTesoreria.Name = "lblTesoreria";
            this.lblTesoreria.Size = new System.Drawing.Size(46, 13);
            this.lblTesoreria.TabIndex = 1;
            this.lblTesoreria.Text = "Efectivo";
            // 
            // lblCajaInicial
            // 
            this.lblCajaInicial.AutoSize = true;
            this.lblCajaInicial.Location = new System.Drawing.Point(220, 75);
            this.lblCajaInicial.Name = "lblCajaInicial";
            this.lblCajaInicial.Size = new System.Drawing.Size(46, 13);
            this.lblCajaInicial.TabIndex = 1;
            this.lblCajaInicial.Text = "Efectivo";
            // 
            // lblTarjeta
            // 
            this.lblTarjeta.AutoSize = true;
            this.lblTarjeta.Location = new System.Drawing.Point(220, 51);
            this.lblTarjeta.Name = "lblTarjeta";
            this.lblTarjeta.Size = new System.Drawing.Size(46, 13);
            this.lblTarjeta.TabIndex = 1;
            this.lblTarjeta.Text = "Efectivo";
            // 
            // lblEfectivo
            // 
            this.lblEfectivo.AutoSize = true;
            this.lblEfectivo.Location = new System.Drawing.Point(220, 27);
            this.lblEfectivo.Name = "lblEfectivo";
            this.lblEfectivo.Size = new System.Drawing.Size(46, 13);
            this.lblEfectivo.TabIndex = 1;
            this.lblEfectivo.Text = "Efectivo";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Ventas en efectivo";
            // 
            // frmArqueoCaja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 596);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvTesoreria);
            this.Controls.Add(this.lblLocal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.dgvVentas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmArqueoCaja";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Arqueo de caja";
            this.Activated += new System.EventHandler(this.frmArqueoCajaAdmin_Activated);
            this.Load += new System.EventHandler(this.frmArqueoCaja_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTesoreria)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvVentas;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.Label lblLocal;
        private System.Windows.Forms.DataGridView dgvTesoreria;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblEfectivoEntregar;
        private System.Windows.Forms.Label lblCajaFinal;
        private System.Windows.Forms.Label lblEfectivoExistente;
        private System.Windows.Forms.Label lblTesoreria;
        private System.Windows.Forms.Label lblCajaInicial;
        private System.Windows.Forms.Label lblTarjeta;
        private System.Windows.Forms.Label lblEfectivo;
    }
}