﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HVS
{
    public partial class ClearSetting_FR : Form
    {
        MainForm mf;
        public SortObject[] sortedArray;
        public string referenceImage;

        public ClearSetting_FR(MainForm mf, string referenceImage)
        {
            InitializeComponent();
            this.referenceImage = referenceImage;
            this.mf = mf;
        }

        private void confirm_Click(object sender, EventArgs e)
        {
            Dispose();
            int tot = 0;
            for (int i = 0; i < MainInfo.tot; i++)
            {
                if (MainInfo.selected[i])
                {
                    tot++;
                }
            }
            int index = 0;

            if (ratioButton.Checked)
            {
                SortObject[] sortedArray = new SortObject[tot];
                for (int i = 0; i < MainInfo.tot; i++)
                {
                    if (MainInfo.selected[i])
                    {
                        double result = FR_algorithm.FR_method(SettingInfo.imageClear_FRmethod,
                            MainInfo.picInfo[MainInfo.path_name[i]].rgb,
                            MainInfo.picInfo[referenceImage].rgb,
                            MainInfo.picInfo[referenceImage].height,
                            MainInfo.picInfo[referenceImage].width);
                        MainInfo.picInfo[MainInfo.path_name[i]].grade_full = FR_algorithm.FR_grade(SettingInfo.imageClear_FRmethod, result);
                        sortedArray[index++] = new SortObject(MainInfo.picInfo[MainInfo.path_name[i]].grade_full, MainInfo.path_name[i], MainInfo.name[i]);
                    }
                }

                Array.Sort(sortedArray, new sortObjectComparer());
                DeleteComfirmForm imageDeleteComfirmForm = new DeleteComfirmForm(clearingRatioBar.Value, mf, sortedArray);
                imageDeleteComfirmForm.Show();
            }
            else
            {
                SortObject[] sortedArrayTemp = new SortObject[tot];

                for (int i = 0; i < MainInfo.tot; i++)
                {
                    if (MainInfo.selected[i])
                    {
                        double result = FR_algorithm.FR_method(SettingInfo.imageClear_FRmethod,
                            MainInfo.picInfo[MainInfo.path_name[i]].rgb,
                            MainInfo.picInfo[referenceImage].rgb,
                            MainInfo.picInfo[referenceImage].height,
                            MainInfo.picInfo[referenceImage].width);
                        MainInfo.picInfo[MainInfo.path_name[i]].grade_full = FR_algorithm.FR_grade(SettingInfo.imageClear_FRmethod, result);
                        if (MainInfo.picInfo[MainInfo.path_name[i]].grade_full <= threadsoldBar.Value)
                        {
                            sortedArrayTemp[index++] = new SortObject(MainInfo.picInfo[MainInfo.path_name[i]].grade_full, MainInfo.path_name[i], MainInfo.name[i]);
                        }
                    }
                }
                MainForm.previous_pic = referenceImage;
                SortObject[] sortedArray = new SortObject[index];
                for (int i = 0; i < index; i++)
                {
                    sortedArray[i] = sortedArrayTemp[i];
                }

                Array.Sort(sortedArray, new sortObjectComparer());
                DeleteComfirmForm imageDeleteComfirmForm = new DeleteComfirmForm(100, mf, sortedArray);
                imageDeleteComfirmForm.Show();
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void ratioButton_Click(object sender, EventArgs e)
        {
            ratioButton.Checked = true;
            threadsoldButton.Checked = false;
        }

        private void thresholdButton_Click(object sender, EventArgs e)
        {
            ratioButton.Checked = false;
            threadsoldButton.Checked = true;
        }

        private void ratioBar_Scroll(object sender, EventArgs e)
        {
            clearRatio.Text = clearingRatioBar.Value.ToString() + "%";
        }

        private void threadsoldBar_Scroll(object sender, EventArgs e)
        {
            clearThreadsold.Text = threadsoldBar.Value.ToString();
        }
    }
}