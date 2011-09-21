﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginInterface;

namespace Fonts
{
    public partial class FontControl : UserControl
    {
        IPluginHost pluginHost;
        sNFTR font;
        List<CharControl> chars = new List<CharControl>();
        Dictionary<int, int> charTile;

        public FontControl()
        {
            InitializeComponent();
        }
        public FontControl(IPluginHost pluginHost, sNFTR font)
        {
            InitializeComponent();

            this.pluginHost = pluginHost;
            this.font = font;

            for (int i = 0; i < font.plgc.tiles.Length; i++)
                comboBox1.Items.Add("Char " + i.ToString());

            picFont.Image = NFTR.Get_Chars(font, 250);

            Fill_CharTile();

            comboBox1.SelectedIndex = 0;
            comboEncoding.SelectedIndex = 0;
        }
        private void Fill_CharTile()
        {
            charTile = new Dictionary<int, int>();
            for (int p = 0; p < font.pamc.Count; p++)
            {
                if (font.pamc[p].info is sNFTR.PAMC.Type0)
                {
                    sNFTR.PAMC.Type0 type0 = (sNFTR.PAMC.Type0)font.pamc[p].info;
                    int interval = font.pamc[p].last_char - font.pamc[p].first_char;

                    for (int j = 0; j < interval; j++)
                        charTile.Add(font.pamc[p].first_char + j, type0.fist_char_code + j);
                }
                else if (font.pamc[p].info is sNFTR.PAMC.Type1)
                {
                    sNFTR.PAMC.Type1 type1 = (sNFTR.PAMC.Type1)font.pamc[p].info;

                    for (int j = 0; j < type1.char_code.Length; j++)
                        charTile.Add(font.pamc[p].first_char + j, type1.char_code[j]);
                }
                else if (font.pamc[p].info is sNFTR.PAMC.Type2)
                {
                    sNFTR.PAMC.Type2 type2 = (sNFTR.PAMC.Type2)font.pamc[p].info;

                    for (int j = 0; j < type2.num_chars; j++)
                        charTile.Add(type2.chars_code[j], type2.chars[j]);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelCharEdit.Controls.Clear();
            panelCharEdit.Controls.Add(new CharControl(
                (from k in charTile where int.Equals(k.Value, comboBox1.SelectedIndex) select k.Key).FirstOrDefault(),
                font.hdwc.info[comboBox1.SelectedIndex],
                font.plgc.tiles[comboBox1.SelectedIndex],
                font.plgc.depth,
                font.plgc.tile_width,
                font.plgc.tile_height,
                font.plgc.rotateMode));
        }

        private void comboEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBox_TextChanged(txtBox, null);
        }
        private void txtBox_TextChanged(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(picText.Width, picText.Height);
            Graphics graphic = Graphics.FromImage(image);
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            int width = 0;
            int height = 0;
            for (int i = 0; i < txtBox.Text.Length; i++)
            {
                if (txtBox.Text[i] == '\n')
                {
                    width = 0;
                    height += font.plgc.tile_height;
                    continue;
                }

                byte[] codes = new byte[1];
                if (comboEncoding.SelectedIndex == 0)
                    codes = Encoding.GetEncoding("shift-jis").GetBytes(new char[] { txtBox.Text[i] }).Reverse().ToArray();
                else if (comboEncoding.SelectedIndex == 1)
                    codes = Encoding.Unicode.GetBytes(new char[] { txtBox.Text[i] });
                else if (comboEncoding.SelectedIndex == 2)
                    codes = Encoding.BigEndianUnicode.GetBytes(new char[] { txtBox.Text[i] });

                int charCode = (codes.Length == 2 ? BitConverter.ToUInt16(codes, 0) : codes[0]);

                int tileCode;
                if (!charTile.TryGetValue(charCode, out tileCode))
                {
                    width += font.plgc.tile_width;
                    continue;
                }

                width += font.hdwc.info[tileCode].pixel_start;
                graphic.DrawImageUnscaled(NFTR.Get_Char(font, tileCode), width, height);
                width += font.hdwc.info[tileCode].pixel_length - font.hdwc.info[tileCode].pixel_start;

                if (width + font.plgc.tile_width > image.Width)
                {
                    width = 0;
                    height += font.plgc.tile_height;
                }
            }

            picText.Image = image;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            CharControl charControl = (CharControl)panelCharEdit.Controls[0];
            font.hdwc.info[comboBox1.SelectedIndex] = charControl.TileInfo;
            font.plgc.tiles[comboBox1.SelectedIndex] = charControl.Tiles;
            picFont.Image = NFTR.Get_Chars(font, 250);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            String fontFile = System.IO.Path.GetTempFileName();
            NFTR.Write(font, fontFile);
            pluginHost.ChangeFile(font.id, fontFile);
        }

        private void picFont_MouseClick(object sender, MouseEventArgs e)
        {
            int charX = e.X / font.plgc.tile_width;
            int charY = e.Y / font.plgc.tile_height;
            int totalX = 250 / font.plgc.tile_width;

            comboBox1.SelectedIndex = charX + charY * totalX;
        }


    }
}