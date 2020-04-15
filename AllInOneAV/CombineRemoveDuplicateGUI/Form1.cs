using Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using Utils;

namespace CombineRemoveDuplicateGUI
{
    public partial class Form1 : Form
    {
        private List<CheckDuplcateModel> model;
        private FileInfo currentFi;
        private TreeNode currentTN;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void LoadFile()
        {
            var files = Directory.GetFiles("c:\\setting\\checkresult\\");

            if (files.Length > 0)
            {
                var file = files.FirstOrDefault();
                StreamReader sr = new StreamReader(file);
                var content = sr.ReadToEnd();

                model = JsonConvert.DeserializeObject<List<CheckDuplcateModel>>(content);
            }
        }

        private void ShowTreeView()
        {
            if (model != null && model.Count > 0)
            {
                treeView1.BeginUpdate();

                foreach (var key in model)
                {
                    TreeNode tn = new TreeNode(key.Key);
                    tn.BackColor = Color.Green;
                    int currentIndex = 0;

                    foreach (var subItem in key.ContainsFiles)
                    {
                        FileInfo fi = new FileInfo(subItem);

                        TreeNode child = new TreeNode(fi.FullName);
                        child.Tag = fi;

                        if (fi.Extension.ToLower() == ".iso")
                        {
                            child.BackColor = Color.Red;
                        }

                        if (currentIndex == key.Biggest)
                        {
                            child.BackColor = Color.Yellow;
                        }

                        tn.Nodes.Add(child);
                        currentIndex++;
                    }

                    treeView1.Nodes.Add(tn);

                    if (key.IsExpend == true)
                    {
                        tn.Expand();
                    }
                    else
                    {
                        tn.BackColor = Color.Gray;
                        tn.Collapse();
                    }
                }

                treeView1.EndUpdate();
            }
        }

        private void ResetUI()
        {
            currentFi = null;
            currentTN = null;

            txtName.Text = "";
            txtSize.Text = "";
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                currentFi = (FileInfo)e.Node.Tag;
                currentTN = e.Node;

                txtName.Text = currentFi.FullName;
                txtSize.Text = FileSize.GetAutoSizeString(currentFi.Length, 2);
            }
            else
            {
                ResetUI();
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (currentFi != null)
            {
                FileUtility.PlayVideo(currentFi.FullName);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentFi != null && currentTN != null)
            {
                var rs = MessageBox.Show("是否要删除 " + currentFi.FullName + "?", "警告", MessageBoxButtons.YesNo);

                if (rs == DialogResult.Yes || rs == DialogResult.OK)
                {
                    FileSystem.DeleteFile(currentFi.FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

                    treeView1.Nodes.Remove(currentTN);
                }
            }
        }

        private void Init()
        {
            Dictionary<string, List<FileInfo>> fileContainer = new Dictionary<string, List<FileInfo>>();
            string folder = "fin\\";
            var drivers = Environment.GetLogicalDrives();
            int single = 0;
            int duplicate = 0;
            int total = 0;
            long totalSize = 0; ;
            long singleSize = 0;
            long duplicateSize = 0;
            List<CheckDuplcateModel> res = new List<CheckDuplcateModel>();

            foreach (var driver in drivers)
            {
                string targetFolder = driver + folder;

                if (Directory.Exists(targetFolder))
                {
                    var files = Directory.GetFiles(targetFolder);

                    foreach (var file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        var fileSplitName = fi.Name.Split('-');

                        if (fileSplitName.Length >= 3)
                        {
                            var key = fileSplitName[0] + "-" + fileSplitName[1] + "-" + fileSplitName[2];

                            if (fileContainer.ContainsKey(key))
                            {
                                fileContainer[key].Add(fi);
                            }
                            else
                            {
                                fileContainer.Add(key, new List<FileInfo>() { fi });
                            }
                        }
                    }
                }
            }

            foreach (var key in fileContainer)
            {
                if (key.Value.Count > 1)
                {
                    duplicate++;
                    total++;

                    CheckDuplcateModel cdm = new CheckDuplcateModel();
                    List<string> files = new List<string>();

                    cdm.ContainsFiles = files;
                    cdm.Key = key.Key;

                    int currentIndex = 0;
                    bool isExpend = false;
                    var first = key.Value.FirstOrDefault();
                    var firstFolder = first.DirectoryName[0];
                    var currentSize = first.Length; 

                    foreach (var fi in key.Value)
                    {
                        cdm.ContainsFiles.Add(fi.FullName);
                        var file = fi.FullName;
                        var fileName = fi.Name;
                        var fileSize = fi.Length;

                        duplicateSize += fileSize;
                        totalSize += fileSize;

                        if (fi.DirectoryName[0] != firstFolder)
                        {
                            isExpend = true;
                        }

                        if (fi.Length > currentSize)
                        {
                            currentSize = fi.Length;
                            cdm.Biggest = currentIndex;
                        }

                        currentIndex++;
                    }

                    cdm.IsExpend = isExpend;

                    if (cdm.IsExpend == false)
                    {
                        cdm.Biggest = -1;
                    }

                    res.Add(cdm);
                }
                else
                {
                    var fi = key.Value.FirstOrDefault();

                    var file = fi.FullName;
                    var fileName = fi.Name;
                    var fileSize = fi.Length;

                    single++;
                    total++;
                    singleSize += fi.Length;
                    totalSize += fileSize;
                }
            }

            var logFolder = "c:\\setting\\checkresult\\";
            var logFile = logFolder + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".json";

            if (Directory.Exists(logFolder))
            {
                Directory.Delete(logFolder);
            }

            Directory.CreateDirectory(logFolder);

            File.Create(logFile).Close();

            StreamWriter sw = new StreamWriter(logFile);
            sw.WriteLine(JsonConvert.SerializeObject(res));
            sw.Close();
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            Init();

            LoadFile();

            if (model != null && model.Count > 0)
            {
                ShowTreeView();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowWarning();
        }

        private void ShowWarning()
        {
            int totalNode = 0;
            int totalSubNode = 0;
            int greenNode = 0;
            int grayNode = 0;
            int deleteNode = 0;
            long totalDeleteSize = 0;
            Dictionary<string, TreeNode> deleteFiles = new Dictionary<string, TreeNode>();

            foreach (TreeNode node in treeView1.Nodes)
            {
                totalNode++;

                if (node.Parent == null)
                {
                    if (node.BackColor == Color.Gray)
                    {
                        grayNode++;
                    }
                    else
                    {
                        greenNode++;

                        foreach (TreeNode subNode in node.Nodes)
                        {
                            totalSubNode++;

                            if (subNode.BackColor != Color.Yellow)
                            {
                                deleteNode++;
                                totalDeleteSize += ((FileInfo)subNode.Tag).Length;
                                deleteFiles.Add(((FileInfo)subNode.Tag).FullName, subNode);
                            }
                        }
                    }
                }
            }

            var rs = MessageBox.Show(string.Format("一共 -> {0} 个节点, \r绿色节点 -> {1}, \r灰色节点 -> {2}, \r总共子节点 -> {5}, \r需要删除节点 -> {3}, \r总共删除大小 -> {4}", totalNode, greenNode, grayNode, deleteNode, FileSize.GetAutoSizeString(totalDeleteSize, 2), totalSubNode), "警告", MessageBoxButtons.YesNo);

            if (rs == DialogResult.Yes)
            {
                foreach (var file in deleteFiles)
                {
                    FileSystem.DeleteFile(file.Key, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

                    treeView1.Nodes.Remove(file.Value);
                }
            }
        }
    }
}
