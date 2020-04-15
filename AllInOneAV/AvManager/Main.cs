using DataBaseManager.AvManagerDataBaseManager;
using DataBaseManager.JavDataBaseHelper;
using Model.JavModels;
using Model.ScanModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace AvManager
{
    public partial class Main : Form
    {
        #region GlobalVar
        private static List<string> formats = JavINIClass.IniReadValue("Scan", "Format").Split(',').ToList();
        private static List<string> excludes = JavINIClass.IniReadValue("Scan", "Exclude").Split(',').ToList();
        private static readonly string imageFolder = JavINIClass.IniReadValue("Jav", "imgFolder");
        private static List<FileInfo> srcFi = new List<FileInfo>();
        private static List<FileInfo> desFi = new List<FileInfo>();
        private static List<FileInfo> renameFi = new List<FileInfo>();
        private static int indexOfRename = 0;
        private static int indexOfCurrentAV = 0;
        private static string renameTotal = "{0}/{1}   {2}";
        private List<AV> avs = new List<AV>();
        private List<AV> currentAVS = new List<AV>();
        private List<string> prefixs = new List<string>();
        private static FileInfo currentFi;
        private static string currentFolder = "";
        #endregion

        #region System
        public Main()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region Move
        private void txtMoveSrc_Click(object sender, EventArgs e)
        {
            MoveSrcClick();
        }

        private void txtMoveDes_Click(object sender, EventArgs e)
        {
            MoveDesClick();
        }

        private void btnMoveMove_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(MoveBtnClick);
            thread.Start();
        }

        private void btnMoveStart_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMoveDes.Text) && !string.IsNullOrEmpty(txtMoveSrc.Text))
            {
                StartBtnClick();
            }
        }

        private void StartBtnClick()
        {
            srcFi = new List<FileInfo>();
            lvMoveSrc.Items.Clear();
            var oriExcludes = excludes;
            excludes.Add(txtMoveDes.Text);

            if (!string.IsNullOrEmpty(txtMoveSrc.Text))
            {
                FileUtility.GetFilesRecursive(txtMoveSrc.Text, formats, excludes, srcFi, 200);

                lvMoveSrc.BeginUpdate();
                foreach (var f in srcFi)
                {
                    ListViewItem lvi = new ListViewItem(f.Name);
                    lvi.SubItems.Add(FileSize.GetAutoSizeString(f.Length, 1));

                    if (desFi.Exists(x => x.Name.ToLower() == f.Name.ToLower()))
                    {
                        lvi.BackColor = Color.Red;
                    }

                    lvMoveSrc.Items.Add(lvi);
                }
                lvMoveSrc.EndUpdate();
            }

            excludes = oriExcludes;
        }

        private void MoveSrcClick()
        {
            InitFD(fdMoveSrc, txtMoveSrc);
        }

        private void MoveBtnClick()
        {
            if (srcFi.Count > 0 && !string.IsNullOrEmpty(txtMoveDes.Text))
            {
                pbMove.Value = 0;
                pbMove.Maximum = srcFi.Count;
                pbMove.Minimum = 0;
                pbMove.Step = 1;

                var logFile = "MoveLog" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                var des = txtMoveDes.Text;
                DateTime now = DateTime.Now;

                foreach (var fi in srcFi)
                {
                    var newFi = "";

                    if (desFi.Exists(x => x.Name.ToLower() == fi.Name.ToLower()))
                    {
                        LogHelper.WriteLog(logFile, "找到重复的文件");

                        var desF = desFi.Find(x => x.Name.ToLower() == fi.Name.ToLower());

                        if (cbAutoReplace.Checked)
                        {
                            LogHelper.WriteLog(logFile, "自动模式");

                            if (fi.Length >= desF.Length)
                            {
                                newFi = des + "/" + fi.Name;
                                File.Delete(desF.FullName);
                                MoveFile(fi.FullName, newFi, now);

                                LogHelper.WriteLog(logFile, "删除 -> " + desF.FullName + " 移动 " + fi.FullName + " -> " + newFi);
                            }
                            else
                            {
                                File.Delete(fi.FullName);

                                LogHelper.WriteLog(logFile, "删除 -> " + fi.FullName);
                            }
                        }
                        else
                        {
                            LogHelper.WriteLog(logFile, "手动模式");

                            CompareMove cm = new AvManager.CompareMove();
                            cm.oriFile = fi;
                            cm.desFile = desF;
                            cm.desPath = desF.DirectoryName + "/";
                            cm.logFile = logFile;

                            cm.ShowDialog();
                        }
                    }
                    else
                    {
                        LogHelper.WriteLog(logFile, "没找到重复的文件");

                        newFi = des + "/" + fi.Name;
                        MoveFile(fi.FullName, newFi, now);

                        LogHelper.WriteLog(logFile, "删除 -> " + fi.FullName + " 移动 -> " + newFi);

                        desFi.Add(new FileInfo(newFi));
                    }


                    var item = lvMoveSrc.FindItemWithText(fi.Name);

                    if (item != null)
                    {
                        lvMoveSrc.Items.Remove(item);
                    }
                    pbMove.PerformStep();
                }

                MessageBox.Show("Finished!");
            }
        }

        private void MoveFile(string src, string des, DateTime time)
        {
            File.Move(src, des);
            AvManagerDataBaseManager.InserMoveLog(FileUtility.ReplaceInvalidChar(src), FileUtility.ReplaceInvalidChar(des), time);
        }

        private void MoveDesClick()
        {
            InitFD(fdMoveDes, txtMoveDes);
            desFi = new List<FileInfo>();
            lvMoveDes.Items.Clear();

            if (!string.IsNullOrEmpty(txtMoveDes.Text))
            {
                var files = Directory.GetFiles(txtMoveDes.Text);

                lvMoveDes.BeginUpdate();
                foreach (var f in files)
                {
                    FileInfo fi = new FileInfo(f);

                    ListViewItem lvi = new ListViewItem(fi.Name);
                    lvi.SubItems.Add(FileSize.GetAutoSizeString(fi.Length, 1));

                    lvMoveDes.Items.Add(lvi);
                    desFi.Add(fi);
                }
                lvMoveDes.EndUpdate();
            }
        }
        #endregion

        #region Rename
        private void btRenameStart_Click(object sender, EventArgs e)
        {
            ShowRenameFiles(txtRename.Text);
        }

        private void txtRename_Click(object sender, EventArgs e)
        {
            InitFD(fdRename, txtRename);
        }

        private void btnRenameNext_Click(object sender, EventArgs e)
        {
            RenameNext();
        }

        private void btnRenamePre_Click(object sender, EventArgs e)
        {
            RenamePre();
        }

        private void btRenameLeft_Click(object sender, EventArgs e)
        {
            RenameLeft();
        }

        private void btRenameRight_Click(object sender, EventArgs e)
        {
            RenameRight();
        }

        private void btnRenameSearch_Click(object sender, EventArgs e)
        {
            currentAVS = new List<AV>();
            currentAVS.AddRange(JavDataBaseManager.GetAllAV(txtKeyword.Text));

            if (currentAVS.Count > 0)
            {
                indexOfCurrentAV = 0;
                ShowRenameInfo();
            }
            else
            {
                //MessageBox.Show("Not found, please add manually.");
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            RenameConfirm();
        }

        private void rbCensor_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRename.Text))
            {
                SetFinalFilePath();
            }
        }

        private void rbEnglish_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRename.Text))
            {
                SetFinalFilePath();
            }
        }

        private void rbUncensor_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRename.Text))
            {
                SetFinalFilePath();
            }
        }

        private void rbNoteFound_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRename.Text))
            {
                SetFinalFilePath();
            }
        }

        private void btnRenamePlay_Click(object sender, EventArgs e)
        {
            RenamePlay();
        }

        private void ShowRenameFiles(string folder)
        {
            RenameInit();
            currentFolder = folder + "/";
            renameFi = new List<FileInfo>();
            FileUtility.GetFilesRecursive(folder, formats, excludes, renameFi);
            renameFi = renameFi.Where(x => !x.Name.Contains("-fin")).ToList();
            avs = JavDataBaseManager.GetAllAV();
            prefixs = FileUtility.GetPrefix(avs);
            pbRename.Minimum = 0;
            pbRename.Maximum = renameFi.Count;
            pbRename.Step = 1;

            if (renameFi.Count > 0)
            {
                indexOfRename = 0;
                lbRenameTotal.Text = string.Format(renameTotal, (indexOfRename + 1), renameFi.Count, renameFi[indexOfRename].FullName);
                btnRenamePre.Enabled = true;
                btnRenameNext.Enabled = true;

                ShowRenameDetail();
            }
        }

        private void ShowRenameDetail()
        {
            RenameInitPartial();
            var file = renameFi[indexOfRename];
            currentFi = file;
            var scan = new Scan() {
                FileName = file.FullName,
                Location = file.DirectoryName,
                Size = Utils.FileSize.GetAutoSizeString(file.Length, 1)
            };
            txtRenameFinal.Text = "";
            pbRename.Maximum = renameFi.Count;
            pbRename.Value = indexOfRename;
            txtRenameOri.Text = currentFi.FullName;
            txtRenameFinal.Text = currentFi.FullName;

            if (file != null)
            {
                var ids = FileUtility.GetPossibleID(scan, prefixs);

                if (ids.Count > 0)
                {
                    currentAVS = new List<AV>();

                    currentAVS.AddRange(JavDataBaseManager.GetAllAV(ids));

                    if (currentAVS.Count > 0)
                    {
                        indexOfCurrentAV = 0;
                        ShowRenameInfo();

                        if (currentAVS.Count > 1)
                        {
                            btRenameLeft.Show();
                            btRenameRight.Show();
                        }
                        else
                        {
                            btRenameLeft.Hide();
                            btRenameRight.Hide();
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Not found, please add manually.");
                    }
                }
                else
                {
                    //MessageBox.Show("Not found, please add manually.");
                }
            }
        }

        private void ShowRenamePic(AV av)
        {
            var file = FileUtility.GetImageFile(imageFolder, av);

            if (!string.IsNullOrEmpty(file))
            {
                pictureRename.Image = Image.FromFile(file);
            }
        }

        private void ShowRenameInfo(AV av = null)
        {
            if (currentFi != null)
            {
                if (av == null)
                {
                    av = currentAVS[indexOfCurrentAV];
                }

                txtRenameID.Text = av.ID;
                txtRenameTitle.Text = av.Name;
                txtRenameCategory.Text = av.Category;
                txtRenameCompany.Text = av.Company;
                txtRenameYear.Text = av.ReleaseDate.ToString("yyyy-MM-dd");
                txtRenameActress.Text = av.Actress;
                txtReanmeLength.Text = av.AvLength + "";
                txtKeyword.Text = av.ID;

                ShowRenamePic(av);

                txtRenameFinal.Text = av.ID + av.Name + "-fin" + currentFi.Extension;
                btnConfirm.Enabled = true;
            }
            else
            {
                MessageBox.Show("No file selected.");
            }
        }

        private void RenameNext()
        {
            if (indexOfRename + 1 < renameFi.Count)
            {
                indexOfRename++;
                ShowRenameDetail();
                lbRenameTotal.Text = string.Format(renameTotal, (indexOfRename + 1), renameFi.Count, currentFi.FullName);
            }
        }

        private void RenamePre()
        {
            if (indexOfRename - 1 >= 0)
            {
                indexOfRename--;
                ShowRenameDetail();
                lbRenameTotal.Text = string.Format(renameTotal, (indexOfRename + 1), renameFi.Count, currentFi.FullName);
            }
        }

        private void RenameLeft()
        {
            if (indexOfCurrentAV - 1 >= 0)
            {
                indexOfCurrentAV--;
                ShowRenameInfo();
            }
        }

        private void RenameRight()
        {
            if (indexOfCurrentAV + 1 < currentAVS.Count)
            {
                indexOfCurrentAV++;
                ShowRenameInfo();
            }
        }

        private void SetFinalFilePath()
        {
            currentFolder = txtRename.Text + "/";

            if (rbEnglish.Checked)
            {
                currentFolder += "欧美/";
                btnConfirm.Enabled = true;
            }

            if (rbNoteFound.Checked)
            {
                currentFolder += "没找到/";
                btnConfirm.Enabled = true;
            }

            if (rbUncensor.Checked)
            {
                currentFolder += "无码/";
                btnConfirm.Enabled = true;
            }

            if (rbCensor.Checked)
            {
                currentFolder += "fin/";

                if (string.IsNullOrEmpty(txtRenameID.Text) || string.IsNullOrEmpty(txtRenameTitle.Text))
                {
                    btnConfirm.Enabled = false;
                }
            }
        }

        private void RenameInit()
        {
            btnRenamePre.Enabled = false;
            btnRenameNext.Enabled = false;
            btnConfirm.Enabled = false;

            currentFolder = "";
            txtRenameID.Text = "";
            txtRenameTitle.Text = "";
            txtRenameCategory.Text = "";
            txtRenameCompany.Text = "";
            txtRenameYear.Text = "";
            txtRenameActress.Text = "";
            txtReanmeLength.Text = "";
            pictureRename.Image = null;
            txtRenameOri.Text = "";
            txtRenameFinal.Text = "";
            txtKeyword.Text = "";
        }

        private void RenameInitPartial()
        {
            btRenameLeft.Hide();
            btRenameRight.Hide();
            btnConfirm.Enabled = false;

            txtRenameID.Text = "";
            txtRenameTitle.Text = "";
            txtRenameCategory.Text = "";
            txtRenameCompany.Text = "";
            txtRenameYear.Text = "";
            txtRenameActress.Text = "";
            txtReanmeLength.Text = "";
            txtKeyword.Text = "";
            pictureRename.Image = null;
        }

        private void RenamePlay()
        {
            if (currentFi != null)
            {
                System.Diagnostics.Process.Start(currentFi.FullName);
            }
        }

        private void RenameConfirm()
        {
            if (!string.IsNullOrEmpty(txtRenameFinal.Text))
            {
                SetFinalFilePath();

                var des = currentFolder + txtRenameFinal.Text.Substring(txtRenameFinal.Text.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                if (!Directory.Exists(currentFolder))
                {
                    Directory.CreateDirectory(currentFolder);
                }

                var res = MessageBox.Show("Move to " + des);

                if (res == DialogResult.OK || res == DialogResult.Yes)
                {
                    try
                    {
                        currentFi.MoveTo(des);
                        renameFi.RemoveAt(indexOfRename);
                        if (indexOfRename >= 0)
                        {
                            ShowRenameDetail();
                            lbRenameTotal.Text = string.Format(renameTotal, (indexOfRename + 1), renameFi.Count, currentFi.FullName);
                            //indexOfRename--;
                        }
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.ToString());
                    }
                }
            }
        }
        #endregion

        #region Common
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                if (e.KeyCode == Keys.Left)
                {
                    RenameLeft();
                }

                if (e.KeyCode == Keys.Right)
                {
                    RenameRight();
                }

                if (e.KeyCode == Keys.Up)
                {
                    RenamePre();
                }

                if (e.KeyCode == Keys.Down)
                {
                    RenameNext();
                }

                if (e.KeyCode == Keys.Space)
                {
                    RenamePlay();
                }

                if (e.KeyCode == Keys.Enter)
                {
                    RenameConfirm();
                }

                if (e.KeyCode == Keys.Space)
                {
                    if (!string.IsNullOrEmpty(txtRenameOri.Text))
                    {
                        System.Diagnostics.Process.Start(txtRenameOri.Text);
                    }
                }
            }
        }

        private void InitFD(FolderBrowserDialog fd, TextBox tb)
        {
            fd.RootFolder = Environment.SpecialFolder.MyComputer;

            var result = fd.ShowDialog();

            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                tb.Text = fd.SelectedPath;
            }
        }
        #endregion

        private void pictureRename_Click(object sender, EventArgs e)
        {

        }
    }
}
