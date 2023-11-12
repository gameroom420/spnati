namespace SPNATI_Character_Editor
{
    partial class StageControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cboFrom = new Desktop.Skinning.SkinnedComboBox();
            this.label1 = new Desktop.Skinning.SkinnedLabel();
            this.label2 = new Desktop.Skinning.SkinnedLabel();
            this.cboTo = new Desktop.Skinning.SkinnedComboBox();
            this.recRefCostume = new Desktop.CommonControls.RecordField();
            this.label3 = new Desktop.Skinning.SkinnedLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label4 = new Desktop.Skinning.SkinnedLabel();
            this.SuspendLayout();
            // 
            // cboFrom
            // 
            this.cboFrom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFrom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cboFrom.BackColor = System.Drawing.Color.White;
            this.cboFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cboFrom.FieldType = Desktop.Skinning.SkinnedFieldType.Surface;
            this.cboFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboFrom.FormattingEnabled = true;
            this.cboFrom.KeyMember = null;
            this.cboFrom.Location = new System.Drawing.Point(37, 0);
            this.cboFrom.Name = "cboFrom";
            this.cboFrom.SelectedIndex = -1;
            this.cboFrom.SelectedItem = null;
            this.cboFrom.Size = new System.Drawing.Size(100, 21);
            this.cboFrom.Sorted = false;
            this.cboFrom.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.label1.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.label2.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.label2.Location = new System.Drawing.Point(138, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "to:";
            // 
            // cboTo
            // 
            this.cboTo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboTo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cboTo.BackColor = System.Drawing.Color.White;
            this.cboTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cboTo.FieldType = Desktop.Skinning.SkinnedFieldType.Surface;
            this.cboTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboTo.FormattingEnabled = true;
            this.cboTo.KeyMember = null;
            this.cboTo.Location = new System.Drawing.Point(158, 0);
            this.cboTo.Name = "cboTo";
            this.cboTo.SelectedIndex = -1;
            this.cboTo.SelectedItem = null;
            this.cboTo.Size = new System.Drawing.Size(100, 21);
            this.cboTo.Sorted = false;
            this.cboTo.TabIndex = 3;
            // 
            // recRefCostume
            // 
            this.recRefCostume.AllowCreate = false;
            this.recRefCostume.Location = new System.Drawing.Point(334, 0);
            this.recRefCostume.Name = "recRefCostume";
            this.recRefCostume.PlaceholderText = null;
            this.recRefCostume.Record = null;
            this.recRefCostume.RecordContext = null;
            this.recRefCostume.RecordFilter = null;
            this.recRefCostume.RecordKey = null;
            this.recRefCostume.RecordType = null;
            this.recRefCostume.Size = new System.Drawing.Size(100, 20);
            this.recRefCostume.TabIndex = 4;
            this.toolTip1.SetToolTip(this.recRefCostume, "Only for previewing the layers of the targeted character's alternate skins. Selecting anything\nor leaving this field empty has no effect on the conditions for the line to play.\nIf you want a costume condition, add a Player: Costume test or a costume Tag filter.");
            this.recRefCostume.UseAutoComplete = false;
            this.recRefCostume.RecordChanged += new System.EventHandler<Desktop.CommonControls.RecordEventArgs>(this.recRefCostume_RecordChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.label3.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.label3.Location = new System.Drawing.Point(274, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "(Ref. Skin:";
            this.toolTip1.SetToolTip(this.label3, "Only for previewing the layers of the targeted character's alternate skins. Selecting anything\nor leaving this field empty has no effect on the conditions for the line to play.\nIf you want a costume condition, add a Player: Costume test or a costume Tag filter.");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.label4.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.label4.Location = new System.Drawing.Point(435, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(10, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = ")";
            this.toolTip1.SetToolTip(this.label4, "Only for previewing the layers of the targeted character's alternate skins. Selecting anything\nor leaving this field empty has no effect on the conditions for the line to play.\nIf you want a costume condition, add a Player: Costume test or a costume Tag filter.");
            // 
            // StageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.recRefCostume);
            this.Controls.Add(this.cboTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboFrom);
            this.Name = "StageControl";
            this.Size = new System.Drawing.Size(693, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Desktop.Skinning.SkinnedComboBox cboFrom;
        private Desktop.Skinning.SkinnedLabel label1;
        private Desktop.Skinning.SkinnedLabel label2;
        private Desktop.Skinning.SkinnedComboBox cboTo;
        private Desktop.CommonControls.RecordField recRefCostume;
        private Desktop.Skinning.SkinnedLabel label3;
        private System.Windows.Forms.ToolTip toolTip1;
        private Desktop.Skinning.SkinnedLabel label4;
    }
}
