﻿namespace NINOKUNI
{
    partial class SQcontrol
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtOriginal = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTranslated = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.listsBlock = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.listfBlock = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtOriginal
            // 
            this.txtOriginal.Location = new System.Drawing.Point(7, 20);
            this.txtOriginal.Multiline = true;
            this.txtOriginal.Name = "txtOriginal";
            this.txtOriginal.ReadOnly = true;
            this.txtOriginal.Size = new System.Drawing.Size(500, 100);
            this.txtOriginal.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "S00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 162);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "S01";
            // 
            // txtTranslated
            // 
            this.txtTranslated.Location = new System.Drawing.Point(7, 179);
            this.txtTranslated.Multiline = true;
            this.txtTranslated.Name = "txtTranslated";
            this.txtTranslated.Size = new System.Drawing.Size(500, 100);
            this.txtTranslated.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::NINOKUNI.Properties.Resources.page_white_edit;
            this.btnSave.Location = new System.Drawing.Point(415, 475);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 35);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "S03";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // listsBlock
            // 
            this.listsBlock.FormattingEnabled = true;
            this.listsBlock.Location = new System.Drawing.Point(10, 322);
            this.listsBlock.Name = "listsBlock";
            this.listsBlock.Size = new System.Drawing.Size(130, 69);
            this.listsBlock.TabIndex = 5;
            this.listsBlock.SelectedIndexChanged += new System.EventHandler(this.listBlock_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 303);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "S02";
            // 
            // listfBlock
            // 
            this.listfBlock.FormattingEnabled = true;
            this.listfBlock.Location = new System.Drawing.Point(10, 427);
            this.listfBlock.Name = "listfBlock";
            this.listfBlock.Size = new System.Drawing.Size(130, 82);
            this.listfBlock.TabIndex = 7;
            this.listfBlock.SelectedIndexChanged += new System.EventHandler(this.listfBlocks_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 411);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Final blocks:";
            // 
            // SQcontrol
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listfBlock);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listsBlock);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtTranslated);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOriginal);
            this.Name = "SQcontrol";
            this.Size = new System.Drawing.Size(512, 512);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOriginal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTranslated;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListBox listsBlock;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listfBlock;
        private System.Windows.Forms.Label label4;
    }
}
