using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.ScanDataBaseHelper;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace AvReName
{
    public partial class Form1 : Form
    {
        private readonly string imageFolder = JavINIClass.IniReadValue("Jav", "imgFolder");
        private List<string> allPrefix = new List<string>();
        private FileInfo currentFi = null;
        private CookieContainer javBusCC = null;
        private Dictionary<string, string> mapping = new Dictionary<string, string>();

        #region 行为
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshCache();
            RefreshJabBusCache();
        }

        private void cbChinese_CheckedChanged(object sender, EventArgs e)
        {
            AppendPostfix();
            ConfirmMethod();
        }

        private void rb0_CheckedChanged(object sender, EventArgs e)
        {
            AppendPostfix();
        }

        private void rb1_CheckedChanged(object sender, EventArgs e)
        {
            AppendPostfix();
            ConfirmMethod();
        }

        private void rb2_CheckedChanged(object sender, EventArgs e)
        {
            AppendPostfix();
            ConfirmMethod();
        }

        private void rb3_CheckedChanged(object sender, EventArgs e)
        {
            AppendPostfix();
            ConfirmMethod();
        }

        private void rb4_CheckedChanged(object sender, EventArgs e)
        {
            AppendPostfix();
            ConfirmMethod();
        }

        private void rb5_CheckedChanged(object sender, EventArgs e)
        {
            AppendPostfix();
            ConfirmMethod();
        }

        private void rb6_CheckedChanged(object sender, EventArgs e)
        {
            AppendPostfix();
            ConfirmMethod();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            var result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void folderConfirmBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                ResetListView();
                var files = Directory.GetFiles(textBox1.Text);
                InitListViewAv(files);
                UpdateLabelCount(files.Count());
            }
        }

        private void listViewAvs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ListView.SelectedIndexCollection indexes = listViewAvs.SelectedIndices;
                if (indexes.Count > 0)
                {
                    ResetInfos();
                    int index = indexes[0];
                    currentFi = (FileInfo)listViewAvs.Items[index].Tag;

                    AvFileSelect();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void findBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(targetText.Text))
            {
                FindAv(targetText.Text);
            }
        }

        private void avItemClicked(object sender, EventArgs e)
        {
            AvItemClick(sender);
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            DeleteCurrent();
        }

        private void UnBtn_Click(object sender, EventArgs e)
        {
            UnMethod();
        }

        private void UsBtn_Click(object sender, EventArgs e)
        {
            EuMethod();
        }

        private void realBtn_Click(object sender, EventArgs e)
        {
            RealMethod();
        }

        private void cnBtn_Click(object sender, EventArgs e)
        {
            CnMethod();
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            ConfirmMethod();
        }

        private void otherBtn_Click(object sender, EventArgs e)
        {
            SkipMethod();
        }

        private void listViewAvs_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (listViewAvs.SelectedItems != null && listViewAvs.SelectedItems.Count > 0)
                {
                    Play(listViewAvs.SelectedItems[0]);
                }
            }
        }

        private void fetchBtn_Click(object sender, EventArgs e)
        {
            Fetch f = new Fetch();

            var result = f.ShowDialog();

            if (result == DialogResult.Yes)
            {
                RefreshCache();
            }
        }

        private void btnBus_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(targetText.Text) && javBusCC != null)
            {
                var list = JavBusDownload.JavBusDownloadHelper.GetJavBusSearchListModel(targetText.Text, javBusCC);

                if (list != null && list.Count > 0)
                {
                    JavBus jb = new JavBus(list, javBusCC, mapping, targetText);
                    var rs = jb.ShowDialog();

                    if (rs == DialogResult.Yes)
                    {
                        RefreshCache();
                    }
                }
                else
                {
                    MessageBox.Show("没有找到 " + targetText.Text);
                }
            }
        }

        private void btnRemoveZero_Click(object sender, EventArgs e)
        {
            RemoveZeroClick();
        }
        #endregion

        #region 方法
        private void InitListViewAv(string[] files)
        {
            currentFi = null;
            listViewAvs.BeginUpdate();

            foreach (var f in files)
            {
                FileInfo fi = new FileInfo(f);

                ListViewItem lvi = new ListViewItem(fi.Name);
                lvi.Tag = fi;

                listViewAvs.Items.Add(lvi);
            }

            listViewAvs.EndUpdate();
        }

        private void UpdateLabelCount(int total)
        {
            labelCount.Text = total + "剩余";
        }

        private void AvFileSelect()
        {
            var fiNameUpper = currentFi.Name.ToUpper();

            var fileNameWithoutFormat = fiNameUpper.Replace(currentFi.Extension, "");
            oriText.Text = fiNameUpper;

            foreach (var prefix in allPrefix)
            {
                if (fileNameWithoutFormat.Contains(prefix))
                {
                    var pattern = prefix + "{1}-?\\d{1,5}";
                    var possibleId = Regex.Match(fileNameWithoutFormat, pattern).Value;

                    if (possibleId.Contains("-"))
                    {
                        targetText.Text = possibleId;
                    }
                    else
                    {
                        bool isFirst = true;
                        StringBuilder sb = new StringBuilder();

                        foreach (var c in possibleId)
                        {
                            if (c >= '0' && c <= '9')
                            {
                                if (isFirst)
                                {
                                    sb.Append("-");
                                    isFirst = false;
                                }
                            }
                            sb.Append(c);
                        }

                        targetText.Text = sb.ToString();
                    }

                    FindAv(targetText.Text);

                    break;
                }
            }
        }

        private void ResetInfos()
        {
            currentFi = null;
            oriText.Text = "";
            targetText.Text = "";
            cbChinese.Checked = false;
            rb0.Checked = true;
            panel8.Controls.Clear();
        }

        private void ResetListView()
        {
            listViewAvs.Items.Clear();
        }

        private void FindAv(string id)
        {
            var avs = JavDataBaseManager.GetAllAV(id);

            if (avs != null && avs.Count > 0)
            {
                ShowAv(avs);
            }
        }

        private void ShowAv(List<AV> avs)
        {
            panel8.Controls.Clear();
            var count = avs.Count;

            var outerWidth = panel8.Width - 10;
            var outerHeight = panel8.Height;

            var singleWidth = (int)(outerWidth / 3);
            var singleHeight = (int)(singleWidth / 1.5);

            int x = 2, y = 2;

            while (count > 0)
            {
                count -= 3;

                foreach (var av in avs.Take(3))
                {
                    AvItem item = new AvItem(av.Name, imageFolder + av.ID + av.Name + ".jpg", av.ID)
                    {
                        Width = singleWidth,
                        Height = singleHeight,
                        Location = new Point(x, y)
                    };

                    item.AvItemClicked += new AvItem.AvItemClickHandle(avItemClicked);

                    panel8.Controls.Add(item);

                    x += singleWidth + 5;
                }

                x = 2;
                y += singleHeight + 2;

                avs = avs.Skip(3).ToList();
            }

            if (panel8.Controls.Count > 0)
            {
                AvItemClick(panel8.Controls[0].Controls[1].Controls[0]);
                ConfirmMethod();
            }
        }

        private void ResetPanel8()
        {
            var controls = panel8.Controls;

            foreach (var c in controls)
            {
                AvItem av = (AvItem)c;

                av.Panel.BackColor = Color.Empty;
            }
        }

        private void SetName(string avName, string id)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(id);
            sb.Append("-" + avName);

            targetText.Text = sb.ToString();
        }

        private void AppendPostfix()
        {
            AvItem item = null;

            foreach (var control in panel8.Controls)
            {
                AvItem temp = (AvItem)control;
                if (temp.Panel.BackColor == Color.Blue)
                {
                    item = temp;
                }
            }

            if (item != null)
            {
                AvItemClick(item.Controls[1].Controls[0]);

                StringBuilder sb = new StringBuilder();

                if (cbChinese.Checked)
                {
                    sb.Append("-C");
                }

                if (rb0.Checked)
                {
                    sb.Append("");
                }

                if (rb1.Checked)
                {
                    sb.Append("-1");
                }

                if (rb2.Checked)
                {
                    sb.Append("-2");
                }

                if (rb3.Checked)
                {
                    sb.Append("-3");
                }

                if (rb4.Checked)
                {
                    sb.Append("-4");
                }

                if (rb5.Checked)
                {
                    sb.Append("-5");
                }

                if (rb6.Checked)
                {
                    sb.Append("-6");
                }

                targetText.Text = targetText.Text + sb.ToString();
            }
        }

        private void DeleteCurrent()
        {
            if (currentFi != null)
            {
                var result = MessageBox.Show("确定要删除 -> " + currentFi.FullName + " ? " + Utils.FileSize.GetAutoSizeString(currentFi.Length, 1), "警告", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    File.Delete(currentFi.FullName);
                }

                int next = RemoveCurrentItem();

                UpdateHighlight(next);
            }
        }

        private bool MoveTo(string subFolder, bool IsRename = false)
        {
            if (currentFi != null)
            {
                string fileName = "";

                if (IsRename)
                {
                    fileName = targetText.Text + currentFi.Extension;
                }
                else
                {
                    fileName = currentFi.Name;
                }

                var targetFolder = currentFi.DirectoryName + "/" + subFolder;

                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                var result = MessageBox.Show("确定要移动 -> " + currentFi.FullName + " " + Utils.FileSize.GetAutoSizeString(currentFi.Length, 1) + " 到 " + targetFolder + "/" + fileName + "?", "警告", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    var isExist = IsFileExistInDesc(targetFolder + "/" + fileName);

                    if (isExist != null)
                    {
                        var reult = MessageBox.Show("是否要替换 -> " + Utils.FileSize.GetAutoSizeString(currentFi.Length, 2) + " *** " + isExist.FullName + " Size -> " + Utils.FileSize.GetAutoSizeString(isExist.Length, 2), "警告", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            MoveFile(isExist, isExist.FullName + ".bak" + DateTime.Now.ToString("yyyyMMddhhmmss"));
                            MoveFile(currentFi, targetFolder + "/" + fileName);
                        }
                    }
                    else
                    {
                        MoveFile(currentFi, targetFolder + "/" + fileName);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        private int RemoveCurrentItem()
        {
            var currentItem = listViewAvs.SelectedItems[0];

            int ret = 0;

            if (currentItem != null)
            {
                ret = currentItem.Index;

                listViewAvs.Items.Remove(currentItem);

                ResetInfos();
            }

            return ret;
        }

        private FileInfo IsFileExistInDesc(string filePath)
        {
            FileInfo ret = new FileInfo(filePath);

            return File.Exists(filePath) ? ret : null;
        }

        private void MoveFile(FileInfo fi, string desc)
        {
            File.Move(fi.FullName, desc);
        }

        private void Play(ListViewItem current)
        {
            if (current != null)
            {
                System.Diagnostics.Process.Start(@"" + ((FileInfo)current.Tag).FullName);
            }
        }

        private void UnMethod()
        {
            if(MoveTo("无码"))
            {
                int next = RemoveCurrentItem();
                UpdateLabelCount(listViewAvs.Items.Count);

                UpdateHighlight(next);
            }
        }

        private void EuMethod()
        {
            if(MoveTo("欧美"))
            {
                int next = RemoveCurrentItem();
                UpdateLabelCount(listViewAvs.Items.Count);

                UpdateHighlight(next);
            }
        }

        private void RealMethod()
        {
            if (MoveTo("素人"))
            {
                int next = RemoveCurrentItem();
                UpdateLabelCount(listViewAvs.Items.Count);

                UpdateHighlight(next);
            }
        }

        private void CnMethod()
        {
            if (MoveTo("国产"))
            {
                int next = RemoveCurrentItem();
                UpdateLabelCount(listViewAvs.Items.Count);

                UpdateHighlight(next);
            }
        }

        private void SkipMethod()
        {
            int next = RemoveCurrentItem();
            UpdateLabelCount(listViewAvs.Items.Count);

            UpdateHighlight(next);
        }

        private void ConfirmMethod()
        {
            if (MoveTo("Fin", true))
            {
                int next = RemoveCurrentItem();
                UpdateLabelCount(listViewAvs.Items.Count);

                UpdateHighlight(next);
            }
        }

        private void RefreshJabBusCache()
        {
            javBusCC = JavBusDownload.JavBusDownloadHelper.GetJavBusCookie();
            JavBusDownload.JavBusDownloadHelper.UpdateJavBusCategory(javBusCC);
            mapping = JavDataBaseManager.GetJavBusCategoryMapping();
        }

        private void RefreshCache()
        {
            allPrefix = ScanDataBaseManager.GetPrefix().Select(x=>x.Prefix).ToList();
            allPrefix = allPrefix.OrderByDescending(x => x.Length).ToList();
        }

        private void AvItemClick(object sender)
        {
            ResetPanel8();

            PictureBox pb = (PictureBox)sender;
            AvItem av = (AvItem)pb.Parent.Parent;

            av.Panel.BackColor = Color.Blue;

            SetName(av.AvName, av.AvId);
        }

        private void UpdateHighlight(int next)
        {
            try
            {
                if (listViewAvs.Items[next] != null)
                {
                    listViewAvs.HideSelection = false;
                    listViewAvs.Items[next].Selected = true;
                    listViewAvs.Select();
                }
            }
            catch (Exception ee)
            {

            }
        }

        private void RemoveZeroClick()
        {
            if (!string.IsNullOrEmpty(targetText.Text) && targetText.Text.Contains("-") && targetText.Text.Contains("0"))
            {
                var str = targetText.Text;
                var strSplit = str.Split('-');
                var removeZeroStr = strSplit[1];

                if (removeZeroStr[0] == '0')
                {
                    targetText.Text = strSplit[0] + "-" + removeZeroStr.Substring(1);
                }
            }
        }
        #endregion
    }
}